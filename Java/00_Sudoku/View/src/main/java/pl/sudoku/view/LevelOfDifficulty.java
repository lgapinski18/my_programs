package pl.sudoku.view;

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

import java.util.Random;
import pl.sudoku.model.SudokuBoard;

public enum LevelOfDifficulty {
    UNKNOWN(0), EASY(1), MEDIUM(2), HARD(3);

    LevelOfDifficulty(int levelOfDifficulty) {
        this.levelOfDifficulty = levelOfDifficulty;
    }

    public String getCommonNameOfLevelOfDifficulty() {
        return switch (levelOfDifficulty) {
            case 1:
                yield "easy";
            case 2:
                yield "medium";
            case 3:
                yield "hard";
            default:
                yield "unknown";
        };
    }

    public String getNameOfLevelOfDifficulty() {
        return switch (levelOfDifficulty) {
            case 1:
                yield "EASY";
            case 2:
                yield "MEDIUM";
            case 3:
                yield  "HARD";
            default:
                yield "UNKNOWN";
        };
    }

    public SudokuBoard processSudokuBoard(SudokuBoard sudokuBoard) {
        int numberOfFieldToDelete = switch (levelOfDifficulty) {
            case 1:
                yield 10;
            case 2:
                yield 40;
            case 3:
                yield 60;
            default:
                yield 0;
        };

        for (int i = 0; i < numberOfFieldToDelete;) {
            Random random = new Random();
            int row = random.nextInt(0, 9);
            int col = random.nextInt(0, 9);

            if (sudokuBoard.get(row, col) != 0) {
                sudokuBoard.set(row, col, 0);
                i++;
            }
        }

        return sudokuBoard;
    }

    private int levelOfDifficulty;
}
