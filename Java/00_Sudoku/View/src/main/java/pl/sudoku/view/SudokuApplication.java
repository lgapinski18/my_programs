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

import java.io.FileWriter;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Locale;
import java.util.ResourceBundle;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.stage.Stage;
import org.apache.log4j.Logger;
import org.json.simple.JSONObject;

/**
 * Klasa aplikacji okienkowej.
 */
public class SudokuApplication extends Application {
    private static final Logger loggerOfSudokuApplication
            = Logger.getLogger(SudokuApplication.class);

    @Override
    public void start(final Stage stage) {
        ResourceBundle messageBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.logmessages", Locale.getDefault());
        if (!Files.exists(Paths.get("appdata.json"))) {
            try (FileWriter fileWriter = new FileWriter("appdata.json")) {
                JSONObject jsonObject = new JSONObject();
                jsonObject.put("programName", "Sudoku");
                fileWriter.write(jsonObject.toJSONString());
                fileWriter.flush();
                //out.write("{}".getBytes(StandardCharsets.UTF_8));
            } catch (IOException e) {
                loggerOfSudokuApplication.error(
                        messageBundle.getString("loadingSceneErrorText"), e);
                Alert alert = new Alert(Alert.AlertType.ERROR);
                alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
                alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
                alert.setContentText(messageBundle.getString("creatingFileError"));
                alert.showAndWait();
                return;
            }
        }

        Locale language = null;
        try {
            language = new Locale((String) JsonFileWriterReader
                    .readTagValue("appdata.json", "language", true));
        } catch (CatchedExceptionException e) {
            loggerOfSudokuApplication.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
        }

        if (language == null) {
            language = new Locale(
                    switch (Locale.getDefault().getLanguage()) {
                        case "pl":
                            yield "pl";
                        default:
                            yield "en";
                    }
            );
        }

        Locale.setDefault(language);
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.viewTextes", language);
        messageBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.logmessages", language);

        FXMLLoader fxmlLoader = new FXMLLoader(SudokuApplication.class
                        .getResource("Startup-view.fxml"), resourceBundle);
        Scene scene;
        try {
            scene = new Scene(fxmlLoader.load());
        } catch (IOException e) {
            loggerOfSudokuApplication.error(
                    messageBundle.getString("loadingSceneErrorText"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("loadingSceneErrorText"));
            alert.showAndWait();
            return;
        }
        stage.setTitle("SUDOKU");
        stage.setScene(scene);
        stage.show();
    }

    public static void main(final String[] args) {
        launch();
    }
}
