/*
 *  @Description:   Przykład definiowania uniwersalnych typów.
 */
#ifndef GENERAL_H
#define GENERAL_H

typedef unsigned char             tU8;
typedef unsigned short            tU16;
typedef unsigned int              tU32;
typedef unsigned long long        tU64;
typedef signed char               tS8;
typedef signed short              tS16;
typedef signed int                tS32;
typedef signed long long          tS64;
typedef enum{FALSE = 0, 
             TRUE  = !FALSE}      tBool;

#define BIT(n)                   (1u << (n))
//#define PIN(port,bit)            ((IOPIN##port & _BIT(bit)) != 0)

#endif  //__GENERAL_H__

