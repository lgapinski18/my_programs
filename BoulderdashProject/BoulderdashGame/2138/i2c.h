#ifndef I2C_H
#define I2C_H

#include "general.h"

/* modes */
#define I2C_MODE_ACK0 0
#define I2C_MODE_ACK1 1
#define I2C_MODE_READ 2

/* return codes */
#define I2C_CODE_OK   1
#define I2C_CODE_DATA 2
#define I2C_CODE_RTR  3

#define I2C_CODE_ERROR -1
#define I2C_CODE_FULL  -2
#define I2C_CODE_EMPTY -3
#define I2C_CODE_BUSY  -4



#define I2C_SLAVEADR_RCV  0xA1
#define I2C_SLAVEADR_SEND 0xA0

/* I2C Control set register */

#define I2C_CONSET_I2EN              0x40   /* Bit 6: I2EN (I2C interface enable) */
#define I2C_CONSET_I2EN_DISABLED     0x00   /* Disabled */
#define I2C_CONSET_I2EN_ENABLED      0x01   /* Enabled */

#define I2C_CONSET_STA               0x20   /* Bit 5: STA (Start flag) */
#define I2C_CONSET_STA_NO_START      0x00   /* No start */
#define I2C_CONSET_STA_START         0x01   /* Start */

#define I2C_CONSET_STO               0x10   /* Bit 4: STO (Stop flag) */
#define I2C_CONSET_STO_NO_STOP       0x00   /* No stop */
#define I2C_CONSET_STO_STOP          0x01   /* Stop */

#define I2C_CONSET_SI                0x08   /* Bit 3: SI (I2C interrupt flag) */
#define I2C_CONSET_SI_NO_INTERRUPT   0x00   /* No interrupt */
#define I2C_CONSET_SI_INTERRUPT      0x01   /* Interrupt */

#define I2C_CONSET_AA                0x04   /* Bit 2: AA (Assert acknowledge flag) */
#define I2C_CONSET_AA_NO_ACK         0x00   /* Not Acknowledge (Not Acknowledge will be sent when data byte is received) */
#define I2C_CONSET_AA_ACK            0x01   /* Acknowledge (Acknowledge will be sent if certain conditions occur) */

/* I2C status register */
#define I2C_STAT_STATUS              0xff   /* Bit 7-0: STATUS (Status) */

/* I2C data register */

#define I2C_DATA_DATA                0xff   /* Bit 7-0: DATA (Data) */


/* I2C Slave Address Register */

#define I2C_ADDR_ADDR                0xfe   /* Bit 7-1: ADDR (Slave mode address) */

#define I2C_ADDR_GC                  0x01   /* Bit 0: GC (General call bit) */
#define I2C_ADDR_GC_NOT_GENERAL      0x00   /* Not general call */
#define I2C_ADDR_GC_GENERAL          0x01   /* General call received */

/* I2C SCL Duty Cycle high register */
#define I2C_SCLH_COUNT               0xffff /* Bit 15-0: COUNT (Count for SCL high time period) */

/*  I2C SCL Duty Cycle Low Register */

#define I2SCLL_COUNT               0xffff /* Bit 15-0: COUNT (Count for SCL low time period) */


/*  I2C Control Clear Register */

#define I2C_CONCLR_I2ENC             0x40   /* Bit 6: I2ENC (I2C interface disable) */
#define I2C_CONCLR_I2ENC_NO_EFFECT   0x00   /* No effect */
#define I2C_CONCLR_I2ENC_CLEAR       0x01   /* Clear  (I2C Disabled) */

#define I2C_CONCLR_STAC              0x20   /* Bit 5: STAC (Start flag clear) */
#define I2C_CONCLR_STAC_NO_EFFECT    0x00   /* No effect */
#define I2C_CONCLR_STAC_CLEAR        0x01   /* Clear start */

#define I2C_CONCLR_SIC               0x08   /* Bit 3: SIC (I2C interrupt clear) */
#define I2C_CONCLR_SIC_NO_EFFECT     0x00   /* No effect */
#define I2C_CONCLR_SIC_CLEAR         0x01   /* Clear interrupt */

#define I2C_CONCLR_AAC               0x04   /* Bit 2: AAC (Assert acknowledge clear) */
#define I2C_CONCLR_AAC_NO_EFFECT     0x00   /* No effect */
#define I2C_CONCLR_AAC_CLEAR         0x01   /* Clear acknowledge */



/*
 *  @brief			Checks the I2C status.
 *
 *  @Description	Checks the I2C status.
 *
 *  @param 			brak
 *
 *  @returns  		00h Bus error
 *                  08h START condition transmitted
 *                  10h Repeated START condition transmitted
 *                  18h SLA + W transmitted, ACK received
 *                  20h SLA + W transmitted, ACK not received
 *                  28h Data byte transmitted, ACK received
 *                  30h Data byte transmitted, ACK not received
 *                  38h Arbitration lost
 *                  40h SLA + R transmitted, ACK received
 *                  48h SLA + R transmitted, ACK not received
 *                  50h Data byte received in master mode, ACK transmitted
 *                  58h Data byte received in master mode, ACK not transmitted
 *                  60h SLA + W received, ACK transmitted
 *                  68h Arbitration lost, SLA + W received, ACK transmitted
 *                  70h General call address received, ACK transmitted
 *                  78h Arbitration lost, general call addr received, ACK transmitted
 *                  80h Data byte received with own SLA, ACK transmitted
 *                  88h Data byte received with own SLA, ACK not transmitted
 *                  90h Data byte received after general call, ACK transmitted
 *                  98h Data byte received after general call, ACK not transmitted
 *                  A0h STOP or repeated START condition received in slave mode
 *                  A8h SLA + R received, ACK transmitted
 *                  B0h Arbitration lost, SLA + R received, ACK transmitted
 *                  B8h Data byte transmitted in slave mode, ACK received
 *                  C0h Data byte transmitted in slave mode, ACK not received
 *                  C8h Last byte transmitted in slave mode, ACK received
 *                  F8h No relevant status information, SI=0
 *                  FFh Channel error
  *
  *  @side effects	brak
  */
tU8 i2cCheckStatus(void);

/*
 *  @brief			Reset the I2C module.
 *
 *  @Description	Reset the I2C module.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void i2cInit(void);

/*
 *  @brief			Generates a start condition on I2C when bus is free.
 *
 *  @Description	Generates a start condition on I2C when bus is free.
 *                  Master mode will also automatically be entered.
 *
 *                  Note: After a stop condition, you may need a bus free time before you
 *                        can generate a new start condition.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
tS8 i2cStart(void);

/*
 *  @brief			Generates a start condition on I2C when bus is free.
 *
 *  @Description	Generates a start condition on I2C when bus is free.
 *                  Master mode will also automatically be entered.
 *
 *                  Note: After a stop condition, you may need a bus free time before you
 *                        can generate a new start condition.
 *
 *  @param 			brak
 *
 *  @returns  		I2C_CODE_OK or I2C status code
 *
 *  @side effects	brak
 */
tS8 i2cRepeatStart(void);

/*
 *  @brief			Generates a stop condition in master mode or recovers from an error
 *                  condition in slave mode.
 *
 *  @Description	Generates a stop condition in master mode or recovers from an error
 *                  condition in slave mode.
 *
 *                   Note: After this function is run, you may need a bus free time before
 *                         you can generate a new start condition.
 *
 *  @param 			brak
 *
 *  @returns  		I2C_CODE_OK
 *
 *  @side effects	brak
 */
tS8 i2cStop(void);

/*
 *  @brief			Sends a character on the I2C network
 *
 *  @Description	Sends a character on the I2C network
 *
 *  @param 			[in]    data
 *                          the character to send
 *
 *  @returns  		I2C_CODE_OK   - successful
 *                  I2C_CODE_BUSY - data register is not ready -> byte was not sent
 *
 *  @side effects	brak
 */
tS8 i2cPutChar(tU8 data);

/*
 *  @brief			Read a character.
 *
 *  @Description	Read a character. I2C master mode is used.
 *                  This function is also used to prepare if the master shall generate
 *                  acknowledge or not acknowledge.
 *
 *  @param 			[in]  mode
 *                      I2C_MODE_ACK0 Set ACK=0. Slave sends next b
 *                      I2C_MODE_ACK1 Set ACK=1. Slave sends last b
 *                      I2C_MODE_READ Read data from data register
 *
 *  @param 			[out] pData
 *                  a pointer to where the data shall be saved.
 *
 *  @returns  		I2C_CODE_OK    - successful
 *                  I2C_CODE_EMPTY - no data is available
 *
 *  @side effects	brak
 */
tS8 i2cGetChar(tU8  mode, tU8* pData);

/*
 *  @brief			Sends data on the I2C network
 *
 *  @Description	Sends data on the I2C network
 *
 *                  Note: After this function is run, you may need a bus free time before a
 *                       new data transfer can be initiated.
 *
 *  @param 			[in] addr
 *                      address
 *
 *  @param 			[in] pData
 *                      data to transmit

 *  @param 			[in] len
 *                      number of bytes to transmitnic
 *
 *  @returns  	    I2C_CODE_OK    - successful
 *                  I2C_CODE_ERROR - an error occured
 *
 *  @side effects	brak
 */
tS8 i2cWrite(tU8  addr, tU8* pData, tU16 len);

/*
 *  @brief			Read a specified number of bytes from the I2C network.
 *
 *  @Description	Read a specified number of bytes from the I2C network.
 *
 *                  Note: After this function is run, you may need a bus free time before a
 *                        new data transfer can be initiated.
 *
 *  @param 			[in] addr
 *                      address
 *
 *  @param          [in] pBuf
 *                      receive buffer
 *
 *  @param          [in] len
 *                      number of bytes to receive
 *
 *  @returns  		I2C_CODE_OK or I2C status code
 *
 *  @side effects	brak
 */
tS8 i2cRead(tU8  addr, tU8* pBuf, tU16 len);




#endif
