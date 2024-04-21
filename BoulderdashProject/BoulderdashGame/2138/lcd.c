/******************************************************************************
 *
 * Copyright:
 *    (C) 2006 Embedded Artists AB
 *
 * File:
 *    lcd.c
 *
 * Description:
 *    Implements routines for the LCD.
 *
 *****************************************************************************/

 /* Includes */
#include "general.h"
//#include "osapi.h"
#include "lcd.h"
#include "ascii.h"
#include "lcd_hw.h"
#include "functionals.h"


/* Typedefs and defines */
#define LCD_CMD_SWRESET   0x01
#define LCD_CMD_BSTRON    0x03
#define LCD_CMD_SLEEPIN   0x10
#define LCD_CMD_SLEEPOUT  0x11
#define LCD_CMD_INVON     0x21
#define LCD_CMD_SETCON    0x25
#define LCD_CMD_DISPON    0x29
#define LCD_CMD_CASET     0x2A
#define LCD_CMD_PASET     0x2B
#define LCD_CMD_RAMWR     0x2C
#define LCD_CMD_RGBSET    0x2D
#define LCD_CMD_MADCTL    0x36
#define LCD_CMD_COLMOD    0x3A

#define MADCTL_HORIZ      0x48
#define MADCTL_VERT       0x68

 /* External variables */
extern const tU8 charMap[1372];


 /* Local variables */
static tU8 lcd_x;
static tU8 lcd_y;
static tU8 bkgColor;
static tU8 textColor;


/* Local declarations */
static void lcdWindow1(tU8 xp, tU8 yp, tU8 xe, tU8 ye);
static void lcdData(tU8 data);
static void lcdContrast(tU8 contr);
static void lcdWindow(tU8 xp, tU8 yp, tU8 xe, tU8 ye);


/*
 *  @brief			Funckja inicjalizujaca SPI oraz poprawnie uruchamiajaca wyswietlacz LCD.
 *
 *  @Description	Funkcja majaca na celu ustawienie magistrali SPI oraz poprawna konfiguracje wyswietlacza LCD w celu jego
 *                  poprawnego wlaczenia.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja ustawien rejestrow PINSEL0, SPI_SPCCR, SPI_SPCR
 */
void lcdInit(void)
{
    bkgColor  = 0;
    textColor = 0;
    
    //init SPI interface
    initSpiForLcd();
    
    //select controller
    selectLCD(TRUE);
    
    lcdWrcmd(LCD_CMD_SWRESET);
    
    delay_ms(1);
    lcdWrcmd(LCD_CMD_SLEEPOUT);
    lcdWrcmd(LCD_CMD_DISPON);
    lcdWrcmd(LCD_CMD_BSTRON);
    delay_ms(1);
    	
    lcdWrcmd(LCD_CMD_MADCTL);   //Memory data acces control
    lcdWrdata(MADCTL_HORIZ);    //X Mirror and BGR format
    lcdWrcmd(LCD_CMD_COLMOD);   //Colour mode
    lcdWrdata(0x02);            //256 colour mode select
    lcdWrcmd(LCD_CMD_INVON);    //Non Invert mode
    
    lcdWrcmd(LCD_CMD_RGBSET);   //LUT write
    lcdWrdata(0);               //Red
    lcdWrdata(2);
    lcdWrdata(4);
    lcdWrdata(6);
    lcdWrdata(9);
    lcdWrdata(11);
    lcdWrdata(13);
    lcdWrdata(15);
    lcdWrdata(0);               //Green
    lcdWrdata(2);
    lcdWrdata(4);
    lcdWrdata(6);
    lcdWrdata(9);
    lcdWrdata(11);
    lcdWrdata(13);
    lcdWrdata(15);
    lcdWrdata(0);               //Blue
    lcdWrdata(6);
    lcdWrdata(10);
    lcdWrdata(15);
    
    //deselect controller
    selectLCD(FALSE);
    
    lcdContrast(56);
    
    lcdClrscr();
    delay_ms(1);
    
}

/*
 *  @brief			Funkcja odpowiedzialna za wyczyszczenie ekranu kolorem tla.
 *
 *  @Description	Funkcja odpowiedzialna za wyczyszczenie ekranu kolorem tla.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja ustawien rejestrow PINSEL0, SPI_SPCCR, SPI_SPCR
 */
void lcdClrscr(void)
{
    tU32 i;
    
    lcd_x = 0;
    lcd_y = 0;
    
    //select controller
    selectLCD(TRUE);
    
    lcdWindow1(255,255,128,128);
    lcdWrcmd(LCD_CMD_RAMWR);    //write memory
    
    for(i=0; i < 16900u; i++)
    {
        //lcdWrdata(0x00);
        lcdWrdata(bkgColor);
    }
    
    //deselect controller
    selectLCD(FALSE);
}

/*
 *  @brief			Funkcja wylaczajaca wyswietlacz LCD.
 *
 *  @Description	Funkcja wylaczajaca wyswietlacz LCD.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Przejscie wyswietlacza LCD w stan uspienia.
 */
void lcdOff(void)
{
    lcdClrscr();
    
    //select controller
    selectLCD(TRUE);
    
    lcdWrcmd(LCD_CMD_SLEEPIN);
    
    //deselect controller
    selectLCD(FALSE);
}

/*
 *  @brief			Funckja ustawiajaca kolor tla i kolor tekstu.
 *
 *  @Description	Funckja ustawiajaca kolor tla i kolor tekstu.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Ustawienie kontrastu wyswietlacza LCD.
 */
void lcdColor(tU8 bkg, tU8 text)
{
    bkgColor  = bkg;
    textColor = text;
}

/*
 *  @brief			Funckja odpowiadajaca za ustawienie odbicia lustrzanego wzdloz osi X oraz osi Y wyswietlacza LCD.
 *
 *  @Description	Funckja odpowiadajaca za ustawienie odbicia lustrzanego wzdloz osi X oraz osi Y wyswietlacza LCD.
 *
 *  @param 			[in]    mirrorX
 *                      Jesli TRUE ma nastapic odbicie lustrzone wzdluz osi X.
 *
 *  @param 			[in]    mirrorY
 *                      Jesli TRUE ma nastapic odbicie lustrzone wzdluz osi Y.
 *
 *  @returns  		nic
 *
 *  @side effects	Ustawienie kontrastu wyswietlacza LCD.
 */
void lcdMirror(tBool mirrorX, tBool mirrorY)
{
    tU16 data = 0x08;//(1 << 5) | (1 << 4) | (1 << 2) | (1 << 1);

    //Ustawienie odbica lustrzanego wzdluz osi X
    if (mirrorX == TRUE)
    {
        data |= 0x80;
    }

    //Ustawienie odbica lustrzanego wzdluz osi Y
    if (mirrorY == TRUE)
    {
        data |= 0x40;
    }

    selectLCD(TRUE);
    //Wysłanie do LCD odpowiedniej komendy powiazanej z ustawianiem odbicia lustrzanego.
    lcdWrcmd(LCD_CMD_MADCTL);
    //Przesłanie danych do komendy.
    lcdWrdata(data);
    selectLCD(FALSE);
}

/*
 *  @brief			Funckja ustawiajaca kontrast.
 *
 *  @Description	Funkcja ustawiajaca kontrast wy�wietlacza LCD.
 *
 *  @param 			[in]    contrast
 *                      Wartosc calkowita z przedzialu 0 - 127 okreslajaca kontrast.
 *
 *  @returns  		nic
 *
 *  @side effects	Ustawienie kontrastu wyswietlacza LCD.
 */
static void lcdContrast(tU8 cont) //vary between 0 - 127
{
    //select controller
    selectLCD(TRUE);
    
    //set contrast cmd.
    lcdWrcmd(LCD_CMD_SETCON);
    lcdWrdata(cont);
    
    //deselect controller
    selectLCD(FALSE);
}

/*
 *  @brief			Funkcja rysujaca prostokat.
 *
 *  @Description	Funkcja rysujaca prostokat o okreslonym kolorze.
 *
 *  @param 			[in]    x
 *                      Pozycja x rysowanego prostokata.
 *
 *  @param 			[in]    y
 *                      Pozycja y rysowanego prostokata.
 *
 *  @param 			[in]    xLen
 *                      Szerokosc rysowanego prostokata.
 *
 *  @param 			[in]    yLen
 *                      Wysokosc rysowanego prostokata.
 *
 *  @param 			[in]    color
 *                      Kolor rysowanego prostokata.
 *
 *  @returns  		nic
 *
 *  @side effects	Przeslanie danych przez magistrale.
 */
void lcdRect(tU8 x, tU8 y, tU8 xLen, tU8 yLen, tU8 color)
{
    tU32 i;
    tU32 len;
    
    //select controller
    selectLCD(TRUE);   
    
    lcdWindow1(x, y, x + xLen - 1u, y + yLen - 1u);
    
    lcdWrcmd(LCD_CMD_RAMWR);    //write memory
    
    len = (tU32) xLen * yLen;
    for (i = 0; i < len; i++)
    {
        lcdWrdata(color);
    }
    
    //deselect controller
    selectLCD(FALSE);
}

/*
 *  @brief			Funkcja rysujaca prostokat o okreslonym wypelnieniu i okreslonych kolorach krawedzi.
 *
 *  @Description	Funkcja rysujaca prostokat o okreslonym wypelnieniu i okreslonych kolorach krawedzi.
 *
 *  @param 			[in]    x
 *                      Pozycja x rysowanego prostokata.
 *
 *  @param 			[in]    y
 *                      Pozycja y rysowanego prostokata.
 *
 *  @param 			[in]    xLen
 *                      Szerokosc rysowanego prostokata.
 *
 *  @param 			[in]    yLen
 *                      Wysokosc rysowanego prostokata.
 *
 *  @param 			[in]    color
 *                      Kolor rysowanego prostokata.
 *
 *  @returns  		nic
 *
 *  @side effects	Przeslanie danych przez magistrale.
 */
void lcdRectBrd(tU8 x, tU8 y, tU8 xLen, tU8 yLen, tU8 color1, tU8 color2, tU8 color3)
{
    tU8 i;
    tU8 j;
    
    //select controller
    selectLCD(TRUE);
    
    lcdWindow1(x, y, x + xLen - 1u, y + yLen - 1u);
    
    lcdWrcmd(LCD_CMD_RAMWR);    //write memory
    
    for (i = 0; i < xLen; i++)
    {
        lcdWrdata(color2);
    }
    for(j = 1; j < (yLen - 2u); j++)
    {
        lcdWrdata(color2);
        for (i = 0; i < (xLen - 2u); i++)
        {
            lcdWrdata(color1);
        }
        lcdWrdata(color3);
    }
    for (i = 0; i < xLen; i++)
    {
        lcdWrdata(color3);
    }
    
    //deselect controller
    selectLCD(FALSE);
}


/*****************************************************************************
 *
 * Description:
 *    Draw rectangular area from bitmap. Specify xy-position and xy-length.
 *    Compressed format is supported.
 *
 *    In uncompressed color mode, pData points to an area of xLen * yLen
 *    bytes with the icon.
 *    In compressed mode the escapeChar is used to denote that the next
 *    two bytes contain a length and a color (run length encoding)
 *    Note that is is still possible to specify the color value that
 *    equals the escape value in a compressed string.
 *
 ****************************************************************************/
void lcdIcon(tU8 x, tU8 y, tU8 xLen, tU8 yLen, tBool compressionOn, tU8 escapeChar, const tU8* pData)
{
    tU8  j;
    tU32 i;
    tU32 len;
    const tU8* wData = pData;
    
    //select controller
    selectLCD(TRUE);
    
    lcdWindow1(x, y, x + xLen - 1u, y + yLen - 1u);
    
    lcdWrcmd(LCD_CMD_RAMWR);    //write memory
    
    len = (tU32) xLen  * yLen;
    if (compressionOn == FALSE)
    {
        for (i = 0; i < len; i++)
        {
            lcdWrdata(*wData++);
        }
    }
    else
    {
        while (len > 0u)
        {
            if (*wData == escapeChar)
            {
                wData++;
                j = *wData;
                wData++;
                for (i = 0; i < j; i++)
                {
                    lcdWrdata(*wData);
                }
                wData++;
                len -= j;
            }
            else
            {
                lcdWrdata(*wData++);
                len--;
            }
        }
    }
    
    //deselect controller
    selectLCD(FALSE);
}

/*
 *  @brief			Funckja odpowiadajaca za narysowanie w wyznaczonym obszarze przekazanej mapy bitowej
 *
 *  @Description	Funckja odpowiadajaca za narysowanie w wyznaczonym obszarze przekazanej mapy bitowej
 *
 *  @param 			[in]    x
 *                      Pozycja x obszaru rysowanej mapy bitowej.
 *
 *  @param 			[in]    y
 *                      Pozycja y obszaru rysowanej mapy bitowej.
 *
 *  @param 			[in]    width
 *                      Szerokosc obszaru rysowanej mapy bitowej.
 *
 *  @param 			[in]    height
 *                      Wysokosc obszaru rysowanej mapy bitowej.
 *
 *  @param 			[in]    pData
 *                      Mapa bitowa w postaci tablicy kolorow.
 *
 *  @returns  		nic
 *
 *  @side effects	Przeslanie danych przez magistrale.
 */
void lcdDrawIcon(tU8 x, tU8 y, tU8 width, tU8 height, const tU8 * pData)
{
	//W��czenie wy�wietlacza LCD jako slave w transmisji z wykorzystanie SPI
	  selectLCD(TRUE);

	  //lcdWindow1(x, y, x + xLen - 1, y + yLen - 1);

	  //Ustawienie obszaru prostok�ta do narysowania
	  lcdWrcmd(LCD_CMD_CASET);
	  lcdWrdata(x);
	  lcdWrdata(x + width - 1u);

	  lcdWrcmd(LCD_CMD_PASET);
	  lcdWrdata(y);
	  lcdWrdata(y + height - 1u);

	  lcdWrcmd(LCD_CMD_RAMWR);    //write memory

	  //Wyznaczenie ilo�ci iteracji aby przes�a� wszystkie dane mapy bitowej
	  tU32 len = (tU32) width * height;
	  //P�tla przesy�aj�ca bajty danych kolor�w
	  tU32 i = 0;
	  for(; i < len; i++)
	  {
		//Przes�anie koloru
	    lcdWrdata(pData[i]);
	  }

	  //Wy��czenie wy�wietlacza LCD jako slave w transmisji z wykorzystanie SPI
	  selectLCD(FALSE);
}

/*
 *  @brief			Funckja odpowiedzialna za ustawienie pozycji XY.
 *
 *  @Description	Funckja odpowiedzialna za ustawienie pozycji XY (dodatkowo tworzy okno).
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void lcdGotoxy(tU8 x, tU8 y)
{
    lcd_x = x;
    lcd_y = y;
    lcdWindow(x, y, 129, 129);
}

/*
 *  @brief			Funckja odpowiedzialna za ustalenie obszaru wyświtlacza obecnie edytowanego
 *
 *  @Description	Funckja odpowiedzialna za ustalenie obszaru wyświtlacza obecnie edytowanego
 *
 *  @param 			[in]    xp
 *                      Początkowa pozycja x tworzonego okna.
 *
 *  @param 			[in]    yp
 *                      Początkowa pozycja y tworzonego okna.
 *
 *  @param 			[in]    xe
 *                      Koncowa pozycja x tworzonego okna.
 *
 *  @param 			[in]    ye
 *                      Koncowa pozycja y tworzonego okna.
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
static void lcdWindow(tU8 xp, tU8 yp, tU8 xe, tU8 ye)
{
    //select controller
    selectLCD(TRUE);
    
    lcdWindow1(xp, yp, xe, ye);
    
    //deselect controller
    selectLCD(FALSE);
}


/*****************************************************************************
 *
 * Description:
 *    Initialize LCD controller for a window (to write in).
 *    Set start xy-position and xy-length
 *    No select/deselect of LCD controller.
 *
 ****************************************************************************/
static void lcdWindow1(tU8 xp, tU8 yp, tU8 xe, tU8 ye)
{
    lcdWrcmd(LCD_CMD_CASET);    //set X
    lcdWrdata(xp + 2u);
    lcdWrdata(xe + 2u);
    
    lcdWrcmd(LCD_CMD_PASET);    //set Y
    lcdWrdata(yp + 2u);
    lcdWrdata(ye + 2u);
}

/*
 *  @brief			Funckja odpowiedzialna za przej�cie do nowej lini podczas pisania.
 *
 *  @Description	Funckja odpowiedzialna za przej�cie do nowej lini podczas pisania.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
static void lcdNewline(void)
{
    lcd_x   =  0u;
    lcd_y   += 14u;
    if (lcd_y >= 126u)
    {
        lcd_y = 126u;
    }
}

/*
 *  @brief			Funckja odpowiedzialna za wyswietlenie pojedynczego znaku.
 *
 *  @Description	Funckja odpowiedzialna za wyswietlenie pojedynczego znaku, na obecnej pozycji xy, po czym ktualizuje pozycje X.
 *
 *  @param 			[in]    data
 *                      Znak do wyswietlenia.
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
static void lcdData(tU8 data)
{
    tU8 fData = data;
    //select controller
    selectLCD(TRUE);
    
    if (fData <= 127u)
    {
        tU32 mapOffset;
        tU8  i;
        tU8  j;
        tU8  byteToShift;
        
        fData -= 30u;
        mapOffset = 14u * (tU32) fData;
        
        lcdWrcmd(LCD_CMD_CASET);
        lcdWrdata(lcd_x + 2u);
        lcdWrdata(lcd_x + 9u);
        lcdWrcmd(LCD_CMD_PASET);
        lcdWrdata(lcd_y + 2u);
        lcdWrdata(lcd_y + 15u);
        lcdWrcmd(LCD_CMD_RAMWR);
        
        for(i=0; i < 14u; i++)
        {
            byteToShift = charMap[mapOffset];
            mapOffset++;

            for(j=0; j < 8u; j++)
            {
                if ((byteToShift & 0x80u) != 0u)
                {
                    lcdWrdata(textColor);
                }
                else
                {
                    lcdWrdata(bkgColor);
                }
                byteToShift <<= 1;
            }
        }
    }
    
    //deselect controller
    selectLCD(FALSE);
    
    lcd_x += 8u;
}

/*
 *  @brief			Funckja odpowiedzialna za wy�wietlenie pojedynczego znaku.
 *
 *  @Description	Funckja odpowiedzialna za wy�wietlenie pojedynczego znaku, na obecnej pozycji xy, po czym nast�puje jej aktualizacja.
 *
 *  @param 			[in]    data
 *                      Znak do wyswietlenia.
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void lcdPutchar(tU8 data)
{
    static tBool setcolmark;

    if (data == u'\n')
    {
        lcdNewline();
    }
    else if (data != u'\r')
    {
        if (setcolmark == TRUE)
        {
            textColor = data;
            setcolmark = FALSE;
        }
        else if (data == 0xffu)
        {
            setcolmark = TRUE;
        }
        else if (lcd_x <= 124u)
        {
            lcdData(data);
        }
        /* nic nie rob */
        else
        {
            ;
        }
    }
    /* nic nie rob */
    else
    {
        ;
    }
}

/*
 *  @brief			Funckja odpowiedzialna za wyswietlenie lancucha znakow.
 *
 *  @Description	Funckja odpowiedzialna za wyswietlenie lancucha znakow.
 *
 *  @param 			[in]    s
 *                      Lancuch znakow do wy�wietlenia.
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void lcdPuts(const char* s)
{
    const char* str = s;
    while (*str != '\0')
    {
        lcdPutchar(*str++);
    }
}

/*
 *  @brief			Funckja odpowiedzialna za przeslanie polecenia do wykonania przez LCD z wykorzystanie magistrali SPI.
 *
 *  @Description	Funckja odpowiedzialna za przeslanie polecenia do wykonania przez LCD z wykorzystanie magistrali SPI.
 *
 *  @param 			[in]    cont
 *                      Kod polecenia, ktore ma zostac wykonane przez LCD.
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void lcdWrcmd(tU8 data)
{
    sendToLCD(0, data);
}

/*
 *  @brief			Funckja odpowiedzialna za przeslanie danych polecenia z wykorzystanie magistrali SPI.
 *
 *  @Description	Funckja odpowiedzialna za przeslanie danych polecenia z wykorzystanie magistrali SPI.
 *
 *  @param 			[in]    data
 *                      Dane potrzebne do wykonania polecenia
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void lcdWrdata(tU8 data)
{
    sendToLCD(1, data);
}
