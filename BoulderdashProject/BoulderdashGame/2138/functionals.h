/*
 *	@Descritpion:	Zawariera zbior deklaracji funkcji uzywanych w programie
 *					(nie sa sklasyfikowane wedlug konkretnego przeznaczenia)
 */

#ifndef FUNCTIONALS_H
#define FUNCTIONALS_H

#include "general.h"
#include "adc.h"

/* Definicje */

/* Lista enumerowana przechowujaca informacje o ogolnym kierunku przyspieszenia */
typedef enum { UP = 0, DOWN = 1, RIGHT = 2, LEFT = 3, FRONT = 4, BACK = 5 } AccelerationDirection;

/* Deklaracje funckji */

/*
 *  @brief			Funkcja informujaca o kierunku dzialania przypieszenia wzgledem urzadzenia.
 *
 *  @Description	Funkcja ta realizuje odczyt za pomoca przetwornikow analogowo-cyfrowych wartosci przyspieszenia
 * 					dla osi X, Y, Z i odpowiednio je analizuje, aby wyznaczyc kierunek wypadkowego przyspieszenia
 * 					(uwzgledniajac grawitacyjne).
 *
 *  @param 			brak
 *
 *  @returns  		informacja o kierunku dzialania przyspieszenia w formie jednej z wartosci listy wyliczeniowej AccelerationDirection
 *
 *  @side effects	Modyfikacja rejestru ADCR przetwornika analogowo-cyfrowego ADC0 (wykorzystanie funkcji getAnalogueInputAdc0)
 * 					Modyfikacja rejestru AD1CR przetwornika analogowo-cyfrowego ADC1 (wykorzystanie funkcji getAnalogueInputAdc1)
 */
AccelerationDirection getAccelerationDirection(void);


/*
 *  @brief			Funkcja, ktora wlacza odtwarzanie melodi w tle do gry.
 *
 *  @Description	Funkcja realizuje wlaczenie odtwarzania utworu muzycznego w tle, po przez
 * 					ustawianienie dzialania Timer'a #1 aby co okreslony interwal czasu (225 ms)
 * 					generowal przerwanie. Funkcja ta takze ustawia odpowiednia funkcje (IRQ_PutNextSample)
 * 					stanowiaca obsluge zglaszanego przerwania.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestru VICIntSelect
 * 					Modyfikacja rejestru VICVectAddr3
 * 					Modyfikacja rejestru VICVectCntl3
 * 					Modyfikacja rejestru VICIntEnable
 * 					Modyfikacja rejestrow Timer'a #1 (T1TCR, T1PR, T1MR0, T1IR, T1MCR)
 */
void playBackgroundSoundtrack(void);


/*
 *  @brief			Funkcja sluzaca do wstrzymania dzialania programu na okreslona liczbe milisekund.
 *
 *  @Description	Funkcja realizuje opoznienie dzialania programu o przekazana liczbe milisekund.
 * 					Do wykonania tej operacji wykorzystywany jest Timer #0
 *
 *  @param 			[in]	delayIn_ms
 *						liczba milisekund na ile ma byc wstrzymany program
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestrow Timer'a #0 (T0TCR, T0PR, T0MR0, T0IR, T0MCR)
 */
void delay_ms(tU16 delayIn_ms);

#endif
