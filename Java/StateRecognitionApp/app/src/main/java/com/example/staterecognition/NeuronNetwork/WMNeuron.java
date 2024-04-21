package com.example.staterecognition.NeuronNetwork;

import java.util.Arrays;
import java.util.function.Function;

/**
 * Neuron pozwalający na przekazanie funkcji podyfikującej wagi.
 */
public class WMNeuron extends ANeuron {
    public WMNeuron(int numberOfInputs, Function<Float, Float> activationFunction) {
        super(numberOfInputs, activationFunction);
    }

    public WMNeuron(float[] weights, Function<Float, Float> activationFunction) {
        super(weights, activationFunction);
    }

    public void modifyWeights(Function<float[], float[]> modify) {
        float[] weights = getWeights();
        float[] newWeights = modify.apply(weights);
        setWeights(newWeights);
    }

    @Override
    public float train(float[] X, float r) {
        return 0;
    }
}