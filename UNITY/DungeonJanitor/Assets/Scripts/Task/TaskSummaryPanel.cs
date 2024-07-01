using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSummaryPanel : MonoBehaviour
{
    private static TaskSummaryPanel instance = null;
    private List<ASummaryEntry> SummaryEntries = new List<ASummaryEntry>();
    public GameObject BlindPanel;
    public GameObject SummaryTasksContentPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }


    public static TaskSummaryPanel Instance() { 
        return instance; 
    }


    public void AddTaskSummaryEntry(ASummaryEntry summaryEntry)
    {
        summaryEntry.gameObject.transform.parent = SummaryTasksContentPanel.transform;
        summaryEntry.gameObject.SetActive(true);

        SummaryEntries.Add(summaryEntry);
    }

    void Clear()
    {
        ASummaryEntry summaryEntry;
        while (SummaryEntries.Count > 0)
        {
            summaryEntry = SummaryEntries[0];
            SummaryEntries.RemoveAt(0);
            Destroy(summaryEntry.gameObject);
        }
    }

    public void EndShift()
    {
        BlindPanel.SetActive(false);
        Clear();
        GameManager.instance.EndShift();
    }
}
