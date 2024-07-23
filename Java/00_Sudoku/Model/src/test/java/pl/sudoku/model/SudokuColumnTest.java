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


public class SudokuColumnTest {

    public SudokuColumnTest() {
    }

    @Test
    public void testVerify() {
        SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());
        sudokuBoard.solveGame();
        int col = 1;
        boolean isValid = true;

        loop:
        for (int i = 0; i < 9; i++) {
            int v = sudokuBoard.get(i, col);
            for (int j = i + 1; j < 9; j++) {
                if (v == sudokuBoard.get(j, col)) {
                    isValid = false;
                    break loop;
                }
            }
        }

        assertEquals(isValid, sudokuBoard.getColumn(col).verify());

        sudokuBoard.set(0, col, 0);
        assertTrue(sudokuBoard.getColumn(col).verify());

        sudokuBoard.set(1, col, sudokuBoard.get(2, col));
        assertFalse(sudokuBoard.getColumn(col).verify());
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
        SudokuColumn sudokuColumn = new SudokuColumn(Arrays.asList(array));

        sudokuColumn.setAutoverification(false);
        assertFalse(sudokuColumn.getAutoverifiacation());
        sudokuColumn.set(0, 2);
        sudokuColumn.set(1, 2);

        sudokuColumn.set(1, 3);
        sudokuColumn.setAutoverification(true);
        assertTrue(sudokuColumn.getAutoverifiacation());
        sudokuColumn.set(0, 2);
        assertThrows(SudokuBoardElementException.class, () -> sudokuColumn.set(1, 2));
    }

    @Test
    public void testToString() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }
        SudokuColumn sudokuColumn = new SudokuColumn(Arrays.asList(array));

        Pattern pattern = Pattern.compile(
                "SudokuColumn\\u007bsudokuFields=\\u005b*");
        assertTrue(pattern.matcher(sudokuColumn.toString()).region(0,28).find());
        pattern = Pattern.compile("\\u005d, enableAutoverification=false.");
        assertTrue(pattern.matcher(sudokuColumn.toString())
                .region(sudokuColumn.toString().length() - 33,
                sudokuColumn.toString().length()).find());
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
        SudokuColumn sudokuColumn1 = new SudokuColumn(Arrays.asList(array));

        assertTrue(sudokuColumn1.equals(sudokuColumn1));
        assertFalse(sudokuColumn1.equals(null));
        assertFalse(sudokuColumn1.equals(new Object()));

        SudokuColumn sudokuColumn2 = new SudokuColumn(Arrays.asList(array2));
        assertTrue(sudokuColumn1.equals(sudokuColumn2));
        sudokuColumn2.set(0, 1);
        assertFalse(sudokuColumn1.equals(sudokuColumn2));
        sudokuColumn2.set(0, 0);
        sudokuColumn2.setAutoverification(true);
        assertFalse(sudokuColumn1.equals(sudokuColumn2));
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
        SudokuColumn sudokuColumn1 = new SudokuColumn(Arrays.asList(array));
        SudokuColumn sudokuColumn2 = new SudokuColumn(Arrays.asList(array2));

        assertTrue(sudokuColumn1.equals(sudokuColumn2));
        assertTrue(sudokuColumn1.hashCode() == sudokuColumn2.hashCode());
    }

    @Test
    public void testClone() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
            array[i].setFieldValue(i);
        }

        SudokuColumn sudokuColumn = new SudokuColumn(Arrays.asList(array));
        SudokuColumn clonedSudokuColumn;

        /*
        try {
            clonedSudokuColumn = sudokuColumn.clone();
        } catch (CloneNotSupportedException e) {
            throw new RuntimeException(e);
        }*/
        clonedSudokuColumn = sudokuColumn.clone();

        assertNotSame(sudokuColumn, clonedSudokuColumn);
        assertEquals(sudokuColumn, clonedSudokuColumn);

        for (int i = 0; i < 9; i++) {
            int v = clonedSudokuColumn.get(i);
            clonedSudokuColumn.set(i, (v + 1) % 9);
            assertFalse(sudokuColumn.equals(clonedSudokuColumn));
            clonedSudokuColumn.set(i, v);
        }

        clonedSudokuColumn.setAutoverification(!clonedSudokuColumn.getAutoverifiacation());
        assertFalse(sudokuColumn.getAutoverifiacation()
                == clonedSudokuColumn.getAutoverifiacation());
    }

    @Test
    public void testSerialization() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }
        SudokuColumn sudokuColumn = new SudokuColumn(Arrays.asList(array));
        String fileName = "SerializationTest.txt";

        try (ObjectOutputStream out = new ObjectOutputStream(new FileOutputStream(fileName))) {
            out.writeObject(sudokuColumn);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

        SudokuColumn readSudokuColumn;
        try (ObjectInputStream in = new ObjectInputStream(new FileInputStream(fileName))) {
            readSudokuColumn = (SudokuColumn) in.readObject();
        } catch (IOException e) {
            throw new RuntimeException(e);
        } catch (ClassNotFoundException e) {
            throw new RuntimeException(e);
        }

        assertEquals(sudokuColumn, readSudokuColumn);
    }
}
