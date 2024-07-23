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

/**
 * Interfejs Dao, którego celem jest zapisywanie i odczytywanie danych ze strumienia.
 * @param <T> Klasa obiektu jaki będzie odczytywany i zapisywany w strumieniu danych.
 * @version 1.0
 */
public interface Dao<T> extends AutoCloseable {

    /**
     * Metoda służąca do odczytywania ze strumienia danych obiektu klasy T.
     * @return Odczytany obiekt klasy T.
     * @version 1.0
     * @since 1.0
     */
    T read();

    /**
     * Metoda służąca do zapisywania obiektu klasy T do strumienia danych.
     * @param obj Obiekt, który ma zostać zapisany do strumienia danych.
     * @version 1.0
     * @since 1.0
     */
    void write(T obj);
}

