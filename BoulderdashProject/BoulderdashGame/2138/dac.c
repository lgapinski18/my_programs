/*
 *	@Descritpion:	Zawariera zbior deklaracji funkcji zwiazanych
 *					z wykorzystaniem przetwornika cyfrowo-analogowego
 */

#include "general.h"
#include <printf_P.h>
#include <lpc2xxx.h>
#include <config.h>
#include "dac.h"
#include "dac_reasources.h"

/* Tablica probek do odtworzenia */
extern tU8 soundtrack[numberOfSamples];

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
void initDac(void)
{
	/* Jedynka 32-bitowa */
	const tU32 one32 = 1u;

	/*	Ustawienie funkcji pinu P0.25 na wyjscie analogowe przetwornika cyfrowo-analogowego. */
	PINSEL1 &= ~((one32 << 19) | (one32 << 18));
	PINSEL1 |= (one32 << 19);

	DACR &= ~(one32 << 16); /* Ustawienie BIAS na 0, aby settling time wyniosl 1us */
}


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
void putNextSample(void)
{
	static tU32 sampleNumberToSend = 0; /* Statyczna zmienna przechowujaca informacje o numerze probki do odtworzenia */

	/* W prowadzenie do rejestru przetwornika DA (na bity 15.-6.) zawartosc 8-bitowej probki, ktora ma przetworzyc */
	DACR = (0xFFFF003FU & DACR) | (((tU32)soundtrack[sampleNumberToSend]) << 6);

	sampleNumberToSend++;
	if(sampleNumberToSend >= (tU32)numberOfSamples)
	{
		sampleNumberToSend = 0;
	}
}
