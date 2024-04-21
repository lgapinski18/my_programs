package com.example.staterecognition.NeuronNetwork;


import com.example.staterecognition.NeuronNetwork.weightsmodificationfynctions.InputStarTraining;
import com.example.staterecognition.NeuronNetwork.weightsmodificationfynctions.KohonenPunishFunction;

import java.util.function.Function;

public class OneLayerKohonenNetwork implements INeuralNetwork {
    private int inputNumber;

    private int outputNumber;

    private Function<Float, Float> activationFunction;

    private WMNeuron[] neurons;

    public OneLayerKohonenNetwork(int inputNumber, int outputNumber,
                                  Function<Float, Float> activationFunction) {
        this.inputNumber = inputNumber;
        this.outputNumber = outputNumber;
        this.activationFunction = activationFunction;

        WMNeuron[] newNeurons = new WMNeuron[outputNumber];

        for (int i = 0; i < outputNumber; i++) {
            newNeurons[i] = new WMNeuron(inputNumber, activationFunction);
        }

        neurons = newNeurons;
    }

    public OneLayerKohonenNetwork(int inputNumber, int outputNumber,
                                  Function<Float, Float> activationFunction, float [][] weights) {
        this.inputNumber = inputNumber;
        this.outputNumber = outputNumber;
        this.activationFunction = activationFunction;

        WMNeuron[] newNeurons = new WMNeuron[outputNumber];

        for (int i = 0; i < outputNumber; i++) {
            newNeurons[i] = new WMNeuron(weights[i], activationFunction);
        }

        neurons = newNeurons;
    }

    @Override
    public int process(float[] input) {
        int result = 0;
        float max = -2f;
        float current;

        /*
        float sum = 0;
        float maxIn = -Float.MAX_VALUE;

        for (float in : input) {
            //sum += in * in;
            if (in > maxIn) {
                maxIn = in;
            }
        }
        sum = (float) Math.sqrt(sum);

        float[] newInputs = new float[input.length];

        for (int i = 0; i < input.length; i++) {
            //newInputs[i] = input[i] / sum;
            newInputs[i] = input[i] / maxIn;
        }

        input = newInputs;/**/

        for (int i = 0; i < outputNumber; i++) {
            current = neurons[i].process(input);
            if (current > max) {
                max = current;
                result = i;
            }
        }

        return result;
    }

    @Override
    public void train(float[] input, int expectedOutput) {
        int output = process(input);

        if (output == expectedOutput) {
            neurons[output].modifyWeights(new InputStarTraining(input, 0.001f));
        } else {
            neurons[output].modifyWeights(new KohonenPunishFunction(input, 0.001f));
        }
    }

    public float[][] getWeights() {
        float[][] weights = new float[outputNumber][];

        for (int i = 0; i < outputNumber; i++) {
            weights[i] = neurons[i].getWeights();
        }

        return weights;
    }

    public void setWeights(float[][] weights) {
        for (int i = 0; i < outputNumber; i++) {
            neurons[i].setWeights(weights[i]);
        }
    }

    public int getInputNumber() {
        return inputNumber;
    }

    public int getOutputNumber() {
        return outputNumber;
    }
}
