using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATaskManager : MonoBehaviour
{
    public int RankSinceAvailable = 0;

    public abstract InteractableTaskboardList InitTask();

    public abstract ASummaryEntry SummarizeTask();
}
