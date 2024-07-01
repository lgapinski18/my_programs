using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TrapManager : ScriptableObject
{
    public bool GenerateTrap(float posX, float posY, Trap.TrapType trapType, int interval)
    {
        GameObject trap = Instantiate(Resources.Load<GameObject>("Prefabs/" + trapType.ToString() + "Trap"));
        switch (trapType)
        {
            case Trap.TrapType.None:
                Debug.LogError("None trap type");
                break;
            case Trap.TrapType.Spike:
                trap.GetComponent<SpikeTrap>().interval = interval;
                break;
            case Trap.TrapType.Fire:
                trap.GetComponent<FireTrap>().interval = interval;
                break;
        }
        
        // Set position for the new trap
        trap.transform.position = new Vector3(posX, posY, 0);
        return true;
    }
}
