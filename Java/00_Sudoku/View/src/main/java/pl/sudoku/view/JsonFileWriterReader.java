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

import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Locale;
import java.util.ResourceBundle;
import javafx.scene.control.Alert;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;


public class JsonFileWriterReader {

    private JsonFileWriterReader() {
    }

    public static void writeTagValue(String fileName, String key, Object value)
            throws CatchedExceptionException {
        //ObjectOutputStream out = new ObjectOutputStream(new FileOutputStream(fileName))
        JSONObject jsonObject = readJsonObject(fileName, key);
        ResourceBundle messageBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.logmessages", Locale.getDefault());

        /*try (FileReader fileReader = new FileReader(fileName)) {
            jsonObject = (JSONObject) new JSONParser().parse(fileReader);
        } catch (ParseException e) {
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("JFRMessageP1") + key
                    + messageBundle.getString("JFRMessageP3-1") + "\n" + e.getMessage()
                    + " " + e.getCause() + "\n" + e.getStackTrace());
            alert.showAndWait();
            throw new CatchedExceptionException(e);
        } catch (FileNotFoundException e) {
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("JFRMessageP1") + key
                    + messageBundle.getString("JFRMessageP4") + "\n" + e.getMessage()
                    + " " + e.getCause() + "\n" + e.getStackTrace());
            alert.showAndWait();
            throw new CatchedExceptionException(e);
        } catch (IOException e) {
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("JFRMessageP1") + key
                    + messageBundle.getString("JFRMessageP3-2") + "\n" + e.getMessage() + " "
                    + e.getCause() + "\n" + e.getStackTrace());
            alert.showAndWait();
            throw new CatchedExceptionException(e);
        }*/

        jsonObject.put(key, value);

        try (FileWriter fileWriter = new FileWriter(fileName)) {
            fileWriter.write(jsonObject.toJSONString());
        } catch (FileNotFoundException e) {
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("JFWMessageP1") + key
                    + messageBundle.getString("JFWMessageP4") + "\n" + e.getMessage()
                    + " " + e.getCause() + "\n" + e.getStackTrace());
            alert.showAndWait();
            throw new CatchedExceptionException(e);
        } catch (IOException e) {
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("JFWMessageP1") + key
                    + messageBundle.getString("JFWMessageP3-2") + "\n" + e.getMessage() + " "
                    + e.getCause() + "\n" + e.getStackTrace());
            alert.showAndWait();
            throw new CatchedExceptionException(e);
        }
    }

    public static Object readTagValue(String fileName, String key, boolean supressNSKE)
            throws NotSuchKeyException, CatchedExceptionException {
        Object obj;
        ResourceBundle messageBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.logmessages", Locale.getDefault());

        try {
            JSONObject jsonObject = readJsonObject(fileName, key);
            if (jsonObject.containsKey(key)) {
                obj = jsonObject.get(key);
            } else {
                throw new NotSuchKeyException(messageBundle.getString("NotSuchKeyMessage"));
            }
        } catch (NotSuchKeyException e) {
            if (supressNSKE) {
                throw new CatchedExceptionException(e);
            }
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("JFRMessageP1") + key
                    + messageBundle.getString("JFRMessageP2"));
            alert.showAndWait();

            throw new CatchedNotSuchKeyExceptionException(e);
        }

        return obj;
    }

    private static JSONObject readJsonObject(String fileName, String key) {
        //JSONObject jsonObject;
        ResourceBundle messageBundle = ResourceBundle.getBundle(
                "pl.sudoku.view.logmessages", Locale.getDefault());

        try (FileReader fileReader = new FileReader(fileName)) {
            //jsonObject = (JSONObject) new JSONParser().parse(fileReader);
            return (JSONObject) new JSONParser().parse(fileReader);
        } catch (ParseException e) {
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("JFRMessageP1") + key
                    + messageBundle.getString("JFRMessageP3-1") + "\n" + e.getMessage()
                    + " " + e.getCause() + "\n" + e.getStackTrace());
            alert.showAndWait();
            throw new CatchedExceptionException(e);
        } catch (FileNotFoundException e) {
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("JFRMessageP1") + key
                    + messageBundle.getString("JFRMessageP4") + "\n" + e.getMessage()
                    + " " + e.getCause() + "\n" + e.getStackTrace());
            alert.showAndWait();
            throw new CatchedExceptionException(e);
        } catch (IOException e) {
            Alert alert = new Alert(Alert.AlertType.ERROR);
            alert.setTitle(messageBundle.getString("someTroubleHasOccured"));
            alert.setHeaderText(messageBundle.getString("someTroubleHasOccured"));
            alert.setContentText(messageBundle.getString("JFRMessageP1") + key
                    + messageBundle.getString("JFRMessageP3-2") + "\n" + e.getMessage() + " "
                    + e.getCause() + "\n" + e.getStackTrace());
            alert.showAndWait();
            throw new CatchedExceptionException(e);
        }

        //return jsonObject;
    }
}
