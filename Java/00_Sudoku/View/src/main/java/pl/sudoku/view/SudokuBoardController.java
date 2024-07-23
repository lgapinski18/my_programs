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

import java.io.File;
import java.io.IOException;
import java.lang.reflect.InvocationTargetException;
import java.util.Arrays;
import java.util.List;
import java.util.Locale;
import java.util.ResourceBundle;
import java.util.function.Consumer;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.control.ListView;
import javafx.scene.control.TextField;
import javafx.scene.control.ToolBar;
import javafx.scene.layout.Pane;
import javafx.stage.FileChooser;
import javafx.stage.Stage;
import org.apache.log4j.Logger;
//import pl.sudoku.model.*;
import pl.sudoku.model.BackTrackingSudokuSolver;
//import pl.sudoku.model.FileSudokuBoardDao;
import pl.sudoku.model.Dao;
import pl.sudoku.model.JdbcSudokuBoardDao;
import pl.sudoku.model.SudokuBoard;
import pl.sudoku.model.SudokuBoardDaoFactory;
import pl.sudoku.model.SudokuBoardWithPreviousVersionSaver;
import pl.sudoku.model.SudokuBox;
import pl.sudoku.model.SudokuColumn;
import pl.sudoku.model.SudokuRow;
import pl.sudoku.model.exceptions.FileSudokuBoardDaoIoException;
import pl.sudoku.model.exceptions.JdbcSudokuBoardDaoNotSuchSudokuBoardNameException;
import pl.sudoku.model.exceptions.JdbcSudokuBoardDaoSQLException;
import pl.sudoku.model.exceptions.SudokuBoardException;
import pl.sudoku.model.exceptions.SudokuBoardWithPreviousVersionSaverException;
import pl.sudoku.model.exceptions.SudokuClassNotFoundException;
//import pl.sudoku.model.;
/*import pl.sudoku.model.BackTrackingSudokuSolver;
import pl.sudoku.model.Dao;
import pl.sudoku.model.exceptions.FileSudokuBoardDaoIoException;
import pl.sudoku.model.SudokuBoard;
import pl.sudoku.model.exceptions.SudokuBoardAutoverificationNotPassedException;
import pl.sudoku.model.SudokuBoardDaoFactory;
import pl.sudoku.model.SudokuBox;
import pl.sudoku.model.SudokuColumn;
import pl.sudoku.model.SudokuRow;/**/


public class SudokuBoardController {

    //private Log loggerOfSudokuBoardController = LogFactory.getLog(SudokuBoardController.class);
    //private static final Log loggerOfSudokuBoardController
    // = LogFactory.getLog("SudokuBoardControllerLogger");
    //private static final Logger loggerOfSudokuBoardController
    // = Logger.getLogger("SudokuBoardControllerLogger");
    private static final Logger loggerOfSudokuBoardController
            = Logger.getLogger(SudokuBoardController.class);

    @FXML
    private Button checkSudokuButton;
    @FXML
    private Pane solvingCongratulationPane;
    @FXML
    private Pane levelOfDifficultyPane;
    @FXML
    private Pane sudokuBoardMainPane;
    @FXML
    private Pane chooseSavingWayPane;
    @FXML
    private Pane chooseLoadingWayPane;
    @FXML
    private Pane toLoadPane;
    @FXML
    private Pane boardNamePane;
    @FXML
    private ToolBar toolBar;
    @FXML
    private Label levelOfDifficultyValue;
    @FXML
    private TextField boardNameInputField;
    @FXML
    private ListView toLoadListView;

    private SudokuBoard originalSudokuBoard;
    private SudokuBoard processedSudokuBoard;
    private SudokuBoard currentSudokuBoard;
    private LevelOfDifficulty levelOfDifficulty = LevelOfDifficulty.UNKNOWN;
    private List<TextField> textFieldBoard = Arrays.asList(new TextField[81]);

    private Consumer<String> actionAfterInsertingName;

    private boolean autoverification;
    private boolean isToLoad = false;
    private ResourceBundle messageBundle = ResourceBundle.getBundle(
            "pl.sudoku.view.logmessages",
            Locale.getDefault());

    @FXML
    protected void initialize() {

        loggerOfSudokuBoardController.info(messageBundle.getString(
                "SudkoBoardControlerInitialization"));
        ObservableList<Node> sudokuBoardMainPaneChildren = sudokuBoardMainPane.getChildren();
        for (int i = 0; i < 9; i++) {
            ObservableList<Node> sudokuBoxPaneChildren =
                    ((Pane) sudokuBoardMainPaneChildren.get(i)).getChildren();
            for (int j = 0; j < 9; j++) {
                TextField textField = (TextField) sudokuBoxPaneChildren.get(j);
                final int index = i / 3 * 27 + i % 3 * 3 + j / 3 * 9 + j % 3;
                textField.textProperty().addListener((observable, oldValue, newValue) -> {
                    textFieldValueChanged(index, newValue, oldValue);
                });
                textFieldBoard.set(index, textField);/**/
            }
        }

        try {
            isToLoad = (boolean) JsonFileWriterReader.readTagValue("appdata.json",
                    "isToLoad", true);
            autoverification =  (boolean) JsonFileWriterReader.readTagValue("appdata.json",
                    "autoverification", false);
        } catch (CatchedNotSuchKeyExceptionException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
        }

        if (isToLoad) {
            loadSudoku();
            isToLoad = false;
            /*sudokuBoardMainPane.setDisable(false);
            toolBar.setDisable(false);
            checkSudokuButton.setDisable(false);*/
        } else {
            beginNewGame();
        }

    }

    @FXML
    protected void beginNewGame() {
        sudokuBoardMainPane.setDisable(true);
        toolBar.setDisable(true);
        checkSudokuButton.setDisable(true);
        levelOfDifficultyPane.setVisible(true);
    }

    @FXML
    protected void setLoDHard() { //messageBundle.getString()
        levelOfDifficulty = LevelOfDifficulty.HARD;
        loggerOfSudokuBoardController.info(messageBundle.getString("hardSetted"));

        continueGeneratingSudoku();
    }

    @FXML
    protected void setLoDMedium() {
        levelOfDifficulty = LevelOfDifficulty.MEDIUM;
        loggerOfSudokuBoardController.info(messageBundle.getString("mediumSetted"));

        continueGeneratingSudoku();
    }

    @FXML
    protected void setLoDEasy() {
        levelOfDifficulty = LevelOfDifficulty.EASY;
        loggerOfSudokuBoardController.info(messageBundle.getString("easySetted"));

        continueGeneratingSudoku();
    }

    private void continueGeneratingSudoku() {
        loggerOfSudokuBoardController.info(messageBundle.getString("boardGenerationBegun"));
        originalSudokuBoard = new SudokuBoard(new BackTrackingSudokuSolver());
        originalSudokuBoard.solveGame();
        loggerOfSudokuBoardController.info(messageBundle.getString("boardGenerationFinished"));

        loggerOfSudokuBoardController.info(messageBundle.getString("boardProccesingBegun"));
        /*try {
            processedSudokuBoard = levelOfDifficulty
                    .processSudokuBoard(originalSudokuBoard.clone());
            currentSudokuBoard = processedSudokuBoard.clone();
        }  catch (CloneNotSupportedException e) {
            throw new RuntimeException(e);
        }*/
        processedSudokuBoard = levelOfDifficulty
                .processSudokuBoard(originalSudokuBoard.clone());
        currentSudokuBoard = processedSudokuBoard.clone();
        loggerOfSudokuBoardController.info(messageBundle.getString("boardProccesingFinished"));

        levelOfDifficultyValue.setText(ResourceBundle.getBundle(
                        "pl.sudoku.view.viewTextes", Locale.getDefault()).getString(
                levelOfDifficulty.getCommonNameOfLevelOfDifficulty()));
        loggerOfSudokuBoardController.info(messageBundle.getString("boardDisplayingBegun"));
        prepareSudokuBoard();
        loggerOfSudokuBoardController.info(messageBundle.getString("boardDisplayingFinished"));

        sudokuBoardMainPane.setDisable(false);
        toolBar.setDisable(false);
        checkSudokuButton.setDisable(false);
        levelOfDifficultyPane.setVisible(false);
    }

    @FXML
    protected void restartGame() {
        loggerOfSudokuBoardController.info(messageBundle.getString("gameRestartingBegun"));
        /*try {
            currentSudokuBoard = processedSudokuBoard.clone();
        } catch (CloneNotSupportedException e) {
            throw new RuntimeException(e);
        }*/
        currentSudokuBoard = processedSudokuBoard.clone();

        loggerOfSudokuBoardController.info(messageBundle.getString("boardDisplayingBegun"));
        prepareSudokuBoard();
        loggerOfSudokuBoardController.info(messageBundle.getString("boardDisplayingFinished"));
        loggerOfSudokuBoardController.info(messageBundle.getString("gameRestartingFinished"));
    }

    @FXML
    protected void doAfterInsertingName() {
        String sudokuBoardName = boardNameInputField.getText();

        if (sudokuBoardName.length() > 27) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("someTroubleHasOccured"));
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("namesLengthNotice"));
            alert.showAndWait();
            return;
        }

        actionAfterInsertingName.accept(sudokuBoardName);
    }

    @FXML
    protected void saveSudoku() {
        sudokuBoardMainPane.setDisable(true);
        toolBar.setDisable(true);
        checkSudokuButton.setDisable(true);
        chooseSavingWayPane.setVisible(true);

        actionAfterInsertingName = this::saveSudokuToDB;
    }

    @FXML
    protected void returnFromChoosingSavingWay() {
        sudokuBoardMainPane.setDisable(false);
        toolBar.setDisable(false);
        checkSudokuButton.setDisable(false);
        chooseSavingWayPane.setVisible(false);
    }

    @FXML
    protected void saveSudokuToFile() {
        returnFromChoosingSavingWay();
        loggerOfSudokuBoardController.info(messageBundle.getString("savingLODBegun"));
        /*try {
            JsonFileWriterReader.writeTagValue("appdata.json", "levelOfDifficulty",
                    levelOfDifficulty.getNameOfLevelOfDifficulty());
        } catch (CatchedExceptionException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
            return;
        }*/
        loggerOfSudokuBoardController.info(messageBundle.getString("savingLODFinished"));

        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.viewTextes", Locale.getDefault());

        FileChooser fileChooser = new FileChooser();
        fileChooser.setTitle(resourceBundle.getString("SaveFileChooserTitle"));
        fileChooser.getExtensionFilters().addAll(
                new FileChooser.ExtensionFilter("SudokuBoards", "*.sbs")
        );
        fileChooser.setInitialDirectory(
                new File(System.getProperty("user.home"))
        );
        File file = fileChooser.showSaveDialog((Stage) toolBar.getScene().getWindow());

        if (file == null) {
            goToMainMenu();
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("fileNotSelected"));
            alert.showAndWait();
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("fileNotSelected"));
            return;
        }

        try (SudokuBoardWithPreviousVersionSaver procSaver =
                new SudokuBoardWithPreviousVersionSaver(SudokuBoardDaoFactory.getFileDao(
                        file.getAbsolutePath())); SudokuBoardWithPreviousVersionSaver curSaver =
                new SudokuBoardWithPreviousVersionSaver(procSaver)) {
            procSaver.addPreviousSudokuBoard(originalSudokuBoard);
            curSaver.addPreviousSudokuBoard(processedSudokuBoard);
            curSaver.setNameOfLOD(levelOfDifficulty.getNameOfLevelOfDifficulty());
            curSaver.write(currentSudokuBoard);
        } catch (FileSudokuBoardDaoIoException
                 | SudokuBoardWithPreviousVersionSaverException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("tDuringWritingBoardsToFile"));
            alert.showAndWait();
            return;
        }

        loggerOfSudokuBoardController.info(messageBundle.getString("savingBoardsFinished"));

        Alert alert = new Alert(Alert.AlertType.INFORMATION);
        alert.setTitle(messageBundle.getString("savingAlertTitle"));
        alert.setHeaderText(messageBundle.getString("savingAlertHeader"));
        alert.setContentText(messageBundle.getString("savingAlertText"));
        alert.showAndWait();
    }

    @FXML
    protected void showBoardNameInput() {
        chooseSavingWayPane.setVisible(false);
        chooseLoadingWayPane.setVisible(false);
        boardNamePane.setVisible(true);
        boardNameInputField.setText("");
    }

    @FXML
    protected void cancelBoardNameInput() {
        sudokuBoardMainPane.setDisable(false);
        toolBar.setDisable(false);
        checkSudokuButton.setDisable(false);
        boardNamePane.setVisible(false);
    }

    protected void saveSudokuToDB(String sudokuBoardName) {
        loggerOfSudokuBoardController.info(messageBundle.getString("savingToDBBegun"));

        try (Dao<SudokuBoard> dbDao = SudokuBoardDaoFactory.getJdbcDao(
                "SudokuBoardsSaves", sudokuBoardName + "-cs",
                levelOfDifficulty.getNameOfLevelOfDifficulty());
             Dao<SudokuBoard> dbDao2 = SudokuBoardDaoFactory.getJdbcDao(
                     "SudokuBoardsSaves", sudokuBoardName + "-ps",
                     levelOfDifficulty.getNameOfLevelOfDifficulty());
             Dao<SudokuBoard> dbDao3 = SudokuBoardDaoFactory.getJdbcDao(
                     "SudokuBoardsSaves", sudokuBoardName + "-os",
                     levelOfDifficulty.getNameOfLevelOfDifficulty())) {
            dbDao.write(currentSudokuBoard);
            dbDao2.write(processedSudokuBoard);
            dbDao3.write(originalSudokuBoard);
        } catch (JdbcSudokuBoardDaoSQLException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("tDuringWritingBoardsToDB"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("tDuringWritingBoardsToDB"));
            alert.showAndWait();
            return;
        } catch (Exception e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("duringClosingErrorText"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("duringClosingErrorText"));
            alert.showAndWait();
            return;
        }

        /*try (Dao<SudokuBoard> dbDao2 = SudokuBoardDaoFactory.getJdbcDao(
                "SudokuBoardsSaves", sudokuBoardName + "-ps")) {
            dbDao2.write(processedSudokuBoard);
        } catch (JdbcSudokuBoardDaoSQLException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("tDuringWritingBoardsToDB"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("tDuringWritingBoardsToDB"));
            alert.showAndWait();
            return;
        } catch (Exception e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("duringClosingErrorText"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("duringClosingErrorText"));
            alert.showAndWait();
            return;
        }

        try (Dao<SudokuBoard> dbDao3 = SudokuBoardDaoFactory.getJdbcDao(
                "SudokuBoardsSaves", sudokuBoardName + "-os")) {
            dbDao3.write(originalSudokuBoard);
        } catch (JdbcSudokuBoardDaoSQLException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("tDuringWritingBoardsToDB"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("tDuringWritingBoardsToDB"));
            alert.showAndWait();
            return;
        } catch (Exception e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("duringClosingErrorText"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("duringClosingErrorText"));
            alert.showAndWait();
            return;
        }*/

        loggerOfSudokuBoardController.info(messageBundle.getString("savingToDBCompleted"));

        Alert alert = new Alert(Alert.AlertType.INFORMATION);
        alert.setTitle(messageBundle.getString("savingAlertTitle"));
        alert.setHeaderText(messageBundle.getString("savingAlertHeader"));
        alert.setContentText(messageBundle.getString("savingAlertText"));
        alert.showAndWait();

        cancelBoardNameInput();
    }

    @FXML
    protected void loadSudoku() {
        sudokuBoardMainPane.setDisable(true);
        toolBar.setDisable(true);
        checkSudokuButton.setDisable(true);
        chooseLoadingWayPane.setVisible(true);

        //actionAfterInsertingName = this::loadSudokuFromDB;
    }

    @FXML
    protected void returnFromLoadingWay() {
        sudokuBoardMainPane.setDisable(false);
        toolBar.setDisable(false);
        checkSudokuButton.setDisable(false);
        chooseLoadingWayPane.setVisible(false);
    }

    @FXML
    protected  void showLoadFromDbPane() {

        List<String> listOfSudokuBoardNames;
        try (JdbcSudokuBoardDao dbDao = (JdbcSudokuBoardDao) SudokuBoardDaoFactory.getJdbcDao(
                "SudokuBoardsSaves", "-cs",
                levelOfDifficulty.getNameOfLevelOfDifficulty())) {
            listOfSudokuBoardNames = dbDao.getSudokuBoardNames();
        } catch (JdbcSudokuBoardDaoSQLException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("tDuringReadingBoardsFromDB"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("tDuringReadingBoardsFromDB"));
            alert.showAndWait();
            return;
        } catch (Exception e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("duringClosingErrorText"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("duringClosingErrorText"));
            alert.showAndWait();
            return;
        }

        ObservableList<String> observableList = FXCollections.observableList(listOfSudokuBoardNames
                .stream().map(name -> name.substring(0, name.length() - 3)).distinct().toList());

        toLoadListView.setItems(observableList);

        chooseLoadingWayPane.setVisible(false);
        toLoadPane.setVisible(true);
    }

    @FXML
    protected void returnFromToLoadSelection() {
        sudokuBoardMainPane.setDisable(false);
        toolBar.setDisable(false);
        checkSudokuButton.setDisable(false);
        toLoadPane.setVisible(false);
    }

    @FXML
    protected void loadSudokuFromDB() { /*String sudokuBoardName*/
        loggerOfSudokuBoardController.info(messageBundle.getString("loadingFromDBBegun"));

        String sudokuBoardName = (String) toLoadListView.getSelectionModel().getSelectedItem();

        SudokuBoard tempCurrentSudokuBoard;
        SudokuBoard tempProccessedSudokuBoard;
        SudokuBoard tempOriginalSudokuBoard;
        String tempLodName = "UNKNOWN";

        try (JdbcSudokuBoardDao dbDao = (JdbcSudokuBoardDao) SudokuBoardDaoFactory.getJdbcDao(
                "SudokuBoardsSaves", sudokuBoardName + "-cs",
                levelOfDifficulty.getNameOfLevelOfDifficulty());
             Dao<SudokuBoard> dbDao2 = SudokuBoardDaoFactory.getJdbcDao(
                "SudokuBoardsSaves", sudokuBoardName + "-ps",
                     levelOfDifficulty.getNameOfLevelOfDifficulty());
             Dao<SudokuBoard> dbDao3 = SudokuBoardDaoFactory.getJdbcDao(
                     "SudokuBoardsSaves", sudokuBoardName + "-os",
                     levelOfDifficulty.getNameOfLevelOfDifficulty())) {
            tempCurrentSudokuBoard = dbDao.read();
            tempProccessedSudokuBoard = dbDao2.read();
            tempOriginalSudokuBoard = dbDao3.read();
            tempLodName = dbDao.getLevelOfDifficultyName();
        } catch (JdbcSudokuBoardDaoSQLException | InstantiationException | IllegalAccessException
                | InvocationTargetException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("tDuringReadingBoardsFromDB"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("tDuringReadingBoardsFromDB"));
            alert.showAndWait();
            return;
        } catch (SudokuClassNotFoundException e) {
            loggerOfSudokuBoardController.error(
                    e.getMessage(), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(e.getMessage());
            alert.showAndWait();
            return;
        } catch (JdbcSudokuBoardDaoNotSuchSudokuBoardNameException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("insertNameErrorText"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("insertNameErrorText"));
            alert.showAndWait();
            return;
        } catch (Exception e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("duringClosingErrorText"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("duringClosingErrorText"));
            alert.showAndWait();
            return;
        }

        currentSudokuBoard = tempCurrentSudokuBoard;
        processedSudokuBoard = tempProccessedSudokuBoard;
        originalSudokuBoard = tempOriginalSudokuBoard;

        /*originalSudokuBoard.setAutoverification(autoverification);
        currentSudokuBoard.setAutoverification(autoverification);
        processedSudokuBoard.setAutoverification(autoverification);*/

        loggerOfSudokuBoardController.info(messageBundle.getString("loadingFromDBCompleted"));
        loggerOfSudokuBoardController.info(messageBundle.getString("boardDisplayingBegun"));
        levelOfDifficulty = Enum.valueOf(LevelOfDifficulty.class, tempLodName);
        levelOfDifficultyValue.setText(ResourceBundle.getBundle(
                "pl.sudoku.view.viewTextes", Locale.getDefault()).getString(
                levelOfDifficulty.getCommonNameOfLevelOfDifficulty()));
        prepareSudokuBoard();
        loggerOfSudokuBoardController.info(messageBundle.getString("boardDisplayingFinished"));

        Alert alert = new Alert(Alert.AlertType.INFORMATION);
        alert.setTitle(messageBundle.getString("loadingAlertTitle"));
        alert.setHeaderText(messageBundle.getString("loadingAlertHeader"));
        alert.setContentText(messageBundle.getString("loadingAlertText"));
        alert.showAndWait();

        returnFromToLoadSelection();
    }

    @FXML
    protected void loadSudokuFromFile() {
        returnFromLoadingWay();

        loggerOfSudokuBoardController.info(messageBundle.getString("loadingLODBegun"));
        String filepath = "";
        try {
            /*levelOfDifficulty = LevelOfDifficulty.UNKNOWN;
            levelOfDifficulty = Enum.valueOf(LevelOfDifficulty.class,
                    JsonFileWriterReader.readTagValue("appdata.json",
                            "levelOfDifficulty", false).toString());
            levelOfDifficultyValue.setText(ResourceBundle.getBundle(
                    "pl.sudoku.view.viewTextes", Locale.getDefault()).getString(
                    levelOfDifficulty.getCommonNameOfLevelOfDifficulty()));*/
            if (isToLoad) {
                filepath = (String) JsonFileWriterReader.readTagValue("appdata.json",
                        "filePath", false);
            }

        } catch (CatchedNotSuchKeyExceptionException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
        } catch (CatchedExceptionException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
            return;
        }
        loggerOfSudokuBoardController.info(messageBundle.getString("loadingLODFinished"));

        loggerOfSudokuBoardController.info(messageBundle.getString("loadingBoardsBegun"));

        try {
            loadSudokuBoards(filepath);
        } catch (CatchedExceptionException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("fileNotSelected"));
            return;
        }

        /*originalSudokuBoard.setAutoverification(autoverification);
        currentSudokuBoard.setAutoverification(autoverification);
        processedSudokuBoard.setAutoverification(autoverification);*/

        loggerOfSudokuBoardController.info(messageBundle.getString("loadingBoardsFinished"));
        loggerOfSudokuBoardController.info(messageBundle.getString("boardDisplayingBegun"));
        prepareSudokuBoard();
        loggerOfSudokuBoardController.info(messageBundle.getString("boardDisplayingFinished"));

        Alert alert = new Alert(Alert.AlertType.INFORMATION);
        alert.setTitle(messageBundle.getString("loadingAlertTitle"));
        alert.setHeaderText(messageBundle.getString("loadingAlertHeader"));
        alert.setContentText(messageBundle.getString("loadingAlertText"));
        alert.showAndWait();
    }

    private void loadSudokuBoards(String filePath) {

        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.viewTextes", Locale.getDefault());

        if (filePath.equals("")) {
            FileChooser fileChooser = new FileChooser();
            fileChooser.setTitle(resourceBundle.getString("LoadFileChooserTitle"));
            fileChooser.getExtensionFilters().addAll(
                    new FileChooser.ExtensionFilter("SudokuBoards", "*.sbs")
            );
            fileChooser.setInitialDirectory(
                    new File(System.getProperty("user.home"))
            );
            File file = fileChooser.showOpenDialog((Stage) checkSudokuButton.getScene()
                    .getWindow());

            if (file == null) {
                Alert alert = new Alert(Alert.AlertType.ERROR);
                alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
                alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
                alert.setContentText(messageBundle.getString("fileNotSelected"));
                alert.showAndWait();
                loggerOfSudokuBoardController.error(
                        messageBundle.getString("fileNotSelected"));
                throw new CatchedExceptionException(messageBundle.getString("fileNotSelected"));
            }

            if (!file.canRead()) {
                Alert alert = new Alert(Alert.AlertType.ERROR);
                alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
                alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
                alert.setContentText(messageBundle.getString("fileNotReadable"));
                alert.showAndWait();
                loggerOfSudokuBoardController.error(
                        messageBundle.getString("fileNotReadable"));
                throw new CatchedExceptionException(messageBundle.getString("fileNotReadable"));
            }

            filePath = file.getAbsolutePath();
        }


        try (SudokuBoardWithPreviousVersionSaver curSaver =
                new SudokuBoardWithPreviousVersionSaver(new SudokuBoardWithPreviousVersionSaver(
                        SudokuBoardDaoFactory.getFileDao(filePath)))) {
            currentSudokuBoard = curSaver.read();
            processedSudokuBoard = curSaver.readPreviousSudokuBoard();
            originalSudokuBoard = ((SudokuBoardWithPreviousVersionSaver) curSaver.getDao())
                    .readPreviousSudokuBoard();

            levelOfDifficulty = LevelOfDifficulty.UNKNOWN;
            levelOfDifficulty = Enum.valueOf(LevelOfDifficulty.class, curSaver.getNameOfLOD());
            levelOfDifficultyValue.setText(ResourceBundle.getBundle(
                    "pl.sudoku.view.viewTextes", Locale.getDefault()).getString(
                    levelOfDifficulty.getCommonNameOfLevelOfDifficulty()));
        } catch (SudokuClassNotFoundException | FileSudokuBoardDaoIoException
                 | SudokuBoardWithPreviousVersionSaverException e) {
            loggerOfSudokuBoardController.error(
                    messageBundle.getString("someTroubleHasOccured"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("tDuringReadingBoardsFromFile"));
            alert.showAndWait();
            throw new CatchedExceptionException(e);
        }
    }

    private void markColumnByValue(int column, int value) {
        SudokuColumn sudokuColumn = currentSudokuBoard.getColumn(column);
        SudokuColumn processedSudokuColumn = processedSudokuBoard.getColumn(column);
        for (int i = 0; i < 9; i++) {
            if (value == sudokuColumn.get(i) && processedSudokuColumn.get(i) == 0) {
                textFieldBoard.get(i * 9 + column)
                    .getStyleClass().add("incorrect_value");
            }
        }
    }

    private void unmarkColumnByValue(int column, int value) {
        SudokuColumn sudokuColumn = currentSudokuBoard.getColumn(column);
        SudokuColumn processedSudokuColumn = processedSudokuBoard.getColumn(column);
        for (int i = 0; i < 9; i++) {
            if (value == sudokuColumn.get(i) && processedSudokuColumn.get(i) == 0
                    && checkColumnValidity(i, column, value)
                    && checkRowValidity(i, column, value)
                    && checkBoxValidity(i, column, value)) {
                textFieldBoard.get(i * 9 + column)
                        .getStyleClass().removeAll("incorrect_value");
            }
        }
    }

    private boolean checkColumnValidity(int row, int column, int value) {
        if (value != 0) {
            SudokuColumn sudokuColumn = currentSudokuBoard.getColumn(column);

            int count = 0;
            for (int i = 0; i < 9; i++) {
                if (value == sudokuColumn.get(i)) {
                    count += 1;
                }
            }
            if (count > 1) {
                return false;
            } else {
                return true;
            }
        }
        if (textFieldBoard.get(row * 9 + column)
                .getStyleClass().contains("incorrect_value")) {
            return false;
        }
        return true;
    }

    private void markRowByValue(int row, int value) {
        SudokuRow sudokuRow = currentSudokuBoard.getRow(row);
        SudokuRow processedSudokuRow = processedSudokuBoard.getRow(row);
        for (int i = 0; i < 9; i++) {
            if (value == sudokuRow.get(i) && processedSudokuRow.get(i) == 0) {
                textFieldBoard.get(row * 9 + i)
                        .getStyleClass().add("incorrect_value");
            }
        }
    }

    private void unmarkRowByValue(int row, int value) {
        SudokuRow sudokuRow = currentSudokuBoard.getRow(row);
        SudokuRow processedSudokuRow = processedSudokuBoard.getRow(row);
        for (int i = 0; i < 9; i++) {
            if (value == sudokuRow.get(i) && processedSudokuRow.get(i) == 0
                    && checkColumnValidity(row, i, value)
                    && checkRowValidity(row, i, value)
                    && checkBoxValidity(row, i, value)) {
                /*loggerOfSudokuBoardController.info("UR: " + (checkColumnValidity(row, i, value)
                        && checkRowValidity(row, i, value)
                        && checkBoxValidity(row, i, value)));*/
                textFieldBoard.get(row * 9 + i)
                        .getStyleClass().removeAll("incorrect_value");
            }

        }
    }

    private boolean checkRowValidity(int row, int column, int value) {
        if (value != 0) {
            SudokuRow sudokuRow = currentSudokuBoard.getRow(row);
            //SudokuRow processedSudokuRow = processedSudokuBoard.getRow(row);

            int count = 0;
            for (int i = 0; i < 9; i++) {
                if (value == sudokuRow.get(i)) {
                    count += 1;
                }
            }

            /*
            for (int i = 0; i < 9; i++) {
                if (value == sudokuRow.get(i)) {
                    if (processedSudokuRow.get(i) == 0 && count > 1) {
                        textFieldBoard.get(row * 9 + i)
                                .getStyleClass().add("incorrect_value");
                    } else {
                        textFieldBoard.get(row * 9 + i)
                                .getStyleClass().remove("incorrect_value");
                    }
                }
            }*/
            if (count > 1) {
                return false;
            } else {
                return true;
            }
        }
        /*if (textFieldBoard.get(row * 9 + column)
                .getStyleClass().contains("incorrect_value")) {
            return false;
        }
        return true;*/
        return !textFieldBoard.get(row * 9 + column).getStyleClass().contains("incorrect_value");
    }

    private void markBoxByValue(int row, int column, int value) {
        SudokuBox sudokuBox = currentSudokuBoard.getBox(column / 3, row / 3);
        SudokuBox processedSudokuBox = processedSudokuBoard.getBox(column / 3, row / 3);
        for (int i = 0; i < 9; i++) {
            if (value == sudokuBox.get(i) && processedSudokuBox.get(i) == 0) {
                textFieldBoard.get((row / 3 * 3 + i / 3) * 9 + column / 3 * 3 + i % 3)
                        .getStyleClass().add("incorrect_value");
            }
        }
    }

    private void unmarkBoxByValue(int row, int column, int value) {
        /*loggerOfSudokuBoardController.info("In Unmark Box Validity");
        loggerOfSudokuBoardController.info("R: " + row + " C: " + column + " V: " + value);
        loggerOfSudokuBoardController.info("R: "
        + (row / 3) + " C: " + (column / 3) + " V: " + value);*/
        SudokuBox sudokuBox = currentSudokuBoard.getBox(column / 3, row / 3);
        SudokuBox processedSudokuBox = processedSudokuBoard.getBox(column / 3, row / 3);
        for (int i = 0; i < 9; i++) {
            //loggerOfSudokuBoardController.info(sudokuBox.get(i));
            if (value == sudokuBox.get(i)
                    && processedSudokuBox.get(i) == 0
                    && checkColumnValidity(row / 3 * 3 + i / 3,
                    column / 3 * 3 + i % 3, value)
                    && checkRowValidity(row / 3 * 3 + i / 3,
                    column / 3 * 3 + i % 3, value)
                    && checkBoxValidity(row / 3 * 3 + i / 3,
                    column / 3 * 3 + i % 3, value)) {
                /*loggerOfSudokuBoardController.info("UB: "
                        + (checkColumnValidity(((row / 3) * 3 + i / 3),
                        (column / 3) * 3 + (i % 3), value)
                        && checkRowValidity(((row / 3) * 3 + i / 3),
                        (column / 3) * 3 + (i % 3), value)
                        && checkBoxValidity(((row / 3) * 3 + i / 3),
                        (column / 3) * 3 + (i % 3), value)));*/
                /*loggerOfSudokuBoardController.info("Cleaning: "
                        + (((row / 3) * 3 + i / 3) * 9 + (column / 3) * 3 + (i % 3)));/**/
                textFieldBoard.get((row / 3 * 3 + i / 3) * 9 + column / 3 * 3 + i % 3)
                        .getStyleClass().removeAll("incorrect_value");
            }
        }
    }

    private boolean checkBoxValidity(int row, int column, int value) {
        //loggerOfSudokuBoardController.info("In Out Box Validity");
        /*!textFieldBoard.get(row * 9 + column)
                .getStyleClass().contains("incorrect_value")
                && */
        if (value != 0) {
            /*loggerOfSudokuBoardController.info("In Box Validity");
            loggerOfSudokuBoardController.info("R: " + row + " C: " + column + " V: " + value);
            loggerOfSudokuBoardController.info("R: "
            + (row / 3) + " C: " + (column % 3) + " V: " + value);*/
            SudokuBox sudokuBox = currentSudokuBoard.getBox(column / 3, row / 3);
            //SudokuBox processedSudokuBox = processedSudokuBoard.getBox(row / 3, column % 3);

            int count = 0;
            for (int i = 0; i < 9; i++) {
                if (value == sudokuBox.get(i)) {
                    count += 1;
                }
            }

            /*
            for (int i = 0; i < 9; i++) {
                if (value == sudokuBox.get(i)) {
                    if (processedSudokuBox.get(i) == 0 && count > 1) {
                        textFieldBoard.get((row / 3 * 3 + i / 3) * 9 + column / 3 * 3 + i % 3)
                                .getStyleClass().add("incorrect_value");
                    } else {
                        textFieldBoard.get((row / 3 * 3 + i / 3) * 9 + column / 3 * 3 + i % 3)
                                .getStyleClass().remove("incorrect_value");
                    }
                }
            }*/
            if (count > 1) {
                return false;
            } else {
                return true;
            }
        }
        if (textFieldBoard.get(row * 9 + column)
                .getStyleClass().contains("incorrect_value")) {
            return false;
        }
        return true;
    }

    private boolean checkFieldValidity(int row, int column, int value) {
        loggerOfSudokuBoardController.info(messageBundle.getString("checkingCorrectnesBegun")
                + "R: " + row + " C: " + column + " V: " + value);/**/
        if (value != 0) {
            boolean columnValidity = checkColumnValidity(row, column, value);
            boolean rowValidity = checkRowValidity(row, column, value);
            boolean boxValidity = checkBoxValidity(row, column, value);

            loggerOfSudokuBoardController.info(
                    messageBundle.getString("columnCorrectness") + columnValidity);
            if (columnValidity) {
                unmarkColumnByValue(column, value);
            } else {

                markColumnByValue(column, value);
            }

            loggerOfSudokuBoardController.info(
                    messageBundle.getString("rowCorrectness") + rowValidity);
            if (rowValidity) {
                unmarkRowByValue(row, value);
            } else {
                markRowByValue(row, value);
            }

            loggerOfSudokuBoardController.info(
                    messageBundle.getString("boxCorrectness") + boxValidity);
            if (boxValidity) {
                unmarkBoxByValue(row, column, value);
            } else {
                markBoxByValue(row, column, value);
            }

            /*boolean clear = checkBoxValidity(row, column, value)
                    && checkColumnValidity(row, column, value)
                    && checkRowValidity(row, column, value);*/
            /*loggerOfSudokuBoardController.info("CRBV: "
                    + (boxValidity && columnValidity && rowValidity));*/
            if (boxValidity && columnValidity && rowValidity) {
                textFieldBoard.get(row * 9 + column).getStyleClass().remove("incorrect_value");
            } else {
                textFieldBoard.get(row * 9 + column).getStyleClass().add("incorrect_value");
            }
            loggerOfSudokuBoardController.info(messageBundle.getString(
                    "checkingCorrectnesFinished")
                    + "R: " + row + " C: " + column + " V: " + value);
            return boxValidity && columnValidity && rowValidity;
        }
        loggerOfSudokuBoardController.info(messageBundle.getString(
                "checkingCorrectnesFinished")
                + "R: " + row + " C: " + column + " V: " + value);
        return true;
    }

    private void bindSudokuBoardWithTextFields() {
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                final int x = j;
                final int y = i;
                final SudokuBoard sb = currentSudokuBoard;
                final TextField tf = textFieldBoard.get(i * 9 + j);
                currentSudokuBoard.addObserver(i, j, () -> {
                    tf.setText(
                            switch (sb.get(y, x)) {
                                case 1, 2, 3, 4, 5, 6, 7, 8, 9:
                                    yield "" + sb.get(y, x);
                                default:
                                    yield "";
                            }
                    );
                });
            }
        }
    }

    private void prepareSudokuBoard() {
        originalSudokuBoard.setAutoverification(autoverification);
        currentSudokuBoard.setAutoverification(autoverification);
        processedSudokuBoard.setAutoverification(autoverification);

        bindSudokuBoardWithTextFields();

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                int v = currentSudokuBoard.get(i, j);
                if (v != 0) {
                    textFieldBoard.get(i * 9 + j).setText("" + v);
                } else {
                    textFieldBoard.get(i * 9 + j).setText("");
                }
                textFieldBoard.get(i * 9 + j).getStyleClass()
                        .removeAll("incorrect_value");
            }
        }

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                int v = processedSudokuBoard.get(i, j);
                if (v != 0) {
                    textFieldBoard.get(i * 9 + j).setDisable(true);
                } else {
                    textFieldBoard.get(i * 9 + j).setDisable(false);
                    if (autoverification) {

                        //checkBoxValidity(i, j, currentSudokuBoard.get(i, j));
                        //checkColumnValidity(i, j, currentSudokuBoard.get(i, j));
                        //checkRowValidity(i, j, currentSudokuBoard.get(i, j));
                        checkFieldValidity(i, j, currentSudokuBoard.get(i, j));

                        /*try {
                            textFieldBoard.get(i * 9 + j).getStyleClass()
                                    .removeAll("incorrect_value");
                            currentSudokuBoard.set(i, j, currentSudokuBoard.get(i, j));
                        } catch (AutoverificationNotPassedException e) {
                            textFieldBoard.get(i * 9 + j).getStyleClass().add("incorrect_value");
                        }*/
                    }
                }
            }
        }
    }

    protected void textFieldValueChanged(int index, String newValue, String oldValue) {

        switch (newValue) {
            case "1", "2", "3", "4", "5", "6", "7", "8", "9":
                //loggerOfSudokuBoardController.info("newValue: " + newValue);
                try {
                    textFieldBoard.get(index).getStyleClass().removeAll("incorrect_value");
                    currentSudokuBoard.set(index / 9, index % 9,
                            Integer.parseInt(newValue));
                } catch (SudokuBoardException e) {
                    loggerOfSudokuBoardController.error(messageBundle.getString(
                            "autoverThrownException") + e.getMessage(), e);
                    int intNewValue = 0;
                    if (newValue != "") {
                        intNewValue = Integer.parseInt(newValue);
                    }
                    checkFieldValidity(index / 9, index % 9, intNewValue);
                }
                break;
            case  "":
                try {
                    textFieldBoard.get(index).getStyleClass().removeAll("incorrect_value");
                    currentSudokuBoard.set(index / 9, index % 9, 0);
                } catch (SudokuBoardException e) {
                    loggerOfSudokuBoardController.error(messageBundle.getString(
                            "autoverThrownExceptionSEBLM") + e.getMessage(), e);
                    //It is, so that PMD won't throw error.
                    //int i = 0;
                    //i = i++;
                }
                //loggerOfSudokuBoardController.info("OldValue: " + oldValue);
                int intOldValue = 0;
                if (oldValue != "") {
                    intOldValue = Integer.parseInt(oldValue);
                }
                checkFieldValidity(index / 9, index % 9, intOldValue);
                textFieldBoard.get(index).getStyleClass().removeAll("incorrect_value");
                break;
            default:
                if (currentSudokuBoard.get(index / 9, index % 9) == 0) {
                    textFieldBoard.get(index).setText("");
                } else {
                    textFieldBoard.get(index).setText("" + currentSudokuBoard
                            .get(index / 9, index % 9));
                }
                /*
                textFieldBoard.get(index).getStyleClass().removeAll("incorrect_value");
                intOldValue = 0;
                if (oldValue != "") {
                    intOldValue = Integer.parseInt(oldValue);
                }
                checkFieldValidity(index / 9, index % 9, intOldValue);*/
        }
    }

    @FXML
    protected void checkSudokuBoard() {
        if (currentSudokuBoard.equals(originalSudokuBoard)) {
            sudokuBoardMainPane.setDisable(true);
            toolBar.setDisable(true);
            checkSudokuButton.setDisable(true);
            solvingCongratulationPane.setVisible(true);
        } else {
            loggerOfSudokuBoardController.error(messageBundle.getString(
                    "checkingAlertText"));
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("checkingAlertTitle"));
            alert.setHeaderText(messageBundle.getString("checkingAlertHeader"));
            alert.setContentText(messageBundle.getString("checkingAlertText"));
            alert.showAndWait();
        }
    }

    @FXML
    protected void beginNewGameAfterFinishingOne() {
        solvingCongratulationPane.setVisible(false);
        beginNewGame();
    }

    @FXML
    protected  void goToMainMenu() {
        solvingCongratulationPane.setVisible(false);

        ResourceBundle resourceBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.viewTextes", Locale.getDefault());
        FXMLLoader fxmlLoader = new FXMLLoader(SudokuApplication.class
                .getResource("Startup-view.fxml"), resourceBundle);

        Scene scene = null;
        try {
            scene = new Scene(fxmlLoader.load());
        } catch (IOException e) {
            loggerOfSudokuBoardController.error(
                messageBundle.getString("loadingSceneErrorText"), e);
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("loadingSceneErrorText"));
            alert.showAndWait();
            return;
        }

        Stage stage = (Stage) solvingCongratulationPane.getScene().getWindow();
        stage.setScene(scene);
        stage.show();
    }
}
