package com.example.staterecognition.NeuronNetwork;

public interface INeuralNetwork {
    /**
     * Metoda przetwarzająca wejście przez sieć neuroną i zwracającą.
     * @param input Wejście sieci neuronowej.
     * @return Wyjście sieci neuronowej..
     */
    int process(float[] input);

    /**
     * Metoda ucząca sieć na podstawie podane wejścia i oczekiwanego wyjścia.
     * @param input Wejście sieci neuronowej.
     * @param expecedOutput Oczekiwane wyjście sieci neuronowej.
     */
    void train(float[] input, int expecedOutput);


    ///**
    // * Metoda pobierająca tablicę macierzy wag dla poszczególnych warstw sieci.
    // * @return Tablica macierzy wag dla poszczególnych warstw sieci.
    // */
    //float[][][] getWeights();

    ///**
    // * Metoda ustawiająca tablicę macierzy wag dla poszczególnych warstw sieci.
    // * @param weights Tablica macierzy wag dla poszczególnych warstw sieci.
    // */
    //void setWeights(float[][][] weights);
}
