package pl.sudoku.model;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotEquals;
import static org.junit.jupiter.api.Assertions.assertNotSame;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import org.junit.jupiter.api.Test;

public class SudokuFieldTest {

    public SudokuFieldTest() {
    }

    @Test
    public void testGetSetFieldValue() {
        SudokuField sudokuField = new SudokuField();
        int value = 1;

        sudokuField.setFieldValue(value);
        assertEquals(sudokuField.getFieldValue(), value);
    }

    @Test
    public void testAddActionListener() {
        SudokuField sudokuField = new SudokuField();
        sudokuField.addObserver(() -> {
            throw new RuntimeException();
        });

        int value = 1;
        assertThrows(RuntimeException.class, () -> sudokuField.setFieldValue(value));
    }

    @Test
    public void testRemoveAllObservers() {
        SudokuField sudokuField = new SudokuField();
        sudokuField.addObserver(() -> {
            throw new RuntimeException();
        });
        sudokuField.removeAllObservers();

        int value = 1;
        assertDoesNotThrow(() -> sudokuField.setFieldValue(value));
    }

    @Test
    public void testToString() {
        SudokuField sudokuField = new SudokuField();
        assertTrue(sudokuField.toString()
                        .equals(new String("SudokuField{value=0}")));
    }

    @Test
    public void testEquals() {
        SudokuField sudokuField1 = new SudokuField();
        sudokuField1.setFieldValue(1);
        SudokuField sudokuField2 = new SudokuField();
        sudokuField2.setFieldValue(1);

        assertTrue(sudokuField1.equals(sudokuField1));
        assertFalse(sudokuField1.equals(null));
        assertFalse(sudokuField1.equals(new Object()));
        assertTrue(sudokuField1.equals(sudokuField2));
        sudokuField2.setFieldValue(2);
        assertFalse(sudokuField1.equals(sudokuField2));
    }

    @Test
    public void testHashCode() {
        SudokuField sudokuField1 = new SudokuField();
        sudokuField1.setFieldValue(1);
        SudokuField sudokuField2 = new SudokuField();
        sudokuField2.setFieldValue(1);

        assertTrue(sudokuField1.equals(sudokuField2));
        assertTrue(sudokuField1.hashCode() == sudokuField2.hashCode());
    }

    @Test
    public void testClone() {
        SudokuField sudokuField = new SudokuField();
        sudokuField.setFieldValue(1);

        SudokuField clonedSudokuField = sudokuField.clone();

        assertNotSame(sudokuField, clonedSudokuField);
        assertEquals(sudokuField, clonedSudokuField);

        clonedSudokuField.setFieldValue(2);
        assertNotEquals(sudokuField, clonedSudokuField);
    }

    @Test
    public void testCompareTo() {
        SudokuField sudokuField1 = new SudokuField();
        sudokuField1.setFieldValue(1);
        SudokuField sudokuField2 = new SudokuField();
        sudokuField2.setFieldValue(0);

        assertTrue(sudokuField1.compareTo(sudokuField2) > 0);

        sudokuField2.setFieldValue(1);

        assertTrue(sudokuField1.compareTo(sudokuField2) == 0);

        sudokuField2.setFieldValue(2);

        assertTrue(sudokuField1.compareTo(sudokuField2) < 0);

        assertThrows(NullPointerException.class, () -> {
            sudokuField1.compareTo(null);
        });
    }
}
