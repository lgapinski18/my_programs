package pl.sudoku.model;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotSame;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.util.Arrays;
import java.util.List;
import java.util.regex.Pattern;
import org.junit.jupiter.api.Test;
import pl.sudoku.model.exceptions.SudokuBoardAutoverificationNotPassedException;
import pl.sudoku.model.exceptions.SudokuBoardException;


public class SudokuBoardTest {
    private SudokuBoard sudokuBoardPrototype = new SudokuBoard(new BackTrackingSudokuSolver());

    public SudokuBoardTest() {
    }

    @Test
    public void testSolveGame() {
        SudokuBoard sudokuBoard = sudokuBoardPrototype.clone();
        sudokuBoard.solveGame();

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
    public void testSet() {
        SudokuBoard sudokuBoard = sudokuBoardPrototype.clone();
        int val = 1;
        int x = 1;
        int y = 1;
        sudokuBoard.set(y, x, val);
        assertEquals(sudokuBoard.get(y, x), val);
    }

    @Test
    public void testToString() {
        SudokuBoard sudokuBoard = sudokuBoardPrototype.clone();

        Pattern pattern = Pattern.compile("SudokuBoard\\u007bboard=\\u005b");
        assertTrue(pattern.matcher(sudokuBoard.toString()).region(0,19).find());
        pattern = Pattern.compile("\\u005d, enableAutoverification=false,"
                                + " sudokuSolver=BackTrackingSudokuSolver. ..");
        assertTrue(pattern.matcher(sudokuBoard.toString())
                .region(sudokuBoard.toString().length() - 78,
                sudokuBoard.toString().length()).find());
    }

    @Test
    public void testEquals() {
        SudokuSolver sudokuSolver = new BackTrackingSudokuSolver();
        SudokuBoard sudokuBoard1 = new SudokuBoard(sudokuSolver);
        Integer intg = 1;

        assertTrue(sudokuBoard1.equals(sudokuBoard1));
        assertFalse(sudokuBoard1.equals(null));
        assertFalse(sudokuBoard1.equals(intg));

        SudokuBoard sudokuBoard2 = new SudokuBoard(sudokuSolver);
        assertTrue(sudokuBoard1.equals(sudokuBoard2));
        sudokuBoard2.set(1, 1, 1);
        assertFalse(sudokuBoard1.equals(sudokuBoard2));
        sudokuBoard2.set(1, 1, 0);
        sudokuBoard2.setAutoverification(true);
        assertFalse(sudokuBoard1.equals(sudokuBoard2));

        SudokuBoard sudokuBoard3 = new SudokuBoard(null);
        assertFalse(sudokuBoard1.equals(sudokuBoard3));
    }

    @Test
    public void testHashCode() {
        List<Integer> list = Arrays.asList(new Integer[81]);
        SudokuSolver sudokuSolver = new BackTrackingSudokuSolver();
        SudokuBoard sudokuBoard = new SudokuBoard(sudokuSolver);
        sudokuBoard.solveGame();

        SudokuBoard sudokuBoard2 = new SudokuBoard(sudokuSolver);
        for (int x = 0; x < 9; x++) {
            for (int y = 0; y < 9; y++) {
                sudokuBoard2.set(x, y, sudokuBoard.get(x, y));
            }
        }

        assertTrue(sudokuBoard.equals(sudokuBoard2));
        assertEquals(sudokuBoard2.hashCode(), sudokuBoard.hashCode());
    }

    @Test
    public void testNotRepeating() {
        SudokuSolver sudokuSolver = new BackTrackingSudokuSolver();
        SudokuBoard sudokuBoard1 = new SudokuBoard(sudokuSolver);
        SudokuBoard sudokuBoard2 = new SudokuBoard(sudokuSolver);
        SudokuBoard sudokuBoard3 = new SudokuBoard(sudokuSolver);

        sudokuBoard1.solveGame();
        sudokuBoard2.solveGame();
        sudokuBoard3.solveGame();

        assertFalse(sudokuBoard1.equals(sudokuBoard2) && sudokuBoard1.equals(sudokuBoard3)
                && sudokuBoard2.equals(sudokuBoard3));
    }

    @Test
    public void testAutoverification() {
        SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());

        sudokuBoard.setAutoverification(false);
        assertFalse(sudokuBoard.getAutoverifiacation());
        sudokuBoard.set(1,1, 1);

        sudokuBoard.setAutoverification(true);
        assertTrue(sudokuBoard.getAutoverifiacation());

        //Testowanie wszystkich branchy dla checkBoard
        sudokuBoard.set(1,1, 1);
        assertThrows(SudokuBoardException.class,
                () -> sudokuBoard.set(1,5, 1));
        sudokuBoard.set(1,5, 0);
        assertThrows(SudokuBoardException.class,
                () -> sudokuBoard.set(4,1, 1));
        sudokuBoard.set(4,1, 0);
        assertThrows(SudokuBoardException.class,
                () -> sudokuBoard.set(2,2, 1));
        sudokuBoard.set(2,2, 0);
    }

    @Test
    public void testList() {
        List<SudokuField> list = Arrays.asList(new SudokuField[3]);

        assertThrows(UnsupportedOperationException.class, () -> {
            list.add(new SudokuField()); });
        assertDoesNotThrow(() -> {
            list.set(1, new SudokuField()); });
    }

    @Test
    public void testClone() {
        SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());
        SudokuBoard clonedSudokuBoard;
        /*try {
            clonedSudokuBoard = sudokuBoard.clone();
        } catch (CloneNotSupportedException e) {
            throw new RuntimeException(e);
        }*/
        clonedSudokuBoard = sudokuBoard.clone();

        assertNotSame(sudokuBoard, clonedSudokuBoard);
        assertEquals(sudokuBoard, clonedSudokuBoard);

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                int v = clonedSudokuBoard.get(i, j);
                clonedSudokuBoard.set(i, j, (v + 1) % 9);
                assertFalse(sudokuBoard.equals(clonedSudokuBoard));
                clonedSudokuBoard.set(i, j, v);
            }
        }

        clonedSudokuBoard.setAutoverification(!clonedSudokuBoard.getAutoverifiacation());
        assertFalse(sudokuBoard.getAutoverifiacation()
                == clonedSudokuBoard.getAutoverifiacation());

        assertNotSame(sudokuBoard.getSudokuSolver(), clonedSudokuBoard.getSudokuSolver());
    }

    @Test
    public void testAddObserver() {
        SudokuBoard sudokuBoard = sudokuBoardPrototype.clone();
        sudokuBoard.setAutoverification(false);
        sudokuBoard.addObserver(0,0, () -> {
            throw new RuntimeException();
        });

        int value = 1;
        assertThrows(RuntimeException.class, () -> sudokuBoard.set(0, 0, value));
        sudokuBoard.setAutoverification(true);
        assertThrows(SudokuBoardException.class,
                () -> sudokuBoard.set(0, 1, sudokuBoard.get(0, 0)));
    }

    @Test
    public void testRemoveAllObservers() {
        SudokuBoard sudokuBoard = sudokuBoardPrototype.clone();
        sudokuBoard.setAutoverification(false);
        sudokuBoard.addObserver(0,0, () -> {
            throw new RuntimeException();
        });
        sudokuBoard.removeAllObservers();

        int value = 1;
        assertDoesNotThrow(() -> sudokuBoard.set(0, 0, value));
        sudokuBoard.setAutoverification(true);
        assertThrows(SudokuBoardException.class,
                () -> sudokuBoard.set(0, 1, sudokuBoard.get(0, 0)));
    }
}
