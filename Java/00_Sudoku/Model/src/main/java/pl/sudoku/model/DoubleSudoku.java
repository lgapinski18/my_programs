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

class DoubleSudoku extends SudokuBoard {
    private SudokuBoard prevSudokuBoard;
    private SudokuBoard writtenSudokuBoard;

    String nameOfLOD;

    public DoubleSudoku(SudokuBoard prevSudokuBoard,
                        SudokuBoard writtenSudokuBoard, String nameOfLOD) {
        super(new BackTrackingSudokuSolver());
        this.prevSudokuBoard = prevSudokuBoard;
        this.writtenSudokuBoard = writtenSudokuBoard;
        this.nameOfLOD = nameOfLOD;
    }

    public SudokuBoard getPrevSudokuBoard() {
        return prevSudokuBoard;
    }

    public SudokuBoard getWrittenSudokuBoard() {
        return writtenSudokuBoard;
    }

    public String getNameOfLOD() {
        return nameOfLOD;
    }
}
