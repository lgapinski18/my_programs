/*
 *	@Descritpion:	Zawariera zbior definicji funkcji zwiazanych
 *					z wykorzystaniem przetwornikow analogowo-cyfrowych
 */

#include "general.h"
#include <printf_P.h>
#include <lpc2xxx.h>
#include <config.h>
#include "functionals.h"
#include "adc.h"

/* Definicje */
#define CRYSTAL_FREQUENCY	FOSC		/* Czestotliwosc taktowania zewetrznego zegara */
#define PLL_FACTOR			PLL_MUL	/* Wspolczynnik mnozenia PLL */
#define VPBDIV_FACTOR		PBSD		/* Wspolczynik dzielenia dla predkosci magistrli urzadzen peryferyjnych */

/* Jedynka 32-bitowa */
static const tU32 one32 = 1u;


/*
 *  @brief			Funkcja odczytujaca wartosc podanego kanalu przetwornika analogowo-cyfrowego ADC0
 *
 *  @Description	Funkcja wykonuje zapis do rejestru kontrolnego przetwornika ADC0 informacji o wybranym
 *					kanale, z ktorego ma byc odczytana wartosc (po przez ustawienie odpowiedniego bitu
 *					- numer bitu rowny numerowi kanalu), oraz ustawia bit informujacy (nr 24), iz przetworzenie
 *					ma nastapic odrazu.
 *
 *  @param 			[in]	channel
 *						numer kanalu, z ktorego ma zostac odczytana wartosc
 *
 *  @returns  		odczytana wartosc z rejestru ADC0 po wykonaniu konwersji dla podanego kanalu
 *
 *  @side effects	Modyfikacja rejestru ADCR przetwornika analogowo-cyfrowego ADC0
 */
tU16 getAnalogueInputAdc0(AdcChanel channel)
{
	/*Ustawienie stanu pinu P0.14 na niski, aby dokladnosc pomiarowa byla +- 1.5 g (800mV/g) */
    IOCLR = one32 << GS2;

	/* Zmienna, do ktorej zapisany zostanie wynik odczytu */
	tU16 returnResult;
	
	/* Wybor kanalu, z ktorego maja byc odczytywane dane */
	ADCR &= ~(0xFFu);
	ADCR |= one32 << (tU8)channel;

	/* Ustawienie trybu przetwornika na operacyjny i ustawienie konwersji na teraz */
	ADCR |= (one32 << 21) | (one32 << 24);
	ADCR &= ~((one32 << 26) | (one32 << 25));
	
	/* Sprawdzanie, czy konwersja sie zakonczyla */
	while ((ADDR & 0x80000000U) == 0)
	{
		;
	}
	  
	/* Odczytanie z rejestru wyniku, ktory jest 10 bitowa liczba calkowita */
	returnResult = (ADDR >> 6) & 0x3FF;

	/* Ustawienie trybu przetwornika na power-down i wylaczenie rozpoczecia*/
	ADCR &= ~((one32 << 21) | (one32 << 24));
	
	delay_ms(100);

	/* Ustawienie pinu P0.14, zwiazanego z dokladoscia akcelerometru i chip select kontrolera Ethernet w SPI na stan wysoki*/
    IOSET  = one32 << GS2;

	return returnResult;
}

/*
 *  @brief			Funkcja odczytujaca wartosc podanego kanalu przetwornika analogowo-cyfrowego ADC1
 *
 *  @Description	Funkcja wykonuje zapis do rejestru kontrolnego przetwornika ADC1 informacji o wybranym
 *					kanale, z ktorego ma byc odczytana wartosc (po przez ustawienie odpowiedniego bitu
 *					- numer bitu rowny numerowi kanalu), oraz ustawia bit informujacy (nr 24), iz przetworzenie
 *					ma nastapic odrazu.
 *
 *  @param 			[in]	channel
 *						numer kanalu, z ktorego ma zostac odczytana wartosc
 *
 *  @returns  		odczytana wartosc z rejestru ADC1 po wykonaniu konwersji dla podanego kanalu
 *
 *  @side effects	Modyfikacja rejestru AD1CR przetwornika analogowo-cyfrowego ADC1
 */
tU16 getAnalogueInputAdc1(AdcChanel channel)
{
	/*Ustawienie stanu pinu P0.14 na niski, aby dokladnosc pomiarowa byla +- 1.5 g (800mV/g) */
    IOCLR  = one32 << GS2;
	/* Zmienna, do ktorej zapisany zostanie wynik odczytu */
	tU16 returnResult;

	/* Wybor kanalu, z ktorego maja byc odczytywane dane */
	AD1CR &= ~(0xFF);
	AD1CR |= one32 << (tU8)channel;

	/* Ustawienie trybu przetwornika na operacyjny i ustawienie konwersji na teraz */
	AD1CR |= (one32 << 21) | (one32 << 24);
	AD1CR &= ~((one32 << 26) | (one32 << 25));

	/* Sprawdzanie, czy konwersja sie zakonczyla */
	while ((AD1DR & 0x80000000U) == 0)
	{
		;
	}

	/* Odczytanie z rejestru wyniku, ktory jest 10 bitowa liczba calkowita */
	returnResult = (AD1DR >> 6) & 0x3FF;

	/* Ustawienie trybu przetwornika na power-down i wylaczenie rozpoczecia*/
	AD1CR &= ~((one32 << 21) | (one32 << 24));

	delay_ms(100);

	/* Ustawienie pinu P0.14, zwiazanego z dokladoscia akcelerometru i chip select kontrolera Ethernet w SPI na stan wysoki*/
	IOSET = one32 << GS2;
  
	return returnResult;
}


/*
 *  @brief			Funkcja incijalizujaca przetworniki analogowo-cyfrowe ADC0 i ADC1, aby moc odczytywac
 *					przyspieszenie z akcelerometru w trzeh osiach
 *
 *  @Description	Funckja ustawia odpowiednia wartosci w rejestrze PINSEL1 (bity 10.,13.,29. na 0,
 *					a bity 11., 12., 28. na 1), aby odpowiednie piny (P0.21, P0.22, P0.30) pelnily
 * 					funkcje zwiazane z odczytem z akcelerometru wartosci przyspieszenia wzdluz osi X, Y i Z.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestru PINSEL1
 * 					Modyfikacja rejestru ADCR
 * 					Modyfikacja rejestru AD1CR
 */
void initAdc(void)
{
	/* Zero 32-bitowe */
	const tU32 zero32 = 0u;

	/* Ustawienie dokladnosci pomiarowej na +- 1.5 g (800mV/g) - ustawienie stanu niskiego na pinach P0.13 i P0.14 */
    IODIR |= (one32 << GS1) | (one32 << GS2);
    IOCLR  = (one32 << GS1) | (one32 << GS2);

	/*	Inicjalizacja wejscia ADC1: AIN1.6 (P0.21)*/
	/*	Ustawienie funkcji pinu P0.21 na AIN1.6.
	*	Umozliwia po przez przetwornik ADC1 (kanal 6)
	*	odczyt przyspieszenia z akcelerometru wzdluz osi X.
	*/
	PINSEL1 &= ~((one32 << 10) | (one32 << 11));
	PINSEL1 |= (one32 << 11);
	
	/*	Inicjalizacja wejscia ADC1: AIN1.7 (P0.22)*/
	/*	Ustawienie funkcji pinu P0.22 na AIN1.7.
	*	Umozliwia po przez przetwornik ADC1 (kanal 7)
	*	odczyt przyspieszenia z akcelerometru wzdluz osi Y.
	*/
	PINSEL1 &= ~((one32 << 12) | (one32 << 13));
	PINSEL1 |= (one32 << 12);

	/*	Inicjalizacja wejscia ADC0: AIN0.3 (P0.30)*/
	/*	Ustawienie funkcji pinu P0.30 na AIN0.3.
	*	Umozliwia po przez przetwornik ADC0 (kanal 3)
	*	odczyt przyspieszenia z akcelerometru wzdluz osi Z.
	*/
	PINSEL1 &= ~((one32 << 28) | (one32 << 29));
	PINSEL1 |= (one32 << 28);
	
	/* Inicjalizacja przetwornikow analogow0-cyfrowych ADC0 i ADC1 */

	/* Inicjalizacja przetwornika ADC0
	*	Wybranie probnego kanalu
	*	Ustawienie odpowiedniej wartosci dzielnika dla zegara, aby czestotliwosc taktowania zegara dla ADC0 bylo rowne 4.5MHz
	*	Ustawienie BURST na 0 (sterowanie software'em), umozliwia m.in. wybranie opcji "conversion now" i wymaga ustawienia liczby wykorzystywanych zegarow do konwersji na 11.
	*	Ustawienie liczby wykorzystywanych zegarow na 11 (wartosc 0)
	*	Ustawienie trybu przetwornika na power-down (wartosc 0)
	*	Ustawienie rozpoczrcia konwersji no start (wartosc 1)
	*/
	ADCR = (one32)                             		| /* Kanal 0 */
	       ((((CRYSTAL_FREQUENCY *
	         PLL_FACTOR /
	         VPBDIV_FACTOR) / 2000000) - 1) << 8)	| /* Ustawienie dzielnika */
	       (zero32 << 16)								| /* BURST = 0 */
	       (zero32 << 17)								| /* CLKS = 0 */
	       (zero32 << 21)								| /* PDN = 0 */
	       (zero32 << 24);								  /* START = 0 */
	

	/*Inicjalizacja przetwornika ADC1
	*	Wybranie probnego kanalu
	*	Ustawienie odpowiedniej wartosci dzielnika dla zegara, aby czestotliwosc taktowania zegara dla ADC1 bylo rowne 4.5MHz
	*	Ustawienie BURST na 0 (sterowanie software'em), umozliwia m.in. wybranie opcji "conversion now" i wymaga ustawienia liczby wykorzystywanych zegarow do konwersji na 11.
	*	Ustawienie liczby wykorzystywanych zegarow na 11 (wartosc 0)
	*	Ustawienie trybu przetwornika na power-down (wartosc 0)
	*	Ustawienie rozpoczecia konwersji no start (wartosc 0)
	*/
	AD1CR = (1u)                            		| /* Kanal 0 */
	       ((((CRYSTAL_FREQUENCY *
	         PLL_FACTOR /
	         VPBDIV_FACTOR) / 2000000) - 1) << 8)	| /* Ustawienie dzielnika */
	       (zero32 << 16)								| /* BURST = 0 */
	       (zero32 << 17)								| /* CLKS = 0 */
	       (zero32 << 21)								| /* PDN = 0 */
	       (zero32 << 24);								  /* START = 0 */
}

