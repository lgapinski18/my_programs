using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StainsCleaningSummaryEntry : ASummaryEntry
{
    public Text Header;
    public Text TaskSummaryText;
    public Text PointsText;

    private string TaskSummaryMessage = "Sprz¹tniêto {0}% zabrudzeñ.";
    private string PointsMessage = "Uzyskano {0} punktów.";

    private int numberOfStains = 0;
    public int NumberOfStains
    {
        set
        {
            numberOfStains = value;
        }
        get => numberOfStains;
    }

    private int numberOfCleaned = 0;
    public int NumberOfCleaned
    {
        set
        {
            numberOfCleaned = value;
        }
        get => numberOfCleaned;
    }

    public override void UpdateData()
    {
        if (numberOfStains == 0)
        {
            TaskSummaryText.text = string.Format(TaskSummaryMessage, 100);
        }
        else
        {
            TaskSummaryText.text = string.Format(TaskSummaryMessage,
                            System.Math.Round((double)numberOfCleaned / (double)numberOfStains * 100.0, 2, System.MidpointRounding.ToEven));
        }
        PointsText.text = string.Format(PointsMessage, NumberOfPoints);
    }
}
