package com.example.staterecognition;

import android.widget.TextView;

import com.example.staterecognition.NeuronNetwork.INeuralNetwork;

import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.locks.ReentrantLock;

public class NeuralNetworkUser {
    private INeuralNetwork neuralNetwork;

    private float accelX;
    private float accelY;
    private float accelZ;

    private float gyroX;
    private float gyroY;
    private float gyroZ;

    private int inputNumber;
    private int outputNumber;
    private int previousNumber;

    private ArrayList<Float> data = new ArrayList<>();
    private ArrayList<Float> previousResults = new ArrayList<>();

    private Timer timer;

    private long interval;

    private ArrayList<Integer> lastOutputs = new ArrayList<>();
    private int numberOfLastOutputs = 15;


    private TextView stateTextView;
    private StatesChart statesChart;

    private ReentrantLock accelLock = new ReentrantLock();
    private ReentrantLock gyroLock = new ReentrantLock();

    public NeuralNetworkUser(INeuralNetwork neuralNetwork, long interval, int inputNumber,
                             int outputNumber, int previousResultsNumber, TextView stateTextView,
                             StatesChart statesChart) {
        this.neuralNetwork = neuralNetwork;
        this.interval = interval;
        this.inputNumber = inputNumber;
        this.outputNumber = outputNumber;
        this.previousNumber = previousResultsNumber;
        this.stateTextView = stateTextView;
        this.statesChart = statesChart;

        for (int i = 0; i < (inputNumber - previousResultsNumber); i++) {
            data.add(0.f);
        }

        for (int i = 0; i < previousResultsNumber; i++) {
            previousResults.add(0.f);
        }


        timer = new Timer("DisplayingTimer", true);
    }

    public void setAccel(float accelX, float accelY, float accelZ) {
        accelLock.lock();
        this.accelX = accelX;
        this.accelY = accelY;
        this.accelZ = accelZ;
        accelLock.unlock();
    }

    public void setGyro(float gyroX, float gyroY, float gyroZ) {
        gyroLock.lock();
        this.gyroX = gyroX;
        this.gyroY = gyroY;
        this.gyroZ = gyroZ;
        gyroLock.unlock();
    }

    private void process() {

        //Removing old data
        for (int i = 0; i < 3; i++) {
            data.remove(0);
        }

        //Adding new data
        accelLock.lock();
        data.add(accelX);
        data.add(accelY);
        data.add(accelZ);
        accelLock.unlock();

        data.addAll(previousResults);

        float[] input = new float[inputNumber];
        for (int i = 0; i < inputNumber; i++) {
            input[i] = (float) data.get(i);
        }

        int output = neuralNetwork.process(input);

        previousResults.add((float) output);
        previousResults.remove(0);

        lastOutputs.add(output);
        if (!(lastOutputs.size() < numberOfLastOutputs)) {
           int[] tab = new int[] {0, 0, 0};

           for (int i : lastOutputs) {
               tab[i] += 1;
           }

            //Dla weights3P2_23
            if (tab[1] >= 15) {
                output = 0;
            }
            else if (tab[1] >= 11) {
                output = 1;
            }
            else {
                output = 2;
            }

           lastOutputs.remove(0);
        }

        if (output == 0) {
            stateTextView.setText("Stanie");
        } else if (output == 1) {
            stateTextView.setText("Chodzenie");
        } else if (output == 2) {
            stateTextView.setText("Bieganie");
        }

        //statesChart.addState(output);

        //Removing previous states from end
        for (int i = 0; i < previousNumber; i++) {
            data.remove(data.size() - 1);
        }
    }

    public void startProcessing() {
        timer.schedule(new TimerTask() {
            public void run() {
                process();
            }
        }, interval, interval);
    }

    public void stopProcessing() {
        timer.cancel();
        timer.purge();
    }
}
