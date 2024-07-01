using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CLEANING_PRODUCTS
{
    DETERGENT,
    LEMON,
    PARAFFIN  
};

public class CleaningProductsInventory : MonoBehaviour
{
    public Vector3 cleaning_power;
    public readonly Vector3[] cleaning_products = { new Vector3(1.0f, 0.0f, 0.0f), 
                                                    new Vector3(0.0f, 1.0f, 0.0f), 
                                                    new Vector3(0.0f, 0.0f, 1.0f) };

    private Queue2<CLEANING_PRODUCTS> queue = new Queue2<CLEANING_PRODUCTS>(CLEANING_PRODUCTS.LEMON, CLEANING_PRODUCTS.DETERGENT);

    public void PushLemon()
    {
        queue.Push(CLEANING_PRODUCTS.LEMON);
        UpdateCleaningPower();
    }

    public void PushDetergent()
    {
        queue.Push(CLEANING_PRODUCTS.DETERGENT);
        UpdateCleaningPower();
    }

    public void PushPraffin()
    {
        queue.Push(CLEANING_PRODUCTS.PARAFFIN);
        UpdateCleaningPower();
    }

    public CLEANING_PRODUCTS Zero()
    {
        return queue.Zero();
    }

    public CLEANING_PRODUCTS One()
    {
        return queue.One();
    }

    public  void UpdateCleaningPower() 
    {
        cleaning_power = cleaning_products[(int)queue.Zero()] + cleaning_products[(int)queue.One()];
        Debug.Log(cleaning_power);
    }

    public Vector3 GetCleaningPower()
    {
        return cleaning_power;
    }
}
