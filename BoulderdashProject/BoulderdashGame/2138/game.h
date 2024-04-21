/*
* @Description: Czesc programu realizujaca glowna czesc gry
*
* @author:      Łukasz Gapiński 242386
* @author:      Mateusz Gapiński 242387
*/

#ifndef GAME_H
#define GAME_H

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
void initGame(void);

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
void setLcdBackgound(void);

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
void updateScreen(void);

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
void moveLeft(void);

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
void moveUp(void);

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
void moveRight(void);

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
void moveDown(void);

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
void resetGame(void);

#endif
