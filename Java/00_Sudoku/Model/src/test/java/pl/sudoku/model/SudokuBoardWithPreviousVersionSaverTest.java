package pl.sudoku.model;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotSame;
import static org.junit.jupiter.api.Assertions.assertSame;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import org.junit.jupiter.api.Test;
import pl.sudoku.model.exceptions.SudokuBoardWithPreviousVersionSaverException;

public class SudokuBoardWithPreviousVersionSaverTest {

    @Test
    public void testWriteAndRead() {
        try (SudokuBoardWithPreviousVersionSaver saver = new SudokuBoardWithPreviousVersionSaver(
                SudokuBoardDaoFactory.getFileDao(
                        "SudokuBoardWithPreviousVersionSaver.sbs"))) {

            SudokuBoard sudokuBoardOriginal = new SudokuBoard(new BackTrackingSudokuSolver());
            sudokuBoardOriginal.set(1, 1, 1);

            SudokuBoard sudokuBoardCurrent = new SudokuBoard(new BackTrackingSudokuSolver());
            sudokuBoardCurrent.set(1, 1, 2);

            saver.addPreviousSudokuBoard(sudokuBoardOriginal);
            saver.setNameOfLOD("EASY");
            saver.write(sudokuBoardCurrent);

            SudokuBoardWithPreviousVersionSaver reader = new SudokuBoardWithPreviousVersionSaver(
                    SudokuBoardDaoFactory.getFileDao(
                            "SudokuBoardWithPreviousVersionSaver.sbs"));

            SudokuBoard sudokuBoardCurrentRead = reader.read();
            SudokuBoard sudokuBoardOriginalRead = reader.readPreviousSudokuBoard();
            String levelOfDifficulty = reader.getNameOfLOD();

            assertEquals(sudokuBoardCurrent, sudokuBoardCurrentRead);
            assertEquals(sudokuBoardOriginal, sudokuBoardOriginalRead);
            assertEquals("EASY", levelOfDifficulty);
        } /*catch (Exception e) {
            assertTrue(e.getClass().getName().equals(
                    "pl.sudoku.model.exceptions.SudokuClassNotFoundException")
                    || e.getClass().getName().equals(
                    "pl.sudoku.model.exceptions.FileSudokuBoardDaoIoException")
                    || e.getClass().getName().equals(
                    "pl.sudoku.model.exceptions.SudokuBoardWithPreviousVersionSaverException"));
        }*/
    }

    @Test
    public void testGetDao() {
        Dao<SudokuBoard> dao = SudokuBoardDaoFactory.getFileDao(
                "SudokuBoardWithPreviousVersionSaver.sbs");
        SudokuBoardWithPreviousVersionSaver saver = new SudokuBoardWithPreviousVersionSaver(dao);
        assertSame(dao, saver.getDao());
    }
}
