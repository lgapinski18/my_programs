using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Windows;
using UnityEngine.UI;

public class InteractableTaskboardList : AInteractable
{
    public SpriteRenderer exclamationMarkup;
    private SpriteRenderer exclamationMarkupInstance;

    public GameObject TaskList;

    private TaskEntry taskEntry = null;
    public TaskEntry Task_Entry { set
        {
            taskEntry = value;
        }
    } 

    private Taskboard taskboard = null;
    public Taskboard TaskBoard { set 
        { 
            taskboard = value; 
        } 
    }

    void Awake()
    {
        TaskList = GameObject.Find("Player").GetComponent<Player>().TaskList;
    }

    // Start is called before the first frame update
    void Start()
    {
        if ((exclamationMarkup != null) && (exclamationMarkupInstance == null))
        {
            exclamationMarkupInstance = Instantiate(exclamationMarkup);
            exclamationMarkupInstance.gameObject.transform.position = transform.position;
            exclamationMarkupInstance.transform.position += new Vector3(-0.25f, 0.25f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool interact()
    {
        Debug.Log(string.Format("InteractableTaskboardList: {0} - interaction!", getObjectName()));

        if (taskboard != null)
        {
            taskboard.removeTaskpaper(this);
        }

        taskEntry.gameObject.transform.parent = TaskList.transform;
        taskEntry.gameObject.SetActive(true);
        //Instantiate(taskEntry, TaskList.transform);

        MessageManager.Instance().AddNotification(MessageManager.Icons.BOOK_ICON, "Dodano nowe zadanie do listy");

        Destroy(exclamationMarkupInstance.gameObject);

        return true;
        //Destroy(this.gameObject);
        //Destroy(exclamationMarkupInstance.gameObject);
    }
}
