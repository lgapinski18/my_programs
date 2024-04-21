#include "game.h"
#include "general.h"
#include <printf_P.h>
#include "game_resources.h"
#include "lcd.h"
#include "ledi2c.h"
#include "ethdrv_ENC28J60.h"

/* External variables */
//Odwołania do mapy z game_resources
extern const tU8	mapResourceOrigin[905];
extern tU8			mapResource[905];

//Odwołania do assetów z game_resources
extern tU8			backgroundImage[144];
extern tU8			earthImage[144];
extern tU8			wallImage[144];
extern tU8			stoneImage[144];
extern tU8			exitImage[144];
extern tU8			playerImage[144];
extern tU8			diamondImage[144];

/* Local variables */
static tU8		currentPositionX = 0; //Zmienna globalna przechowujaca wspolrzedne X pozycji gracza
static tU8		currentPositionY = 0; //Zmienna globalna przechowujaca wspolrzedne Y pozycji gracza

static tU8		*map = 0; //Wskasnik na mape

static tU8		mapWidth 	= 0; //Zmienna globalna przechowujaca szerokosc mapy
static tU8		mapHeight 	= 0; //Zmienna globalna przechowujaca wysokosc mapy

static tU8		gatheredDiamonds = 0; //Zmienna globalna przechowujaca liczbe zebranych diamentow
static tU8		neededDiamonds	= 0; //Zmienna globalna przechowujaca liczbe koniecznych do zebrania diamentow

static tBool	canMove = TRUE; //Zmienna globalna okreslajaca czy gracz jest w stanie sie poruszac.

/* Local declarations */
void endGame(void);

/*
 *  @brief			Funckja wyswietlajaca na wyswietlaczu tło gry.
 *
 *  @Description	Funckja wyswietlajaca na wyswietlaczu tło gry.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void setLcdBackgound(void)
{
	tU8 i = 0;
	tU8 j = 0;
	const tU8 columnsNumber = 132u;
	const tU8 rowsNumber = 132u;
	for (; i < columnsNumber; i += IMAGE_WIDTH)
	{
		for (j = 0; j < rowsNumber; j += IMAGE_HEIGHT)
		{
			//Narysowanie okre�lonego pola na mapie.
			lcdDrawIcon(i, j, IMAGE_WIDTH, IMAGE_HEIGHT, wallImage);
		}
	}
}

/*
 *  @brief			Funckja incjalizujaca gre.
 *
 *  @Description	Jest to funkcja, ktara ma przeprowadzic inicjalizacje gry.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void initGame(void)
{
	tU8 *mapptr = mapResource;
	//Pobranie z zasobu mapy jej szerokosci
	mapWidth  = *mapptr;
	mapptr++;

	//Pobranie z zasobu mapy jej wysokosci
	mapHeight = *mapptr;
	mapptr++;

	//Pobranie z zasobu mapy wspolrzednej X pozycji poczatkowej gracza
	currentPositionX = *mapptr;
	mapptr++;

	//Pobranie z zasobu mapy wspolrzednej Y pozycji poczatkowej gracza
	currentPositionY = *mapptr;
	mapptr++;

	//Pobranie z zasobu mapy liczby wymaganych diamentow, aby zakonczyc gre
	neededDiamonds = *mapptr;
	mapptr++;

	//Przypisanie mapy do zmiennej ja przechowujacej
	map = mapptr;

	//Resetownie liczby zebranych diament�w
	gatheredDiamonds = 0;

	//Ustawienie mozliwosci poruszania
	canMove = TRUE;

	setLcdBackgound();
}

/*
 *  @brief			Funckja odpowiadajaca za zakonczenie gry.
 *
 *  @Description	Funkcja odpowiadajaca za zako�czenie gry, dodatkowo wywolujaca funkcje zapalajaca diody LED z wykorzystaniem i2c.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Modyfikacja rejestr�w LS0, LS1, LS2, LS3. Sa to efekty wynikajace z uzycia funkcji winLEDMovement() z ledi2c.h.
 */
void endGame(void)
{
	//Ustawienie braku mozliwosci poruszania
	canMove = FALSE;

	lcdColor(0xFF, 0x00);

	lcdGotoxy(20, 20);

	lcdPuts("Wynik: ");
	
	tU8 score = gatheredDiamonds; //Zmienna przechowujaca uzyskany wynik w grze na potrzby przetwarzania.
	tU8 currentX = 100u; //Zmienna przechowująca pozycję X wyświetlanego napisu wyniku.
	tU8 currentY = 40u; //Zmienna przechowująca pozycję Y wyświetlanego napisu wyniku.

	//Wypisanie jesli uzyskany wynik wyniosl 0.
	if (score == 0u)
	{
		lcdGotoxy(currentX, currentY);
		lcdPutchar(u'0');
	}

	//Wypisanie wartosci liczbowej wyniku po przez wypisanie cyfr liczby od konca
	while (score != 0u)
	{
		lcdGotoxy(currentX, currentY);
		lcdPutchar((score % 10u) + u'0');
		score	 /= 10u;
		currentX -= 9u;
	}
	
	tU8 tensDigit = (gatheredDiamonds / 10u) + u'0';
	tU8 unitDigit = (gatheredDiamonds % 10u) + u'0';

	/* Wyslanie wyniku gry po przez protokol UDP interfejsem Ethernet */
	(void) sendResult(tensDigit, unitDigit);
	/*
	if(sendResult(tensDigit, unitDigit)) {
		simplePrintf("\n\tPrzeslano\n");
	}
	else {
		simplePrintf("\n\tNieprzeslano\n");
	}*/

	winLEDMovement();
}

/*
 *  @brief			Funkcja odpowiadajaca za zaktualizowanie tego co wyswietla ekran LCD.
 *
 *  @Description	Funkcja odpowiadajaca za zaktualizowanie tego co wyswietla ekran LCD.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	Efekty uboczne wynikajace z przeslania danych magistrale SPI.
 */
void updateScreen(void)
{
	//Sprawdzenie czy gracz moze sie poruszyc w celu okreslenia czy jest sens aktualizowania LCD.
	if (canMove == TRUE)
	{
		tS8 mapBeginXPos = (tS8)currentPositionX - 5;
		tS8 mapBeginYPos = (tS8)currentPositionY - 5;
		tU8 mapEndXPos = currentPositionX + 5u;
		tU8 mapEndYPos = currentPositionY + 5u;

		//Pozycja pocz�tkowa X, od ktorej bedzie sie rysowac na ekranie.
		tU8 screenBeginXPos = 0u;
		tU8 screenBeginYPos = 0u;

		//Sprawdzenie czy pozycja X, od ktorej chcielibysmy rysowac mape na ekranie, nie jest przypadkiem ujemna.
		if (mapBeginXPos < 0)
		{
			screenBeginXPos = -mapBeginXPos * IMAGE_WIDTH;
			mapBeginXPos = 0;
		}

		//Sprawdzenie czy pozycja Y, od ktorej chcielibysmy rysowac mape na ekranie, nie jest przypadkiem ujemna.
		if (mapBeginYPos < 0)
		{
			screenBeginYPos = -mapBeginYPos * IMAGE_HEIGHT;
			mapBeginYPos = 0;
		}

		//Sprawdzenie czy pozycja X, do ktorej chcielibysmy rysowac mape na ekranie, wykracza po za zakres mapy.
		if (mapEndXPos >= mapWidth)
		{
			mapEndXPos = mapWidth - 1u;
		}

		//Sprawdzenie czy pozycja Y, do ktorej chcielibysmy rysowac mape na ekranie, wykracza po za zakres mapy.
		if (mapEndYPos >= mapHeight)
		{
			mapEndYPos = mapHeight - 1u;
		}

		//Zmienne wykorzystywane do iteracji wzgl�dem pozycji X i Y.
		tU8  i = 0u;
		tU8	 j = 0u;
		tU32 index = 0u;
		//Zmienna okreslajaca pozycje X rysowanego pola.
		tU8  xPos = 0u;
		//Zmienna okreslajaca pozycje Y rysowanego pola.
		tU8  yPos = 0u;
		//Zmienna przechowujaca wskazanie na pierwszy element tablicy przechowujacej grafike okreslonego typu pola.
		tU8* imageToDraw = 0;

		for (i = mapBeginXPos; i <= mapEndXPos; i++)
		{
			for (j = mapBeginYPos; j <= mapEndYPos; j++)
			{
				//Okreslenie index pola do narysowania.
				index = ((tU32)j * mapWidth) + i;
				//Wybor graiki jaka posiada okreolone przez index pole.
				switch (map[index])
				{
				case '#':
					imageToDraw = wallImage;
					break;
				case '.':
					imageToDraw = earthImage;
					break;
				case ' ':
					imageToDraw = backgroundImage;
					break;
				case 'P':
					imageToDraw = playerImage;
					break;
				case '^':
					imageToDraw = diamondImage;
					break;
				case 'o':
					imageToDraw = stoneImage;
					break;
				case 'W':
					imageToDraw = exitImage;
					break;
				default:
					imageToDraw = backgroundImage;
					break;
				}
				//Okreslenie pozycji na ekranie pola do narysowania.
				xPos = screenBeginXPos + (i - (tU8)mapBeginXPos) * IMAGE_WIDTH;
				yPos = screenBeginYPos + (j - (tU8)mapBeginYPos) * IMAGE_HEIGHT;
				//Narysowanie okreslonego pola na mapie.
				lcdDrawIcon(xPos, yPos, IMAGE_WIDTH, IMAGE_HEIGHT, imageToDraw);
			}
		}
	}
}

/*
 *  @brief			Funckja przemieszczajaca gracza w lewo.
 *
 *  @Description	Jest to funkcja, ktora ma zrealizowac przemieszczenie gracza w lewo.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void moveLeft(void)
{
	//Sprawdzenie czy gracz moze sie poruszyc
	if (canMove == TRUE)
	{
		//Wyliczanie indeksu pola na lewo od gracza
		tU32 index = ((tU32)currentPositionY * mapWidth) + currentPositionX - 1u;

		//Sprawdzenie czy gracz moze pojsc w lewo
		if ((map[index] != u'#') && (map[index] != u'o') && (map[index] != u'W'))
		{
			//Sprawdzenie czy gracz nie wszedl na pole z diamentem
			if (map[index] == u'^')
			{
				gatheredDiamonds++;
			}

			//Usuniecie gracza ze starej pozycji
			map[index + 1u] = u' ';
			//Dodanie gracza na nowa pozycje
			map[index] = u'P';
			//Przeniesienie sie w lewo
			currentPositionX--;
		}
		else
		{
			//Sprawdzenie czy gracz nie probuje wejsc na pole z wyjsciem
			if ((map[index] == u'W') && (gatheredDiamonds >= neededDiamonds)) {
				endGame();
			}
		}
	}
}

/*
 *  @brief			Funckja przemieszczajaca gracza w gore.
 *
 *  @Description	Jest to funkcja, kt�ra ma zrealizowac przemieszczenie gracza w gore.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void moveUp(void)
{
	//Sprawdzenie czy gracz moze sie poruszyc
	if (canMove == TRUE)
	{
		//Wyliczanie indeksu pola w gore od gracza
		tU32 index = (((tU32)currentPositionY - 1u) * mapWidth) + currentPositionX;

		//Sprawdzenie czy gracz mo�e  w gore
		if ((map[index] != u'#') && (map[index] != u'o') && (map[index] != u'W'))
		{
			//Sprawdzenie czy gracz nie wszedl na pole z diamentem
			if (map[index] == u'^')
			{
				gatheredDiamonds++;
			}

			//Usuniecie gracza ze starej pozycji
			map[index + mapWidth] = u' ';
			//Dodanie gracza na nowa pozycje
			map[index] = u'P';
			//Przeniesienie sie w gore
			currentPositionY--;
		}
		else
		{
			//Sprawdzenie czy gracz nie probuje wejsc na pole z wyjsciem
			if ((map[index] == u'W') && (gatheredDiamonds >= neededDiamonds))
			{
				endGame();
			}
		}
	}
}

/*
 *  @brief			Funckja przemieszczajaca gracza w prawo.
 *
 *  @Description	Jest to funkcja, kt�ra ma zrealizowac przemieszczenie gracza w prawo.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void moveRight(void)
{
	//Sprawdzenie czy gracz moze sie poruszyc
	if (canMove == TRUE)
	{
		//Wyliczanie indeksu pola na prawo od gracza
		tU32 index = ((tU32)currentPositionY * mapWidth) + currentPositionX + 1u;

		//Sprawdzenie czy gracz moze pojsc w prawo
		if ((map[index] != u'#') && (map[index] != u'o') && (map[index] != u'W'))
		{
			//Sprawdzenie czy gracz nie wszed� na pole z diamentem
			if (map[index] == u'^')
			{
				gatheredDiamonds++;
			}

			//Usuniecie gracza ze starej pozycji
			map[index - 1u] = u' ';
			//Dodanie gracza na nowa pozycje
			map[index] = u'P';
			//Przeniesienie sie w prawo
			currentPositionX++;
		}
		else
		{
			//Sprawdzenie czy gracz nie probuje wejsc na pole z wyjsciem
			if ((map[index] == u'W') && (gatheredDiamonds >= neededDiamonds))
			{
				endGame();
			}
		}
	}
}

/*
 *  @brief			Funckja przemieszczajaca gracza w dol.
 *
 *  @Description	Jest to funkcja, kt�ra ma zrealizowac przemieszczenie gracza w dol.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void moveDown(void)
{
	//Sprawdzenie czy gracz moze sie poruszyc
	if (canMove == TRUE)
	{
		//Wyliczanie indeksu pola w dol od gracza
		tU32 index = (((tU32)currentPositionY + 1u) * mapWidth) + currentPositionX;

		//Sprawdzenie czy gracz mo�e pojsc w dol
		if ((map[index] != u'#') && (map[index] != u'o') && (map[index] != u'W'))
		{
			//Sprawdzenie czy gracz nie wszedl na pole z diamentem
			if (map[index] == u'^')
			{
				gatheredDiamonds++;
			}

			//Usuniecie gracza ze starej pozycji
			map[index - mapWidth] = u' ';
			///Dodanie gracza na nowa pozycje
			map[index] = u'P';
			//Przeniesienie sie w gol
			currentPositionY++;
		}
		else
		{
			//Sprawdzenie czy gracz nie probuje wejsc na pole z wyjsciem
			if ((map[index] == u'W') && (gatheredDiamonds >= neededDiamonds))
			{
				endGame();
			}
		}
	}
}

/*
 *  @brief			Wykonanie resetu gry.
 *
 *  @Description	Wykonanie resetu gry, pod warunkiem ze gra sie zakonczyla.
 *
 *  @param 			brak
 *
 *  @returns  		nic
 *
 *  @side effects	brak
 */
void resetGame(void)
{
	if (!canMove)
	{
		//Resetowanie mapy
		tU32 itr = 0u;
		tU32 mapSize = (tU32) mapWidth * mapHeight;
		const tU8 *mapptr = mapResourceOrigin;

		//Pominiecie piecielementowego naglowka mapy
		mapptr++;
		mapptr++;
		mapptr++;
		mapptr++;
		mapptr++;

		for(; itr < mapSize; itr++)
		{
			map[itr] = mapptr[itr];
		}
		initGame();
	}
}
