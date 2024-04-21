#include "i2c.h"
#include "general.h"
#include "functionals.h"
#include <lpc2xxx.h>

/* Defines and typedefs */

#define I2C_CONSET (*((volatile unsigned char *) 0xE001C000U))
#define I2C_STAT   (*((volatile unsigned char *) 0xE001C004U))
#define I2C_DATA   (*((volatile unsigned char *) 0xE001C008U))
#define I2C_ADDR   (*((volatile unsigned char *) 0xE001C00CU))
#define I2C_SCLH   (*((volatile unsigned short*) 0xE001C010U))
#define I2C_SCLL   (*((volatile unsigned short*) 0xE001C014U))
#define I2C_CONCLR (*((volatile unsigned char *) 0xE001C018U))


#define I2C_REG_CONSET      0x00000040 /* Control Set Register         */
#define I2C_REG_CONSET_MASK 0x0000007C /* Used bits                    */
#define I2C_REG_DATA        0x00000000 /* Data register                */
#define I2C_REG_DATA_MASK   0x000000FF /* Used bits                    */
#define I2C_REG_ADDR        0x00000000 /* Slave address register       */
#define I2C_REG_ADDR_MASK   0x000000FF /* Used bits                    */
#define I2C_REG_SCLH        0x00000100 /* SCL Duty Cycle high register */
#define I2C_REG_SCLH_MASK   0x0000FFFF /* Used bits                    */
#define I2C_REG_SCLL        0x00000100 /* SCL Duty Cycle low register  */
#define I2C_REG_SCLL_MASK   0x0000FFFF /* Used bits                    */

 /* Declarations */

tS8 i2cMyWrite(tU8 addr, tU8* pData, tU16 len);

//tS8 i2cWaitTransmit(void);

tS8 i2cWriteWithWait(tU8 data);

 /*
  *  @brief			Checks the I2C status.
  *
  *  @Description	Checks the I2C status.
  *
  *  @param 		brak
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
tU8
i2cCheckStatus(void)
{
  tU8 status = 0;

  /* wait for I2C Status changed */
  while( (I2C_CONSET & 0x08u)  == 0u)   /* while SI == 0 */
  {
    ;
  }

  /* Read I2C State */
  status = I2C_STAT;

  /* NOTE! SI flag is not cleared here */

  return status;
}

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
void
i2cInit(void)
{
  PINSEL0  |= 0x50;

  /* clear flags */
  I2C_CONCLR = 0x6c;    

  /* reset registers */
  I2C_SCLL   = ( I2C_SCLL   & ~I2C_REG_SCLL_MASK )   | I2C_REG_SCLL;
  I2C_SCLH   = ( I2C_SCLH   & ~I2C_REG_SCLH_MASK )   | I2C_REG_SCLH;
  I2C_ADDR   = ( I2C_ADDR   & ~I2C_REG_ADDR_MASK )   | I2C_REG_ADDR;
  I2C_CONSET = ( I2C_CONSET & ~I2C_REG_CONSET_MASK ) | I2C_REG_CONSET;
}

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
tS8 i2cStart(void)
{
  tU8   status  = 0;
  tS8   retCode = 0;

  /* issue a start condition */
  I2C_CONSET |= 0x20; /* STA = 1, set start flag */

  /* flag for continuation of doing while to specified status of i2c was met */
  tBool notTransmittedFlag = TRUE;

  /* wait until START transmitted */
  while(notTransmittedFlag == TRUE)
  {
    status = i2cCheckStatus();

    /* start transmitted */
    if((status == (tU8) 0x08) || (status == (tU8) 0x10))
    {
      retCode = I2C_CODE_OK;
      notTransmittedFlag = FALSE;
      //break;
    }

    /* error */
    else if(status != (tU8) 0xf8)
    {
      retCode = (tS8) status;
      notTransmittedFlag = FALSE;
      //break;
    }

    else
    {
      /* clear SI flag */
      I2C_CONCLR = 0x08;
    }    
  }

  /* clear start flag */
  I2C_CONCLR = 0x20;

  return retCode;
}

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
tS8 i2cRepeatStart(void)
{
  tU8   status  = 0;
  tS8   retCode = 0;

  /* issue a start condition */
  I2C_CONSET |= 0x20; /* STA = 1, set start flag */
  I2C_CONCLR = 0x08;  /* clear SI flag           */

  /* flag for continuation of doing while to specified status of i2c was met */
  tBool notTransmittedFlag = TRUE;

  /* wait until START transmitted */
  while(notTransmittedFlag == TRUE)
  {
    status = i2cCheckStatus();

    /* start transmitted */
    if((status == (tU8) 0x08) || (status == (tU8) 0x10))
    {
      retCode = I2C_CODE_OK;
      notTransmittedFlag = FALSE;
      //break;
    }

    /* error */
    else if(status != (tU8) 0xf8)
    {
      retCode = (tS8) status;
      notTransmittedFlag = FALSE;
      //break;
    }

    else
    {
      /* clear SI flag */
      I2C_CONCLR = 0x08;
    }    
  }

  /* clear start flag */
  I2C_CONCLR = 0x20;

  return retCode;
}

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
tS8 i2cStop(void)
{
  I2C_CONSET |= 0x10; /* STO = 1, set stop flag */ 
  I2C_CONCLR = 0x08;  /* clear SI flag          */ 

  /* wait for STOP detected (while STO = 1) */
  while((I2C_CONSET & 0x10) == 0x10)
  {
    /* do nothing */
    ;
  }

  return I2C_CODE_OK;
}

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
tS8 i2cPutChar(tU8 data)
{
  tS8 retCode = 0;

  /* check if I2C Data register can be accessed */
  if((I2C_CONSET & 0x08) != 0)  /* if SI = 1 */
  {
    /* send data */
    I2C_DATA   = data;
    I2C_CONCLR = 0x08; /* clear SI flag */ 
    retCode    = I2C_CODE_OK;
  }
  else
  {
    /* data register not ready */
    retCode = I2C_CODE_BUSY;
  }

  return retCode;
}

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
tS8 i2cGetChar(tU8  mode,
           tU8* pData)
{
  tS8 retCode = I2C_CODE_OK;

  if(mode == (tU8) I2C_MODE_ACK0)
  {
    /* the operation mode is changed from master transmit to master receive */

    /* set ACK=0 (informs slave to send next byte) */

    I2C_CONSET |= 0x04; /* AA=1          */
    I2C_CONCLR = 0x08;  /* clear SI flag */   
  }
  else if(mode == (tU8) I2C_MODE_ACK1)
  {
    /* set ACK=1 (informs slave to send last byte) */
    I2C_CONCLR = 0x04;     
    I2C_CONCLR = 0x08; /* clear SI flag */ 
  }
  else if(mode == (tU8) I2C_MODE_READ)
  {
    /* check if I2C Data register can be accessed */
    if((I2C_CONSET & 0x08) != 0)  /* SI = 1 */
    {
      /* read data  */
      *pData = (tU8) I2C_DATA;
    }
    else
    {
      /* No data available */
      retCode = I2C_CODE_EMPTY;
    }
  }
  /* do nothing else */
  else
  {
    ;
  }

  return retCode;
}

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
tS8 i2cWrite(tU8  addr,
         tU8* pData,
         tU16 len)
{
  tS8   retCode = 0;
  tU8   status  = 0;
  tU16  i       = 0;
  tU8*  sData   = pData;
  

  /* generate Start condition */
  retCode = i2cStart();

  /* Transmit address */
  if(retCode == I2C_CODE_OK)
  {
    /* write SLA+W */
    retCode = i2cPutChar(addr);
    while(retCode == I2C_CODE_BUSY)
    {
      retCode = i2cPutChar(addr);
    }
  }

  if(retCode == I2C_CODE_OK)
  {
    /* flag for earlier ending for loop when error occured */
    tBool endFlag = FALSE;
    /* wait until address transmitted and transmit data */
    for(i = 0; (i < len) && !endFlag; i++)
    {
      /* flag for continuation of doing while loop and ending it when specified status of i2c occured */
      tBool innerContinueFlag = TRUE;
      /* wait until data transmitted */
      while(innerContinueFlag == TRUE)
      {
        /* get new status */
        status = i2cCheckStatus();

        /* 
         * SLA+W transmitted, ACK received or
         * data byte transmitted, ACK received
         */
        if( (status == (tU8) 0x18) || (status == (tU8) 0x28) )
        {
          /* Data transmitted and ACK received */

          /* write data */
          retCode = i2cPutChar(*sData);
          while(retCode == I2C_CODE_BUSY)
          {
            retCode = i2cPutChar(*sData);
          }
          sData++;

          innerContinueFlag = FALSE;
          //break;
        }

        /* no relevant status information */
        else if( status != (tU8) 0xf8 )
        {
          /* error */
          //i = len;
          endFlag = TRUE;
          retCode = I2C_CODE_ERROR;
          innerContinueFlag = FALSE;
          //break;
        }

        /* do nothing else */
        else
        {
            ;
        }
      }
    }
  }

  /* flag for continuation of doing while to specified status of i2c was met */
  tBool notTransmittedFlag = TRUE;

  /* wait until data transmitted */
  while(notTransmittedFlag == TRUE)
  {
    /* get new status */
    status = i2cCheckStatus();

    /* 
     * SLA+W transmitted, ACK received or
     * data byte transmitted, ACK received
     */
    if( (status == (tU8) 0x18) || (status == (tU8) 0x28) )
    {
      /* data transmitted and ACK received */
      notTransmittedFlag = FALSE;
      //break;
    }

    /* no relevant status information */
    else if(status != (tU8) 0xf8 )
    {
      /* error */
      i = len;
      retCode = I2C_CODE_ERROR;
      notTransmittedFlag = FALSE;
      //break;
    }

    /* do nothing else */
    else
    {
      ;
    }
  }

  /* generate Stop condition */
  (void) i2cStop();

  return retCode;
}

static tS8
i2cWaitTransmit(void)
{
  tU8 status            = 0;
  tS8 returnedStatus    = 0;

  /* flag for continuation of doing while to specified status of i2c was met */
  tBool notTransmittedFlag = TRUE;

  /* wait until data transmitted */
  while(notTransmittedFlag == TRUE)
  {
    /* get new status */
    status = i2cCheckStatus();

    /* 
     * SLA+W transmitted, ACK received or
     * data byte transmitted, ACK received
     */
    if( (status == (tU8) 0x18) || (status == (tU8) 0x28) )
    {
      /* data transmitted and ACK received */
      //return I2C_CODE_OK;
      returnedStatus        = I2C_CODE_OK;
      notTransmittedFlag    = FALSE;
    }

    /* no relevant status information */
    else if(status != (tU8) 0xf8 )
    {
      /* error */
      //return I2C_CODE_ERROR;
      returnedStatus = I2C_CODE_ERROR;
      notTransmittedFlag = FALSE;
    }

    /* do nothing else */
    else
    {
      ;
    }
  }
  return returnedStatus;
}

tS8
i2cWriteWithWait(tU8 data)
{
  tS8 retCode = 0;
  
  retCode = i2cPutChar(data);
  while(retCode == I2C_CODE_BUSY)
  {
    retCode = i2cPutChar(data);
  }

  if (retCode == I2C_CODE_OK)
  {
    retCode = i2cWaitTransmit();
  }

  return retCode;
}

tS8
i2cMyWrite(tU8  addr,
           tU8* pData,
           tU16 len)
{
  tS8   retCode = 0;
  tU8   i       = 0;
  tU8*  sData   = pData;

  do
  {
    /* generate Start condition */
    retCode = i2cStart();
    if (retCode != I2C_CODE_OK) {
      break;
    }
    /* write address */
    retCode = i2cWriteWithWait(addr);
    if (retCode == I2C_CODE_OK) {
      for (i = 0; i < len; i++)
      {
        retCode = i2cWriteWithWait(*sData);
        if (retCode != I2C_CODE_OK) {
            break;
        }

        sData++;
      }
    }

  } while(0);

  /* generate Stop condition */
  (void) i2cStop();


  return retCode;
}


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
tS8 i2cRead(tU8  addr,
        tU8* pBuf,
        tU16 len)
{
  tS8   retCode = 0;
  tU8   status  = 0;
  tU16  i       = 0;
  tU8*  gBuf     = pBuf;

  /* Generate Start condition */
  retCode = i2cStart();

  /* Transmit address */
  if(retCode == I2C_CODE_OK )
  {
    /* write SLA+R */
    retCode = i2cPutChar(addr);
    while(retCode == I2C_CODE_BUSY)
    {
      retCode = i2cPutChar(addr);
    }
  }

  if(retCode == I2C_CODE_OK )
  {
    /* flag for earlier ending for loop when error occured */
    tBool endFlag = FALSE;
    /* wait until address transmitted and receive data */
    for(i = 1; (i <= len) && !endFlag; i++ )
    {
      /* flag for continuation of doing while loop and ending it when specified status of i2c occured */
      tBool innerContinueFlag = TRUE;
      /* wait until data transmitted */
      while(innerContinueFlag == TRUE)
      {
        /* get new status */
        status = i2cCheckStatus();

        /*
         * SLA+R transmitted, ACK received or
         * SLA+R transmitted, ACK not received
         * data byte received in master mode, ACK transmitted
         */
        if((status == (tU8) 0x40 ) || (status == (tU8) 0x48 ) || (status == (tU8) 0x50 ))
        {
          /* data received */

          if(i == len)
          {
            /* Set generate NACK */
            retCode = i2cGetChar(I2C_MODE_ACK1, gBuf);
          }
          else
          {
            retCode = i2cGetChar(I2C_MODE_ACK0, gBuf);
          }

          /* Read data */
          retCode = i2cGetChar(I2C_MODE_READ, gBuf);
          while(retCode == I2C_CODE_EMPTY)
          {
            retCode = i2cGetChar(I2C_MODE_READ, gBuf);
          }
          gBuf++;

          innerContinueFlag = FALSE;
          //break;
        }

        /* no relevant status information */
        else if(status != (tU8) 0xf8 )
        {
          /* error */
          //i = len;
          endFlag = TRUE;
          retCode = I2C_CODE_ERROR;
          innerContinueFlag = FALSE;
          //break;
        }
        
        /* do nothing else */
        else
        {
          ;
        }
      }
    }
  }

  /*--- Generate Stop condition ---*/
  (void) i2cStop();

  return retCode;
}



