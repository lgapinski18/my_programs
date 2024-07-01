using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CorpseAction : ScriptableObject
{

    public abstract void perform(CorpseActionArgs args);
}
