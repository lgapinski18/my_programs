/*
* @Description: Część programu związana z inicjalizacją i obsługą modułu diod LED podłączonych do I2C.
*
* @author:      Łukasz Gapiński 242386
* @author:      Mateusz Gapiński 242387
*/

#ifndef LEDI2C_H
#define LEDI2C_H

 /**************************************************************************************************************************
 * Typedefs and defines
 **************************************************************************************************************************/
//Adresy rejestrów
/*
#define LEDI2C_INPUT0	0x00
#define LEDI2C_INPUT1	0x01
#define LEDI2C_PWM0		0x02
#define LEDI2C_PSC0		0x03
#define LEDI2C_PWM1		0x04
#define LEDI2C_PSC1		0x05*/
#define LEDI2C_LS0		0x06
/*
#define LEDI2C_LS1		0x07
#define LEDI2C_LS2		0x08
#define LEDI2C_LS3		0x09

//Flaga autoinkrementacji*/
#define LEDI2C_AI		0x10

/*
//Diody LED rejestru LS0
#define LEDI2C_LED0_OFF		0x00
#define LEDI2C_LED0_ON		0x01
#define LEDI2C_LED0_MASK	0x03
#define LEDI2C_LED1_OFF		0x00
#define LEDI2C_LED1_ON		0x04
#define LEDI2C_LED1_MASK	0x0C
#define LEDI2C_LED2_OFF		0x00
#define LEDI2C_LED2_ON		0x10
#define LEDI2C_LED2_MASK	0x30
#define LEDI2C_LED3_OFF		0x00
#define LEDI2C_LED3_ON		0x40
#define LEDI2C_LED3_MASK	0xC0

//Diody LED rejestru LS1
#define LEDI2C_LED4_OFF		0x00
#define LEDI2C_LED4_ON		0x01
#define LEDI2C_LED4_MASK	0x03
#define LEDI2C_LED5_OFF		0x00
#define LEDI2C_LED5_ON		0x04
#define LEDI2C_LED5_MASK	0x0C
#define LEDI2C_LED6_OFF		0x00
#define LEDI2C_LED6_ON		0x10
#define LEDI2C_LED6_MASK	0x30
#define LEDI2C_LED7_OFF		0x00
#define LEDI2C_LED7_ON		0x40
#define LEDI2C_LED7_MASK	0xC0

//Diody LED rejestru LS2
#define LEDI2C_LED8_OFF		0x00
#define LEDI2C_LED8_ON		0x01
#define LEDI2C_LED8_MASK	0x03
#define LEDI2C_LED9_OFF		0x00
#define LEDI2C_LED9_ON		0x04
#define LEDI2C_LED9_MASK	0x0C
#define LEDI2C_LED10_OFF	0x00
#define LEDI2C_LED10_ON		0x10
#define LEDI2C_LED10_MASK	0x30
#define LEDI2C_LED11_OFF	0x00
#define LEDI2C_LED11_ON		0x40
#define LEDI2C_LED11_MASK	0xC0

//Diody LED rejestru LS3
#define LEDI2C_LED12_OFF	0x00
#define LEDI2C_LED12_ON		0x01
#define LEDI2C_LED12_MASK	0x03
#define LEDI2C_LED13_OFF	0x00
#define LEDI2C_LED13_ON		0x04
#define LEDI2C_LED13_MASK	0x0C
#define LEDI2C_LED14_OFF	0x00
#define LEDI2C_LED14_ON		0x10
#define LEDI2C_LED14_MASK	0x30
#define LEDI2C_LED15_OFF	0x00
#define LEDI2C_LED15_ON		0x40
#define LEDI2C_LED15_MASK	0xC0*/

/* Declarations */


 /*
  *  @brief			Funkcja odpowiadajaca za odpowienia aktywacje diod LED, aby uzyskac efekt wizualny na ukonczenie gry.
  *
  *  @Description	Funkcja odpowiadajaca za odpowienia aktywacje diod LED, aby uzyskac efekt wizualny na ukonczenie gry.
  *
  *  @param 			brak
  *
  *  @returns  		nic
  *
  *  @side effects	Modyfikacja rejestr�w LS0, LS1, LS2, LS3
  */
void winLEDMovement(void);


#endif /* LEDI2C_H */
