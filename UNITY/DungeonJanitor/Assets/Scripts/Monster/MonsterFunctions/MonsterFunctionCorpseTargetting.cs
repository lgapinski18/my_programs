using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MonsterFunctionCorpseTargetting : MonsterFunctionTargetting
{
    [Header("Corpse targeting properties"), Space(10)]
    [SerializeField]
    private Corpse.CorpseSize[] targetCorpseSizes;
    private Corpse.CorpseSize resultCorpseSizeMask;
    public Corpse.CorpseSize ResultCorpseSizeMask { get => resultCorpseSizeMask; }

    [SerializeField]
    private Corpse.CorpseType[] targetCorpseTypes;
    private Corpse.CorpseType resultCorpseTypeMask;
    public Corpse.CorpseType ResultCorpseTypeMask { get => resultCorpseTypeMask; }

    [SerializeField]
    protected ReactionsController reactionsController;

    protected bool isStartedPerforming = false;



    //public delegate void CanPerformAction(MonsterFunctionCorpseTargetting function, GameObject target);
    //public event CanPerformAction canPerform;
    //protected void OnCanPerform(MonsterFunctionCorpseTargetting function, GameObject target)
    //{
    //    canPerform?.Invoke(function, target);
    //}




    #region MONSTER_FUNCTION_MANAGEMENT

    protected override void SetupMonsterFunction()
    {
        base.SetupMonsterFunction();

        foreach (Corpse.CorpseSize size in targetCorpseSizes)
        {
            resultCorpseSizeMask |= size;
        }

        foreach (Corpse.CorpseType type in targetCorpseTypes)
        {
            resultCorpseTypeMask |= type;
        }
    }
    protected override void ClearOnEndPerforming()
    {
        base.ClearOnEndPerforming();
    }

    #endregion

 


}
