using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rank0TutorialManager : TutorialManager //MonoBehaviour
{
    public TutorialMessageTrigger StainsTMTPrefab;

    public TutorialMessageTrigger StainsCleaningTMTPrefab;
    public GameObject StainsCleaningTMTPosition;

    void Awake()
    {
        TutorialMessageTrigger tutorialMessage = Instantiate(StainsCleaningTMTPrefab);
        tutorialMessage.gameObject.transform.position = StainsCleaningTMTPosition.transform.position;
    }
}
