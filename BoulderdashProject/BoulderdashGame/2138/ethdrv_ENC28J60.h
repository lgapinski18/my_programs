/*
 *	@Descritpion:	Zawariera zbior deklaracji funkcji uzywanych przy
 * 					obsludze kontrolera ENC28J60 wraz z definiowanie
 * 					wielu zmiennych zawierajacych niezbedne informacje
 * 					do sterowania.
 */

#ifndef ETHDRV_ENC28J60_H
#define ETHDRV_ENC28J60_H

#include "general.h"

/* denicje danych */

/* kody polecen SPI dla ENC28J60 */
#define RCR 	0u
#define WCR 	(2u << 5)
#define WBM 	0x7Au

/* Lista wyliczeniowa numerow Bankow dla rejestrow kontrolnych */
typedef enum  { BANK0 = 0u, BANK1 = 1u, BANK2 = 2u, BANK3 = 3u} Bank;

#define ETH_CS 		14 /* numer pinu P0.14, ktorego ustawienie stanu na 0 umozliwi wybranie ENC28J60 dla magistrali SPI */
#define SCK_POS		8 /* numer bitu w PINSEL0, ktory nalezy ustawic aby wybrac dla pinu P0.4 tryb SCK */
#define MOSI_POS	12 /* numer bitu w PINSEL0, ktory nalezy ustawic aby wybrac dla pinu P0.5 tryb MOSI */
#define MISO_POS	10 /* numer bitu w PINSEL0, ktory nalezy ustawic aby wybrac dla pinu P0.6 tryb MISO */
#define SCK_PIN		4 /* numer pinu z funkcja SCK0 */
#define SPICCR_VAL	0x08 /* wasrtosc, ktora nalezy wprowadzic do rejestru SPI Counter Clock Register */
#define MSTR_MODE	1 << 5 /* wasrtosc ktora w rejestrze SPI Control Register ustawia wartosc tryb pracy MASTER */
#define BITS_PER_T	1 << 11 /* ustawienie tej wartosci w rejestrze SPICR powoduje wybranie 8 bitowna transmisje */

/* Adresy uzywanych rejestrow kontrolnych */
#define ECON1 	0x1Fu
#define ECON2 	0x1Eu
#define ESTAT	0x1Du
#define ERXSTL 	0x08u
#define ERXSTH 	0x00u
#define ERXNDL 	0x0Au
#define ERXNDH 	0x0Bu
#define ETXSTL 	0x04u
#define ETXSTH 	0x05u
#define ETXNDL 	0x06u
#define ETXNDH 	0x07u
#define EWRPTL 	0x02u
#define EWRPTH 	0x03u
#define MACON1 	0x00u
#define MACON3 	0x02u
#define MAMXFLL	0x0Au
#define MAMXFLH 0x0Bu
#define MABBIPG 0x04u
#define MAIPGL 	0x06u
#define MAADR1 	0x04u
#define MAADR2 	0x05u
#define MAADR3 	0x02u
#define MAADR4 	0x03u
#define MAADR5 	0x00u
#define MAADR6 	0x01u

/* numery bitow, niektorych rejestrow */
#define	AUTOINC	7u
#define	MARXEN	0u
#define	TXPAUS	3u
#define	RXPAUS	2u
#define	TXCRCEN	4u
#define	FULDPX	0u
#define	TXRTS	3u
#define	TXABRT	1u

/* wartosci dla niektorych rejestrow */
#define ERXST_VALUE 	0x0u
#define ERXND_VALUE 	0xAAAu
#define MAMXFL_VALUE 	0x5EEu
#define MABBIPG_VALUE 	0x15u
#define MAIPGL_VALUE 	0x12u
#define ETXST_VALUE		0xB00u
#define ETXND_VALUE		(0xB2Cu + 16u)

#define WAITING_TIME 	1u /* Czas oczekiwania przed rozpoczeciem inicjalizacji ustawien dla MAC */
#define PACKET_CON_B	0x0Eu /* Wartosc bajtu kontrolnego dla wysylanych danych */

#define TYPE_MAC		0x0800u /* tym zawartosci danych ramki Ethernetowej wskazuje na protokol IP */

#define IP_V			4u /* wersja protokolu IP */
#define HEADER_LEN		5u /* dlugosc naglowka w 32-bitowych slowach */
#define DSCP			(32u << 2) /* charakterystyka uslug zroznicowanych */
#define ECN				0u /* ECN */
#define TOTAL_LEN		30u /* calkowty rozmiar datagramu IP w bajtach */
#define IP_DAT_ID		0x6EDEu /* numer identyfikacyjny datagramu IP */
#define FLAGS			0u /* bitowo 010 wwartosc pola flagnaglowka IP */
#define OFFSET			0u /* przesuniecie w datagramie IP */
#define TTL				128u /* czas zycia pakietu */
#define PROT_TYPE		17u /* wartosc identyfikujaca rodzaj protokolu wartwy wyzszej w polu danych datagramu IP */
#define	BEG_NO_OF_CS	24u /* numer pierwszego bajtu sumy kontrolnej w naglowku IP wzgledem poczatku ramki Ethernetowej */

#define SOURCE_PORT		52735u /* numer portu na hoscie zrodlowym */
#define TARGET_PORT		10000u /* numer portu na hoscie docelowym */
#define UDP_LEN			10u /* rozmiar datagramu UDP */

#define PACKET_SIZE 	(44u + 16u) /* rozmiar przygotowywanej czesci ramki przez urzytkownika */
#define ETH_HED_SIZE	14u /* rozmiar naglowka ramki Ethernetowej */
#define IP_HED_SIZE		HEADER_LEN * 4u /* rozmiar naglowka datagramu IP w bajtach */

/* deklaracje funkcji */

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
tU8 sendSpi(tU8 byte);

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
void selectEthSpi(void);

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
void unselectEthSpi(void);

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
void changeBank(Bank bank);

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
tU8 ReadControlRegister(tU8 RegisterAddr);

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
tU8 ReadControlRegisterMacMii(tU8 RegisterAddr);

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
void WriteControlRegister(tU8 RegisterAddr, tU8 value);

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
void initENC28J60(void);

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
tBool sendResult(tU8 tensDigit, tU8 unitDigit);

#endif /* ETHDRV_ENC28J60_H_ */
