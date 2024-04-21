package com.example.staterecognition.NeuronNetwork.activationfunctions;

//import com.movementstateai.Main;

public class ActivationFunctionSigmoid implements ActivationFunction {
    /**
     * Metoda przekazująca wartość do fukncji aktywacji.
     *
     * @param value Wartość, która ma zostać przetworzona przez funkcję aktywacji.
     * @return Wartość zwrócona przez funkcję aktywacji.
     */
    @Override
    public Float apply(Float value) {
        return (float) (1.0f / (1.0f + Math.exp(-value)));
    }
}
