using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu]
public class PickUpCorpse : ItemData
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
        Debug.Log("You cannot use this item");
        return false;
    }

    public override void ReloadItem()
    {
    }
}
