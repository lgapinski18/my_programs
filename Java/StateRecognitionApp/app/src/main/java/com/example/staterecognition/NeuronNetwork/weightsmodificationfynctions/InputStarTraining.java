package com.example.staterecognition.NeuronNetwork.weightsmodificationfynctions;

import java.util.function.Function;

public class InputStarTraining implements Function<float[], float[]> {
    private float[] inputs;
    private float n;

    /**
     *
     * @param inputs Aktualne wejście do sieci, które jest wykorzystywane na potrzeby obliczeń.
     * @param n Parametr ni wzoru Gwaizdy Wejść.
     */
    public InputStarTraining(float[] inputs, float n) {
        float sum = 0;
        float maxIn = inputs[0];

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
        //this.inputs = inputs;
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
            newWeights[i] = weights[i] + n * (inputs[i] - weights[i]);
        }

        return newWeights;
    }
}
