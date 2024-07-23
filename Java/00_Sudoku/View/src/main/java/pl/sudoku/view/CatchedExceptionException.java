package pl.sudoku.view;

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
 * Ten wyjątek służy do zamykania stosu po przechwyceniu (obsłużeniu) pewnego wyjątku, w celu
 * poinformowania o tym, że został taki obsłużony.
 */
public class CatchedExceptionException extends RuntimeException {
    public CatchedExceptionException() {
    }

    public CatchedExceptionException(String message) {
        super(message);
    }

    public CatchedExceptionException(String message, Throwable cause) {
        super(message, cause);
    }

    public CatchedExceptionException(Throwable cause) {
        super(cause);
    }

    public CatchedExceptionException(String message, Throwable cause, boolean enableSuppression,
                                     boolean writableStackTrace) {
        super(message, cause, enableSuppression, writableStackTrace);
    }
}
