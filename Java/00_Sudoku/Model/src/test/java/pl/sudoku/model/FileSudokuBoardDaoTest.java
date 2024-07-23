package pl.sudoku.model;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import org.junit.jupiter.api.Test;
import pl.sudoku.model.exceptions.FileSudokuBoardDaoIoException;
import pl.sudoku.model.exceptions.SudokuClassNotFoundException;


public class FileSudokuBoardDaoTest {

    public FileSudokuBoardDaoTest() {
    }

    @Test
    public void writeAndReadTest() {
        String fileName = "SudokuBoardSaveFile.txt";
        //Dao<SudokuBoard> fileSudokuBoardDao = SudokuBoardDaoFactory.getFileDao(fileName);

        SudokuBoard sudokuBoard1 = new SudokuBoard(new BackTrackingSudokuSolver());
        sudokuBoard1.solveGame();

        SudokuBoard sudokuBoard2 = null;

        try (Dao<SudokuBoard> fileSudokuBoardDao = SudokuBoardDaoFactory.getFileDao(fileName)) {
            fileSudokuBoardDao.write(sudokuBoard1);
            sudokuBoard2 = fileSudokuBoardDao.read();
        } catch (Exception e) {
            assertTrue(e.getClass().getName().equals("SudokuClassNotFoundException")
                    || e.getClass().getName().equals("FileSudokuBoardDaoIoException"));
        }

        assertTrue(sudokuBoard1.equals(sudokuBoard2));
    }

    @Test
    public  void testExceptionsThrowing() {
        Dao<SudokuBoard> dao = SudokuBoardDaoFactory.getFileDao("K:/sudoku.sbs");
        SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());
        assertThrows(FileSudokuBoardDaoIoException.class, () -> dao.write(sudokuBoard));
        assertThrows(FileSudokuBoardDaoIoException.class, () -> dao.read());
    }

    @Test
    public void writeTest() {
        String fileName = "SudokuBoardSaveFile.txt";
        //Dao<SudokuBoard> fileSudokuBoardDao = SudokuBoardDaoFactory.getFileDao(fileName);

        SudokuBoard sudokuBoard1 = new SudokuBoard(new BackTrackingSudokuSolver());
        sudokuBoard1.solveGame();

        //fileSudokuBoardDao.write(sudokuBoard1);

        try (Dao<SudokuBoard> fileSudokuBoardDao = SudokuBoardDaoFactory.getFileDao(fileName)) {
            fileSudokuBoardDao.write(sudokuBoard1);
        } catch (Exception e) {
            assertTrue(e.getClass().getName().equals("FileSudokuBoardDaoIoException"));
        }

        SudokuBoard sudokuBoard2 = null;
        try (ObjectInputStream in = new ObjectInputStream(new FileInputStream(fileName))) {
            sudokuBoard2 = (SudokuBoard) in.readObject();
        } catch (IOException e) {
            //throw new RuntimeException(e);
            assertFalse(true);
        } catch (ClassNotFoundException e) {
            //throw new RuntimeException(e);
            assertFalse(true);
        }

        assertTrue(sudokuBoard1.equals(sudokuBoard2));
    }

    @Test
    public void readTest() {
        String fileName = "SudokuBoardSaveFile.txt";
        SudokuBoard sudokuBoard1 = new SudokuBoard(new BackTrackingSudokuSolver());
        sudokuBoard1.solveGame();

        try (ObjectOutputStream out = new ObjectOutputStream(new FileOutputStream(fileName))) {
            out.writeObject(sudokuBoard1);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

        SudokuBoard sudokuBoard2 = null;

        try (Dao<SudokuBoard> fileSudokuBoardDao = SudokuBoardDaoFactory.getFileDao(fileName)) {
            sudokuBoard2 = fileSudokuBoardDao.read();
        } catch (Exception e) {
            assertTrue(e.getClass().getName().equals("SudokuClassNotFoundException")
                    || e.getClass().getName().equals("FileSudokuBoardDaoIoException"));
        }

        assertTrue(sudokuBoard1.equals(sudokuBoard2));
    }
}