package pl.sudoku.model;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotSame;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.Arrays;
import java.util.regex.Pattern;
import org.junit.jupiter.api.Test;
import pl.sudoku.model.exceptions.SudokuBoardElementException;


public class SudokuRowTest {

    public SudokuRowTest() {
    }

    @Test
    public void testVerify() {
        SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());
        sudokuBoard.solveGame();
        int row = 1;
        boolean isValid = true;

        loop:
        for (int i = 0; i < 9; i++) {
            int v = sudokuBoard.get(row, i);
            for (int j = i + 1; j < 9; j++) {
                if (v == sudokuBoard.get(row, j)) {
                    isValid = false;
                    break loop;
                }
            }
        }

        assertEquals(isValid, sudokuBoard.getRow(row).verify());

        sudokuBoard.set(row, 0, 0);
        assertTrue(sudokuBoard.getRow(row).verify());

        sudokuBoard.set(row, 1, sudokuBoard.get(row, 2));
        assertFalse(sudokuBoard.getRow(row).verify());
    }

    @Test
    public void testGetAndSet() {
        SudokuField[] array = new SudokuField[9];
        Arrays.fill(array, new SudokuField());
        SudokuBox sudokuBox = new SudokuBox(Arrays.asList(array));
        sudokuBox.set(0, 2);
        assertEquals(2, sudokuBox.get(0));
    }

    @Test
    public void testAutoverification() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }
        SudokuRow sudokuRow = new SudokuRow(Arrays.asList(array));

        sudokuRow.setAutoverification(false);
        assertFalse(sudokuRow.getAutoverifiacation());
        sudokuRow.set(0, 2);
        sudokuRow.set(1, 2);

        sudokuRow.set(1, 3);
        sudokuRow.setAutoverification(true);
        assertTrue(sudokuRow.getAutoverifiacation());
        sudokuRow.set(0, 2);
        assertThrows(SudokuBoardElementException.class, () -> sudokuRow.set(1, 2));
    }

    @Test
    public void testToString() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }
        SudokuRow sudokuRow = new SudokuRow(Arrays.asList(array));

        Pattern pattern = Pattern.compile(
                "SudokuRow\\u007bsudokuFields=\\u005b*");
        assertTrue(pattern.matcher(sudokuRow.toString()).region(0, 25).find());
        pattern = Pattern.compile("\\u005d, enableAutoverification=false.");
        assertTrue(pattern.matcher(sudokuRow.toString())
                .region(sudokuRow.toString().length() - 33,
                        sudokuRow.toString().length()).find());
    }

    @Test
    public void testEquals() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }

        SudokuField[] array2 = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array2[i] = new SudokuField();
        }
        SudokuRow sudokuRow1 = new SudokuRow(Arrays.asList(array));

        assertTrue(sudokuRow1.equals(sudokuRow1));
        assertFalse(sudokuRow1.equals(null));
        assertFalse(sudokuRow1.equals(new Object()));

        SudokuRow sudokuRow2 = new SudokuRow(Arrays.asList(array2));
        assertTrue(sudokuRow1.equals(sudokuRow2));
        sudokuRow2.set(0, 1);
        assertFalse(sudokuRow1.equals(sudokuRow2));
        sudokuRow2.set(0, 0);
        sudokuRow2.setAutoverification(true);
        assertFalse(sudokuRow1.equals(sudokuRow2));
    }

    @Test
    public void testHashCode() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }

        SudokuField[] array2 = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array2[i] = new SudokuField();
        }
        SudokuRow sudokuRow1 = new SudokuRow(Arrays.asList(array));
        SudokuRow sudokuRow2 = new SudokuRow(Arrays.asList(array2));

        assertTrue(sudokuRow1.equals(sudokuRow2));
        assertTrue(sudokuRow1.hashCode() == sudokuRow2.hashCode());
    }

    @Test
    public void testClone() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }

        SudokuRow sudokuRow = new SudokuRow(Arrays.asList(array));
        SudokuRow clonedSudokuRow;

        /*try {
            clonedSudokuRow = sudokuRow.clone();
        } catch (CloneNotSupportedException e) {
            throw new RuntimeException(e);
        }*/
        clonedSudokuRow = sudokuRow.clone();

        assertNotSame(sudokuRow, clonedSudokuRow);
        assertEquals(sudokuRow, clonedSudokuRow);

        for (int i = 0; i < 9; i++) {
            int v = clonedSudokuRow.get(i);
            clonedSudokuRow.set(i, (v + 1) % 9);
            assertFalse(sudokuRow.equals(clonedSudokuRow));
            clonedSudokuRow.set(i, v);
        }

        clonedSudokuRow.setAutoverification(!clonedSudokuRow.getAutoverifiacation());
        assertFalse(sudokuRow.getAutoverifiacation()
                == clonedSudokuRow.getAutoverifiacation());
    }

    @Test
    public void testSerialization() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }
        SudokuRow sudokuRow = new SudokuRow(Arrays.asList(array));
        String fileName = "SerializationTest.txt";

        try (ObjectOutputStream out = new ObjectOutputStream(new FileOutputStream(fileName))) {
            out.writeObject(sudokuRow);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

        SudokuRow readSudokuRow;
        try (ObjectInputStream in = new ObjectInputStream(new FileInputStream(fileName))) {
            readSudokuRow = (SudokuRow) in.readObject();
        } catch (IOException e) {
            throw new RuntimeException(e);
        } catch (ClassNotFoundException e) {
            throw new RuntimeException(e);
        }

        assertEquals(sudokuRow, readSudokuRow);
    }
}
