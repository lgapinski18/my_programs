using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Trap : MonoBehaviour
{
    public string trapType;
    public bool activated = false;
    public int interval = 5;

    [SerializeField]
    private Corpse.CorpseSize[] targetCorpseSizes;
    private Corpse.CorpseSize resultCorpseSizeMask;
    public Corpse.CorpseSize ResultCorpseSizeMask { get => resultCorpseSizeMask; }

    [SerializeField]
    private Corpse.CorpseType[] targetCorpseTypes;
    private Corpse.CorpseType resultCorpseTypeMask;
    public Corpse.CorpseType ResultCorpseTypeMask { get => resultCorpseTypeMask; }

    public enum TrapType
    {
        None,
        Spike,
        Fire,
        Crush
    }

    public bool ValidateIsTarget(GameObject potentailTarget)
    {
        Corpse corpse = potentailTarget.GetComponent<Corpse>();
        bool result = corpse != null
                    && ((corpse.Size & ResultCorpseSizeMask) != 0)
                    && ((corpse.Type & ResultCorpseTypeMask) != 0);

        return result;
    }

    protected void SetupTrapMask()
    {

        foreach (Corpse.CorpseSize size in targetCorpseSizes)
        {
            resultCorpseSizeMask |= size;
        }

        foreach (Corpse.CorpseType type in targetCorpseTypes)
        {
            resultCorpseTypeMask |= type;
        }
    }
    public void ActivateTrap()
    {
        activated = true;
    }
    public void DeactivateTrap()
    {
        activated = false;
    }
}
