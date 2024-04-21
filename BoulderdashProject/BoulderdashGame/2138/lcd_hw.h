/******************************************************************************
 *
 * Copyright:
 *    (C) 2007 Embedded Artists AB
 *
 * File:
 *    pca9532.h
 *
 * Description:
 *    Expose hardware specific routines
 *
 *****************************************************************************/
#ifndef PCA9532_H
#define PCA9532_H

/* Includes */
#include "general.h"
#include <lpc2xxx.h>


 /* Typedefs and defines */
#define LCD_CS     0x00000080
#define LCD_CLK    0x00000010
#define LCD_MOSI   0x00000040


 /* Functions declarations */

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
void sendToLCD(tU8 firstBit, tU8 data);

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
void initSpiForLcd(void);

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
void selectLCD(tBool select);

#endif
