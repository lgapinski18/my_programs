using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpseCleaningTaskEntry : TaskEntry
{
    private static string[] TaskHeaderMessages = {
        "UTYLIZACJA TRUCHE£",
        "ELIMINACJA ZW£OK",
        "SPRZ¥TANIE CIA£"
    };
    private static string[] TaskIntrductionMessages = {
        "Pozbyæ siê resztek po ofermach, którzy sczeŸli w dungeonie.",
        "Uprz¹tn¹æ cia³a biednych dusz, które spotka³y tragiczny koniec.",
        "Usun¹æ pozosta³oœci g³upców zb³¹kanych w tych murach."
    };
    public Text TaskHeader;
    public Text TaskIntroduction;
    public Text TaskMessage;

    private string messagePattern = "Postêp {0} z {1} ({2}%).";
    //private string messagePattern = "Posprz¹taj {0} pozosta³oœci zw³ok ({1}%).";

    public int NumberOfCorpses = 0;

    public int NumberOfDisposed = 0;

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
        if (NumberOfCorpses == 0)
        {
            TaskMessage.text = string.Format(messagePattern, NumberOfDisposed, NumberOfCorpses, 100);
        }
        else
        {
            TaskMessage.text = string.Format(messagePattern, NumberOfDisposed, NumberOfCorpses,
                            System.Math.Round((double)NumberOfDisposed / (double)NumberOfCorpses * 100.0, 2, System.MidpointRounding.ToEven));
        }
    }
}
