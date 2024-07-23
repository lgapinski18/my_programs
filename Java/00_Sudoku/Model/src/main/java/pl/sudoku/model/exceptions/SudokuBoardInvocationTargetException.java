package pl.sudoku.model.exceptions;

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

public class SudokuBoardInvocationTargetException extends SudokuBoardException {
    /**
     * Konstruktruktor klasy SudokuException przyjmujący wiadomość rzucanego wyjątku.
     *
     * @param message Określa wiadomość rzucanego wyjątku.
     */
    public SudokuBoardInvocationTargetException(String message) {
        super(message);
    }

    /**
     * Konstruktruktor klasy SudokuException przyjmujący wiadomość rzucanego wyjątku
     * wraz z przyczyną.
     *
     * @param message Określa wiadomość rzucanego wyjątku.
     * @param cause   Określa przyczynę rzucanego wyjątku, którą jest inny wyjątek.
     */
    public SudokuBoardInvocationTargetException(String message, Throwable cause) {
        super(message, cause);
    }

    public SudokuBoardInvocationTargetException(Throwable cause) {
        super(cause);
    }
}
