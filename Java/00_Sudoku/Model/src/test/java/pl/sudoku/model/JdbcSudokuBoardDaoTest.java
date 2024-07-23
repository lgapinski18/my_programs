package pl.sudoku.model;

import static org.junit.Assert.assertFalse;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotSame;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.Statement;
import java.util.List;
import org.junit.jupiter.api.Test;
import pl.sudoku.model.exceptions.JdbcSudokuBoardDaoNotSuchSudokuBoardNameException;
import pl.sudoku.model.exceptions.JdbcSudokuBoardDaoSQLException;
import pl.sudoku.model.exceptions.SudokuBoardInstantionException;
import pl.sudoku.model.exceptions.SudokuBoardInvocationTargetException;
import pl.sudoku.model.exceptions.SudokuClassNotFoundException;


public class JdbcSudokuBoardDaoTest {

    @Test
    public void testWriteAndRead() {
        SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());

        sudokuBoard.set(1, 1, 1);

        try (Dao<SudokuBoard> dao = SudokuBoardDaoFactory
                .getJdbcDao("testSudokuBoardSaves", "testowaPlansza",
                        "UNKOWN")) {
            dao.write(sudokuBoard);

            SudokuBoard readSudokuBoard = dao.read();

            assertNotSame(sudokuBoard, readSudokuBoard);
            assertEquals(sudokuBoard, readSudokuBoard);
        } catch (Exception e) {
            assertTrue(e.getClass().getName().equals(
                            "pl.sudoku.model.exceptions.JdbcSudokuBoardDaoSQLException")
                    || e.getClass().getName().equals(
                            "pl.sudoku.model.exceptions.SudokuClassNotFoundException")
                    || e.getClass().getName().equals(
                            "pl.sudoku.model.exceptions.SudokuBoardInstantionException")
                    || e.getClass().getName().equals(
                            "pl.sudoku.model.exceptions.SudokuBoardIllegalAccessException")
                    || e.getClass().getName().equals(
                            "pl.sudoku.model.exceptions."
                                    + "JdbcSudokuBoardDaoNotSuchSudokuBoardNameException")
                    || e.getClass().getName().equals(
                            "pl.sudoku.model.exceptions.SudokuBoardInvocationTargetException"));
        }
    }

    @Test
    public void testThrowingExceptions() {
        SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());

        sudokuBoard.set(1, 1, 1);

        try (Dao<SudokuBoard> dao = SudokuBoardDaoFactory
                .getJdbcDao("testSudokuBoardSavesWrong", "testowaPlansza",
                        "UNKOWN")) {

            try (Connection connection = DriverManager.getConnection(
                    "jdbc:hsqldb:file:testSudokuBoardSavesWrong");
                 Statement statement = connection.createStatement()) {

                dao.write(sudokuBoard);
                statement.executeUpdate("UPDATE SudokuBoards sb "
                        + "SET sb.sudokuSolver='SudokuSolver';");
                assertThrows(SudokuClassNotFoundException.class, () -> {
                    SudokuBoard readSudokuBoard = dao.read();
                });
                statement.executeUpdate("UPDATE SudokuBoards sb "
                        + "SET sb.sudokuSolver='pl.sudoku.model.SudokuElement';");
                assertThrows(SudokuBoardInstantionException.class, () -> {
                    SudokuBoard readSudokuBoard = dao.read();
                });

                statement.executeUpdate("UPDATE SudokuBoards sb "
                        + "SET sb.sudokuSolver='pl.sudoku.model.TestSudokuSolver';");
                assertThrows(SudokuBoardInvocationTargetException.class, () -> {
                    SudokuBoard readSudokuBoard = dao.read();
                });

                statement.executeUpdate("ALTER TABLE SudokuBoards DROP COLUMN name;");
                assertThrows(JdbcSudokuBoardDaoSQLException.class, () -> {
                    dao.write(sudokuBoard);
                });
                assertThrows(JdbcSudokuBoardDaoSQLException.class, () -> {
                    SudokuBoard readSudokuBoard = dao.read();
                });
                assertThrows(JdbcSudokuBoardDaoSQLException.class, () -> {
                    JdbcSudokuBoardDao jdao = (JdbcSudokuBoardDao) dao;
                    jdao.getSudokuBoardNames();
                });

                statement.executeUpdate(
                        "ALTER TABLE SudokuBoards ADD COLUMN name VARCHAR(50);");
            }


        } catch (Exception e) {
            throw new RuntimeException(e);
        }

        try (Dao<SudokuBoard> dao = SudokuBoardDaoFactory
                .getJdbcDao("testSudokuBoardSavesWrong", "testowaPlanszaN",
                        "UNKOWN")) {
            assertThrows(JdbcSudokuBoardDaoNotSuchSudokuBoardNameException.class, () -> {
                SudokuBoard readSudokuBoard = dao.read();
            });
        } catch (Exception e) {
            assertTrue(e.getClass().getName().equals(
                    "pl.sudoku.model.exceptions.JdbcSudokuBoardDaoSQLException")
                    || e.getClass().getName().equals(
                    "pl.sudoku.model.exceptions.SudokuClassNotFoundException")
                    || e.getClass().getName().equals(
                    "pl.sudoku.model.exceptions.SudokuBoardInstantionException")
                    || e.getClass().getName().equals(
                    "pl.sudoku.model.exceptions.SudokuBoardIllegalAccessException")
                    || e.getClass().getName().equals(
                    "pl.sudoku.model.exceptions."
                            + "JdbcSudokuBoardDaoNotSuchSudokuBoardNameException")
                    || e.getClass().getName().equals(
                    "pl.sudoku.model.exceptions.SudokuBoardInvocationTargetException"));
        }
    }

    @Test
    public void testGetNames() {
        try (JdbcSudokuBoardDao dao = (JdbcSudokuBoardDao)
                SudokuBoardDaoFactory.getJdbcDao("testDB", "test",
                        "UNKKOWN");
                JdbcSudokuBoardDao dao2 = (JdbcSudokuBoardDao)
                    SudokuBoardDaoFactory.getJdbcDao("testDB", "test2",
                            "UNKKOWN")) {
            SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());
            dao.write(sudokuBoard);
            dao2.write(sudokuBoard);
            List<String> names = dao.getSudokuBoardNames();
            assertEquals(names.get(0), "test");
            assertEquals(names.get(1), "test2");
        } catch (Exception e) {
            assertFalse(true);
            //throw new RuntimeException(e);
        }
    }

    @Test
    public void testGetLevelOfDifficulty() {
        try (JdbcSudokuBoardDao dao = (JdbcSudokuBoardDao)
                SudokuBoardDaoFactory.getJdbcDao("testDB", "test",
                        "EASY")) {
            SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());
            dao.write(sudokuBoard);
        } catch (Exception e) {
            assertFalse(true);
            //throw new RuntimeException(e);
        }
        try (JdbcSudokuBoardDao dao = (JdbcSudokuBoardDao)
                SudokuBoardDaoFactory.getJdbcDao("testDB", "test",
                        "UNKNOWN")) {
            SudokuBoard sudokuBoard = dao.read();
            assertEquals(dao.getLevelOfDifficultyName(), "EASY");
        } catch (Exception e) {
            assertFalse(true);
        }
    }

    @Test
    public void testTransaction() {
        try (JdbcSudokuBoardDao dao = (JdbcSudokuBoardDao)
                SudokuBoardDaoFactory.getJdbcDao("atomicityTestDB", "test",
                        "EASY")) {
            SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());
            dao.write(sudokuBoard);
            List<String> names = dao.getSudokuBoardNames();
            assertEquals(names.get(0), "test");
        } catch (Exception e) {
            assertFalse(true);
        }

        try (JdbcSudokuBoardDao dao = (JdbcSudokuBoardDao)
                SudokuBoardDaoFactory.getJdbcDao("atomicityTestWrongDB", "test",
                        "EASY")) {
            try (Connection connection = DriverManager.getConnection(
                    "jdbc:hsqldb:file:atomicityTestWrongDB");
                 Statement statement = connection.createStatement()) {
                statement.executeUpdate("ALTER TABLE SudokuFields DROP COLUMN row");
            }
            SudokuBoard sudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());
            dao.write(sudokuBoard);
        } catch (Exception e) {
            try (JdbcSudokuBoardDao dao = (JdbcSudokuBoardDao)
                    SudokuBoardDaoFactory.getJdbcDao("atomicityTestWrongDB", "test",
                            "EASY")) {
                List<String> names = dao.getSudokuBoardNames();
                assertTrue(names.isEmpty());
            } catch (Exception ex) {
                assertFalse(true);
            }
        }

        SudokuBoard sudokuBoardDurability = new SudokuBoard(new BackTrackingSudokuSolver());
        try (JdbcSudokuBoardDao dao = (JdbcSudokuBoardDao)
                SudokuBoardDaoFactory.getJdbcDao("durabilityTestDB", "test",
                        "EASY")) {
            dao.write(sudokuBoardDurability);
        } catch (Exception e) {
            assertFalse(true);
        }

        try (JdbcSudokuBoardDao dao = (JdbcSudokuBoardDao)
                SudokuBoardDaoFactory.getJdbcDao("durabilityTestDB", "test",
                        "EASY")) {
            SudokuBoard readSudokuBoard = dao.read();
            assertEquals(sudokuBoardDurability, readSudokuBoard);
            assertNotSame(sudokuBoardDurability, readSudokuBoard);
        } catch (Exception e) {
            assertFalse(true);
        }
    }
}

