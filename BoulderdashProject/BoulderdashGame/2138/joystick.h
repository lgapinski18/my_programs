/*
* @Description: Część programu związana z inicjalizacją i obsługą joysticka.
*
* @author:      Łukasz Gapiński 242386
* @author:      Mateusz Gapiński 242387
*/

#ifndef JOYSTICK_H
#define JOYSTICK_H

/* Types */

//typ enum reprezentujący stan pozycji joysticka
typedef enum { JIDLE = 0, JLEFT = 1, JUP = 2, JRIGHT = 3, JDOWN = 4, JCENTER = 5 } JoystickState;


/* Functions declarations */

/*
 *  @brief			Funckja sluzaca do zainicjalizowania dzialania joysticka
 *
 *  @Description	Funkcja incjalizuje joystick po przez ustawienie  pin�w P0.8, P0.9, P0.10, P0.11, P0.12 na piny GPIO
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestru PINSEL0, IODIR0
 */
void joystickInit(void);

/*
 *  @brief			Funckja pobierajaca stan polozenia joysticka.
 *
 *  @Description	Funkcja pobierajaca stan polozenia joysticka po przez sprawdzenie, ktory z pin�w P0.8 - P0.12 jest aktywny.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Zwraca jeden z pieciu stanow joysticka, reprezentowanych przez typ enum JoystickState.
 */
JoystickState getJoystickState(void);


#endif // !JOYSTICK_H
