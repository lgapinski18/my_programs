using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class DetergentItem : ItemData
{
    //public GameObject toolObject;

    public void Awake()
    {
        toolObject = GameObject.Find(itemName + "_v2");
        //toolObject.SetActive(false);
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

        if (toolObject == null)
        {
            Awake();
        }
        if (currentDurability > 0)
        {
            //toolObject.SetActive(true);
            toolObject.GetComponent<ADetergent>().Use();
            //toolObject.GetComponent<ADetergent>().usesLeft--;
            //currentDurability = toolObject.GetComponent<ADetergent>().usesLeft;
            Debug.Log("Use detergent, uses left: " + currentDurability.ToString());

            //tool = toolObject.GetComponent<AbstractTool>();
            //tool.Use();
            return true;
        }
        else
        {
            Debug.Log("Detergent has no uses left");
            return false;
        }
    }

    public override void ReloadItem()
    {
        toolObject = GameObject.Find(itemName + "_v2");
    }


}
