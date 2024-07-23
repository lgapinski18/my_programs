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
import java.util.Collections;
import java.util.List;
import java.util.Locale;
import java.util.ResourceBundle;
import pl.sudoku.model.exceptions.SudokuBoardElementException;
import pl.sudoku.model.exceptions.SudokuElementAutoverificationNotPassedException;
import pl.sudoku.model.exceptions.SudokuFieldException;
//import java.util.Objects;

/**
 * Jest to klasa abstrakcyjna wpomagaj�ca spos�b weryfikacji
 * poprawno�ci wype�nienia elementu Sudoku.
 * @author Lukasz Gapinski 242387
 * @author Mateusz Gapinski 242387
 * @version 1.3
 */
public abstract class SudokuElement implements Cloneable, Serializable {
    //private SudokuField[] sudokuFields;
    private List<SudokuField> sudokuFields = Arrays.asList(new SudokuField[9]);

    private boolean enableAutoverification = false;


    /**
     * Konstruktor wymagaj�cy przekazanie odpowiedniego obszaru w Sudoku,
     * kt�ry b�dzie poddawany weryfikacji.
     * @param sudokuFields Jest to 9-elementowa lista p�l Sudoku okre�lonego obszaru Sudoku,
     *                     kt�ry ma zosta� zweryfikowany.
     * @version 1.1
     * @since 1.0
     */
    public SudokuElement(List<SudokuField> sudokuFields) {
        Collections.copy(this.sudokuFields, sudokuFields);

        ObserverOfSudokuField observer = () -> {
            if (enableAutoverification && !this.verify()) {
                ResourceBundle resourceBundle = ResourceBundle.getBundle(
                        "pl.sudoku.model.exceptionsmessage", Locale.getDefault());
                throw new SudokuElementAutoverificationNotPassedException(
                        resourceBundle.getString(
                                "sudokuElementAutoverificationNotPassedException"));
            }
        };

        sudokuFields.forEach(item -> item.addObserver(observer));
    }

    /**
     * Metoda weryfikuj�ca czy dany element sudoku jest wype�niony w poprawny spos�b.
     * @return Zwraca true je�li dany element sudoku jest wype�niony w spos�b poprawny, w przecuwnym
     *      razie zwraca false.
     * @version 1.0
     * @since 1.0
     */
    public boolean verify() {
        for (int j = 0; j < 9; j++) {
            int v = sudokuFields.get(j).getFieldValue();

            if (v == 0) {
                continue;
            }

            for (int k = j + 1; k < 9; k++) {
                if (v == sudokuFields.get(k).getFieldValue()) {
                    return false;
                }
            }
        }
        return true;
    }

    /**
     * Metoda s�u��ca do pobierania warto�ci pola sudoku z danego SudokuElement.
     * @param index Indeks okre�lonego pola sudoku.
     * @return Warto�� wybranego pola sudoku.
     * @version 1.0
     * @since 1.1
     */
    public int get(int index) {
        return sudokuFields.get(index).getFieldValue();
    }

    /**
     * Metoda s�u��ca do ustawienia odpowiedniego pola sudoku w denym SudokuElement.
     * @param index Indeks elementu, kt�ry nale�y ustawi� argumentem sudokuField.
     * @param value Warto��, kt�ra zostanie wstawiana na dany indeks sudokuElement.
     * @throws SudokuBoardElementException Rzucany przy nie pomyślnej autoweryfikacji.
     * @version 1.0
     * @since 1.1
     */
    public void set(int index, int value) {
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.model.exceptionsmessage", Locale.getDefault());
        try {
            sudokuFields.get(index).setFieldValue(value);
        } catch (SudokuFieldException e) {
            throw new SudokuBoardElementException(resourceBundle.getString(
                    "sudokuElementAutoverificationNotPassedException"), e);
        }
    }

    /**
     * Metoda sprawdzaj�ca czy jest w��czona autoweryfikacja.
     * @return  true je�li autoweryfikacja jest w��czona, false je�li nie.
     * @version 1.0
     * @since 1.1
     */
    public boolean getAutoverifiacation() {
        return enableAutoverification;
    }

    /**
     * Metoda ustawiaj�ca czy ma by� w��czona autoweryfikacja.
     * @param autoverification True je�li ma by� w��czona weryfikacja, a je�li nie to false.
     * @version 1.0
     * @since 1.1
     */
    public void setAutoverification(boolean autoverification) {
        this.enableAutoverification = autoverification;
    }

    /**
     * Metoda przekształcająca SudokuElement na wartość tekstową klasy String.
     *
     * @return Obiekt klasy String zawierający tekstową reprezentację SudokuElement.
     * @version 1.0
     * @since 1.2
     */
    @Override
    public String toString() {
        return MoreObjects.toStringHelper(this)
                .add("sudokuFields", sudokuFields)
                .add("enableAutoverification", enableAutoverification)
                .toString();
    }

    /**
     * Metoda służaca do porównywania obiektu klasy SudokuElement z innymi obiektami.
     * @param o Obiekt, z którym ma zostać porównany obiekt klasy String.
     * @return Wartość logiczna reprezentująca czy porównywane obiekty są równe.
     * @version 1.0
     * @since 1.2
     */
    @Override
    public boolean equals(Object o) {
        if (this == o) {
            return true;
        }
        if (o == null || getClass() != o.getClass()) {
            return false;
        }
        SudokuElement that = (SudokuElement) o;
        return enableAutoverification == that.enableAutoverification
                && Objects.equal(sudokuFields, that.sudokuFields);
    }

    /**
     * Metoda służaca do wygenerowania hashCode dla określonego obiektu klasy SudokuElement.
     * @return Kod skrótu obiektu, na rzecz którego ta metoda została wywołana.
     * @version 1.0
     * @since 1.2
     */
    @Override
    public int hashCode() {
        return Objects.hashCode(sudokuFields, enableAutoverification);
    }

    /**
     * Metoda służąca do klonowania obiektów klsy SudokuElement.
     * @return Kopia obiektu klasy SudokuElement.
     * @version 1.0
     * @since 1.3
     */
    @Override
    public SudokuElement clone() {
        List<SudokuField> clonedSudokuFields = Arrays.asList(new SudokuField[9]);
        for (int i = 0; i < 9; i++) {
            clonedSudokuFields.set(i, sudokuFields.get(i).clone());
        }
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.model.exceptionsmessage", Locale.getDefault());
        try {
            SudokuElement sudokuElement = (SudokuElement) getClass()
                    .getDeclaredConstructors()[0].newInstance(clonedSudokuFields);
            sudokuElement.enableAutoverification = enableAutoverification;
            return sudokuElement;
        } catch (InstantiationException e) {
            throw new SudokuBoardElementException(
                    resourceBundle.getString("duringCloningThrownInstantiationException"), e);
        } catch (IllegalAccessException e) {
            throw new SudokuBoardElementException(
                    resourceBundle.getString("duringCloningThrownIllegalAccessException"), e);
        } catch (InvocationTargetException e) {
            throw new SudokuBoardElementException(
                    resourceBundle.getString("duringCloningThrownInvocationTargetException"), e);
        }
    }
}