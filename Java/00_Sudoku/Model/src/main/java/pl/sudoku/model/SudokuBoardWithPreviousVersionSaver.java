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

//import pl.sudoku.model.exceptions.SudokuBoardWithPreviousVersionSaverException;

/**
 * Klasa umożliwiająca serializację SudokuBoard na raz.
 * @version 1.0
 * @author Łukasz Gapiński 242386
 * @author Mateusz Gapiński 242387
 */
public class SudokuBoardWithPreviousVersionSaver implements Dao<SudokuBoard> {
    Dao<SudokuBoard> dao;
    SudokuBoard prevSudokuBoard;

    String nameOfLOD = "";

    /**
     * Konstruktora klasy SudokuBoardWithPreviousVersionSaver.
     * @param dao Obiekt dało służący do zapisu obiektów SudokuBoard
     * @version 1.0
     * @since 1.0
     */
    public SudokuBoardWithPreviousVersionSaver(Dao<SudokuBoard> dao) {
        this.dao = dao;
    }

    /**
     * Metoda odczytująca przy pomocy podanego dao SudokuBoard z wypełnionymi polami.
     * @return Zapisane SudokuBoard z wypełnionymi polami.
     * @version 1.0
     * @since 1.0
     */
    @Override
    public SudokuBoard read() {
        DoubleSudoku readedSB = (DoubleSudoku) dao.read();
        prevSudokuBoard = readedSB.getPrevSudokuBoard();
        nameOfLOD = readedSB.getNameOfLOD();
        return readedSB.getWrittenSudokuBoard();
    }

    /**
     * Metoda zapisująca obiekt SudokuBoard reprezentujące planszę sudoku z wypełnionymi polami
     * wraz z obiektem oryginalnym SudokuBoard.
     * @param currentSudokuBoard Obiekt, który ma zostać zapisany do strumienia danych.
     * @version 1.0
     * @since 1.0
     */
    @Override
    public void write(SudokuBoard currentSudokuBoard) {
        DoubleSudoku doubleSudoku = new DoubleSudoku(
                prevSudokuBoard, currentSudokuBoard, nameOfLOD);
        dao.write(doubleSudoku);
    }

    /*
    /**
     * Metoda zapisująca obiekt SudokuBoard reprezentujące planszę sudoku z wypełnionymi polami
     * wraz z obiektem oryginalnym SudokuBoard oraz nazwą poziomu trudności.
     * @param currentSudokuBoard Obiekt SudokuBoard reprezentujące planszę sudoku z wypełnionymi
     *                           polami, który ma zostać zapisany do strumienia danych.
     * @param prevSudokuBoard Obiekt SudokuBoard reprezentujące oryginalną planszę sudoku
     *                        polami, który ma zostać zapisany do strumienia danych.
     * @param nameOfLOD Nazwa poziomu trudności która ma zostać zapisana.
     * @version 1.0
     * @since 1.0
     *//*
    public void write(SudokuBoard currentSudokuBoard, SudokuBoard prevSudokuBoard,
                      String nameOfLOD) {
        DoubleSudoku doubleSudoku = new DoubleSudoku(prevSudokuBoard,
                currentSudokuBoard, nameOfLOD);
        dao.write(doubleSudoku);
    }*/

    /**
     * Metoda służąca do zamknięcia otwartych zasobów.
     * @version 1.0
     * @since 1.0
     */
    @Override
    public void close() {

    }


    /**
     * Maetoda służąca do ustawienia oryginalnego SudokuBoard w celu jego zapisu.
     * @param sudokuBoard Obiekt SudokuBoard reprezentujący oryginalną planszę
     *                    po usunięciu elementów.
     * @version 1.0
     * @since 1.0
     */
    public void addPreviousSudokuBoard(SudokuBoard sudokuBoard) {
        prevSudokuBoard = sudokuBoard;
    }

    /**
     * Metoda odczytująca SudokuBoard reprezentujący oryginalną planszę po usunięciu elementów.
     * Możliwe jest to gdy doszło do wywołania metody read().
     * @return SudokuBoard reprezentujący oryginalną planszę po usunięciu elementów.
     * @version 1.0
     * @since 1.0
     */
    public SudokuBoard readPreviousSudokuBoard() {
        return prevSudokuBoard;
    }

    /**
     * Metoda zwracająca ustawiony obiekt Dao.
     * @return Ustawiony obiekt Dao.
     * @version 1.0
     * @since 1.0
     */
    public Dao<SudokuBoard> getDao() {
        return dao;
    }


    /**
     * Metoda służąca do ustawienia nazwy poziomu trudności na potrzeby jego zapisania.
     * @param nameOfLOD Zapisywana nazwa poziomu trudności.
     * @version 1.0
     * @since 1.0
     */
    public void setNameOfLOD(String nameOfLOD) {
        this.nameOfLOD = nameOfLOD;
    }

    /**
     * Metoda zwracająca nazwę poziomu trudności odczytanego po wykonaniu operacji odczytu.
     * @return Odczytana nazwa poziomu trudności.
     * @version 1.0
     * @since 1.0
     */
    public String getNameOfLOD() {
        return nameOfLOD;
    }
}
