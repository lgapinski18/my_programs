package pl.sudoku.model;

import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertTrue;

import org.junit.jupiter.api.Test;


public class BackTrackingSudokuSolverTest {

    public BackTrackingSudokuSolverTest() {
    }

    @Test
    public void testSolve() {
        SudokuSolver sudokuSolver = new BackTrackingSudokuSolver();
        SudokuBoard sudokuBoard = new SudokuBoard(sudokuSolver);
        sudokuSolver.solve(sudokuBoard);

        //weryfikacja braku powtórzeń w kolumnach
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                int v = sudokuBoard.get(j, i);
                for (int k = j + 1; k < 9; k++) {
                    if (v == sudokuBoard.get(k, i)) {
                        assertTrue(false);
                    }
                }
            }
        }

        //weryfikacja braku powtórzeń w wierszach
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                int v = sudokuBoard.get(i, j);
                for (int k = j + 1; k < 9; k++) {
                    if (v == sudokuBoard.get(i, k)) {
                        assertTrue(false);
                    }
                }
            }
        }

        //weryfikacja braku powtórzeń w kwadratach
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                int v = sudokuBoard.get(i / 3 * 3 + j / 3, i % 3 * 3 + j % 3);
                for (int k = j + 1; k < 9; k++) {
                    if (v == sudokuBoard.get(i / 3 * 3 + k / 3, i % 3 * 3 + k % 3)) {
                        assertTrue(false);
                    }
                }
            }
        }
    }

    @Test
    public void testToString() {
        BackTrackingSudokuSolver solver = new BackTrackingSudokuSolver();
        assertTrue(solver.toString() == "BackTrackingSudokuSolver{ }");
    }

    @Test
    public void testEquals() {
        final BackTrackingSudokuSolver solver1 = new BackTrackingSudokuSolver();
        final BackTrackingSudokuSolver solver2 = new BackTrackingSudokuSolver();
        assertTrue(solver1.equals(solver1));
        assertFalse(solver1.equals(null));
        assertFalse(solver1.equals(new Object()));
        assertTrue(solver1.equals(solver2));

    }

    @Test
    public void testHashCode() {
        BackTrackingSudokuSolver solver1 = new BackTrackingSudokuSolver();
        BackTrackingSudokuSolver solver2 = new BackTrackingSudokuSolver();
        assertTrue(solver1.equals(solver2));
        assertTrue(solver1.hashCode() == 1);
        assertTrue(solver1.hashCode() == solver2.hashCode());
    }
}
