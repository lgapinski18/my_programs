/*
 *	@Description:	Zawiera zbior definicji funkcji zwiazanych obsluga PWM
 */
#include "general.h"
#include <lpc2xxx.h>
#include <config.h>
#include "pwm.h"
#include "functionals.h"


/*
 *  @brief			Funkcja wykonujaca mrugniecie dioda Green LED przekazana ilosc razy.
 *
 *  @Description	Funkcja wykonuje mrugniecie dioda w liczbie zgodna z ta przekazana przez
 * 					parametr. Mrugniecia trwaja 250ms i sa w odstepach 250ms. Nalezy zauwazyc,
 * 					iz ta dioda wykorzystuje ten sam pin (P0.21) (bity 11. i 10. ustawione na 01,
 * 					aby wlaczyc funkcje sterowania dioda) co odczyt przyspieszenia wzdluz
 * 					osi X z akcelerometru. Tym samym nie wolno, uzywac tych polecen w tym samym
 * 					czasie. Sterowanie dioda Green LED odbywa sie za pomoca PWM. Funkcja na
 * 					zakonczenie pracy przywraca zawartosc rejestru PINSEL1 na te z przed wywolania.
 *
 *  @param 			[in]	numberOfBlinks
 *						liczba mrugniec jakie ma wykonac dioda Green LED
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestru PINSEL1 (bity 11., 10.)
 */
void
blinkGreenLed(tU8 numberOfBlinks) 
{
	/* Jedynka 32-bitowa */
	const tU32 one32 = 1u;

	if (numberOfBlinks != 0u) /* Sprawdzenie, czy numberOfBlinks jest zero. Jezeli tak to zakoncz dzialanie funkcji. */
	{
		/* Czas trwania mrugniecia (swiecenia sie diody) w milisekundach */
		tU16 blinkDurationTime = 250;

		/* Czas trwania przerwy miedzy kolejnym zapaleniem sie diody w milisekundach */
		tU16 timeToNextBlink = 250;

		/*	Inicjalizacja wyjscia PWM5 (P0.21) */
		/*	Ustawienie funkcji pinu P0.21 na wyjscie PWM5.
		*	Umozliwia sterowanie dioda Green LED.
		*/
		PINSEL1 &= ~((one32 << 10) | (one32 << 11));
		PINSEL1 |= (one32 << 10);

		/* Zatrzymanie i zresetowanie Timer Counter'a dla PWM */
		PWMTCR = 0x02;

		/* Ustawienie wartosci rejestru Prescaler Counter, aby co kazde PCLK zawartosc rejestru PWMTC byla inkrementowana */
		PWMPC = 0;
		/* Ustawienie aby kanal 5 PWM dzialal w trybie double edge */
		PWMPCR |= (one32 << 5);
		PWMPCR |= (one32 << 13);

		/* Ustawienie funkcji Match Register 0, aby na dopasowanie zresetowany zostal PWMTC (bit 1. ustawiony na 1, na wszelki wypadek pozostale bity zwiazane z zarzadzanie MR0 (0. i 2.) zerowane) */
		PWMMCR &= ~(0x7); 	/* Wyzerowanie bitow w rejestrze PWMMRC zwiazanych z MR0 */
		PWMMCR |= 0x2; 		/* Wybranie opcji resetowania PWMTC na dopasowanie */
		/* Ustawienie wartosci dla Match Register 0 na wartosc reprezentujaca sume blinkDurationTime i timeToNextBlink */
		PWMMR0 = (blinkDurationTime + timeToNextBlink) * (CORE_FREQ / PBSD / 1000);

		/* Ustawienie funkcji Match Register 5 na zadna (zadne operacja zwiazana z rejestrami licznikow maja nie zostac wykonane) */
		PWMMCR &= ~((one32 << 17) | (one32 << 16) | (one32 << 15));
		/* Ustawienie wartosci dla Match Register 5 na wartosc 1, odrazu ma sie wykonac dopasowanie, a dioda tym samym zaswiecic */
		PWMMR5 = (one32) * (CORE_FREQ / PBSD / 1000);
		//PWMMR5 = (blinkDurationTime) * (CORE_FREQ / PBSD / 1000);

		/* Ustawienie funkcji Match Register 4 na zadna (zadne operacja zwiazana z rejestrami licznikow maja nie zostac wykonane) */
		PWMMCR &= ~((one32 << 14) | (one32 << 13) | (one32 << 12));
		/* Ustawienie wartosci dla Match Register 4 na wartosc blinkDurationTime, dioda ma zgasnac po uplywie tego czasu */
		PWMMR4 = (blinkDurationTime) * (CORE_FREQ / PBSD / 1000);

		/* Rozpoczecie dzialania PWM */
		PWMTCR = 1;

		/* Wstrzymanie dzialania do momentu wykonania odpowiedniej liczby mrugniec */
		delay_ms((((tU16)numberOfBlinks) * blinkDurationTime) + ((((tU16)numberOfBlinks) - 1u) * timeToNextBlink) + (blinkDurationTime / 2u));

		/* Zatrzymanie i zresetowanie Timer Counter'a dla PWM */
		PWMTCR = 0x02;
		PWMPCR &= ~(one32 << 13);

		/* Przywrocenie zawartosci rejestru PINSEL1 na bitach 11. i 10. na wartosci z przed wywolania funkcji. */
		PINSEL1 &= ~((one32 << 10) | (one32 << 11));
		PINSEL1 |= (one32 << 11);
	}
}
