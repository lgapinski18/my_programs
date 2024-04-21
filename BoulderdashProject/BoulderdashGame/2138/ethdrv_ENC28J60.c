/*
 *  @Descritpion:   Zawariera zbior definicji funkcji uzywanych przy
 * 				    obsludze kontrolera ENC28J60.
 */

#include "ethdrv_ENC28J60.h"
#include "functionals.h"
#include "watchdog.h"
#include <lpc2xxx.h>
#include <printf_P.h>

extern tU8 SOURCE_MAC[6];
extern tU8 TARGET_MAC[6];
extern tU8 SOURCE_IP[4];
extern tU8 TARGET_IP[4];
tU8 SOURCE_MAC[6] = {0x08, 0x97, 0x98, 0xE9, 0xF2, 0x8C}; /* adres MAC hosta zrodlowego */
tU8 TARGET_MAC[6] = {0x58, 0x8A, 0x5A, 0x46, 0x85, 0xC6}; /* adres MAC hosta docelowego */
tU8 SOURCE_IP[4] = {169, 254, 206, 236}; /* adres IP hosta zrodlowego */
tU8 TARGET_IP[4] =	{169, 254, 206, 237}; /* adres IP hosta docelowego */

static tU16 checksum(tU8* data, tU16 offset, tU16 length);
static void WriteBufferMemory(tU8* data, tU16 dataSize);
/* Jedynka 32-bitowa */
static const tU32 one32 = 1u;


/*
 *  @brief			Funkcja realizujaca wybor kontrolera ENC28J60 dla magistrali SPI
 *
 *  @Description	Funkcja realizujaca wybor kontrolera ENC28J60 dla magistrali SPI po przez ustawienie
 *                  stanu pinu P0.14 na 0.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	modyfikacja rejestrow IODRI i IOCLR
 */
void selectEthSpi(void) {
    IOCLR  = one32 << ETH_CS;

    /* oczekiwanie na ustawie sie odpowiedniego chip select */
    delay_ms(WAITING_TIME);
}

/*
 *  @brief			Funkcja realizujaca wylaczenie wyboru kontrolera ENC28J60 dla magistrali SPI
 *
 *  @Description	Funkcja realizujaca wlaczenie wyboru kontrolera ENC28J60 dla magistrali SPI po przez ustawienie
 *                  stanu pinu P0.14 na 1.
 * 
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	modyfikacja rejestrow IODRI i IOCLR
 */
void unselectEthSpi(void) {
    IOSET  = one32 << ETH_CS;

    /* oczekiwanie na ustawie sie odpowiedniego chip select */
    delay_ms(WAITING_TIME);
}

/*
 *  @brief			Funkcja odczytujaca liczaca sume kontrolna
 *
 *  @Description	Funkcja obliczajaca sume kontrolna wedlug wymaganej przez naglowki metody.
 * 
 *  @param 			[in]    data
 *                      tablica danych, dla ktorych ma zostac policzona suma kontrolna
 * 
 *  @param 			[in]    offset
 *                      przesuniecie oblicen od poczatku tablicy
 * 
 *  @param 			[in]    length
 *                      liczba danych od poczatku tablicy, dla ktorych ma policzona suma
 *
 *  @returns  		nic
 *
 *  @side effects	wartosc sumy kontrolnej dla podanych danych
 */
tU16 checksum(tU8* data, tU16 offset, tU16 length) {
	tU32 chsum = 0;

	for (tU16 i = 0; i < length; i += 2u)
	{
		chsum += (data[offset + i + 1u] << 8) | data[offset + i];
	}

	while ((chsum >> 16) != 0u)
	{
		chsum = (chsum & 0xffffu) + (chsum >> 16);
	}

	return ~chsum;
}

/*
 *  @brief			Funkcja zmieniajaca aktualny Bank
 *
 *  @Description	Funkcja wywoluje na rzecz urzadzenia ENC28J60 polecenie SPI Write Control Register
 *                  z adresem rejestru ECON1 ustwiajac 2 najmlodsze bity na wartosc wybranegoo Banku.
 *
 *  @param 			[in]    bank
 *                      numer banku, z ktorego ma zostac odczytana wartosc rejestru wskazanego przez adres
 *
 *  @returns  		nic
 *
 *  @side effects	modyfikacja rejestru ECON1 kontrolera ENC28J60
 */
void changeBank(Bank bank) {
	tU8 econ1 = ReadControlRegister(ECON1);
	econ1 = (tU8)((econ1 & 0xFCu) | (tU8)bank);
	WriteControlRegister(ECON1, econ1);
}

/*
 *  @brief			Funkcja do transmisji bajtu danych magistrala SPI
 *
 *  @Description	Funkcja wykonuje wyslanie przekazanego bajtu magistrala SPI do kontrolera Ethernet
 *                  oraz zwraca wartosc, ktora po transmisji znajdzie sie w rejestrze SPDR.
 *
 *  @param 			[in]    byte
 *                     bajt danych, ktory ma zostac przeslany przez SPI
 *
 *  @returns  		odczytana wartosc z rejestru SPDR
 *
 *  @side effects	odczytana wartosc z rejestru SPDR
 */
tU8 sendSpi(tU8 byte)
{
    tU8  receivedByte;

	delay_ms(WAITING_TIME);

    SPI_SPDR = byte;

    while((SPI_SPSR & 0x80) == 0) {
    	;
    }

    receivedByte = SPI_SPDR;

    return receivedByte;
}

/*
 *  @brief			Funkcja odczytujaca wartosc rejestru ETH kontrolera ENC28J60
 *
 *  @Description	Funkcja wywoluje na rzecz urzadzenia ENC28J60 polecenie SPI Read Control Register
 *                  z adressem rejestru ETH rownym przekazanej wartosci.
 *
 *  @param 			[in]    RegisterAddr
 *                     adres rejestru kontrolnego, z ktorego ma zostac odczytana wartosc
 *
 *  @returns  		odczytana wartosc z rejestru wybranego rejestru
 *
 *  @side effects	modyfikacja rejestrow SPI_PCCR, SPI_SPCR i SPI_SPDR (wywolanie funkcji sendSpi)
 */
tU8 ReadControlRegister(tU8 RegisterAddr) {
	selectEthSpi();

	tU8 receivedData;

	/* utworzenie bajtu polecenia z kodem instrukcji i argumentem */
	tU8 command = RCR | RegisterAddr;

	/* Przeslanie bajtu informacji o wybranym poleceni RCR i adresie rejestru, ktorego zawartosc bedzie odczytana, wartosc zwracana zostanie zignorowana */
	receivedData = sendSpi(command);

	/* odczytanie stanu rejestru */
	receivedData = sendSpi(0);

	unselectEthSpi();

	return receivedData;
}

/*
 *  @brief			Funkcja odczytujaca wartosc rejestru MAC lub MII kontrolera ENC28J60
 *
 *  @Description	Funkcja wywoluje na rzecz urzadzenia ENC28J60 polecenie SPI Read Control Register
 *                  z adressem rejestru MAC lub MII rownym przekazanej wartosci.
 *
 *  @param 			[in]    RegisterAddr
 *                     adres rejestru kontrolnego, z ktorego ma zostac odczytana wartosc
 *
 *  @returns  		odczytana wartosc z rejestru wybranego rejestru
 *
 *  @side effects	modyfikacja rejestrow SPI_PCCR, SPI_SPCR i SPI_SPDR (wywolanie funkcji sendSpi)
 */
tU8 ReadControlRegisterMacMii(tU8 RegisterAddr) {
	selectEthSpi();

	tU8 receivedData;

	/* utworzenie bajtu polecenia z kodem instrukcji i argumentem */
	tU8 command = RCR | RegisterAddr;

	/* Przeslanie bajtu informacji o wybranym poleceni RCR i adresie rejestru, ktorego zawartosc bedzie odczytana, wartosc zwracana zostanie zignorowana */
	receivedData = sendSpi(command);

	/* odczytanie stanu rejestru */
	receivedData = sendSpi(0);
	receivedData = sendSpi(0);

	unselectEthSpi();

	return receivedData;
}

/*
 *  @brief			Funkcja zapisujaca wartosc do rejestru kontrolera ENC28J60
 *
 *  @Description	Funkcja wywoluje na rzecz urzadzenia ENC28J60 polecenie SPI Write Control Register
 *                  z adressem rejestru rownym przekazanej warto�ci z wartoscia wrowadzanego stanu rowna value.
 *
 *  @param 			[in]    RegisterAddr
 *                    adres rejestru kontrolnego, do ktorego ma zostac zapisana wartosc
 *
 *  @param 			[in]    value
 *                    wartosc zapisywana do rejestru
 *
 *  @returns  		odczytana wartosc z rejestru wybranego rejestru
 *
 *  @side effects	modyfikacja wybranego rejestru kontrolnego kontrolera ENC28J60
 *                  modyfikacja rejestrow SPI_PCCR, SPI_SPCR i SPI_SPDR (wywolanie funkcji sendSpi)
 */
void WriteControlRegister(tU8 RegisterAddr, tU8 value) {
	selectEthSpi();

	/* utworzenie bajtu polecenia z kodem instrukcji i argumentem */
	tU8 command = WCR | RegisterAddr;

	/* Przeslanie bajtu informacji o wybraniu polecenia WCR i rejestru o adresie RegisterAddr */
	tU8 returnValue = sendSpi(command);

	/* Przeslanie bajtu informacji bedacego nowym stanem wczesniej wybranego rejetru */
    returnValue = sendSpi(value);

    /* Po to aby zmienna nie byla zglaszana jako nieuzywana */
    returnValue += 1u;

	unselectEthSpi();
}

/*
 *  @brief			Funkcja zapisujaca dane do bufforu transmisyjnego kontrolera ENC28J60
 *
 *  @Description	Funkcja wywoluje na rzecz urzadzenia ENC28J60 polecenie SPI Write Buffer Memory
 *                  wysylajac jako dane kolejne bajty z tablicy data.
 *
 *  @param 			[in]    data
 *                    tablica bajtow, ktore maja zostac wpisane do pamieci bufora
 *
 *  @param 			[in]    dataSize
 *                    rozmiar wprowadzany danych
 *
 *  @returns  		odczytana wartosc z rejestru wybranego rejestru
 *
 *  @side effects	modyfikacja wybranego rejestru kontrolnego kontrolera ENC28J60
 *                  modyfikacja rejestrow SPI_PCCR, SPI_SPCR i SPI_SPDR (wywolanie funkcji sendSpi)
 */
void WriteBufferMemory(tU8* data, tU16 dataSize) {
	selectEthSpi();

	/* Przeslanie bajtu informacji o wybraniu polecenia WBM */
    tU8 returnValue = sendSpi(WBM);

	for(tU16 i = 0u; i < dataSize; i++) {
		/* Przeslanie bajtu z danych do pamieci bufora */
        returnValue = sendSpi(data[i]);
	}

    /* Po to aby zmienna nie byla zglaszana jako nieuzywana */
    returnValue += 1u;

	unselectEthSpi();
}

/*
 *  @brief			Funkcja realizujaca inicjalizacje kontrolera ENC28J60
 *
 *  @Description	Funkcja wywoluje na rzecz urzadzenia ENC28J60 polecenie SPI Write Buffer Memory
 *                  wysylajac jako dane kolejne bajty z tablicy data.
 * 
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	modyfikacja rejestru ECON1 kontrolera ENC28J60
 *                  modyfikacja rejestru ECON2 kontrolera ENC28J60
 *                  modyfikacja rejestrow ERXST i ERXND kontrolera ENC28J60
 *                  modyfikacja rejestru MACON1 kontrolera ENC28J60
 *                  modyfikacja rejestru MACON3 kontrolera ENC28J60
 *                  modyfikacja rejestru MAMXFL kontrolera ENC28J60
 *                  modyfikacja rejestru MABBIPG kontrolera ENC28J60
 *                  modyfikacja rejestru MAIPG kontrolera ENC28J60
 *                  modyfikacja rejestrow MAADR1 - MAADR6 kontrolera ENC28J60
 */
void initENC28J60(void) {
    IODIR |= one32 << ETH_CS;

    changeBank(BANK0);

    /* Ustawienie rejestrow ERXST */
    WriteControlRegister(ERXSTL, ERXST_VALUE & 0xFFu);
    WriteControlRegister(ERXSTH, (ERXST_VALUE & 0xFF00u) >> 8);

    /* Ustawienie rejestrow ERXND */
    WriteControlRegister(ERXNDL, ERXND_VALUE & 0xFFu);
    WriteControlRegister(ERXNDH, (ERXND_VALUE >> 8) & 0xFFu);

    /* Ustawienie bitu AUTOINC w rejestrze ECON2 */
    tU8 econ2 = ReadControlRegister(ECON2);
    econ2 |= 1u << AUTOINC;
    WriteControlRegister(ECON2, econ2);

    delay_ms(WAITING_TIME);


    changeBank(BANK2);

    /* Ustawienie bitow MARXEN, TXPAUS i RXPAUS w rejestrze MACON1 */
    tU8 macon1 = ReadControlRegisterMacMii(MACON1);
    macon1 |= (1u << MARXEN) | (1u << TXPAUS) | (1u << RXPAUS);
    WriteControlRegister(MACON1, macon1);

    /* Ustawienie bitow TXCRCEN, PADCFG0 i FULDPX w rejestrze MACON3 */
    tU8 macon3 = ReadControlRegisterMacMii(MACON3);
    macon3 |= 0xEu | (1u << FULDPX) | (1u << TXCRCEN); // | (1 << PADCFG0);
    WriteControlRegister(MACON3, macon3);

    /* Ustawienie rejestrow MAMXFL */
    WriteControlRegister(MAMXFLL, MAMXFL_VALUE & 0xFFu);
    WriteControlRegister(MAMXFLH, (MAMXFL_VALUE >> 8) & 0xFFu);

    /* Ustawienie rejestru MABBIPG */
    WriteControlRegister(MABBIPG, MABBIPG_VALUE);

    /* Ustawienie rejestru MAIPGL */
    WriteControlRegister(MAIPGL, MAIPGL_VALUE);


    changeBank(BANK3);

    /* Ustawienie rejestrow adresu MAC */
    WriteControlRegister(MAADR1, SOURCE_MAC[0]);
    WriteControlRegister(MAADR2, SOURCE_MAC[1]);
    WriteControlRegister(MAADR3, SOURCE_MAC[2]);
    WriteControlRegister(MAADR4, SOURCE_MAC[3]);
    WriteControlRegister(MAADR5, SOURCE_MAC[4]);
    WriteControlRegister(MAADR6, SOURCE_MAC[5]);
}

/*
 *  @brief			Funkcja realizujaca wyslananie za pomoca kontrolera ENC28J60 przekazanych danych protokolem UDP
 *
 *  @Description	Fukncja wykonuje utworzenie tablicy danych stanowiaca ramke Ethernetowa (w zgodzie
 *                  z przyjetym oficjalnie formatem) + pierwszy element to bajt kontrolny pakietu.
 *                  Nastepnie wprowadza te ramke do buffora transmisyjnego (funkcja WriteBufferMemory)
 *                  i wlacza transmisje pakietu, oczekujac na wynik transmisji. Funkcja ta po przez
 *                  zwrocona wartosc informuje o powodzeniu transmisji.
 * 
 *  @param 			[in]    result
 *                      wynik, ktory ma zostac przeslany
 *
 *  @returns  		odczytana wartosc z rejestru wybranego rejestru
 *
 *  @side effects	modyfikacja rejestru ECON1 kontrolera ENC28J60
 *                  modyfikacja rejestrow ETXST, ETXND i EWRPT kontrolera ENC28J60
 */
tBool sendResult(tU8 tensDigit, tU8 unitDigit) {
	touchWatchdog();
	/* tablica danych zawierajaca bajty ktore maja zostac wpisane do pamieci bufora */ //TYPE_MAC
	tU8 dataToSend[PACKET_SIZE + 1u] = {
			PACKET_CON_B,
			TARGET_MAC[0], TARGET_MAC[1], TARGET_MAC[2], TARGET_MAC[3], TARGET_MAC[4], TARGET_MAC[5],
			SOURCE_MAC[0], SOURCE_MAC[1], SOURCE_MAC[2], SOURCE_MAC[3], SOURCE_MAC[4], SOURCE_MAC[5],
			(TYPE_MAC >> 8) & 0xFFu, TYPE_MAC & 0xFFu,
			(IP_V << 4) | HEADER_LEN, ((DSCP << 2) | ECN) & 0xFFu, (TOTAL_LEN & 0xFF00u) >> 8, TOTAL_LEN & 0xFFu,
			(IP_DAT_ID >> 8) & 0xFFu, IP_DAT_ID & 0xFFu, (FLAGS << 5) | OFFSET, OFFSET & 0xFFu,
			TTL, PROT_TYPE, 0, 0,
			SOURCE_IP[0], SOURCE_IP[1], SOURCE_IP[2], SOURCE_IP[3],
			TARGET_IP[0], TARGET_IP[1], TARGET_IP[2], TARGET_IP[3],
			(SOURCE_PORT >> 8) & 0xFFu, SOURCE_PORT & 0xFFu, (TARGET_PORT >> 8) & 0xFFu, TARGET_PORT & 0xFFu,
            (UDP_LEN & 0xFF00u) >> 8, UDP_LEN & 0xFFu, 0, 0,
			tensDigit, unitDigit,
			0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0
	};

	/* obliczenie sumy kontrolnej dla naglowka datagramu IP */
	tU16 cksum = checksum(dataToSend, ETH_HED_SIZE + 1u, IP_HED_SIZE);

	dataToSend[BEG_NO_OF_CS + 2u] = (cksum >> 8) & 0xFFu;
	dataToSend[BEG_NO_OF_CS + 1u] = cksum & 0xFFu;

    changeBank(BANK0);

    /* Ustawienie rejestrow ETXST */
    WriteControlRegister(ETXSTL, ETXST_VALUE & 0xFFu);
    WriteControlRegister(ETXSTH, (ETXST_VALUE >> 8) & 0xFFu);

    /* Ustawienie rejestrow ETXND */
    WriteControlRegister(ETXNDL, ETXND_VALUE & 0xFFu);
    WriteControlRegister(ETXNDH, (ETXND_VALUE >> 8) & 0xFFu);

    /* Ustawienie rejestrow EWRPT */
    WriteControlRegister(EWRPTL, ETXST_VALUE & 0xFFu);
    WriteControlRegister(EWRPTH, (ETXST_VALUE >> 8) & 0xFFu);

    WriteBufferMemory(dataToSend, PACKET_SIZE + 1u);

    /* Ustawienie bitu TXRTS w rejestrze ECON1 */
    tU8 econ1 = ReadControlRegister(ECON1);
    econ1 |= 1u << TXRTS;
    WriteControlRegister(ECON1, econ1);

    delay_ms(500);

    econ1 = ReadControlRegister(ECON1);
    while((econ1 & (1u << TXRTS)) != 0u) {
    	touchWatchdog();
    	econ1 = ReadControlRegister(ECON1);
    }

    /* Odczytanie stanu bitu TXABRT w rejestrze ESTAT */
    tU8 estat = ReadControlRegister(ESTAT);

    tBool finishedProperly = TRUE;

    if((estat & (1u << TXABRT)) != 0u) { /* jezeli ten bit zostal ustawiony to oznacza to, iz podczas transmisji doszlo do bledu lub transmisja zostala anulowana */
        finishedProperly = FALSE;
    }

    /* transmisja zakonczyla sie pomyslnie */
    return finishedProperly;
}
