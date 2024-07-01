using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseCleaningTaskManager : ATaskManager
{
    private static CorpseCleaningTaskManager instance = null;
    private static Object InstanceLock = new Object();
    private Object CorpseNumberLock = new Object();
    private Object DisposedNumberLock = new Object();

    public CorpseCleaningTaskEntry entryPrefab;
    private CorpseCleaningTaskEntry entry;
    public InteractableTaskboardList taskboardListPrefab;
    public CorpseCleaningSummaryEntry summaryEntryPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            entry = Instantiate(entryPrefab, GameObject.Find("Player").GetComponent<Player>().TaskList.transform);
            entry.gameObject.transform.parent = GameObject.Find("Player").transform;
            entry.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static CorpseCleaningTaskManager Instance()
    {
        /*if (instance == null)
        {
            Instantiate(instancePrefab);
        }*/
        return instance;
    }

    public static CorpseCleaningTaskManager WaitForInstance()
    {
        lock (InstanceLock)
        {
            while (instance == null)
            {

            }
            return instance;
        }
    }


    public void SetNumberOfCorpses(int number)
    {
        lock(CorpseNumberLock)
        {
            entry.NumberOfCorpses = number;
            entry.UpdateData();
        }
    }

    public void IncrementNumberOfCorpses()
    {
        lock (CorpseNumberLock)
        {
            entry.NumberOfCorpses = entry.NumberOfCorpses + 1;
            entry.UpdateData();
        }
    }

    public void DecrementNumberOfCorpses()
    {
        lock (CorpseNumberLock)
        {
            entry.NumberOfCorpses = entry.NumberOfCorpses - 1;
            entry.UpdateData();
        }
    }

    public int GetNumberOfCorpses()
    {
        lock (CorpseNumberLock)
        {
            return entry.NumberOfCorpses;
        }
    }

    public void SetNumberOfDisposed(int number)
    {
        lock (DisposedNumberLock)
        {
            entry.NumberOfDisposed = number;
            entry.UpdateData();
        }
    }

    public void IncrementNumberOfDisposed()
    {
        lock (DisposedNumberLock)
        {
            entry.NumberOfDisposed = entry.NumberOfDisposed + 1;
            entry.UpdateData();
        }
    }

    public override InteractableTaskboardList InitTask()
    {
        InteractableTaskboardList taskboardList = Instantiate(taskboardListPrefab);
        entry.UpdateData();
        taskboardList.Task_Entry = entry;
        //taskboardList.Task_Entry = entryPrefab;

        return taskboardList;
    }

    public override ASummaryEntry SummarizeTask()
    {
        int points = 0;
        if (entry.NumberOfCorpses == 0)
        {
            //return 0;
        }
        else
        {
            points = 100 * entry.NumberOfDisposed / entry.NumberOfCorpses;

            //Poprawa iloœci punktów
            int treshhold = 25;
            int max = 100;

            int MaxAchived = 100;
            int MinAchieved = -50;

            if (points >= 25)
            {
                points = MaxAchived * (points - treshhold) / (max - treshhold);
            }
            else
            {
                points = MinAchieved * (points - treshhold) / (0 - treshhold);
            }
        }


        //CorpseCleaningSummaryEntry summaryEntry = Instantiate(summaryEntryPrefab, GameObject.Find("Player").GetComponent<Player>().SummaryTaskContent.transform);
        CorpseCleaningSummaryEntry summaryEntry = Instantiate(summaryEntryPrefab);
        summaryEntry.Header.text = entry.TaskHeader.text;
        summaryEntry.gameObject.SetActive(false);
        summaryEntry.NumberOfCorpses = entry.NumberOfCorpses;
        summaryEntry.NumberOfDisposed = entry.NumberOfDisposed;
        summaryEntry.NumberOfPoints = points;
        //summaryEntry.gameObject.transform.parent = GameObject.Find("Player").GetComponent<Player>().SummaryTaskContent.transform;
        summaryEntry.UpdateData();

        return summaryEntry;
    }
}
