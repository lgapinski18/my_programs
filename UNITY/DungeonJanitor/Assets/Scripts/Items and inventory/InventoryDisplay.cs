using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class InventoryDisplay : MonoBehaviour
{
    public Inventory inventory;
    public List<Transform> inventorySlots;
    private GameObject interactionKey;

    public void Start()
    {
        for (int i = 0; i < inventory.maxItems; i++)
        {

            GameObject imgObject = new GameObject("InventorySlot");
            RectTransform trans = imgObject.AddComponent<RectTransform>();
            trans.transform.SetParent(transform);
            if (i == 0)
            {
                trans.localScale = new Vector3(1f, 1f, 1f);
                imgObject.AddComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Sprites/ItemSlot");
                imgObject.GetComponent<UnityEngine.UI.Image>().color = new UnityEngine.Color(0f, 1f, 0f, 1f);
            }
            else
            {
                trans.localScale = new Vector3(0.9f, 0.9f, 1.0f);
                imgObject.AddComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Sprites/ItemSlot");
            }


            GameObject icon = new GameObject("InventorySlotIcon");
            RectTransform iconTrans = icon.AddComponent<RectTransform>();
            iconTrans.transform.SetParent(imgObject.transform);
            iconTrans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            icon.AddComponent<UnityEngine.UI.Image>().color = new UnityEngine.Color(1f, 1f, 1f, 0f);

            GameObject durability = new GameObject("ItemDurability");
            RectTransform durabilityTrans = durability.AddComponent<RectTransform>();
            durabilityTrans.transform.SetParent(icon.transform);
            durabilityTrans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            durability.AddComponent<UnityEngine.UI.Text>().color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            durability.GetComponent<UnityEngine.UI.Text>().fontSize = 36;
            durability.GetComponent<UnityEngine.UI.Text>().alignment = TextAnchor.LowerRight;
            durability.GetComponent<UnityEngine.UI.Text>().font = Resources.Load<Font>("Fonts/alagard");
        }

        inventorySlots = GetChildren(transform);
        UpdateInventory();
    }

    void Update()
    {
        UpdateInventory();
    }

    void UpdateInventory()
    {
        GameObject slotIcon;
        UnityEngine.Color transparentColor = new UnityEngine.Color(1f, 1f, 1f, 0f);
        for (int i = inventorySlots.Count - 1; i > 0; i--)
        {
            slotIcon = GetChildren(inventorySlots[i])[0].gameObject;
            GameObject durability = GetChildren(slotIcon.transform)[0].gameObject;
            if (i <= inventory.items.Length && (inventory.items[i - 1] != null && inventory.items[i - 1].itemType != null))
            {
                UnityEngine.UI.Image img = slotIcon.GetComponent<UnityEngine.UI.Image>();
                img.sprite = inventory.items[i - 1].itemType.icon;
                img.color = UnityEngine.Color.white;

                if (inventory.items[i - 1].itemType.maxDurability > 0)
                {
                    //if (inventory.items[i - 1].GetDurability() == 0 && !inventory.items[i - 1].itemType.toolObject.GetComponent<AbstractTool>().IsUsed)
                    //{
                    //    inventory.items[i - 1].itemType.toolObject.SetActive(false);
                    //    inventory.RemoveItem(i - 1);
                    //}
                    durability.GetComponent<UnityEngine.UI.Text>().color = UnityEngine.Color.white;
                    durability.GetComponent<UnityEngine.UI.Text>().text = (inventory.items[i - 1].GetDurability().ToString() + "/" + inventory.items[i - 1].itemType.maxDurability.ToString());
                }
                else
                {
                    durability.GetComponent<UnityEngine.UI.Text>().color = transparentColor;
                }
                if (inventory.items[i - 1].itemType.maxDurability > 0 || inventory.items[i - 1].itemType.itemName == "Mop")
                {
                    if (inventory.items[i - 1].itemType.toolObject == null)
                    {
                        inventory.items[i - 1].itemType.ReloadItem();
                    }

                    if (i == 1)
                    {
                        inventory.items[i - 1].itemType.toolObject.SetActive(true);
                        int x = 0;
                    }
                    else
                    {
                        if (inventory.items[0] == null || inventory.items[0].itemType == null || inventory.items[0].itemType.toolObject == null || inventory.items[0].itemType.toolObject != inventory.items[i - 1].itemType.toolObject)
                        {
                            inventory.items[i - 1].itemType.toolObject.SetActive(false);
                        }
                        
                        int x = 0;
                    }
                }

            }
            else if (i <= inventorySlots.Count && (inventory.items[i - 1] == null || inventory.items[i - 1].itemType == null))
            {
                UnityEngine.UI.Image img = slotIcon.GetComponent<UnityEngine.UI.Image>();

                img.color = transparentColor;
                img.sprite = null;
                durability.GetComponent<UnityEngine.UI.Text>().color = transparentColor;
            }
        }
    }

    public void DropItem()
    {
        if (inventory.items[0] == null || inventory.items[0].itemType == null)
        {
            UnityEngine.Debug.Log("No item in hand to drop.");
        }
        else if (inventory.items[0].itemType.toolObject != null && inventory.items[0].itemType.toolObject.GetComponent<AbstractTool>().IsUsed)
        {
            return;
        }
        else
        {
            if (inventory.items[0].itemType.maxDurability > 0 || inventory.items[0].itemType.itemName == "Mop")
            {
                if (inventory.items[0].itemType.toolObject == null)
                {
                    inventory.items[0].itemType.ReloadItem();
                }

                inventory.items[0].itemType.toolObject.SetActive(false);
            }
            // Creates a new object and gives it the item data
            GameObject droppedItem = Instantiate(inventory.items[0].itemType.model);
            droppedItem.GetComponent<InstanceItemContainer>().item = inventory.items[0];
            droppedItem.tag = "Interactable";


            Corpse corpse = droppedItem.GetComponent<Corpse>();
            if (corpse != null)
            {
                CorpseCleaningTaskManager.Instance().DecrementNumberOfCorpses();
            }


            // Get player position
            GameObject player = GameObject.Find("Player");
            Vector3 playerPosition = player.transform.position;
            droppedItem.transform.position = playerPosition;

            // Removes the item from the inventory
            inventory.RemoveItem(0);

            // Updates the inventory again
            UpdateInventory();
        }

    }

    public void PickUpItem(GameObject itemToPickUp)
    {

        if (inventory.AddItem(itemToPickUp.GetComponent<InstanceItemContainer>().item))
        {
            itemToPickUp.GetComponent<InstanceItemContainer>().TakeItem();
        }
        itemToPickUp.GetComponent<InstanceItemContainer>().item.itemType.isOnMap = false;
    }

    public void UseItem()
    {
        if (inventory.items[0] != null && inventory.items[0].itemType != null)
        {
            inventory.items[0].UseItem();
        }
    }

    public void DestroyItem(int index)
    {
        inventory.RemoveItem(index);
    }

    List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }

        return children;
    }
}
