package pl.sudoku.model;

/*-
 * #%L
 * Sudoku
 * %%
 * Copyright (C) 2022 mka_PN_1200_3
 * %%
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program.  If not, see
 * <http://www.gnu.org/licenses/gpl-3.0.html>.
 * #L%
 */


import com.google.common.base.MoreObjects;
import com.google.common.base.Objects;
import java.io.Serializable;
import java.lang.reflect.InvocationTargetException;
import java.util.Arrays;
import java.util.List;
import java.util.Locale;
import java.util.ResourceBundle;
import pl.sudoku.model.exceptions.SudokuBoardAutoverificationNotPassedException;
import pl.sudoku.model.exceptions.SudokuBoardException;
import pl.sudoku.model.exceptions.SudokuFieldException;

/**
 * Klasa reprezentuj�ca plansz� Sudoku.
 * @author Lukasz Gapinski 242387
 * @author Mateusz Gapinski 242387
 * @version 1.9
 */
public class SudokuBoard implements Cloneable, Serializable {
    private List<SudokuField> board = Arrays.asList(new SudokuField[81]);
    private boolean enableAutoverification = false;
    SudokuSolver sudokuSolver;



    /**
     * Metoda sprawdzaj�ca poprawno�� planszy Sudoku.
     * @return Zwraca true je�li plansza jest u�o�ona w spos�b poprawn,
     *      w przeciwnym wypadku zwraca false.
     * @version 1.0
     * @since 1.2
     */
    private boolean checkBoard() {
        for (int i = 0; i < 9; i++) {
            if (!getRow(i).verify()) {
                return false;
            } else if (!getColumn(i).verify()) {
                return false;
            } else if (!getBox(i % 3, i / 3).verify()) {
                return false;
            }
        }
        return true;
    }

    /**
     * Konstruktor klasy SudokuBoard.
     * @param sudokuSolver objekt odpowiedzialny za za wype�nianie sudoku.
     * @version 1.0
     * @since 1.1
     */
    public SudokuBoard(SudokuSolver sudokuSolver) {
        this.sudokuSolver = sudokuSolver;


        //ObserverOfSudokuField observerOfSudokuField = () -> {
        final SudokuBoard sudokuBoard = this;

        ObserverOfSudokuField observerOfSudokuField = new ObserverOfSudokuField() {
            public void actionPerformed() {
                if (enableAutoverification && !sudokuBoard.checkBoard()) {
                    throw new SudokuBoardAutoverificationNotPassedException(
                            ResourceBundle.getBundle(
                                    "pl.sudoku.model.exceptionsmessage",
                                            Locale.getDefault())
                                    .getString("autoverificationOfSudokuBoardFailed"));
                }
            }
        };

        for (int i = 0; i < 81; i++) {
            board.set(i, new SudokuField());
            board.get(i).addObserver(observerOfSudokuField);
        }
    }

    /**
     * Metoda generuj�ca plansz� Sudoku.
     * @version 1.0
     * @since 1.0
     */
    public void solveGame() {
        sudokuSolver.solve(this);
    }

    /**
     *  Metoda służąca do doadania obserwatora do pola o podanych współrzędnych.
     * @param row numer wiersza pola w SudokuBoard liczone od 0 do 9
     * @param col numer kolumny pola w SudokuBoard liczone od 0 do 9
     * @param o observator interfejsu ObserverOfSudokuField (można przekazać wyrażenie labda ()->
     *         {}  nie zwracające nic)
     * @version 1.0
     * @since 1.9
     */
    public void addObserver(int row, int col, ObserverOfSudokuField o) {
        board.get(row * 9 + col).addObserver(o);
    }

    /**
     *  Metoda służąca do usuwania wszystkich obserwatoró pól.
     * @version 1.0
     * @since 1.9
     */
    public void removeAllObservers() {
        final SudokuBoard sudokuBoard = this;

        ObserverOfSudokuField observerOfSudokuField = new ObserverOfSudokuField() {
            public void actionPerformed() {
                if (enableAutoverification && !sudokuBoard.checkBoard()) {
                    throw new SudokuBoardAutoverificationNotPassedException(
                            ResourceBundle.getBundle(
                                            "pl.sudoku.model.exceptionsmessage",
                                            Locale.getDefault())
                                    .getString("autoverificationOfSudokuBoardFailed"));
                }
            }
        };

        for (int i = 0; i < 81; i++) {
            board.get(i).removeAllObservers();
            board.get(i).addObserver(observerOfSudokuField);
        }
    }

    /**
     * Metoda umo�liwiaj�ca uzyskanie warto�ci konkretnego pola Sudoku.
     * @param row Numer wiersza Sudoku. Wiersze s� dumerowane od 0 do 8.
     * @param col Numer kolumny Sudoku. Kolumny s� dumerowane od 0 do 8.
     * @return Warto�� pola Sudoku ze wskazanej pozycji.
     * @version 1.1
     * @since 1.0
     */
    public int get(int row, int col) {
        return board.get(row * 9 + col).getFieldValue();
    }

    /**
     * Metoda umo�liwiaj�ca ustawienie warto�ci konkretnego pola Sudoku.
     * @param row Numer wiersza Sudoku. Wiersze s� dumerowane od 0 do 8.
     * @param col Numer kolumny Sudoku. Kolumny s� dumerowane od 0 do 8
     * @param val Warto�� jaka ma zosta� ustawiona na wskazanej pozycji. Warto�ci s� ca�kowite z
     *            zakresu 1 - 9.
     * @throws SudokuBoardException Rzucany przy niepomyślnej autoweryfikacji.
     */
    public void set(int row, int col, int val) {
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.model.exceptionsmessage", Locale.getDefault());
        try {
            board.get(row * 9 + col).setFieldValue(val);
        } catch (SudokuFieldException e) {
            throw new SudokuBoardException(
                    resourceBundle.getString("autoverificationOfSudokuBoardFailed"), e);
        }
    }

    /**
     * Metoda zwracaj�ca okre�lony rz�d sudoku.
     * @param y Indeks okre�lonego rz�du w Sudoku. Rz�dy s� numerowane od 0 do 8 od g�ry.
     * @return Okre�lny przez y rz�d Sudoku.
     * @version 1.1
     * @since 1.2
     */
    public SudokuRow getRow(int y) {
        List<SudokuField> arrayCopy = Arrays.asList(new SudokuField[9]);
        for (int i = 0; i < 9; i++) {
            SudokuField sudokuField = new SudokuField();
            sudokuField.setFieldValue(board.get(y * 9 + i).getFieldValue());
            arrayCopy.set(i, sudokuField);
        }
        return new SudokuRow(arrayCopy);
    }

    /**
     * Metoda zwracaj�ca okre�lon� kolumn� sudoku.
     * @param x Indeks okre�lonej kolumny w Sudoku. Kolumny s� numerowane od 0 do 8 od lewej.
     * @return Okre�lny przez y rz�d Sudoku.
     * @version 1.1
     * @since 1.2
     */
    public SudokuColumn getColumn(int x) {
        List<SudokuField> arrayCopy = Arrays.asList(new SudokuField[9]);
        for (int i = 0; i < 9; i++) {
            SudokuField sudokuField = new SudokuField();
            sudokuField.setFieldValue(board.get(i * 9 + x).getFieldValue());
            arrayCopy.set(i, sudokuField);
        }
        return new SudokuColumn(arrayCopy);
    }

    /**
     * Metoda zwracaj�ca okre�lony rz�d sudoku.
     * @param y Indeks okre�lonego rz�du kwadrat�w w Sudoku. Rz�dy s� numerowane od 0 do 2 od g�ry.
     * @param x Indeks okre�lonej kolumny kwadrat�w w Sudoku.
     *          Kolumny s� numerowane od 0 do 2 od lewej.
     * @return Okre�lny przez y rz�d Sudoku.
     * @version 1.1
     * @since 1.2
     */
    public SudokuBox getBox(int x, int y) {
        List<SudokuField> arrayCopy = Arrays.asList(new SudokuField[9]);
        for (int i = 0; i < 9; i++) {
            SudokuField sudokuField = new SudokuField();
            sudokuField.setFieldValue(
                    board.get(y * 27 + x * 3 + i / 3 * 9 + i % 3).getFieldValue());
            arrayCopy.set(i, sudokuField);
        }
        return new SudokuBox(arrayCopy);
    }

    /**
     * Metoda sprawdzaj�ca czy jest w��czona autoweryfikacja.
     * @return  true je�li autoweryfikacja jest w��czona, false je�li nie.
     * @version 1.0
     * @since 1.4
     */
    public boolean getAutoverifiacation() {
        return enableAutoverification;
    }

    /**
     * Metoda ustawiaj�ca czy ma by� w��czona autoweryfikacja.
     * @param autoverification True je�li ma by� w��czona weryfikacja, a je�li nie to false.
     * @version 1.0
     * @since 1.4
     */
    public void setAutoverification(boolean autoverification) {
        this.enableAutoverification = autoverification;
    }

    /**
     * Getter do pola sudokuSolver klasy SudokuBoard.
     * @return obiekt typu SudokuSolver
     * @version 1.0
     * @since 1.8
     */
    public SudokuSolver getSudokuSolver() {
        return sudokuSolver;
    }

    /**
     * Metoda przekształcająca SudokuBoard na wartość tekstową klasy String.
     *
     * @return Obiekt klasy String zawierający tekstową reprezentację SudokuBoard.
     * @version 1.0
     * @since 1.6
     */
    @Override
    public String toString() {
        return MoreObjects.toStringHelper(this)
                .add("board", board)
                .add("enableAutoverification", enableAutoverification)
                .add("sudokuSolver", sudokuSolver)
                .toString();
    }

    /**
     * Metoda equals zdefiniowana dla klasy SudokuBoard. Umo�liwia por�wnywanie dw�ch obiekt�w
     * klasy SudokuBoard
     * @param o Obiekt, kt�ry chcemy por�wna� z obiektem klasy SudokuBoard, na rzecz kt�rego
     *            wywo�ywana jest metoda.
     * @return Zwraca true je�li obiekty s� takie same, w przeciwnym wypadku zwraca false.
     * @version 2.0
     * @since 1.0
     */
    @Override
    public boolean equals(Object o) {
        if (this == o) {
            return true;
        }
        if (o == null || getClass() != o.getClass()) {
            return false;
        }
        SudokuBoard that = (SudokuBoard) o;
        return enableAutoverification == that.enableAutoverification
                && Objects.equal(board, that.board)
                && Objects.equal(sudokuSolver, that.sudokuSolver);
    }

    /**
     * Metoda hashCode zdefiniowana dla klasy SudokuBoard. Umo�liwia uzyskanie hashCode-e.
     * @return Zwraca haszCode SudokuBoard.
     * @version 2.0
     * @since 1.5
     */
    @Override
    public int hashCode() {
        return Objects.hashCode(board, enableAutoverification, sudokuSolver);
    }

    /**
     * Metoda klonująca obiekt SudokuBoard.
     * @return Kopia obiektu klasy SudokuBoard.
     * @version 1.0
     * @since 1.7
     */
    @Override
    public SudokuBoard clone() {
        SudokuBoard clonedSudokuBoard;
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.model.exceptionsmessage", Locale.getDefault());
        try {
            clonedSudokuBoard = new SudokuBoard((SudokuSolver) sudokuSolver.getClass()
                    .getDeclaredConstructors()[0].newInstance());
        } catch (InstantiationException e) {
            throw new SudokuBoardException(
                    resourceBundle.getString("duringCloningThrownInstantiationException"), e);
        } catch (IllegalAccessException e) {
            throw new SudokuBoardException(
                    resourceBundle.getString("duringCloningThrownIllegalAccessException"), e);
        } catch (InvocationTargetException e) {
            throw new SudokuBoardException(
                    resourceBundle.getString("duringCloningThrownInvocationTargetException"), e);
        }
        clonedSudokuBoard.enableAutoverification = enableAutoverification;

        for (int i = 0; i < 81; i++) {
            clonedSudokuBoard.board.get(i).setFieldValue(board.get(i).getFieldValue());
        }

        return clonedSudokuBoard;
    }
}