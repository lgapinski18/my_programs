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

public class SudokuBoxTest {

    public SudokuBoxTest() {
    }

    @Test
    public void testVerify() {
        SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());
        sudokuBoard.solveGame();
        int boxX = 1;
        int boxY = 1;
        boolean isValid = true;

        loop:
        for (int i = 0; i < 9; i++) {
            int v = sudokuBoard.get(boxY * 3 + i / 3, boxX * 3 + i % 3);
            for (int j = i + 1; j < 9; j++) {
                if (v == sudokuBoard.get(boxY * 3 + j / 3, boxX * 3 + j % 3)) {
                    isValid = false;
                    break loop;
                }
            }
        }

        assertEquals(isValid, sudokuBoard.getBox(boxX, boxY).verify());

        sudokuBoard.set(boxX * 3, boxY * 3, 0);
        assertTrue(sudokuBoard.getBox(boxX, boxY).verify());

        sudokuBoard.set(boxX * 3 + 1, boxY * 3, sudokuBoard.get(boxX * 3 + 2, boxY * 3));
        assertFalse(sudokuBoard.getBox(boxX, boxY).verify());
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
        SudokuBox sudokuBox = new SudokuBox(Arrays.asList(array));

        sudokuBox.setAutoverification(false);
        assertFalse(sudokuBox.getAutoverifiacation());
        sudokuBox.set(0, 2);
        sudokuBox.set(1, 2);

        sudokuBox.set(1, 3);
        sudokuBox.setAutoverification(true);
        assertTrue(sudokuBox.getAutoverifiacation());
        sudokuBox.set(0, 2);
        assertThrows(SudokuBoardElementException.class, () -> sudokuBox.set(1, 2));
    }

    @Test
    public void testToString() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }
        SudokuBox sudokuBox = new SudokuBox(Arrays.asList(array));

        Pattern pattern = Pattern.compile(
                "SudokuBox\\u007bsudokuFields=\\u005b*");
        assertTrue(pattern.matcher(sudokuBox.toString()).region(0,25).find());
        pattern = Pattern.compile("\\u005d, enableAutoverification=false.");
        assertTrue(pattern.matcher(sudokuBox.toString()).region(sudokuBox.toString().length() - 33,
                sudokuBox.toString().length()).find());
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
        SudokuBox sudokuBox1 = new SudokuBox(Arrays.asList(array));

        assertTrue(sudokuBox1.equals(sudokuBox1));
        assertFalse(sudokuBox1.equals(null));
        assertFalse(sudokuBox1.equals(new Object()));

        SudokuBox sudokuBox2 = new SudokuBox(Arrays.asList(array2));
        assertTrue(sudokuBox1.equals(sudokuBox2));
        sudokuBox2.set(0, 1);
        assertFalse(sudokuBox1.equals(sudokuBox2));
        sudokuBox2.set(0, 0);
        sudokuBox2.setAutoverification(true);
        assertFalse(sudokuBox1.equals(sudokuBox2));
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
        SudokuBox sudokuBox1 = new SudokuBox(Arrays.asList(array));
        SudokuBox sudokuBox2 = new SudokuBox(Arrays.asList(array2));

        assertTrue(sudokuBox1.equals(sudokuBox2));
        assertTrue(sudokuBox1.hashCode() == sudokuBox2.hashCode());
    }

    @Test
    public void testClone() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }

        SudokuBox sudokuBox = new SudokuBox(Arrays.asList(array));
        SudokuBox clonedSudokuBox;

        /*try {
            clonedSudokuBox = sudokuBox.clone();
        } catch (CloneNotSupportedException e) {
            throw new RuntimeException(e);
        }*/
        clonedSudokuBox = sudokuBox.clone();

        assertNotSame(sudokuBox, clonedSudokuBox);
        assertEquals(sudokuBox, clonedSudokuBox);

        for (int i = 0; i < 9; i++) {
            int v = clonedSudokuBox.get(i);
            clonedSudokuBox.set(i, (v + 1) % 9);
            assertFalse(sudokuBox.equals(clonedSudokuBox));
            clonedSudokuBox.set(i, v);
        }

        clonedSudokuBox.setAutoverification(!clonedSudokuBox.getAutoverifiacation());
        assertFalse(sudokuBox.getAutoverifiacation()
                == clonedSudokuBox.getAutoverifiacation());
    }

    @Test
    public void testSerialization() {
        SudokuField[] array = new SudokuField[9];
        for (int i = 0; i < 9; i++) {
            array[i] = new SudokuField();
        }
        SudokuBox sudokuBox = new SudokuBox(Arrays.asList(array));
        String fileName = "SerializationTest.txt";

        try (ObjectOutputStream out = new ObjectOutputStream(new FileOutputStream(fileName))) {
            out.writeObject(sudokuBox);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

        SudokuBox readSudokuBox;
        try (ObjectInputStream in = new ObjectInputStream(new FileInputStream(fileName))) {
            readSudokuBox = (SudokuBox) in.readObject();
        } catch (IOException e) {
            throw new RuntimeException(e);
        } catch (ClassNotFoundException e) {
            throw new RuntimeException(e);
        }

        assertEquals(sudokuBox, readSudokuBox);
    }
}
