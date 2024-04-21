/*
* @Description:	Lista stalych uzywanych do programowania timerow
*/
#ifndef TIMER_H
#define TIMER_H

#include "general.h"

//Stale dla timerow
#define TIMER_RESET     BIT(1)
#define TIMER_RUN       BIT(0)
#define MR0_I           BIT(0)
#define MR0_R           BIT(1)
#define MR0_S           BIT(2)
#define MR1_I           BIT(3)
#define MR1_R           BIT(4)
#define MR1_S           BIT(5)
#define MR2_I           BIT(6)
#define MR2_R           BIT(7)
#define MR2_S           BIT(8)
#define MR3_I           BIT(9)
#define MR3_R           BIT(10)
#define MR3_S           BIT(11)

#define TIMER_MR0_INT	BIT(0)
#define TIMER_MR1_INT	BIT(1)
#define TIMER_MR2_INT	BIT(2)
#define TIMER_MR3_INT	BIT(3)
#define TIMER_CR0_INT	BIT(4)
#define TIMER_CR1_INT	BIT(5)
#define TIMER_CR2_INT	BIT(6)
#define TIMER_CR3_INT	BIT(7)
#define TIMER_ALL_INT	(TIMER_MR0_INT | TIMER_MR1_INT | TIMER_MR2_INT | TIMER_MR3_INT | TIMER_CR0_INT | TIMER_CR1_INT | TIMER_CR2_INT | TIMER_CR3_INT)

#endif //__TIMER_H__
