/*
 *	@Descritpion:	Zawariera zbior deklaracji funkcji zwiazanych
 *					z wykorzystaniem przetwornikow analogowo-cyfrowych
 */

#ifndef ADC_H
#define ADC_H

#include "general.h"

/* Definicje dla przerwornikow analogowo cyfrowych ADC0 i ADC1 */

/* Lista wyliczeniowa zawierajaca wartosci numerow kanalow dla przetwornikow analogowo-cyfrowych ADC0 i ADC1 */
typedef enum  { AIN0 = 0u, AIN1 = 1u, AIN2 = 2u, AIN3 = 3u, AIN4 = 4u, AIN5 = 5u, AIN6 = 6u, AIN7 = 7u } AdcChanel;

/* Deklaracja stalych informujacych o numerze kanalu do odczytu wartosci akcelerometru dla odpowiednio osi X (ADC1), Y (ADC1), Z (ADC0) */
#define ACCEL_X_CHANEL_NO AIN6 /* Deklaracja stalych informujacych o numerze kanalu do odczytu wartosci akcelerometru dla odpowiednio osi X (ADC1) */
#define ACCEL_Y_CHANEL_NO AIN7 /* Deklaracja stalych informujacych o numerze kanalu do odczytu wartosci akcelerometru dla odpowiednio osi Y (ADC1) */
#define ACCEL_Z_CHANEL_NO AIN3 /* Deklaracja stalych informujacych o numerze kanalu do odczytu wartosci akcelerometru dla odpowiednio osi Z (ADC0) */

#define GS1 13u
#define GS2 14u

/* Deklaracje funkcji */

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
tU16 getAnalogueInputAdc0(AdcChanel channel);

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
tU16 getAnalogueInputAdc1(AdcChanel channel);

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
void initAdc(void);

#endif
