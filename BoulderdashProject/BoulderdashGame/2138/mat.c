/*
* @Descritpion: Zawariera zbior definicji funkcji zwiazanych z ustawianiem
*				stanu diod Red LED i Blue LED za pomoca External Match
*/

#include "general.h"
#include <printf_P.h>
#include <lpc2xxx.h>
#include <config.h>
#include "mat.h"

/* Jedynka 32-bitowa */
static const tU32 one32 = 1u;

/*
 *  @brief			Funkcja incijalizujaca mozliwosc kontroli diody Red LED.
 *
 *  @Description	Funckja ustawia odpowiednia warto�sc w rejestrze PINSEL1 dla pinu P0.17,
 * 					aby stanowil wyjscie MAT1.2 zwiazany ze stanem bitu dla External Match 2
 * 					dla Timera #1.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestru PINSEL1
 */
void initRedDiod(void)
{
	/*	Ustawienie funkcji pinu P0.17 na wyjscie MAT1.2 reprezentujacego stan bitu dla EM2. */
	PINSEL1 |= (one32 << 3) | (one32 << 2);
	setRedDiodState(FALSE);
}

/*
 *  @brief			Funkcja ustawiajaca stan diody Red LED
 *
 *  @Description	Funkcja zapala albo gasi diode Red LED na podstawie przekazanej wartosci logicznej
 * 					po przez ustawienie odpowiednio bitu nr 2 w rejestrze External Match Register dla
 * 					Timera #1 (T1EMR).
 *
 *  @param 			[in]	toSet
 *						wartosc logiczna informujaca o tym czy dioda ma byc zapalona
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestru T1EMR
 */
void setRedDiodState(tBool toSet)
{
	if (toSet == TRUE) {
		T1EMR &= ~(one32 << 2);
	}
	else
	{
		T1EMR |= one32 << 2;
	}
}

/*
 *  @brief			Funkcja incijalizujaca mozliwosc kontroli diody Blue LED.
 *
 *  @Description	Funckja ustawia odpowiednia wartosc w rejestrze PINSEL1 dla pinu P0.18,
 * 					aby stanowil wyjscie MAT1.3 zwiazany ze stanem bitu dla External Match 3
 * 					dla Timera #1.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestru PINSEL1
 */
void initBlueDiod(void)
{
	/*	Ustawienie funkcji pinu P0.17 na wyjscie MAT1.2 reprezentujacego stan bitu dla EM2. */
	PINSEL1 |= (one32 << 5) | (one32 << 4);
	setBlueDiodState(FALSE);
}

/*
 *  @brief			Funkcja ustawiajaca stan diody Blue LED
 *
 *  @Description	Funkcja zapala albo gasi diode Blue LED na podstawie przekazanej wartosci logicznej
 * 					po przez ustawienie odpowiednio bitu nr 3 w rejestrze External Match Register dla
 * 					Timera #1 (T1EMR).
 *
 *  @param 			[in]	toSet
 *						wartosc logiczna informujaca o tym czy dioda ma byc zapalona
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestru T1EMR
 */
void setBlueDiodState(tBool toSet)
{
	if (toSet == TRUE) {
		T1EMR &= ~(one32 << 3);
	}
	else
	{
		T1EMR |= one32 << 3;
	}
}
