using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StainsCleaningTaskManager : ATaskManager
{
    private static StainsCleaningTaskManager instance = null;
    private object substainLock = new object();
    private object cleanedLock = new object();

    public StainsCleaningTaskEntry entryPrefab;
    private StainsCleaningTaskEntry entry;
    public InteractableTaskboardList taskboardListPrefab;
    public StainsCleaningSummaryEntry summaryEntryPrefab;

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


    public static StainsCleaningTaskManager Instance()
    {
        return instance;
    }

    public void SetNumberOfSubstains(int number)
    {
        entry.NumberOfSubstains = number;
        entry.UpdateData();
    }

    public void AddToNumberOfSubstains(int number)
    {
        lock(substainLock)
        {
            entry.NumberOfSubstains += number;
            entry.UpdateData();
        }
    }

    public void IncrementNumberOfSubstains()
    {
        entry.NumberOfSubstains = entry.NumberOfSubstains + 1;
        entry.UpdateData();
    }

    public void SetNumberOfCleaned(int number)
    {
        entry.NumberOfCleaned = number;
        entry.UpdateData();
    }

    public void IncrementNumberOfCleaned()
    {
        lock(cleanedLock)
        {
            entry.NumberOfCleaned = entry.NumberOfCleaned + 1;
            entry.UpdateData();
        }
    }

    public void AddToNumberOfCleaned(int number)
    {
        lock(cleanedLock)
        {
            entry.NumberOfCleaned += number;
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
        if (entry.NumberOfSubstains == 0)
        {
            //return 0;
        }
        else
        {
            points = 100 * entry.NumberOfCleaned / entry.NumberOfSubstains;

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


        //StainsCleaningSummaryEntry summaryEntry = Instantiate(summaryEntryPrefab, GameObject.Find("Player").GetComponent<Player>().SummaryTaskContent.transform);
        StainsCleaningSummaryEntry summaryEntry = Instantiate(summaryEntryPrefab);
        summaryEntry.Header.text = entry.TaskHeader.text;
        summaryEntry.gameObject.SetActive(false);
        summaryEntry.NumberOfStains = entry.NumberOfSubstains;
        summaryEntry.NumberOfCleaned = entry.NumberOfCleaned;
        summaryEntry.NumberOfPoints = points;
        //summaryEntry.gameObject.transform.parent = GameObject.Find("Player").GetComponent<Player>().SummaryTaskContent.transform;
        summaryEntry.UpdateData();

        return summaryEntry;
    }
}
