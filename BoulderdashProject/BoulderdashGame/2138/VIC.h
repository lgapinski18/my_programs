/*
* @Description: Lista stalych u¿ywanych do programowania Wektoryzowanego Kontrolera Przerwañ (VIC)
*/
#ifndef VIC_H
#define VIC_H

#include "general.h"

#define WATCHDOG_IRQ_NO (0u)
#define WATCHDOG_IRQ    BIT(WATCHDOG_IRQ_NO)
#define TIMER_0_IRQ_NO  (4u)
#define TIMER_0_IRQ     BIT(TIMER_0_IRQ_NO)
#define TIMER_1_IRQ_NO  (5u)
#define TIMER_1_IRQ     BIT(TIMER_1_IRQ_NO)
#define UART_0_IRQ_NO   (6u)
#define UART_0_IRQ      BIT(UART_0_NO)
#define UART_1_IRQ_NO   (7u)
#define UART_1_IRQ      BIT(UART_1_NO)
#define PWM_0_IRQ_NO    (8u)
#define PWM_0_IRQ       BIT(PWM_0_NO)
#define I2C_0_IRQ_NO    (9u)
#define I2C_0_IRQ       BIT(I2C_0_NO)
#define SPI_0_IRQ_NO    (10u)
#define SPI_0_IRQ       BIT(SPI_0_NO)
#define SPI_1_IRQ_NO    (11u)
#define SPI_1_IRQ       BIT(SPI_1_NO)
#define PLL_IRQ_NO      (12u)
#define PLL_IRQ         BIT(PLL_NO)
#define RTC_IRQ_NO      (13u)
#define RTC_IRQ         BIT(RTC_NO)
#define EINT_0_IRQ_NO   (14u)
#define EINT_0_IRQ      BIT(EINT_0_NO)
#define EINT_1_IRQ_NO   (15u)
#define EINT_1_IRQ      BIT(EINT_1_NO)
#define EINT_2_IRQ_NO   (16u)
#define EINT_2_IRQ      BIT(EINT_2_NO)
#define EINT_3_IRQ_NO   (17u)
#define EINT_3_IRQ      BIT(EINT_3_NO)
#define ADC_0_IRQ_NO    (18u)
#define ADC_0_IRQ       BIT(ADC_0_NO)
#define I2C_1_IRQ_NO    (19u)
#define I2C_1_IRQ       BIT(I2C_1_NO)
#define BOD_IRQ_NO      (20u)
#define BOD_IRQ         BIT(BOD_NO)
#define ADC_1_IRQ_NO    (21u)
#define ADC_1_IRQ       BIT(ADC_1_NO)
#define USB_IRQ_NO      (22u)
#define USB_IRQ         BIT(USB_NO)


#define VIC_ENABLE_SLOT BIT(5)


typedef void (__attribute__ ((interrupt("IRQ"))) *IRQ_Handler)(void) ;

// Listê sta³ych warto rozszerzyæ
#endif //__VIC_H__
