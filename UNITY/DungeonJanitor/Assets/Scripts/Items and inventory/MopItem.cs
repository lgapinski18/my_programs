using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class MopItem : ItemData
{
    //public GameObject toolObject;

    public void Awake()
    {
        toolObject = GameObject.Find("MopWeapon");
    }

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
        Debug.Log("Use mop");
        if (toolObject == null)
        {
            Awake();
        }
        //toolObject.SetActive(true);
        toolObject.GetComponent<AbstractTool>().Use();
        return false;
    }

    public override void ReloadItem()
    {
        toolObject = GameObject.Find("MopWeapon");
    }
}
