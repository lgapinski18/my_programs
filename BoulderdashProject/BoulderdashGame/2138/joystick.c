/* Includes */
#include <lpc2xxx.h>
#include "joystick.h"

/* Defines */
#define JOYSTICK_ALL	0x00001F00
#define JOYSTICK_UP		0x00000400
#define JOYSTICK_DOWN	0x00001000
#define JOYSTICK_LEFT	0x00000200
#define JOYSTICK_RIGHT	0x00000800
#define JOYSTICK_CENTER	0x00000100

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
void joystickInit(void)
{
	//Ustwianie pinow P0.8 - P0.12, aby funkcjonowa�y jako piny GPIO
	PINSEL0	&= ~(0x03FF0000);

	//Ustwienie kierunku pinow P0.8 - P0.12 na wejscie
	IODIR0	&= ~(JOYSTICK_ALL);
}

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
JoystickState getJoystickState(void)
{
	//Zmienna przechowujaca odczytany stan pozycji joysticka, ktory ma zostanie zworcony
	JoystickState returnState = JIDLE;
	//Switch, w kt�ory zostanie ustawiony odpowiedni stan portu na podstawie stanu aktywno�ci odpowiedniego pinu
	if ((IOPIN0 & JOYSTICK_LEFT) == 0)
	{
		returnState = JLEFT;
	}
	else if ((IOPIN0 & JOYSTICK_UP) == 0)
	{
		returnState = JUP;
	}
	else if ((IOPIN0 & JOYSTICK_RIGHT) == 0)
	{
		returnState = JRIGHT;
	}
	else if ((IOPIN0 & JOYSTICK_DOWN) == 0)
	{
		returnState = JDOWN;
	}
	else if ((IOPIN0 & JOYSTICK_CENTER) == 0)
	{
		returnState = JCENTER;
	}
	else
	{
		returnState = JIDLE;
	}

	return returnState;
}
