/*
 *   @Description:	Zbior zasobow dla przetwornika DAC 
 */

#ifndef DAC_REASOURCES_H
#define DAC_REASOURCES_H

#include "general.h"

/* Liczba probek do odtworzenia */
#define numberOfSamples 3232

/* Tablica probek do odtworzenia */
tU8 soundtrack[numberOfSamples] = {
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff,
		0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff,
		0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20,
		0x01, 0x20, 0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0xff, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xff, 0xff, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20,
		0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0xff, 0xff, 0xfd, 0xff,
		0xfe, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0x02, 0x20,
		0x03, 0x20, 0x20, 0x20, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff,
		0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02, 0x20, 0x01, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xfe, 0xff, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0x02, 0x20, 0x20, 0x20, 0x01, 0x20, 0x02, 0x20, 0x02, 0x20,
		0x02, 0x20, 0x01, 0x20, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20,
		0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0x01, 0x20, 0x03, 0x20, 0x02, 0x20, 0xff, 0xff, 0xff, 0xff, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0xff, 0xff, 0xfe, 0xff, 0xff, 0xff,
		0x20, 0x20, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xfe, 0xff, 0xff, 0xff,
		0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20,
		0xff, 0xff, 0x20, 0x20, 0x01, 0x20, 0xff, 0xff, 0x20, 0x20, 0x01, 0x20, 0x02, 0x20, 0x01, 0x20, 0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20,
		0xff, 0xff, 0xfe, 0xff, 0xff, 0xff, 0x20, 0x20, 0xff, 0xff, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0xfd, 0xff, 0xfe, 0xff, 0x20, 0x20, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0xfe, 0xff, 0xfd, 0xff, 0x20, 0x20, 0x02, 0x20, 0x01, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20,
		0x01, 0x20, 0xff, 0xff, 0xff, 0xff, 0x02, 0x20, 0x01, 0x20, 0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xfd, 0xff, 0xfc, 0xff, 0xfe, 0xff, 0xff, 0xff,
		0x20, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02, 0x20, 0x02, 0x20, 0x04, 0x20, 0x02, 0x20, 0xff, 0xff, 0x20, 0x20, 0xff, 0xff, 0x20, 0x20, 0x02, 0x20, 0x02, 0x20, 0x01, 0x20, 0x20, 0x20,
		0xfe, 0xff, 0x01, 0x20, 0x02, 0x20, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0xff, 0xff, 0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x02, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02, 0x20, 0x04, 0x20, 0x01, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x03, 0x20, 0x02, 0x20, 0x01, 0x20, 0x02, 0x20, 0x02, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x20, 0x20, 0xfd, 0xff, 0x20, 0x20, 0x04, 0x20,
		0x04, 0x20, 0x04, 0x20, 0x04, 0x20, 0x03, 0x20, 0x02, 0x20, 0x20, 0x20, 0xff, 0xff, 0x02, 0x20, 0x01, 0x20, 0xff, 0xff, 0x01, 0x20, 0x03, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0xfe, 0xff,
		0xfb, 0xff, 0xfd, 0xff, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02, 0x20, 0x03, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xfb, 0xff, 0xfd, 0xff, 0x20, 0x20, 0x20, 0x20,
		0x01, 0x20, 0x01, 0x20, 0x02, 0x20, 0x06, 0x20, 0x06, 0x20, 0x02, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20,
		0x02, 0x20, 0x02, 0x20, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0xfc, 0xff, 0xfd, 0xff, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x20, 0x20, 0xff, 0xff, 0x03, 0x20, 0x02, 0x20, 0x01, 0x20, 0x04, 0x20,
		0x03, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02, 0x20, 0x01, 0x20, 0xff, 0xff, 0xff, 0xff, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0xfe, 0xff, 0x20, 0x20, 0x03, 0x20, 0x02, 0x20, 0xff, 0xff, 0x01, 0x20,
		0xff, 0xff, 0xfe, 0xff, 0xff, 0xff, 0x20, 0x20, 0x20, 0x20, 0xfe, 0xff, 0xfb, 0xff, 0xfc, 0xff, 0xfd, 0xff, 0xff, 0xff, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
		0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x02, 0x20, 0x02, 0x20, 0x02, 0x20, 0x04, 0x20, 0x02, 0x20, 0xff, 0xff, 0xff, 0xff, 0xfe, 0xff,
		0xfd, 0xff, 0xfe, 0xff, 0xfd, 0xff, 0xfd, 0xff, 0xff, 0xff, 0xfd, 0xff, 0xfc, 0xff, 0xfd, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xfd, 0xff, 0xfe, 0xff, 0x20, 0x20, 0xfe, 0xff,
		0xfb, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xfd, 0xff, 0xfd, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0x20, 0x20, 0x02, 0x20, 0x02, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x02, 0x20, 0x02, 0x20,
		0x20, 0x20, 0x20, 0x20, 0x02, 0x20, 0x01, 0x20, 0x20, 0x20, 0xff, 0xff, 0xfe, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0xfe, 0xff, 0xfd, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xfd, 0xff,
		0xfe, 0xff, 0xff, 0xff, 0x01, 0x20, 0x02, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02, 0x20, 0x20, 0x20, 0xfe, 0xff, 0xfe, 0xff, 0xff, 0xff, 0x20, 0x20, 0xfe, 0xff, 0xfe, 0xff, 0xff, 0xff, 0x20, 0x20,
		0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0x01, 0x20, 0x01, 0x20, 0x20, 0x20, 0x01, 0x20, 0x02, 0x20, 0x02, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xff, 0xff, 0xff, 0xff,
		0xff, 0xff, 0x20, 0x20, 0xfd, 0xff, 0xfe, 0xff, 0x01, 0x20, 0x02, 0x20, 0x01, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfd, 0xff, 0xfc, 0xff, 0xfe, 0xff, 0xff, 0xff, 0x20, 0x20,
		0xff, 0xff, 0xfe, 0xff, 0xff, 0xff, 0x20, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0x01, 0x20, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x03, 0x20, 0x03, 0x20, 0x02, 0x20, 0xff, 0xff, 0xfe, 0xff,
		0x20, 0x20, 0x20, 0x20, 0xfe, 0xff, 0xfe, 0xff, 0xff, 0xff, 0xfd, 0xff, 0xfd, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff,
		0x02, 0x20, 0x04, 0x20, 0x01, 0x20, 0x20, 0x20, 0x01, 0x20, 0x20, 0x20, 0x02, 0x20, 0x01, 0x20, 0x20, 0x20, 0x02, 0x20, 0x20, 0x20, 0xff, 0xff, 0x02, 0x20, 0x20, 0x20, 0xff, 0xff, 0x02, 0x20,
		0xff, 0xff, 0xfe, 0xff, 0x20, 0x20, 0x01, 0x20, 0x02, 0x20, 0x01, 0x20, 0x20, 0x20, 0x02, 0x20, 0x03, 0x20, 0x02, 0x20, 0xff, 0xff, 0xfd, 0xff, 0x20, 0x20, 0x01, 0x20, 0xfe, 0xff, 0xff, 0xff,
		0xfe, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0x01, 0x20, 0x02, 0x20, 0x04, 0x20, 0x04, 0x20, 0x04, 0x20, 0x04, 0x20, 0x02, 0x20, 0x02, 0x20, 0x05, 0x20, 0x04, 0x20, 0x02, 0x20,
		0x02, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xfe, 0xff, 0x01, 0x20, 0x01, 0x20, 0xfe, 0xff, 0xfd, 0xff, 0xff, 0xff, 0x01, 0x20, 0x01, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20,
		0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x02, 0x20, 0x01, 0x20, 0xfe, 0xff, 0xfd, 0xff, 0x01, 0x20, 0x01, 0x20, 0xff, 0xff, 0xfe, 0xff, 0xfd, 0xff, 0xff, 0xff, 0x01, 0x20, 0xfe, 0xff,
		0xfd, 0xff, 0xfd, 0xff, 0xfc, 0xff, 0x20, 0x20, 0x04, 0x20, 0x02, 0x20, 0x20, 0x20, 0x02, 0x20, 0x03, 0x20, 0x02, 0x20, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0x01, 0x20, 0x03, 0x20, 0xff, 0xff,
		0xfe, 0xff, 0x01, 0x20, 0x03, 0x20, 0x03, 0x20, 0x02, 0x20, 0x02, 0x20, 0x02, 0x20, 0x02, 0x20, 0x01, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x03, 0x20, 0x20, 0x20, 0xfd, 0xff, 0xfc, 0xff,
		0xfd, 0xff, 0xfd, 0xff, 0xfc, 0xff, 0xfc, 0xff, 0xfd, 0xff, 0xfe, 0xff, 0xff, 0xff, 0xfe, 0xff, 0xfd, 0xff, 0x20, 0x20, 0x20, 0x20, 0xff, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0x20, 0x20, 0x03, 0x20,
		0x03, 0x20, 0x20, 0x20, 0xfe, 0xff, 0x01, 0x20, 0x02, 0x20, 0x20, 0x20, 0x02, 0x20, 0x03, 0x20, 0x03, 0x20, 0x02, 0x20, 0xff, 0xff, 0xfe, 0xff, 0xfd, 0xff, 0xfc, 0xff, 0xfe, 0xff, 0xfd, 0xff,
		0xfc, 0xff, 0xff, 0xff, 0x20, 0x20, 0xff, 0xff, 0x20, 0x20, 0xfe, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0xfe, 0xff, 0x02, 0x20, 0xfc, 0xff, 0xfe, 0xff, 0x20, 0x20, 0xfd, 0xff,
		0x02, 0x20, 0xfe, 0xff, 0x20, 0x20, 0xfe, 0xff, 0xff, 0xff, 0xfe, 0xff, 0x20, 0x20, 0x06, 0x20, 0x20, 0x20, 0x01, 0x20, 0x20, 0x20, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x20, 0x20, 0x04, 0x20,
		0x02, 0x20, 0x02, 0x20, 0x03, 0x20, 0xff, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0x20, 0x20, 0xfe, 0xff, 0x04, 0x20, 0x04, 0x20, 0x20, 0x20, 0xff, 0xff, 0x01, 0x20, 0xff, 0xff, 0xfe, 0xff, 0xff, 0xff,
		0xfe, 0xff, 0x03, 0x20, 0x01, 0x20, 0x04, 0x20, 0x02, 0x20, 0x02, 0x20, 0x01, 0x20, 0x02, 0x20, 0xfe, 0xff, 0xfb, 0xff, 0xfd, 0xff, 0xfd, 0xff, 0x01, 0x20, 0xfc, 0xff, 0x01, 0x20, 0xfd, 0xff,
		0x02, 0x20, 0xfd, 0xff, 0x20, 0x20, 0x20, 0x20, 0x01, 0x20, 0x05, 0x20, 0xfe, 0xff, 0x05, 0x20, 0xfc, 0xff, 0x09, 0x20, 0xf7, 0xff, 0x08, 0x20, 0xf5, 0xff, 0x0c, 0x20, 0xf4, 0xff, 0x10, 0x20,
		0xc4, 0x02, 0x6e, 0x05, 0xf1, 0x06, 0x69, 0x08, 0x47, 0x09, 0x03, 0x0a, 0xab, 0x0a, 0xfc, 0x0a, 0x02, 0x0b, 0x57, 0x0b, 0x66, 0x0b, 0x4c, 0x0b, 0x6d, 0x0b, 0x63, 0x0b, 0x7e, 0x0b, 0xb1, 0x0b,
		0xd2, 0x0b, 0xd2, 0x0b, 0xdd, 0x0b, 0xf2, 0x0b, 0x04, 0x0c, 0xfe, 0x0b, 0x1f, 0x0c, 0x54, 0x0c, 0x48, 0x0c, 0x17, 0x0c, 0xd6, 0x0b, 0xe2, 0x0b, 0xf0, 0x0b, 0xe5, 0x0b, 0x05, 0x0c, 0x1d, 0x0c,
		0x0c, 0x0c, 0x1a, 0x0c, 0x2a, 0x0c, 0x36, 0x0c, 0x2a, 0x0c, 0xdb, 0x0b, 0xd3, 0x0b, 0xcf, 0x0b, 0xbf, 0x0b, 0xd7, 0x0b, 0xb2, 0x0b, 0xc6, 0x0b, 0x16, 0x0c, 0xe5, 0x0b, 0x03, 0x0c, 0x39, 0x0c,
		0xef, 0x0b, 0x7d, 0x0c, 0xbd, 0x0b, 0xa0, 0x07, 0x95, 0x03, 0x2e, 0x01, 0xf6, 0xfe, 0x63, 0xfd, 0x75, 0xfc, 0x83, 0xfb, 0xfe, 0xfa, 0xa6, 0xfa, 0x35, 0xfa, 0xe9, 0xf9, 0xd1, 0xf9, 0xd5, 0xf9,
		0xcd, 0xf9, 0xab, 0xf9, 0x92, 0xf9, 0x7e, 0xf9, 0x90, 0xf9, 0x8e, 0xf9, 0x64, 0xf9, 0x81, 0xf9, 0x6d, 0xf9, 0x3b, 0xf9, 0x4d, 0xf9, 0x40, 0xf9, 0x2b, 0xf9, 0x31, 0xf9, 0x2f, 0xf9, 0x4a, 0xf9,
		0x59, 0xf9, 0x43, 0xf9, 0x49, 0xf9, 0x41, 0xf9, 0x4b, 0xf9, 0x45, 0xf9, 0x26, 0xf9, 0x51, 0xf9, 0x4e, 0xf9, 0x2b, 0xf9, 0x48, 0xf9, 0x2d, 0xf9, 0x1d, 0xf9, 0x41, 0xf9, 0x24, 0xf9, 0x4d, 0xf9,
		0x57, 0xf9, 0x31, 0xf9, 0x6f, 0xf9, 0xfa, 0xf8, 0x8d, 0xf9, 0x9d, 0xfd, 0xdf, 0x01, 0x17, 0x04, 0x15, 0x06, 0xa9, 0x07, 0x98, 0x08, 0x95, 0x09, 0x30, 0x0a, 0x9c, 0x0a, 0x0a, 0x0b, 0x26, 0x0b,
		0x4e, 0x0b, 0x94, 0x0b, 0xbb, 0x0b, 0xca, 0x0b, 0xba, 0x0b, 0xc7, 0x0b, 0xcc, 0x0b, 0xc2, 0x0b, 0xdc, 0x0b, 0xc4, 0x0b, 0xb8, 0x0b, 0xf2, 0x0b, 0xe4, 0x0b, 0xbb, 0x0b, 0xd4, 0x0b, 0xcf, 0x0b,
		0xd0, 0x0b, 0xe3, 0x0b, 0xdb, 0x0b, 0xe2, 0x0b, 0xf7, 0x0b, 0x05, 0x0c, 0xef, 0x0b, 0xe7, 0x0b, 0xf5, 0x0b, 0xe6, 0x0b, 0xfe, 0x0b, 0x18, 0x0c, 0xf5, 0x0b, 0xed, 0x0b, 0xda, 0x0b, 0xdb, 0x0b,
		0x1a, 0x0c, 0xfa, 0x0b, 0xfb, 0x0b, 0x1a, 0x0c, 0xde, 0x0b, 0x58, 0x0c, 0xbd, 0x0b, 0xc7, 0x07, 0xa2, 0x03, 0x4a, 0x01, 0x36, 0xff, 0xa5, 0xfd, 0xb1, 0xfc, 0xc5, 0xfb, 0x33, 0xfb, 0xad, 0xfa,
		0x36, 0xfa, 0x24, 0xfa, 0xe6, 0xf9, 0x95, 0xf9, 0x90, 0xf9, 0x88, 0xf9, 0x7f, 0xf9, 0x7c, 0xf9, 0x68, 0xf9, 0x53, 0xf9, 0x4e, 0xf9, 0x50, 0xf9, 0x40, 0xf9, 0x3a, 0xf9, 0x42, 0xf9, 0x3f, 0xf9,
		0x4f, 0xf9, 0x4f, 0xf9, 0x34, 0xf9, 0x38, 0xf9, 0x2a, 0xf9, 0x27, 0xf9, 0x44, 0xf9, 0x32, 0xf9, 0x31, 0xf9, 0x42, 0xf9, 0x2d, 0xf9, 0x38, 0xf9, 0x3a, 0xf9, 0x26, 0xf9, 0x3f, 0xf9, 0x41, 0xf9,
		0x58, 0xf9, 0x68, 0xf9, 0x39, 0xf9, 0x56, 0xf9, 0x51, 0xf9, 0x3b, 0xf9, 0x7c, 0xf9, 0x14, 0xf9, 0x75, 0xf9, 0x71, 0xfc, 0x77, 0xff, 0xf6, 0x20, 0x4c, 0x02, 0x81, 0x03, 0x4b, 0x04, 0xe8, 0x04,
		0x4a, 0x05, 0xa4, 0x05, 0xdc, 0x05, 0xee, 0x05, 0x2a, 0x06, 0x59, 0x06, 0x64, 0x06, 0x72, 0x06, 0x7d, 0x06, 0x8c, 0x06, 0x8f, 0x06, 0xa8, 0x06, 0xcb, 0x06, 0xb8, 0x06, 0xb1, 0x06, 0xbf, 0x06,
		0xc9, 0x06, 0xe9, 0x06, 0xe0, 0x06, 0xd8, 0x06, 0xea, 0x06, 0xcb, 0x06, 0xdb, 0x06, 0xfb, 0x06, 0xdf, 0x06, 0xfc, 0x06, 0xe9, 0x06, 0x9a, 0x06, 0xc2, 0x06, 0xd7, 0x06, 0xaf, 0x06, 0xb9, 0x06,
		0xaa, 0x06, 0x90, 0x06, 0x94, 0x06, 0xa3, 0x06, 0xa8, 0x06, 0x80, 0x06, 0x98, 0x06, 0x9f, 0x06, 0x73, 0x06, 0x01, 0x07, 0x2d, 0x06, 0x26, 0x02, 0x2a, 0xfe, 0xdf, 0xfb, 0xe1, 0xf9, 0x64, 0xf8,
		0x57, 0xf7, 0x61, 0xf6, 0xdc, 0xf5, 0x65, 0xf5, 0x02, 0xf5, 0xde, 0xf4, 0x9f, 0xf4, 0x74, 0xf4, 0x5a, 0xf4, 0x38, 0xf4, 0x40, 0xf4, 0x2f, 0xf4, 0x1a, 0xf4, 0x1e, 0xf4, 0x02, 0xf4, 0xff, 0xf3,
		0x0b, 0xf4, 0xfb, 0xf3, 0xf1, 0xf3, 0xd9, 0xf3, 0xe2, 0xf3, 0xfd, 0xf3, 0xdf, 0xf3, 0xdc, 0xf3, 0xe6, 0xf3, 0xe5, 0xf3, 0x04, 0xf4, 0xec, 0xf3, 0xe5, 0xf3, 0x29, 0xf4, 0x1a, 0xf4, 0xea, 0xf3,
		0xe3, 0xf3, 0xe7, 0xf3, 0x12, 0xf4, 0x1e, 0xf4, 0x0f, 0xf4, 0x01, 0xf4, 0xe8, 0xf3, 0x0e, 0xf4, 0x09, 0xf4, 0xf4, 0xf3, 0x2a, 0xf4, 0xc1, 0xf3, 0x80, 0xf4, 0x68, 0xf8, 0x65, 0xfc, 0xb9, 0xfe,
		0xd3, 0x20, 0x5f, 0x02, 0x55, 0x03, 0x3f, 0x04, 0xc1, 0x04, 0x32, 0x05, 0x97, 0x05, 0xd1, 0x05, 0x24, 0x06, 0x2a, 0x06, 0x30, 0x06, 0x68, 0x06, 0x4d, 0x06, 0x6c, 0x06, 0xb1, 0x06, 0x97, 0x06,
		0xac, 0x06, 0xb2, 0x06, 0x84, 0x06, 0xa5, 0x06, 0xbe, 0x06, 0xb6, 0x06, 0xba, 0x06, 0xa8, 0x06, 0x99, 0x06, 0x91, 0x06, 0xab, 0x06, 0xbc, 0x06, 0x91, 0x06, 0xa4, 0x06, 0xad, 0x06, 0x61, 0x06,
		0x70, 0x06, 0xa5, 0x06, 0xa8, 0x06, 0xa9, 0x06, 0x93, 0x06, 0x8e, 0x06, 0x97, 0x06, 0xa0, 0x06, 0xbb, 0x06, 0xa5, 0x06, 0xbf, 0x06, 0xd0, 0x06, 0x92, 0x06, 0x1c, 0x07, 0x7c, 0x06, 0x81, 0x02,
		0x58, 0xfe, 0x04, 0xfc, 0x0e, 0xfa, 0x6e, 0xf8, 0x61, 0xf7, 0x8b, 0xf6, 0xeb, 0xf5, 0x61, 0xf5, 0x12, 0xf5, 0xd0, 0xf4, 0x83, 0xf4, 0x72, 0xf4, 0x39, 0xf4, 0x15, 0xf4, 0x4e, 0xf4, 0x2d, 0xf4,
		0xfb, 0xf3, 0x16, 0xf4, 0x12, 0xf4, 0x08, 0xf4, 0x04, 0xf4, 0xf8, 0xf3, 0xef, 0xf3, 0xe0, 0xf3, 0x03, 0xf4, 0x0d, 0xf4, 0xf5, 0xf3, 0x1d, 0xf4, 0x1a, 0xf4, 0x03, 0xf4, 0x0f, 0xf4, 0xe3, 0xf3,
		0xea, 0xf3, 0x0d, 0xf4, 0xf1, 0xf3, 0x08, 0xf4, 0x08, 0xf4, 0xf0, 0xf3, 0x09, 0xf4, 0xfe, 0xf3, 0x20, 0xf4, 0x33, 0xf4, 0xfe, 0xf3, 0x1f, 0xf4, 0xf0, 0xf3, 0xd9, 0xf3, 0x4a, 0xf4, 0xa5, 0xf3,
		0x80, 0xf4, 0x9c, 0xf9, 0xca, 0xfe, 0xc3, 0x01, 0x62, 0x04, 0x5d, 0x06, 0xa3, 0x07, 0xd6, 0x08, 0x8d, 0x09, 0x24, 0x0a, 0xb1, 0x0a, 0xe1, 0x0a, 0x29, 0x0b, 0x65, 0x0b, 0x76, 0x0b, 0xab, 0x0b,
		0xc4, 0x0b, 0xb7, 0x0b, 0xc4, 0x0b, 0xe0, 0x0b, 0xfa, 0x0b, 0xfa, 0x0b, 0x03, 0x0c, 0x1f, 0x0c, 0x1a, 0x0c, 0x1f, 0x0c, 0x2a, 0x0c, 0x1d, 0x0c, 0x20, 0x0c, 0x0c, 0x0c, 0x0d, 0x0c, 0x41, 0x0c,
		0x27, 0x0c, 0x01, 0x0c, 0x08, 0x0c, 0xe5, 0x0b, 0xf9, 0x0b, 0x1f, 0x0c, 0xf6, 0x0b, 0xfd, 0x0b, 0xfb, 0x0b, 0xe2, 0x0b, 0x0e, 0x0c, 0xef, 0x0b, 0xdd, 0x0b, 0x19, 0x0c, 0xe0, 0x0b, 0xe6, 0x0b,
		0x13, 0x0c, 0xce, 0x0b, 0x45, 0x0c, 0x9a, 0x0b, 0xa2, 0x07, 0xa6, 0x03, 0x5c, 0x01, 0x39, 0xff, 0xad, 0xfd, 0xbe, 0xfc, 0xb8, 0xfb, 0x1e, 0xfb, 0xb8, 0xfa, 0x58, 0xfa, 0x42, 0xfa, 0x03, 0xfa,
		0xba, 0xf9, 0x9f, 0xf9, 0x6f, 0xf9, 0x70, 0xf9, 0x91, 0xf9, 0x69, 0xf9, 0x3f, 0xf9, 0x3a, 0xf9, 0x2f, 0xf9, 0x27, 0xf9, 0x34, 0xf9, 0x41, 0xf9, 0x3e, 0xf9, 0x3e, 0xf9, 0x41, 0xf9, 0x45, 0xf9,
		0x52, 0xf9, 0x46, 0xf9, 0x31, 0xf9, 0x43, 0xf9, 0x5f, 0xf9, 0x6a, 0xf9, 0x62, 0xf9, 0x55, 0xf9, 0x4a, 0xf9, 0x41, 0xf9, 0x69, 0xf9, 0x7f, 0xf9, 0x45, 0xf9, 0x48, 0xf9, 0x5f, 0xf9, 0x47, 0xf9,
		0x68, 0xf9, 0x63, 0xf9, 0x53, 0xf9, 0x6f, 0xf9, 0xf5, 0xf8, 0xc8, 0xf9, 0x9f, 0xfd, 0x8b, 0x01, 0xfe, 0x03, 0x36, 0x06, 0xc3, 0x07, 0x9a, 0x08, 0x8a, 0x09, 0x41, 0x0a, 0xc1, 0x0a, 0x26, 0x0b
};

#endif