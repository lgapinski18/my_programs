using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    public int maxItems = 3;
    public ItemInstance[] items;
    public Inventory(int maxNumberOfItems)
    {
        maxItems = maxNumberOfItems;
        items = new ItemInstance[maxItems];
    }

    public bool AddItem(ItemInstance itemToAdd)
    {
        //if (itemToAdd.itemType.isOnMap)
        {   // Lista przechowuj¹ca indeksy o pustych miejscach
            List<int> freeSpaceIndexes = new List<int>();
            // Szukanie pustego miejsca w ekwipunku
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    freeSpaceIndexes.Add(i);
                }
                else if (items[i].itemType == null)
                {
                    freeSpaceIndexes.Add(i);
                }
            }
            if (freeSpaceIndexes.Count >= itemToAdd.itemType.itemSize)
            {
                itemToAdd.id = generateNewId();
                for (int i = 0; i < itemToAdd.itemType.itemSize; i++)
                {
                    if (items[freeSpaceIndexes[i]] == null)
                    {
                        items[freeSpaceIndexes[i]] = itemToAdd;
                    }
                    else
                    {
                        items[freeSpaceIndexes[i]].itemType = itemToAdd.itemType;
                    }
                }
                UnityEngine.Debug.Log("Added an item");
                return true;
            }
            UnityEngine.Debug.Log("No space in the inventory");
            return false;
        }
        UnityEngine.Debug.Log("Cannot add item to the inventory, it is not on the map.");
        return false;
    }

    public void RemoveItem(int itemIndex)
    {
        int idToDelete = items[itemIndex].id;
        for (int i = 0; i < maxItems; i++)
        {
            if (items[i] != null && items[i].id == idToDelete)
            { items[i] = null; }
        }

    }

    public void ShiftLeft()
    {
        if (items[0] != null && items[0].itemType != null && items[0].itemType.toolObject != null)
        {
            if (items[0].itemType.toolObject.GetComponent<AbstractTool>().IsUsed)
            {
                return;
            }
        }
        ItemInstance[] tmp = new ItemInstance[items.Length];
        items.CopyTo(tmp, 0);
        for (int i = 0; i < tmp.Length - 1; i++)
        {
            items[i] = tmp[i + 1];
        }
        items[items.Length - 1] = tmp[0];
    }

    public void ShiftRight()
    {
        if (items[0] != null && items[0].itemType != null && items[0].itemType.toolObject != null)
        {
            if (items[0].itemType.toolObject.GetComponent<AbstractTool>().IsUsed)
            {
                return;
            }
        }
        ItemInstance[] tmp = new ItemInstance[items.Length];
        items.CopyTo(tmp, 0);
        for (int i = 0; i < tmp.Length - 1; i++)
        {
            items[i + 1] = tmp[i];
        }
        items[0] = tmp[tmp.Length - 1];
    }

    private int generateNewId()
    {
        bool loop = true;
        int newId = 0;
        while (loop)
        {
            loop = false;
            newId = Random.Range(0, 99);
            for (int i = 0; i < maxItems && loop == true; i++)
            {
                if (items[i] != null)
                {
                    if (newId == items[i].id)
                    {
                        loop = true;
                    }
                }
                
            }
        }
        return newId;
        
    }
}