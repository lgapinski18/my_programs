package com.example.staterecognition.NeuronNetwork.weightsmodificationfynctions;

import java.util.function.Function;

public class KohonenPunishFunction implements Function<float[], float[]> {
    private float[] inputs;
    private float n;

    public KohonenPunishFunction(float[] inputs, float n) {
        //*
        float[] newInputs = new float[inputs.length];
        for (int i = 0; i < inputs.length; i++) {
            newInputs[i] = inputs[i];
        }
        this.inputs = newInputs;/**/


        /*
        float sum = 0;
        float maxIn = -Float.MAX_VALUE;

        for (float in : inputs) {
            //sum += in * in;
            if (in > maxIn) {
                maxIn = in;
            }
        }
        sum = (float) Math.sqrt(sum);

        float[] newInputs = new float[inputs.length];

        for (int i = 0; i < inputs.length; i++) {
            //newInputs[i] = input[i] / sum;
            newInputs[i] = inputs[i] / maxIn;
        }

        this.inputs = newInputs;/**/
        //this.inputs = inputs;/**/
        this.n = n;
    }

    /**
     * Applies this function to the given argument.
     *
     * @param weights the function argument
     * @return the function result
     */
    @Override
    public float[] apply(float[] weights) {
        float[] newWeights = new float[weights.length];

        for (int i = 0; i < weights.length; i++) {
            newWeights[i] = weights[i] - n * (inputs[i] - weights[i]);
        }

        return newWeights;
    }
}

