using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class PushableCorpse : ItemData
{
    public override bool Interaction()
    {
        GameObject inventoryBar = GameObject.Find("InventoryBar");
        GameObject player = GameObject.Find("Player");
        int index = player.GetComponent<Player>().selectedInterObj;
        player.GetComponent<Player>().InteractableObjects[index].gameObject.GetComponent<InstanceItemContainer>().Push();
        return true;
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
