using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpseCleaningSummaryEntry : ASummaryEntry
{
    public Text Header;
    public Text TaskSummaryText;
    public Text PointsText;

    private string TaskSummaryMessage = "Sprz¹tniêto {0} z {1} zw³ok ({2}%).";
    private string PointsMessage = "Uzyskano {0} punktów.";

    private int numberOfCorpses = 0;
    public int NumberOfCorpses
    {
        set
        {
            numberOfCorpses = value;
        }
        get => numberOfCorpses;
    }

    private int numberOfDisposed = 0;
    public int NumberOfDisposed
    {
        set
        {
            numberOfDisposed = value;
        }
        get => numberOfDisposed;
    }


    public override void UpdateData()
    {
        if (NumberOfCorpses == 0)
        {
            TaskSummaryText.text = string.Format(TaskSummaryMessage, numberOfDisposed, numberOfCorpses, 100);
        }
        else
        {
            TaskSummaryText.text = string.Format(TaskSummaryMessage, numberOfDisposed, numberOfCorpses,
                            System.Math.Round((double)numberOfDisposed / (double)numberOfCorpses * 100.0, 2, System.MidpointRounding.ToEven));
        }
        PointsText.text = string.Format(PointsMessage, NumberOfPoints);
    }
}
