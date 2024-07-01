using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ItemData : ScriptableObject
{
    [SerializeField]
    public string itemName;
    [SerializeField]
    public Sprite icon;
    [SerializeField]
    public GameObject model;
    [SerializeField]
    public bool isOnMap;
    [SerializeField]
    public int itemSize;
    [SerializeField]
    public int maxDurability = 0;
    public GameObject toolObject = null;

    protected ItemData() { }


    abstract public bool Interaction();

    abstract public bool UseItem(int currentDurability);

    abstract public void ReloadItem();
}
