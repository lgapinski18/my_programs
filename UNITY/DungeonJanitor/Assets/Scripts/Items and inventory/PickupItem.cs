using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

abstract public class PickupItem : ItemData 
{

    public override bool Interaction()
    {
        //if (isOnMap)
        {
            //UnityEngine.Debug.Log("Interact with dropped item - " + gameObject);
            GameObject inventoryBar = GameObject.Find("InventoryBar");
            GameObject player = GameObject.Find("Player");
            int index = player.GetComponent<Player>().selectedInterObj;
            inventoryBar.GetComponent<InventoryDisplay>().PickUpItem(player.GetComponent<Player>().InteractableObjects[index].gameObject);
            return true;
        }
        //return false;

    }

    public override bool UseItem(int currentDurability)
    {
        UsePickUpItem();
        return true;
    }

    public override void ReloadItem()
    {
    }

    abstract public bool UsePickUpItem();
}
