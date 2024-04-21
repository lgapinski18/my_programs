/******************************************************************************
 *
 * Copyright:
 *    (C) 2006 Embedded Artists AB
 *
 * File:
 *    pca9532.c
 *
 * Description:
 *    Implements hardware specific routines
 *
 *****************************************************************************/

 /* Includes */
//#include "../pre_emptive_os/api/osapi.h"
//#include "../pre_emptive_os/api/general.h"
#include <lpc2xxx.h>
#include "lcd_hw.h"
#include "general.h"

/*
 *  @brief			Funckja sluzaca do transmisji danych do LCD.
 *
 *  @Description	Funkcja transmituj�ca dane do LCD, z oczekiwaniem na zako�czenie poprzedniej transmisji.
 *
 *  @param 			[in]    data
 *                      Pakiet danych do wyslania do LCD.
 *
 *  @returns  		nic
 *
 *  @side effects	 Modyfikacja ustawie� rejestrow PINSEL0, SPI_SPCCR, SPI_SPCR, IODIR
 */
void sendToLCD(tU8 firstBit, tU8 data)
{
    //Wylaczenie SPI
    IOCLR = LCD_CLK;
    PINSEL0 &= 0xffffc0ffU;
    
    if (firstBit == (tU8) 1)
    {
        IOSET = LCD_MOSI;   //set MOSI
    }
    else
    {
        IOCLR = LCD_MOSI;   //reset MOSI
    }
    
    tU16 dataToSend = 0;
    if (firstBit == (tU8) 1)
    {
        dataToSend = ((tU16) data) & ((tU16) ~0x0100u);
    }
    else
    {
        dataToSend = ((tU16) data) | ((tU16) 0x0100);
    }
    
    //Set clock high
    IOSET = LCD_CLK;
    
    //Set clock low
    IOCLR = LCD_CLK;
    
    //Ponowna inicjalizacja rejetr�w
    SPI_SPCCR = 0x08;    
    SPI_SPCR  = 0x20;
    
    //Ponowna konfiguracja PINSELu
    PINSEL0 |= 0x00001500;
    
    //Przeslanie bajtu informacji
    SPI_SPDR = dataToSend;
    
    //P�tla oczekuj�ca na zako�czenie transmisji
    while((SPI_SPSR & 0x80) == 0)
    {
      ;
    }
}

/*
 *  @brief			Funckja inicjalizujaca interfejs SPI w celu umo�liwienia korzystania z LCD.
 *
 *  @Description	Funkcja ustawiajaca odpowiednie wartosci na rejestrach PINSEL0 oraz rejestrach magistrali SPI
 *                  w celu poprawnego dzialania wyswietlacza LCD
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	 Modyfikacja ustawie� rejestrow PINSEL0, SPI_SPCCR, SPI_SPCR, IODIR
 */
void initSpiForLcd(void)
{
      //make SPI slave chip select an output and set signal high
      IODIR |= (LCD_CS | LCD_CLK | LCD_MOSI);
      
      //Wyłącza slave'a
      selectLCD(FALSE);
    
      //Ustawienie PINSELU to jest:
      //Ustawienie pinu P0.4 na funkcje SCK0
      //Ustawienie pinu P0.5 na funckje MISO0
      //Ustawienie pinu P0.6 na funckje MOSI0
      PINSEL0 |= 0x00001500;
      
      //Ustawienie odpowiednich wartosci rejestrow
      
      //Ustawienie wartosci rejestru Clock Counter
      SPI_SPCCR = 0x08;    
      //W��czenie z wykorzystaniem rejestru kontroli trynu master
      SPI_SPCR  = 0x20;
}

/*
 *  @brief			Funkcja wlaczajaca i wylaczajaca wyswietlacz LCD jako slave w transmisji przez magistale SPI.
 *
 *  @Description	Funkcja wlaczajaca i wylaczajaca wyswietlacz LCD jako slave w transmisji przez magistale SPI
 *                  po przez ustawienie stanu LOW na pinie P0.7 SSEL w celu wybrania LCD jako cel transmisji, 
 *                  w przeciwnym wypadku wylacza go z transmisji.
 *
 *  @param 			[in]    select
 *                      TRUE okresla, ze LCD ma byc celem transmisji przez SPI, FALSE okresla, ze nim ma nie byc.
 *
 *  @returns  		nic
 *
 *  @side effects	Ustawienie pinu P0.7 SSEL na stan wysoki badz niski
 */
void
selectLCD(tBool select)
{
    if (TRUE == select)
    {
        IOCLR = LCD_CS;
    }
    else
    {
        IOSET = LCD_CS;
    }
}
