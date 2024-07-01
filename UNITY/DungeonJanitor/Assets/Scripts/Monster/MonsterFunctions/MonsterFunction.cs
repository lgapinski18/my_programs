using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFunction : MonoBehaviour
{

    protected MonsterController controller;
    protected virtual void SetupMonsterFunction()
    {
        controller = GetComponent<MonsterController>();
    }
    protected virtual void ClearOnEndPerforming()
    {

    }

    public abstract float StartPerforming();
    public abstract void EndPerforming();


    public abstract bool CanBeCanceled();
    public abstract void CancelPerformedAction();


    public abstract bool CanBeDisturbed();
    public abstract void DisturbPerformedAction();




}
