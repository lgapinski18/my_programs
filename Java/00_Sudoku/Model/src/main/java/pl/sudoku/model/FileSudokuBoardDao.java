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
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.Locale;
import java.util.ResourceBundle;
import pl.sudoku.model.exceptions.FileSudokuBoardDaoIoException;
import pl.sudoku.model.exceptions.SudokuClassNotFoundException;


/**
 * Klasa implementująca interfejs Dao. Służy do zapisywania i odczytywania w pliku obiektu
 * klasy SudokuBoard.
 * @version 1.0
 */
public class FileSudokuBoardDao implements Dao<SudokuBoard> { //, AutoCloseable
    private String fileName;

    /**
     * Konstruktor FileSudokuBoardDao.
     * @param fileName Nazwa pliku, do którego ma zostać zapisany lub odczytany obiekt SudokuBoard.
     */
    public FileSudokuBoardDao(String fileName) {
        this.fileName = fileName;
    }

    /**
     * Metoda służąca do odczytywania obiektu SudokuBoad z pliku.
     * @return Odczytany obiekt SudokuBoard.
     * @version 1.0
     * @since 1.0
     */
    @Override
    public SudokuBoard read() {
        Object sudokuBoard;
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.model.exceptionsmessage", Locale.getDefault());
        try (ObjectInputStream in = new ObjectInputStream(new FileInputStream(fileName))) {
            sudokuBoard = in.readObject();
        } catch (IOException e) {
            throw new FileSudokuBoardDaoIoException(
                    resourceBundle.getString("sudokuBoardDaoInputException"), e);
        } catch (ClassNotFoundException e) {
            throw new SudokuClassNotFoundException(
                    resourceBundle.getString("wantedClassNotFound"), e);
        }
        return (SudokuBoard) sudokuBoard;
    }

    /**
     * Metoda służąca do zapisywania do pliku obiektu SudokuBoard.
     * @param obj Obiekt SudokuBoard, który ma zostać zapisany do pliku.
     */
    @Override
    public void write(SudokuBoard obj) {
        /*
        BufferedWriter writer = Files.newBufferedWriter(
                Paths.get(fileName), StandardCharsets.UTF_8, StandardOpenOption.CREATE_NEW)
         */
        obj.removeAllObservers();
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.model.exceptionsmessage", Locale.getDefault());
        try (ObjectOutputStream out = new ObjectOutputStream(new FileOutputStream(fileName))) {
            out.writeObject(obj);
        } catch (IOException e) {
            throw new FileSudokuBoardDaoIoException(
                    resourceBundle.getString("sudokuBoardDaoOutputException"), e);
        }
    }

    @Override
    public void close() {

    }

}
