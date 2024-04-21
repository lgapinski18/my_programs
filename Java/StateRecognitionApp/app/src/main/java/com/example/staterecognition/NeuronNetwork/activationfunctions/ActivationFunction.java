package com.example.staterecognition.NeuronNetwork.activationfunctions;

import java.util.function.Function;

public interface ActivationFunction extends Function<Float, Float> {
    /**
     * Metoda przekazująca wartość do fukncji aktywacji.
     *
     * @param value Wartość, która ma zostać przetworzona przez funkcję aktywacji.
     * @return Wartość zwrócona przez funkcję aktywacji.
     */
    Float apply(Float value);
}
