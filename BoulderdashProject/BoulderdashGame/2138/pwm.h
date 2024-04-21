/*
 *	@Description:	Zawiera zbior deklaracji funkcji zwiazanych obluga PWM
 */
#ifndef PWM_H
#define PWM_H

#include "general.h"

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
void blinkGreenLed(tU8 numberOfBlinks);


#endif // !_PWM_H_
