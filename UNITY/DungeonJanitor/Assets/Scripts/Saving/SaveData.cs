using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int ShiftNo = 1;
    public string SaveDateTime = "Not Set Yet";
    public int RankNo = 0;
    public int RankPoints = 0;
    public bool[] rankTutorialDone = { false, false, false, true, true, true};

    //Dungeon Data
    public float gRoom_num = 5;
    public float gSize = 3;
    public float gNumberOfBodyParts = 20;
    public float gNumberOfStains = 12;

    //Shop Data
    public int Money = 0;
}
