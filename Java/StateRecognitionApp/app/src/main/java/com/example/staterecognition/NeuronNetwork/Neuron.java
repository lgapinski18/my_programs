package com.example.staterecognition.NeuronNetwork;

import java.util.Random;
import java.util.function.Function;

public class Neuron {
    private int numberOfInputs;
    protected float[] weights;
    private Function<Float, Float> activationFunction; //table of weights, table of arguments, level of activation

    public Neuron(int numberOfInputs, Function<Float, Float> activationFunction) {
        this.numberOfInputs = numberOfInputs;
        this.activationFunction = activationFunction;

        weights = new float[numberOfInputs];

        Random random = new Random();
        float min = 0;
        float max = 1;
        for (int i = 0; i < weights.length; i++) {
            weights[i] = random.nextFloat() * (max - min) + min;
        }
    }

    public Neuron(float[] weights, Function<Float, Float> activationFunction) {
        this.numberOfInputs = weights.length;
        //this.weights = weights;
        setWeights(weights);
        this.activationFunction = activationFunction;
    }

    public int getNumberOfInputs() {
        return numberOfInputs;
    }

    public void setWeights(float[] weights) {
        float[] newWeights = new float[weights.length];
        for (int i = 0; i < weights.length; i++) {
            newWeights[i] = weights[i];
        }

        this.numberOfInputs = weights.length;
        this.weights = newWeights;
    }

    public float[] getWeights() {
        float[] newWeights = new float[weights.length];
        for (int i = 0; i < weights.length; i++) {
            newWeights[i] = weights[i];
        }
        return newWeights;
    }

    public Function<Float, Float> getActivationFunction() {
        return activationFunction;
    }

    public float process(float[] X) {
        float sum = 0;
        for (int i = 0; i < weights.length; i++) {
            sum += weights[i] * X[i];
        }

        return getActivationFunction().apply(sum);
    }

    //public abstract float train(float[] X, float r);
}
