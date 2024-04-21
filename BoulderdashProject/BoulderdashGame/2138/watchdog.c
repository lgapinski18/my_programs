#include "watchdog.h"
#include "config.h"

/*
 *  @brief			Funkcja służąca do incjalizowania Watchdoga.
 *
 *  @Description	Funkcja służąca do incjalizowania Watchdoga, z określonym intervałem wyrażonym w sekundach.
 *
 *  @param 			[in]	interval
 *						Czas po którym ma dojść do resetu jeżeli nie doszło do potrącenia Watchdoga.
 *
 *  @returns  		nic
 *
 *  @side effects	Ustawienie wartości rejestru WDMOD oraz WDTC.
 */
void initWatchdog(tU32 interval)
{
	WDMOD 	= WDEN | WDRESET;
	WDTC 	= interval * PERIPHERAL_CLOCK / 4;
}

/*
 *  @brief			Funkcja służąca do potrącenia Watchdoga.
 *
 *  @Description	Funkcja służąca do potrącenia Watchdoga, ktra powoduje reset odliczania czasu do resetu.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Ustawienie wartości rejestru WDFEED.
 */
void touchWatchdog(void)
{
	WDFEED = 0xAA;
	WDFEED = 0x55;
}
