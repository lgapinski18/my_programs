using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Singleton
 */
public class Taskboard : MonoBehaviour
{
    public static Taskboard instance = null;

    [Header("Managing task lists")]
    private List<InteractableTaskboardList> taskPapers = new List<InteractableTaskboardList>();
    public float width = 1.5f;
    public float height = 0.5f;
    public InteractableTaskboardList InteractableTaskboardListPrefab = null;

    [Header("Managing task managers")]
    public ATaskManager[] TaskManagers;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addTaskpaper(InteractableTaskboardList taskpaper)
    {
        taskPapers.Add(taskpaper);
        
        taskpaper.transform.position = new Vector3(transform.position.x + Random.Range(-width / 2.0f, width / 2.0f), 
                                                   transform.position.y + Random.Range(-height / 2.0f, 0.0f), 0.0f);
    }

    public void removeTaskpaper(InteractableTaskboardList taskpaper)
    {
        taskPapers.Remove(taskpaper.gameObject.GetComponent<InteractableTaskboardList>());
    }


    public void InitTasks(int rank)
    {
        Debug.Log("Initiating tasks has begun!");

        InteractableTaskboardList taskboardList;

        foreach (ATaskManager taskManager in TaskManagers)
        {
            if (taskManager.RankSinceAvailable <= rank)
            {
                taskboardList = taskManager.InitTask();
                addTaskpaper(taskboardList);
            }
        }
    }

    public List<ASummaryEntry> SummarizeTasks(int Rank)
    {
        Debug.Log("Summarizing tasks has begun!");

        //int totalScore = 0;
        List<ASummaryEntry> summaryEntries = new List<ASummaryEntry>();

        foreach (ATaskManager taskManager in TaskManagers)
        {
            if (taskManager.RankSinceAvailable <= Rank)
            {
                summaryEntries.Add(taskManager.SummarizeTask());
                //totalScore += 
            }
        }

        return summaryEntries;
    }
}
