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


import java.io.Serializable;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Random;

/**
 * Reprezentuj�ca danego typu algorytm do rozwi�zywania sudoku.
 * @author Lukasz Gapinski 242387
 * @author Mateusz Gapinski 242387
 * @version 1.2
 */
public class BackTrackingSudokuSolver implements SudokuSolver, Serializable {
    /**
     * Metoda odziedziczony po interfejsie SudokuSolver odpowiedzialna za wype�nienie sudoku.
     * @param sudokuBoard - objekt klasy SudokuBoard, kt�ry zostanie wype�niony
     * @version 1.1
     * @since 1.0
     */
    public void solve(SudokuBoard sudokuBoard) {
        Random random = new Random();

        ArrayList<Integer> fullList = new ArrayList<>(Arrays.stream(new Integer[] {1,2,3,4,5,6,
                7,8,9}).toList());

        ArrayList<ArrayList<Integer>> avaiblesInCells = new ArrayList<>(9);
        for (int i = 0; i < 9; i++) {
            avaiblesInCells.add(0, (ArrayList<Integer>) fullList.clone());
        }

        for (int y = 0; y < 9; y++) {
            ArrayList<Integer> avaibleInRow = (ArrayList<Integer>) fullList.clone();
            //int allowedLoops = 50;//////////
            for (int x = 0; x < 9; x++) {
                avaiblesInCells.set(x, (ArrayList<Integer>) avaibleInRow.clone());
                boolean continuing = true;
                again:
                while (continuing) {
                    if (avaiblesInCells.get(x).size() == 0) {
                        //board[y * 9 + x] = 0;
                        sudokuBoard.set(y, x, 0);

                        if (x == 0) {
                            y--;

                            /*allowedLoops--;
                            if (allowedLoops == 0) { /////////////////
                                System.out.println("Cofanie o 2");
                                allowedLoops = 50;
                                if (y != 0) {
                                    for (int i = 0; i < 9; i++) {
                                        //board[i] = 0;
                                        sudokuBoard.set(y, i, 0);
                                    }
                                    y--;
                                }
                            }/**/

                            x = -1;
                            avaibleInRow = (ArrayList<Integer>) fullList.clone();
                            //for (int i = y * 9; i < (y * 9 + 9); i++) {
                            for (int i = 0; i < 9; i++) {
                                //board[i] = 0;
                                sudokuBoard.set(y, i, 0);
                            }
                            break;
                        }
                        x--;
                        avaibleInRow.add(sudokuBoard.get(y, x));
                        avaiblesInCells.get(x).remove(
                                avaiblesInCells.get(x).indexOf(sudokuBoard.get(y, x)));
                        continue;
                    }

                    int index = random.nextInt(0, avaiblesInCells.get(x).size());
                    int v = avaiblesInCells.get(x).get(index);

                    //board[y * 9 + x] = v;
                    sudokuBoard.set(y, x, v);

                    if (!sudokuBoard.getColumn(x).verify()) {
                        avaiblesInCells.get(x).remove(index);
                        continue again;
                    }

                    if (!sudokuBoard.getBox(x / 3, y / 3).verify()) {
                        avaiblesInCells.get(x).remove(index);
                        continue again;
                    }

                    avaibleInRow.remove(avaibleInRow.indexOf(v));
                    continuing = false;
                }
            }
        }
    }

    /**
     * Metoda przekształcająca BackTrackingSudokuSolver na wartość tekstową klasy String.
     * @return Obiekt klasy String zawierający tekstową reprezentację BackTrackingSudokuSolver.
     * @version 1.0
     * @since 1.2
     */
    @Override
    public String toString() {
        return "BackTrackingSudokuSolver{ }";
    }

    /**
     * Metoda służaca do porównywania obiektu klasy BackTrackingSudokuSolver z innymi obiektami.
     * @param obj Obiekt, z którym ma zostać porównany obiekt klasy String.
     * @return Wartość logiczna reprezentująca czy porównywane obiekty są równe.
     * @version 1.0
     * @since 1.2
     */
    @Override
    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (obj == null || getClass() != obj.getClass()) {
            return false;
        }
        return true;
    }

    /**
     * Metoda służaca do wygenerowania hashCode dla określonego obiektu klasy
     * BackTrackingSudokuSolver.
     * @return Kod skrótu obiektu, na rzecz którego ta metoda została wywołana.
     * @version 1.0
     * @since 1.2
     */
    @Override
    public int hashCode() {
        return 1;
    }
}
