/*
 *	@Description:	Deklaracje procedur obslugi przerwan
 */

#ifndef IRQ_HANDLERD
#define IRQ_HANDLERD

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
#ifdef __cplusplus  
extern "C" {
#endif
void IRQ_PutNextSample(void) __attribute__ ((interrupt("IRQ"))) ;
#ifdef __cplusplus
}
#endif


#endif //__IRQ_HANDLER__
