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


import java.util.List;

/**
 * Klasa dziedzicz�ca po SudokuElement wspomagaj�ca weryfikacj� okre�lonego kwadratu w Sudoku.
 * @author Lukasz Gapinski 242387
 * @author Mateusz Gapinski 242387
 * @version 1.2
 */
public class SudokuBox extends SudokuElement {
    /**
     * Konstruktor wymagaj�cy przekazania tablicy p�l zawartych w okre�lonym kwadracie w Sudoku,
     * kt�ry b�dzie poddawany weryfikacji.
     * @param sudokuFields Jest to 9-elementowa lista wskazuj�ca na pola w Sudoku
     *                    nale��ce do jednego kwadratu, kt�re maj� zosta� zweryfikowane.
     * @version 1.0
     * @since 1.0
     */
    public SudokuBox(List<SudokuField> sudokuFields) {
        super(sudokuFields);
    }

    /**
     * Metoda służąca do klonowania obiektów klsy SudokuBox.
     * @return Kopia obiektu klasy SudokuBox.
     * @version 1.0
     * @since 1.2
     */
    @Override
    public SudokuBox clone() {
        return (SudokuBox) super.clone();
    }
}