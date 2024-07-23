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
import pl.sudoku.model.exceptions.SudokuBoardAutoverificationNotPassedException;
import pl.sudoku.model.exceptions.SudokuElementAutoverificationNotPassedException;
import pl.sudoku.model.exceptions.SudokuFieldException;


/**
 * Klasa reprezentująca pole Sudoku.
 * @author Lukasz Gapinski 242387
 * @author Mateusz Gapinski 242387
 * @version 2.2
 */
public class SudokuField extends Observed
        implements Cloneable, Comparable<SudokuField>, Serializable {
    private int value = 0;

    /**
     * Metoda zwracaj�ca warto�� pola Sudoku.
     * @return Warto�� pola Sudoku
     * @version 1.0
     * @since 1.0
     */
    public int getFieldValue() {
        return value;
    }

    /**
     * Metofa ustawiaj�c� warto�� pola Sudoku.
     * @param value Warto�� na jak� musi zosta� ustawione pole w SudokuField.
     * @throws SudokuFieldException Rzycany przy braku pomyślnej weryfikacji otrzymanej
     *      z obserwatora.
     * @version 1.1
     * @since 1.0
     */
    public void setFieldValue(int value) {
        this.value = value;

        try {
            notifyObservers();
        } catch (SudokuBoardAutoverificationNotPassedException e) {
            throw new SudokuFieldException(e.getMessage(), e);
        } catch (SudokuElementAutoverificationNotPassedException e) {
            throw new SudokuFieldException(e.getMessage(), e);
        }
    }

    /**
     * Metoda przekształcająca SudokuField na wartość tekstową klasy String.
     * @return Obiekt klasy String zawierający tekstową reprezentację SudokuField.
     * @version 1.0
     * @since 2.1
     */
    @Override
    public String toString() {
        return MoreObjects.toStringHelper(this)
                .add("value", value)
                .toString();
    }

    /**
     * Metoda służaca do porównywania obiektu klasy SudokuField z innymi obiektami.
     * @param o Obiekt, z którym ma zostać porównany obiekt klasy String.
     * @return Wartość logiczna reprezentująca czy porównywane obiekty są równe.
     * @version 1.0
     * @since 2.1
     */
    @Override
    public boolean equals(Object o) {
        if (this == o) {
            return true;
        }
        if (o == null || getClass() != o.getClass()) {
            return false;
        }
        SudokuField that = (SudokuField) o;
        return value == that.value;
    }

    /**
     * Metoda służaca do wygenerowania hashCode dla określonego obiektu klasy SudokuField.
     * @return Kod skrótu obiektu, na rzecz którego ta metoda została wywołana.
     * @version 1.0
     * @since 2.1
     */
    @Override
    public int hashCode() {
        return Objects.hashCode(value);
    }

    /**
     * Metoda służąca do porównywania abiektów SudokuField.
     * @param o the object to be compared.
     * @return Zwraca liczbę ujemną, gdy przekazany obiekt jest większy, 0,
     *      gdy są równe oraz dodatnią gdy przekazany obiekt jest mniejszy.
     */

    @Override
    public int compareTo(SudokuField o) {
        if (o == null) {
            throw new NullPointerException();
        }
        return value - o.value;
    }

    /**
     * Metoda służąca do klonowania obiektów klsy SudokuField.
     * @return Kopia obiektu klasy SudokuField.
     * @version 1.0
     * @since 2.2
     */
    @Override
    public SudokuField clone() {
        SudokuField clonedSudokuField = new SudokuField();
        clonedSudokuField.value = value;
        return clonedSudokuField;
    }
}