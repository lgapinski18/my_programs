package com.example.staterecognition;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.hardware.Sensor;
import android.hardware.SensorManager;
import android.os.Build;
import android.os.Bundle;
import android.widget.LinearLayout;

import com.example.staterecognition.NeuronNetwork.INeuralNetwork;
import com.example.staterecognition.NeuronNetwork.OneLayerKohonenNetwork;
import com.example.staterecognition.NeuronNetwork.activationfunctions.ActivationFunctionSigmoid;
import com.example.staterecognition.sensorslisteners.IAccelerometerListener;
import com.example.staterecognition.sensorslisteners.SendingAccelerometerListener;

import java.util.Timer;


public class MainActivity extends AppCompatActivity {
    private Timer timer;

    //SENSORS
    private SensorManager sensorManager;
    private Sensor accelerometer;
    private Sensor gyroscope;


    //SENSORS LISTENERS
    private IAccelerometerListener accelerometerListener;// = new AccelerometerListener(findViewById(R.id.accelerometerLayout));

    private IAccelerometerListener sendingAccelerometer;

    static private StringBuilder log;

    //NEURAL NETWORKS
    INeuralNetwork neuralNetwork;

    NeuralNetworkUser neuralNetworkUser;

    private StatesChart statesChart;

    @RequiresApi(api = Build.VERSION_CODES.R)
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //timer = new Timer("AccelGyroGetter");
        sensorManager = (SensorManager) getSystemService(Context.SENSOR_SERVICE);
        //accelerometer = sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
        accelerometer = sensorManager.getDefaultSensor(Sensor.TYPE_LINEAR_ACCELERATION);
        gyroscope = sensorManager.getDefaultSensor(Sensor.TYPE_GYROSCOPE);

        log = new StringBuilder();

        final int numberOfDisplayedStates = 160;
        //statesChart = new TextView[numberOfDisplayedStates];
        LinearLayout lLayout = (LinearLayout) findViewById(R.id.statesChartLayout); // Root ViewGroup in which you want to add textviews

        statesChart = new StatesChart(this, lLayout, numberOfDisplayedStates);

        int inputNumber = 32;
        int outputNumber = 3;
        int previousResults = 2;

        float[][] weights3P2_23 = { //c == 15 - stanie ; c >= 11 - chodzenie; c <= 8 - bieganie
            {0.3919873f, 0.25352466f, 0.3099673f, 0.7426f, 0.34580725f, 0.15589963f, 0.27400845f, 0.51972616f, 0.117459826f, 0.57382417f, 0.46599126f, 0.34564707f, 0.34764424f, 0.8000514f, 0.32421678f, 0.85624325f, 0.45556965f, 0.026957171f, 0.53090304f, 0.6372797f, 0.32670864f, -0.47476625f, -0.035030562f},
            { 0.6098925f, 0.2657454f, 0.20777409f, 0.38159126f, 0.57033306f, -0.043038994f, 0.17750734f, 0.6442591f, -0.053729746f, 0.5457336f, 0.620099f, 0.08289877f, 0.26809216f, 0.7480988f, 0.14056359f, 0.46125013f, 0.2856545f, -0.07208553f, 0.2758609f, 0.46397415f, 0.3089088f, 0.2621983f, 0.23856823f},
            {0.69700056f, 0.17115718f, 0.5520949f, 0.44374618f, 0.9760766f, 0.44977435f, 0.43097073f, 1.0128129f, 0.26708513f, 0.27448395f, 1.4142361f, 0.51514596f, 0.6011556f, 1.2197102f, 0.72213995f, 0.6649844f, -0.034597483f, 0.23292176f, 0.30173618f, 0.85290647f, 0.7377385f, -0.13760148f, -0.40552416f}
        };

        //* PRZESKALOWYWANIE
        /*for (int i = 0; i < weights3P6_21T.length; i++) {
            for (int j = 0; j < weights3P6_21T[i].length;j++) {
                weights3P6_21T[i][j] *= 100;
            }
        }*/

        inputNumber = 23;
        outputNumber = 3;
        previousResults = 2;

        neuralNetwork = new OneLayerKohonenNetwork(inputNumber, outputNumber,
                new ActivationFunctionSigmoid(), weights3P2_23);/**/

        neuralNetworkUser = new NeuralNetworkUser(neuralNetwork, 100, inputNumber,
                outputNumber, previousResults, findViewById(R.id.stateTextView), statesChart);

        sendingAccelerometer = new SendingAccelerometerListener(neuralNetworkUser);

        neuralNetworkUser.startProcessing();
    }

    @Override
    protected void onStart() {
        super.onStart();

        //registering sensors listeners
        System.out.println("Registering sensors listeners");
        //sensorManager.registerListener(accelerometerListener, accelerometer, 100_000);
        sensorManager.registerListener(sendingAccelerometer, accelerometer, 100_000);

    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        neuralNetworkUser.stopProcessing();
    }

    @Override
    protected void onStop() {
        super.onStop();


        //sensorManager.unregisterListener(accelerometerListener);
        sensorManager.unregisterListener(sendingAccelerometer);
    }
}