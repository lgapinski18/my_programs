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

import java.lang.reflect.InvocationTargetException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.ResourceBundle;
import pl.sudoku.model.exceptions.JdbcSudokuBoardDaoNotSuchSudokuBoardNameException;
import pl.sudoku.model.exceptions.JdbcSudokuBoardDaoSQLException;
import pl.sudoku.model.exceptions.SudokuBoardIllegalAccessException;
import pl.sudoku.model.exceptions.SudokuBoardInstantionException;
import pl.sudoku.model.exceptions.SudokuBoardInvocationTargetException;
import pl.sudoku.model.exceptions.SudokuClassNotFoundException;


public class JdbcSudokuBoardDao implements Dao<SudokuBoard>, AutoCloseable {
    private String dataBaseName;
    private String name;

    private String levelOfDifficultyName;

    /**
     * Konstruktor ustawiający nazwę pod jaką zostanie zapisany SudokuBoard w bazie danych.
     * @param dataBaseName Nazwa bazy danych wykorzystywanej do zapisu.
     * @param name Nazwa SudokuBoard pod jaką zostanie zapisany w bazie danych.
     * @param levelOfDifficultyName Nazwa Poziomu trudności dla zapisywanej planszy.
     */
    public JdbcSudokuBoardDao(String dataBaseName, String name, String levelOfDifficultyName) {
        this.dataBaseName = dataBaseName;
        this.name = name;
        this.levelOfDifficultyName = levelOfDifficultyName;
    }

    /**
     * Metoda służąca do odczytywania ze strumienia danych obiektu klasy SudokuBoard.
     *
     * @return Odczytany obiekt klasy SudokuBoard.
     * @version 1.0
     * @since 1.0
     */
    @Override
    public SudokuBoard read() {
        SudokuBoard sudokuBoard;
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.model.exceptionsmessage", Locale.getDefault());

        try (Connection connection = DriverManager.getConnection(
                "jdbc:hsqldb:file:" + dataBaseName)) {
            connection.setAutoCommit(false);
            connection.setTransactionIsolation(Connection.TRANSACTION_READ_COMMITTED);
            try (Statement statement = connection.createStatement()) {

                statement.executeUpdate("CREATE TABLE IF NOT EXISTS SudokuBoards "
                        + "(id INTEGER GENERATED BY DEFAULT AS IDENTITY(START WITH 1, INCREMENT"
                        + " BY 1) PRIMARY KEY, name VARCHAR(30), sudokuSolver VARCHAR(75),"
                        + " levelOfDifficulty VARCHAR(10))");
                statement.executeUpdate("CREATE TABLE IF NOT EXISTS SudokuFields "
                        + "(boardId INTEGER, row INTEGER, column INTEGER, value INTEGER)");

                try (ResultSet resultID = statement.executeQuery(
                        new StringBuilder("SELECT id,sudokuSolver,levelOfDifficulty")
                                .append(" FROM SudokuBoards WHERE name = '")
                                .append(name).append("';").toString())) {

                    if (!resultID.next()) {
                        throw new JdbcSudokuBoardDaoNotSuchSudokuBoardNameException(
                                resourceBundle.getString("jdbcNotSuchSudokuBoardName"));
                    }

                    int id = resultID.getInt(1);

                    String solverClassName = resultID.getString(2);
                    sudokuBoard = new SudokuBoard((SudokuSolver) Class.forName(solverClassName)
                            .getDeclaredConstructors()[0].newInstance());
                    levelOfDifficultyName = resultID.getString(3);

                   try (ResultSet fieldsResult = statement.executeQuery(
                            new StringBuilder("SELECT * FROM SudokuFields WHERE boardId = ")
                                    .append(id).append(";").toString())) {
                       while (fieldsResult.next()) {
                           sudokuBoard.set(fieldsResult.getInt(2),
                                   fieldsResult.getInt(3),
                                   fieldsResult.getInt(4));
                       }
                   }
                }
            }
        } catch (SQLException e) {
            throw new JdbcSudokuBoardDaoSQLException(e);
        } catch (ClassNotFoundException e) {
            throw new SudokuClassNotFoundException(
                    resourceBundle.getString("sudokuSolverClassNotFound"), e);
        } catch (InstantiationException e) {
            throw new SudokuBoardInstantionException(e);
        } catch (IllegalAccessException e) {
            throw new SudokuBoardIllegalAccessException(e);
        } catch (InvocationTargetException e) {
            throw new SudokuBoardInvocationTargetException(e);
        }
        return sudokuBoard;
    }

    /**
     * Metoda służąca do zapisywania obiektu klasy SudokuBoard do strumienia danych.
     *
     * @param sudokuBoard Obiekt, który ma zostać zapisany do strumienia danych.
     * @version 1.0
     * @since 1.0
     */
    @Override
    public void write(SudokuBoard sudokuBoard) {

        try (Connection connection = DriverManager.getConnection(
                "jdbc:hsqldb:file:" + dataBaseName)) {
            connection.setAutoCommit(false);
            connection.setTransactionIsolation(Connection.TRANSACTION_READ_COMMITTED);
            try (Statement statement = connection.createStatement()) {

                statement.executeUpdate("CREATE TABLE IF NOT EXISTS SudokuBoards "
                        + "(id INTEGER GENERATED BY DEFAULT AS IDENTITY(START WITH 1, INCREMENT"
                        + " BY 1) PRIMARY KEY, name VARCHAR(30), sudokuSolver VARCHAR(75),"
                        + " levelOfDifficulty VARCHAR(10))");
                statement.executeUpdate("CREATE TABLE IF NOT EXISTS SudokuFields "
                        + "(boardId INTEGER, row INTEGER, column INTEGER, value INTEGER,"
                        + "CONSTRAINT boardIdFK FOREIGN KEY "
                        + "(boardId) REFERENCES SudokuBoards(id))");

                //statement.executeUpdate("DELETE FROM SudokuFields sf INNER JOIN SudokuBoards sbON"
                statement.executeUpdate("DELETE FROM SudokuFields sf WHERE sf.boardId = (SELECT"
                        + " DISTINCT id FROM SudokuBoards sb WHERE sb.name = '" + name + "');");
                statement.executeUpdate(
                        "DELETE FROM SudokuBoards WHERE name = '" + name + "';");

                statement.executeUpdate(
                        new StringBuilder("INSERT INTO SudokuBoards (name,sudokuSolver,"
                        + "levelOfDifficulty) VALUES ('")
                            .append(name).append("','")
                            .append(sudokuBoard.getSudokuSolver().getClass().getName())
                            .append("','").append(levelOfDifficultyName).append("');")
                            .toString());
                try (ResultSet resultID = statement.executeQuery(
                        new StringBuilder("SELECT id FROM SudokuBoards WHERE name = '")
                                .append(name).append("';").toString())) {

                    int id = 0;
                    if (resultID.next()) {
                        id = resultID.getInt(1);
                    }

                    for (int row = 0; row < 9; row++) {
                        for (int col = 0; col < 9; col++) {
                            statement.executeUpdate(new StringBuilder("INSERT INTO SudokuFields")
                                    .append("(boardId,row,column,value) VALUES (")
                                    .append(id).append(",")
                                    .append(row).append(",")
                                    .append(col).append(",")
                                    .append(sudokuBoard.get(row, col)).append(");").toString());
                        }
                    }
                }
            }
            connection.commit();
        } catch (SQLException e) {
            throw new JdbcSudokuBoardDaoSQLException(e);
        }
    }

    public List<String> getSudokuBoardNames() {
        ArrayList<String> names = new ArrayList<>();

        /*ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.model.exceptionsmessage", Locale.getDefault());*/

        try (Connection connection = DriverManager.getConnection(
                "jdbc:hsqldb:file:" + dataBaseName)) {
            connection.setAutoCommit(false);
            connection.setTransactionIsolation(Connection.TRANSACTION_READ_COMMITTED);
            try (Statement statement = connection.createStatement()) {

                statement.executeUpdate("CREATE TABLE IF NOT EXISTS SudokuBoards "
                        + "(id INTEGER GENERATED BY DEFAULT AS IDENTITY(START WITH 1, INCREMENT"
                        + " BY 1) PRIMARY KEY, name VARCHAR(30), sudokuSolver VARCHAR(75),"
                        + " levelOfDifficulty VARCHAR(10))");
                statement.executeUpdate("CREATE TABLE IF NOT EXISTS SudokuFields "
                        + "(boardId INTEGER, row INTEGER, column INTEGER, value INTEGER)");

                try (ResultSet resultNames = statement.executeQuery("SELECT DISTINCT "
                                + "name FROM SudokuBoards")) {

                    while (resultNames.next()) {
                        names.add(resultNames.getString(1));
                    }
                }
            }
        } catch (SQLException e) {
            throw new JdbcSudokuBoardDaoSQLException(e);
        }
        return names;
    }

    public String getLevelOfDifficultyName() {
        return levelOfDifficultyName;
    }

    @Override
    public void close() throws Exception {

    }
}