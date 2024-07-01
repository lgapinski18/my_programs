using System.Collections;
using System.Collections.Generic;
using System.Xml.Resolvers;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public int id;
    public ItemData itemType;
    public int durability;
    private bool reloaded;

    public ItemInstance(int ID, ItemData itemData)
    {
        id = ID;
        itemType = itemData;
    }

    public bool UseItem()
    {
        if (reloaded != true) 
        {
            reloaded = true;
            durability = itemType.maxDurability;
        }
        if (itemType.UseItem(durability))
        {
            durability--;
        }
        return true;
    }
    
    public int GetDurability()
    {
        if (reloaded != true)
        {
            reloaded = true;
            durability = itemType.maxDurability;
        }
        return durability;
    }
}
