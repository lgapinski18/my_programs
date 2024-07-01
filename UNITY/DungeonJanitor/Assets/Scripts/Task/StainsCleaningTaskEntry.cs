using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StainsCleaningTaskEntry : TaskEntry
{
    private static string[] TaskHeaderMessages = {
        "ZMYWANIE PLAM",
        "PIELÊGNACJA POD£OGI",
        "ELIMINACJA ZABRUDZEÑ"
    };
    private static string[] TaskIntrductionMessages = {
        "Wyczyœciæ pod³ogi z syfu, który zosta³ naniesiony przez dzikusy.",
        "Zmyæ plamy po czymœ, co jedynie Dungeon Master raczy wiedzieæ.",
        "Pozbyæ siê skupisk zabrudzeñ powierzchni p³askich."
    };
    public Text TaskHeader;
    public Text TaskIntroduction;
    public Text TaskMessage;

    private string messagePattern = "Postêp sprz¹tania: {0}%.";
    public int numberOfSubstains = 0;
    public int NumberOfSubstains { set
        {
            numberOfSubstains = value;
        }
        get => numberOfSubstains;
    }

    private int numberOfCleaned = 0;
    public int NumberOfCleaned { set
        {
            numberOfCleaned = value;
        } 
        get => numberOfCleaned;
    }

    // Start is called before the first frame update
    void Start()
    {
        TaskIntroduction.text = TaskIntrductionMessages[Random.Range(0, TaskIntrductionMessages.Length)];
        TaskHeader.text = TaskHeaderMessages[Random.Range(0, TaskHeaderMessages.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void UpdateData()
    {
        if (numberOfSubstains == 0)
        {
            gameObject.GetComponentInChildren<Text>().text = string.Format(messagePattern,100);
        }
        else
        {
            gameObject.GetComponentInChildren<Text>().text = string.Format(messagePattern, 
                            System.Math.Round((double)numberOfCleaned / (double)numberOfSubstains * 100.0, 2, System.MidpointRounding.ToEven));
        }
    }
}
