/*
 * ledi2c.c
 *
 *  Created on: 2023-03-29
 *      Author: embedded
 */

/* Includes */
//#include "functionals.h"
#include "i2c.h"
#include "ledi2c.h"
#include "functionals.h"

/*
 *  @brief			Funkcja odpowiadajaca za odpowienia aktywacje diod LED, aby uzyskac efekt wizualny na ukonczenie gry.
 *
 *  @Description	Funkcja odpowiadajaca za odpowienia aktywacje diod LED, aby uzyskac efekt wizualny na ukonczenie gry.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestr�w LS0, LS1, LS2, LS3
 */
void winLEDMovement(void)
{
	//Zmienna sluzaca do iteracji w petli wysylajacej dane
	tU8 i = 0u;

	//Stala przechowujaca rozmiar tablicy przechowujacej pakiet danych do wys�ania
	const tU8 dataLen = 5;
	//Zmienna przechowujaca pakiet danych do wyslania magistrale I2C
	tU8 dataToSend[dataLen]; // = {0, 0, 0, 0, 0};

	tU8 status = I2C_CODE_OK;
	tU16 iter = 1u;
	for (; (iter != 0u) && (status == I2C_CODE_OK); iter <<= 2u)
	{
		//Rozpoczecie transmisji z wykozystaniem I2C
		(void) i2cStart();
		//Opoznienie konieczne aby transmisja zadziała
		delay_ms(10);
		//Wysyalanie adresu docelowego slave'a w raz z kierunkiem operacji czyli zapisem
		status = i2cPutChar(0xC0);

		//Opoznienie konieczne aby transmisja zadziała
		delay_ms(10);

		//Rozpoczecie wpisywania informacji od rejestru LED 0 - 3 Selector
		dataToSend[0] = (LEDI2C_AI | LEDI2C_LS0);

		//Stan diod LED dla rejestru LS0
		dataToSend[1] = 0x00FFu & iter;
		//Stan diod LED dla rejestru LS1
		dataToSend[2] = (0xFF00u & iter) >> 8;
		//Stan diod LED dla rejestru LS2
		dataToSend[3] = 0x00FFu & iter;
		//Stan diod LED dla rejestru LS3
		dataToSend[4] = (0xFF00u & iter) >> 8;



		//Petla wysylajaca dane
		//Warunek stopu: dotarcie do konca tablicy, badz wystapienie bledu
		//bedacego kodem roznym od I2C_CODE_OK
		for(i = 0; (i < dataLen) && (status == I2C_CODE_OK); i++)
		{
			status = i2cPutChar(dataToSend[i]);
			//Opoznienie konieczne aby transmisja zadziała
			delay_ms(10);
		}

		//Zakonczenie transmisji z wykorzystaniem magistrali I2C
		(void) i2cStop();

		//Opoznienie aby bylo widac efekt na diodach
		delay_ms(500);
	}


	//Rozpoczecie transmisji z wykozystaniem I2C
	(void) i2cStart();
	//Opoznienie konieczne aby transmisja zadziała
	delay_ms(10);
	//Wysyalanie adresu docelowego slave'a w raz z kierunkiem operacji czyli zapisem
	status = i2cPutChar(0xC0);

	//Opoznienie konieczne aby transmisja zadziała
	delay_ms(10);

	//Rozpoczecie wpisywania informacji od rejestru LED 0 - 3 Selector
	status = i2cPutChar(LEDI2C_AI | LEDI2C_LS0);

	//Opoznienie konieczne aby transmisja zadziała
	delay_ms(10);

	status = I2C_CODE_OK;
	//Petla gaszaca wszystkie diody LED
	for(i = 1; (i < dataLen) && (status == I2C_CODE_OK); i++)
	{
		status = i2cPutChar(0x00);
		//Opoznienie konieczne aby transmisja zadziała
		delay_ms(10);
	}

	//Zakonczenie transmisji z wykorzystaniem magistrali I2C
	(void) i2cStop();

}
