using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ASummaryEntry : MonoBehaviour
{
    private int numberOfPoints = 0;
    public int NumberOfPoints
    {
        set
        {
            numberOfPoints = value;
        }
        get => numberOfPoints;
    }


    void Awake()
    {
        DontDestroyOnLoad(gameObject);    
    }


    public abstract void UpdateData();
}
