/*
 *	@Description:	Definicje procedur obslugi przerwan
 */

#include "../general.h"
#include <lpc2xxx.h>
#include "irq_handler.h"
#include "../timer.h"
#include <printf_P.h>

#include "../dac.h"


/*
 *  @brief			Procedura obslugi przerwania zglaszanego przez Timer #1
 *
 *  @Description	Funkcja ta stanowi procedure obslugi przerwania zglaszanego przez Timer #1,
 * 					polega na wywolaniu funkcji putNextSample.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestru T1IR Timer'a #1
 * 					Zerowanie rejestru VICVectAddr
 */
void IRQ_PutNextSample(void)
{
	/* Sprawdzenie, czy wystapilo odpowiednie przerwanie */
	if ((TIMER_MR0_INT & T1IR) != 0u)
	{
		putNextSample();

		T1IR = (tU32)TIMER_MR0_INT; /* Poinformowanie Timera #1, iz jego przerwanie zostalo obsluzone */
	}

	VICVectAddr = 0; /* Informacja dla kontrolera o zakonczeniu obslugi przerwania */
}

