package com.example.staterecognition.sensorslisteners;

import android.hardware.Sensor;
import android.hardware.SensorEvent;

import com.example.staterecognition.NeuralNetworkUser;

import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.util.Locale;

public class SendingAccelerometerListener implements IAccelerometerListener {

    private NeuralNetworkUser networkUser;

    private DecimalFormat df;

    public SendingAccelerometerListener(NeuralNetworkUser networkUser) {
        this.networkUser = networkUser;

        df = new DecimalFormat("0.00");
        df.setDecimalFormatSymbols(new DecimalFormatSymbols(Locale.ENGLISH));
    }

    @Override
    public void onSensorChanged(SensorEvent sensorEvent) {
        networkUser.setAccel(Float.valueOf(df.format(sensorEvent.values[0])),
                Float.valueOf(df.format(sensorEvent.values[1])),
                Float.valueOf(df.format(sensorEvent.values[2])));
    }

    @Override
    public void onAccuracyChanged(Sensor sensor, int i) {

    }
}
