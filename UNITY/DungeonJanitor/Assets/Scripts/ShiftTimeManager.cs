using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftTimeManager : MonoBehaviour
{
    public Image DayTimeImage = null;
    public Text ShiftNumber = null;
    public Text Time = null;
    public int NumberOfSeconds = 1200;

    [Header("Day Time Sprites")]
    public Sprite Dawn;
    public Sprite Day;
    public Sprite Twilight;
    public Sprite Night;
    public enum DayTime { DAWN, DAY, TWILIGHT, NIGHT };


    public void SetDayTimeSprite(DayTime dayTime)
    {
        switch(dayTime)
        {
            case DayTime.DAWN:
                DayTimeImage.sprite = Dawn;
                break;

            case DayTime.DAY:
                DayTimeImage.sprite = Day;
                break;

            case DayTime.TWILIGHT:
                DayTimeImage.sprite = Twilight;
                break;

            case DayTime.NIGHT:
                DayTimeImage.sprite = Night;
                break;
        }
    }

    public void SetShiftNumber(int shiftNumber)
    {
        ShiftNumber.text = shiftNumber.ToString();
    }

    public void UpdateTime()
    {
        string time = "";
        if ((NumberOfSeconds / 60) < 10)
        {
            time += "0";
        }

        time = "" + (NumberOfSeconds / 60) + ":";

        if (NumberOfSeconds % 60 < 10)
        {
            time += "0";
        }

        time += "" + (NumberOfSeconds % 60);

        Time.text = time;

        NumberOfSeconds -= 1;
    }
}
