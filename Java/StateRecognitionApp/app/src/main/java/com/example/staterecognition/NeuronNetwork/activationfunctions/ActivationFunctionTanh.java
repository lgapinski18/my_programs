package com.example.staterecognition.NeuronNetwork.activationfunctions;

public class ActivationFunctionTanh implements ActivationFunction {
    /**
     * Metoda przekazująca wartość do fukncji aktywacji.
     *
     * @param value Wartość, która ma zostać przetworzona przez funkcję aktywacji.
     * @return Wartość zwrócona przez funkcję aktywacji.
     */
    @Override
    public Float apply(Float value) {
        return (float) Math.tanh(value);
    }
}
