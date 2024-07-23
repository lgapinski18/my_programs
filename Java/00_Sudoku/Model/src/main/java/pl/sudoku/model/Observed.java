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

import java.io.Serializable;
import java.util.ArrayList;

/**
 * Klasa realizuj�ca wzorzec obserwator.
 * @author Lukasz Gapinski 242387
 * @author Mateusz Gapinski 242387
 * @version 1.4
 */
public abstract class Observed implements Serializable {
    private ArrayList<ObserverOfSudokuField> observers = new ArrayList<>();

    /**
     * Metoda odpowiedzialna za wywo�anie reakcji u obiekt�w nas�uchuj�cych obiekt �r�d�owy.
     * @version 1.1
     * @since 1.1
     */
    protected final void notifyObservers() {
        for (int i = 0; i < observers.size(); i++) {
            observers.get(i).actionPerformed();
        }
    }

    /**
     * Metoda odpowiedzialna za dołączanie obiektów nasłuchujących zdarzenia do obiekt źródłowego.
     * @param observer obiekt nasłuchujący na wystąpienie zdarzenia
     * @version 1.1
     * @since 1.1
     */
    public final void addObserver(ObserverOfSudokuField observer) {
        observers.add(observer);
    }

    /**
     * Metoda odpowiedzialna za usuwanie wszystkich obiektów nasłucuchujących obiektów po za
     * pierwszym.
     * @version 1.0
     * @since 1.4
     */
    public final void removeAllObservers() {
        for (int i = observers.size() - 1; i >= 0; i--) {
            observers.remove(i);
        }
    }

}
