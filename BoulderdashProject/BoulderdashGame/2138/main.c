/*************************************************************************************
 *
 * @Description:
 * Program przyk�adowy - odpowiednik "Hello World" dla system�w wbudowanych
 * Rekomendujemy wkopiowywanie do niniejszego projektu nowych funkcjonalno�ci
 *
 *
 * UWAGA! Po zmianie rozszerzenia na cpp program automatycznie b�dzie u�ywa�
 * kompilatora g++. Oczywi�cie konieczne jest wprowadzenie odpowiednich zmian w
 * pliku "makefile"
 *
 *
 * Program przyk�adowy wykorzystuje Timer #0 i Timer #1 do "mrugania" diodami
 * Dioda P1.16 jest zapalona i gaszona, a czas pomi�dzy tymi zdarzeniami
 * odmierzany jest przez Timer #0.
 * Program aktywnie oczekuje na up�yni�cie odmierzanego czasu (1s)
 *
 * Druga z diod P1.17 jest gaszona i zapalana w takt przerwa� generowanych
 * przez timer #1, z okresem 500 ms i wype�nieniem 20%.
 * Procedura obs�ugi przerwa� zdefiniowana jest w innym pliku (irq/irq_handler.c)
 * Sama procedura MUSI by� oznaczona dla kompilatora jako procedura obs�ugi
 * przerwania odpowiedniego typu. W przyk�adzie jest to przerwanie wektoryzowane.
 * Odpowiednia deklaracja znajduje si� w pliku (irq/irq_handler.h)
 * 
 * Pr�cz "mrugania" diodami program wypisuje na konsoli powitalny tekst.
 * 
 * @Authors: Micha� Morawski,
 *           Daniel Arendt, 
 *           Przemys�aw Ignaciuk,
 *           Marcin Kwapisz
 *
 * @Change log:
 *           2016.12.01: Wersja oryginalna.
 *
 ******************************************************************************/

#include "general.h"
#include <lpc2xxx.h>
#include <printf_P.h>
#include <printf_init.h>
#include <consol.h>
#include <config.h>
#include "irq/irq_handler.h"
#include "timer.h"
#include "VIC.h"

#include "adc.h"
#include "game.h"
#include "functionals.h"
#include "i2c.h"
#include "joystick.h"
#include "lcd.h"
#include "ledi2c.h"
#include "mat.h"
#include "pwm.h"
#include "watchdog.h"
#include "ethdrv_ENC28J60.h"


/*
 *  @brief		Krótko co procedura robi.
 *  @param 		nazwa  parametru 1
 *             		opis parametru 1
 *  @param 		nazwa  parametru 2
 *             		opis parametru 2
 *
 *  @param 		nazwa  parametru n
 *             		opis parametru n
 *  @returns  	np. tak: true on success, false otherwise
 *  @side effects:
 *            	efekty uboczne
 */
int main(void)
{
	/* uruchomienie 'simple printf' */
    printf_init();

    /* Powitanie */
    simplePrintf("\n\n\n\n");
    simplePrintf("\n*********************************************************");
    simplePrintf("\n*");
    simplePrintf("\n* Systemy Wbudowane");
    simplePrintf("\n* Wydzial FTIMS");
    simplePrintf("\n* Boulderdash");
    simplePrintf("\n*");
    simplePrintf("\n*********************************************************");



    IODIR |= 0x00006000;
    IOSET  = 0x00006000;

    initAdc();

    playBackgroundSoundtrack();

    initRedDiod();
    initBlueDiod();

    delay_ms(500);

    setRedDiodState(TRUE);
    setBlueDiodState(TRUE);

    delay_ms(500);

    setRedDiodState(FALSE);
    setBlueDiodState(FALSE);

    delay_ms(500);

    i2cInit();
    winLEDMovement();
    joystickInit();
    lcdColor(0xFF, 0x00);
    lcdInit();

    initENC28J60();

    initGame();

    initWatchdog(30);
    touchWatchdog();

    tBool mirrorX = FALSE; //Zmienna powiązane ze stanem czy jest aktywne odbiecie lustrzane wzdluz osi X
    tBool mirrorY = TRUE; //Zmienna powiązane ze stanem czy jest aktywne odbiecie lustrzane wzdluz osi Y

    tBool con = TRUE;
    while (con == TRUE) {
       	switch (getAccelerationDirection()) {
       	    case UP:
                if (mirrorX == FALSE) {
                    //mirrorX = FALSE;
                    mirrorX = TRUE;
                    //Wyczysczenie tła gry
                    setLcdBackgound();
                    //Odbicie ekranu
                    lcdMirror(mirrorX, mirrorY);
                }
       	        break;

       	    case DOWN:
                if (mirrorX == TRUE) {
                    //mirrorX = TRUE;
                    mirrorX = FALSE;
                    //Wyczysczenie tła gry
                    setLcdBackgound();
                    //Odbicie ekranu
                    lcdMirror(mirrorX, mirrorY);
                }
       	        break;

       	    case LEFT:
                if (mirrorY == TRUE) {
                    mirrorY = FALSE;
                    //Wyczysczenie tła gry
                    setLcdBackgound();
                    //Odbicie ekranu
                    lcdMirror(mirrorX, mirrorY);
                }
                break;

       	    case RIGHT:
                if (mirrorY == FALSE) {
                    mirrorY = TRUE;
                    //Wyczysczenie tła gry
                    setLcdBackgound();
                    //Odbicie ekranu
                    lcdMirror(mirrorX, mirrorY);
                }
       	        break;

       	    case FRONT:
       	        break;

       	    case BACK:
       	        break;

       	    default:
       	        break;
       	}

        switch (getJoystickState()) {
            case JLEFT:
              	touchWatchdog();

                if (mirrorY == TRUE)
                {
                	moveLeft();
                }
                else
                {
                	moveRight();
                }
                blinkGreenLed(1);
                break;

            case JUP:
               	touchWatchdog();
                //moveUp();
                if (mirrorX == FALSE)
                {
                	moveUp();
                }
                else
                {
                	moveDown();
                }
                blinkGreenLed(1);
                break;

            case JRIGHT:
               	touchWatchdog();
                //moveRight();
                if (mirrorY == TRUE)
                {
                	moveRight();
                }
                else
                {
                	moveLeft();
                }
                blinkGreenLed(1);
                break;

            case JDOWN:
               	touchWatchdog();
                //moveDown();
                if (mirrorX == FALSE)
                {
                	moveDown();
                }
                else
                {
                	moveUp();
                }
                blinkGreenLed(1);
                break;

            case JCENTER:
                touchWatchdog();
                resetGame();
                break;

            case JIDLE:
               	//simplePrintf("Joystick State: IDLE");
                break;

            default:
                break;
        }

        updateScreen();
        delay_ms(100);
    }

    return 0;
}


