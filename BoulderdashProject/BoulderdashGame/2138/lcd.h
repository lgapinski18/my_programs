/******************************************************************************
 *
 * Copyright:
 *    (C) 2006 Embedded Artists AB
 *
 * File:
 *    lcd.h
 *
 * Description:
 *    Expose public functions related to LCD functionality.
 *
 *****************************************************************************/
#ifndef LCD_H
#define LCD_H

 /* Declarations */

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
void lcdInit(void);

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
void lcdOff(void);

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
void lcdClrscr(void);

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
void lcdPutchar(tU8 data);

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
void lcdPuts(const char* s);

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
void lcdMirror(tBool mirrorX, tBool mirrorY);

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
void lcdGotoxy(tU8 x, tU8 y);

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
void lcdColor(tU8 bkg, tU8 text);

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
void lcdRect(tU8 x, tU8 y, tU8 xLen, tU8 yLen, tU8 color);

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
void lcdRectBrd(tU8 x, tU8 y, tU8 xLen, tU8 yLen, tU8 color1, tU8 color2, tU8 color3);

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
void lcdDrawIcon(tU8 x, tU8 y, tU8 xLen, tU8 yLen, const tU8* pData);

void lcdIcon(tU8 x, tU8 y, tU8 xLen, tU8 yLen, tBool compressionOn, tU8 escapeChar, const tU8* pData);

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
void lcdWrdata(tU8 data);

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
void lcdWrcmd(tU8 cmd);
#endif
