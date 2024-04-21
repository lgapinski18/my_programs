/*
* @Descritpion: Zawariera zbior deklaracji funkcji zwiazanych
*				z wykorzystaniem przetwornika cyfrowo-analogowego
*/

#ifndef DAC_H
#define DAC_H

#include "general.h"

/* Definicje */
#define TimeToNextSample 700 /* Czas w milisekunda jaki ma uplynac do momentu wprowadzenia nastepnej probki, wynika z czestosci probkowania utworu. */


/* Deklaracje funkcji */

/*
 *  @brief			Funkcja incijalizujaca przetwornik cyfrowo-analogowy, aby moc odtwarzac
 *					utwor muzyczny.
 *
 *  @Description	Funckja ustawia odpowiednia wartosc w rejestrze PINSEL1 dla pinu P0.25,
 *					aby aktywowac funkcje wyjscia anologowego przetwornika cyfrowo-analogowego
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestru PINSEL1
 */
void initDac(void);

/*
 *  @brief			Funkcja wywolywana okresowo, ktora zapisuje kolejna wartosc probki
 * 					utworu odwarzanego do rejestru przetwornika Cyfrowo-Analogowego.
 *
 *  @Description	Funkcja ta jest wywolywana okresowo na skutek przerwan zglaszany przez
 * 					Timer#1. Co kazde swoje wywolanie wprowadza do rejestru przetwornika DAC
 * 					kolejna 10-bitowa wartosc probki odwtwarzanego utworu.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja zawartosci rejestru DACR
 */
void putNextSample(void);

#endif
