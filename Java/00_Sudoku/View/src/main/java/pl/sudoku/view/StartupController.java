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

import java.io.IOException;
import java.util.Locale;
import java.util.ResourceBundle;
import javafx.application.Platform;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
//import javafx.scene.control.*;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.CheckBox;
import javafx.scene.control.ChoiceBox;
import javafx.scene.control.ListView;
import javafx.scene.layout.Pane;
import javafx.stage.Stage;
import org.apache.log4j.Logger;

public class StartupController {
    private static final Logger loggerOfStartupController
            = Logger.getLogger(StartupController.class);
    @FXML
    private Button newGameButton;

    @FXML
    private Pane startupSettings;

    @FXML
    private CheckBox autoverificationCheckBox;

    @FXML
    private ChoiceBox languageSelection;

    @FXML
    private ListView authorsList;

    @FXML
    protected void initialize() {
        loadChoiceBox();

        ResourceBundle messageBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.logmessages", Locale.getDefault());
        try {
            autoverificationCheckBox.setSelected((boolean) JsonFileWriterReader
                    .readTagValue("appdata.json", "autoverification", false));
        } catch (CatchedNotSuchKeyExceptionException e) {
            loggerOfStartupController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
        } catch (CatchedExceptionException e) {
            loggerOfStartupController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
        }

        ResourceBundle authorListResourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.AuthorListResourceBundle", Locale.getDefault());

        ObservableList items =
                FXCollections.observableArrayList(
                        authorListResourceBundle.getString("author1"),
                        authorListResourceBundle.getString("author2"));

        authorsList.setItems(items);
    }

    private void loadChoiceBox() {
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.viewTextes", Locale.getDefault());
        ObservableList languageList =
                FXCollections.observableArrayList(resourceBundle.getString("polish"),
                        resourceBundle.getString("english"));
        languageSelection.setItems(languageList);
        languageSelection.setValue(
                switch (Locale.getDefault().getLanguage()) {
                     case "pl":
                         yield resourceBundle.getString("polish");
                    default:
                         yield resourceBundle.getString("english");
                }
        );
        languageSelection.valueProperty().addListener((observable, oldValue, newValue) -> {
            languageSelectionChange((String) oldValue, (String) newValue);
        });
    }

    @FXML
    protected void languageSelectionChange(String oldValue, String newValue) {
        Locale prevLocale = Locale.getDefault();
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.viewTextes",
                Locale.getDefault());

        Locale newLocale;
        if (newValue.equals(resourceBundle.getString("polish"))) {
            newLocale = new Locale("pl");
        } else {
            newLocale = new Locale("en");
        }

        Locale.setDefault(newLocale);

        resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.viewTextes", newLocale);

        FXMLLoader fxmlLoader = new FXMLLoader(SudokuApplication.class
                .getResource("Startup-view.fxml"), resourceBundle);

        Scene scene;
        try {
            scene = new Scene(fxmlLoader.load());
        } catch (IOException e) {
            ResourceBundle messageBundle = ResourceBundle.getBundle(
                    "pl.sudoku.view.logmessages", Locale.getDefault());
            loggerOfStartupController.error(
                    messageBundle.getString("loadingSceneErrorText"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("loadingSceneErrorText"));
            alert.showAndWait();

            Locale.setDefault(prevLocale);
            languageSelection.setValue(oldValue);
            return;
        }

        ResourceBundle messageBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.logmessages", Locale.getDefault());

        try {
            JsonFileWriterReader.writeTagValue("appdata.json", "language",
                    newLocale.getLanguage());
        } catch (CatchedExceptionException e) {
            loggerOfStartupController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
            return;
        }

        //languageSelection.setValue(newValue);
        Stage stage = (Stage) newGameButton.getScene().getWindow();
        stage.setScene(scene);
        stage.show();
    }

    @FXML
    protected void newGameButtonClick() throws IOException {
        ResourceBundle messageBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.logmessages", Locale.getDefault());
        try {
            JsonFileWriterReader.writeTagValue("appdata.json", "isToLoad", false);
            JsonFileWriterReader.writeTagValue("appdata.json", "autoverification",
                    autoverificationCheckBox.isSelected());
        } catch (CatchedExceptionException e) {
            loggerOfStartupController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
            return;
        }

        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.viewTextes",
                Locale.getDefault());
        FXMLLoader fxmlLoader = new FXMLLoader(SudokuApplication.class
                .getResource("SudokuBoard-view.fxml"), resourceBundle);
        Scene scene = new Scene(fxmlLoader.load());

        Stage stage = (Stage) newGameButton.getScene().getWindow();

        /*User user = new User();
        user.setToLoad(false);
        stage.setUserData(user);*/

        stage.setScene(scene);
        stage.show();
    }

    @FXML
    protected void loadButtonClick() {
        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.viewTextes", Locale.getDefault());

        ResourceBundle messageBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.logmessages", Locale.getDefault());

        /*FileChooser fileChooser = new FileChooser();
        fileChooser.setTitle(resourceBundle.getString("LoadFileChooserTitle"));
        fileChooser.getExtensionFilters().addAll(
                new FileChooser.ExtensionFilter("SudokuBoars", "*.sbs")
        );
        fileChooser.setInitialDirectory(
                new File(System.getProperty("user.home"))
        );

        File file = fileChooser.showOpenDialog((Stage) newGameButton.getScene().getWindow());

        if (file == null) {
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("fileNotSelected"));
            alert.showAndWait();
            loggerOfStartupController.error(
                    messageBundle.getString("fileNotSelected"));
            throw new CatchedExceptionException(messageBundle.getString("fileNotSelected"));
        }

        if (!file.canRead()) {
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("fileNotReadable"));
            alert.showAndWait();
            loggerOfStartupController.error(
                    messageBundle.getString("fileNotReadable"));
            throw new CatchedExceptionException(messageBundle.getString("fileNotReadable"));
        }

        try {
            JsonFileWriterReader.writeTagValue("appdata.json", "isToLoad", true);
            JsonFileWriterReader.writeTagValue("appdata.json", "filePath",
                    file.getAbsolutePath());
            JsonFileWriterReader.writeTagValue("appdata.json", "autoverification",
                    autoverificationCheckBox.isSelected());
        } catch (CatchedExceptionException e) {
            loggerOfStartupController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
            return;
        }*/

        try {
            JsonFileWriterReader.writeTagValue("appdata.json", "isToLoad", true);
            JsonFileWriterReader.writeTagValue("appdata.json", "autoverification",
                    autoverificationCheckBox.isSelected());
        } catch (CatchedExceptionException e) {
            loggerOfStartupController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
            return;
        }

        FXMLLoader fxmlLoader = new FXMLLoader(SudokuApplication.class
                .getResource("SudokuBoard-view.fxml"), resourceBundle);
        Scene scene = null;
        try {
            scene = new Scene(fxmlLoader.load());
        } catch (IOException e) {
            loggerOfStartupController.error(
                    messageBundle.getString("loadingSceneErrorText"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("loadingSceneErrorText"));
            alert.showAndWait();
            return;
        }

        Stage stage = (Stage) newGameButton.getScene().getWindow();
        stage.setScene(scene);
        stage.show();
    }

    @FXML
    protected void exitButtonClick() {
        Platform.exit();
        System.exit(0);
    }

    @FXML
    protected void settingsButtonClick() {
        startupSettings.setVisible(true);
    }

    @FXML
    protected void returnButtonClicked() {
        startupSettings.setVisible(false);
    }
}



