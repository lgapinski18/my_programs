/*
* @Descritpion: Zawariera zbior definicji funkcji uzywanych w programie
*				(nie sie sklasyfikowane wedlug konkretnego przeznaczenia)
*/

#include "general.h"
#include <lpc2xxx.h>
#include <config.h>
#include <printf_P.h>
#include "functionals.h"
#include "dac.h"
#include "timer.h"
#include "VIC.h"
#include "irq/irq_handler.h"


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
AccelerationDirection getAccelerationDirection(void) {
	/* Zmienna, ktorej wartosc bedzie stanowic ustalony kierunek przyspieszenia */
	AccelerationDirection direction;

	tU16 accelX = getAnalogueInputAdc1(ACCEL_X_CHANEL_NO);
	tU16 accelY = getAnalogueInputAdc1(ACCEL_Y_CHANEL_NO);
	tU16 accelZ = getAnalogueInputAdc0(ACCEL_Z_CHANEL_NO);

	/* Wartosc referencyjna reprezentujaca wartosc, powyzej ktorej dla danego kanalu można stwierdzic, iz przyspieszenie dziala bardzie (w strone dodatnia) */
	const tU16 PC = 700;//0x179; //0x2EB
	/* To samo co powyzej tylko dla osi X */
	const tU16 PCX = 650;
	/* Wartosc referencyjna reprezentujaca wartosc, pownizej ktorej dla danego kanalu można stwierdzic, iz przyspieszenie dziala bardzie (w strone ujemna) */
	const tU16 NC = 400;//0xBA; //0x174
	/* To samo co powyzej tylko dla osi X */
	const tU16 NCX = 575;

	/*	Sprawdzenie przyblizonego kierunku przyspieszenia na podstawie porownania wartosci odczytu przyspieszenia
	*	dla kolejnych osi z wartoscia referencyjna�.
	*/
	if (accelZ > PC)
	{
		direction = BACK;
	}
	else if (accelZ < NC)
	{
		direction = FRONT;
	}
	else if (accelY > PC)
	{
		direction = LEFT;
	}
	else if (accelY < NC)
	{
		direction = RIGHT;
	}
	else if(accelX > PCX)
	{
		direction = DOWN;
	}
	else if (accelX <= NCX)
	{
		direction = UP;
	}
	else 
	{
		direction = BACK;
	}

	return direction;
}

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
void playBackgroundSoundtrack(void)
{
	initDac();

	/* Zainicjowanie VIC przerwania Timer'a #1 */
	VICIntSelect &= ~TIMER_1_IRQ;           	/*Przerwanie od Timera #1 przypisane do IRQ (nie do FIQ)*/
	VICVectAddr5  = (tU32)IRQ_PutNextSample;    /* adres procedury przerwania */
	VICVectCntl5  = VIC_ENABLE_SLOT | TIMER_1_IRQ_NO;
	VICIntEnable  = TIMER_1_IRQ;            	/* Przypisanie i odblokowanie slotu w VIC od Timer'a #1 */

	T1TCR = TIMER_RESET;                    	/* Zatrzymanie i zresetowanie Timer'a #1*/
	T1PR  = 0;                              	/* Ustawienie preskalera na 0, aby co kazde PCLK incrementowany byc T1TC */
	T1MR0 = ((tU64)TimeToNextSample) * ((tU64)PERIPHERAL_CLOCK) / 1000000UL; /* Ustawienie wartosci dopasowania, aby czas jaki uplynie wynosil temu reprezentowanemu przez TimeToNextSample */
	T1IR  = TIMER_ALL_INT;                  	/* Zresetowanie flagi przerwa� */
	T1MCR = MR0_I | MR0_R;          			/* Ustawienie,  aby na dopasowanie MR0 do TC bylo generowane przerwanie (ustawienie bitu nr 0) oraz aby zawartosc rejestru TC zostala zresetowana (ustawienie bitu 1.) */

	T1TCR = TIMER_RUN;                      	/* Uruchomienie Timer'a #1*/
}

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
void delay_ms(tU16 delayIn_ms)
{
	/* Przygotowanie Timer'a #0 do realizacji opoznienia */
	T0TCR = 0x02;          	/* Wstrzymanie i zresetowanie timera */
	T0PR = 0x00;        	/* Ustawienie wartosci rejestru Preskaler Register na 0, tym samym Timer Counter bedzie inkrementowany na kazde PCLK */
	T0MR0 = delayIn_ms * (PERIPHERAL_CLOCK / 1000); /* Ustawienio wartosci rejestru MR0 na ta odpowiadajaca wartosci delayIn_ms tak aby po uplywie tego czasu nastapla odpowiednia akcja */
	T0IR = 0xff;          	/* Zresetowanie flag przerwan */
	/*
	* Okreslenie akcji jaka ma nastapic gdy wartosc rejestru TC bedzie rowna tej w MR0.
	* Akcja to zatrzymanie timer'a na dopasowanie TC i MR0. Skutkujaca ustawieniem
	* najmlodszego bitu rejestru TCR Timera #0 na 0.
	*/
	T0MCR = 0x04;
	T0TCR = 0x01;          /* Uruchomienie Timer'a #0 */

	/* Odczekanie uplywu czasu */
	while ((T0TCR & 1) != 0)
	{
		;
	}
}
