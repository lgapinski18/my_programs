/*
 *	@Description:	Cz�� programu zwi�zana z inicjalizacj� i obs�ug� watchdoga.
 *
 *	@author:		�ukasz Gapi�ski 242386
 *	@author:		Mateusz Gapi�ski 242387
 */

#ifndef WATCHDOG_H
#define WATCHDOG_H

/* Includes */
#include <lpc2xxx.h>
#include "general.h"

/* Defines */
#define	WDEN	0x01
#define	WDRESET	0x02

/* Declarations */

/*
 *  @brief			Funkcja s�u��ca do incjalizowania Watchdoga.
 *
 *  @Description	Funkcja s�u��ca do incjalizowania Watchdoga, z okre�lonym interva�em wyra�onym w sekundach.
 *
 *  @param 			[in]	interval
 *						Czas po kt�rym ma doj�� do resetu je�eli nie dosz�o do potr�cenia Watchdoga.
 *
 *  @returns  		nic
 *
 *  @side effects	Ustawienie warto�ci rejestru WDMOD oraz WDTC.
 */
void initWatchdog(tU32 interval);

/*
 *  @brief			Funkcja s�u��ca do potr�cenia Watchdoga.
 *
 *  @Description	Funkcja s�u��ca do potr�cenia Watchdoga, ktra powoduje reset odliczania czasu do resetu.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Ustawienie warto�ci rejestru WDFEED.
 */
void touchWatchdog(void);

#endif


