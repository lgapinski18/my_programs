using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Trap;

[CreateAssetMenu]
public class HealthPotionShopItem : ShopItem
{
    public override void BuyItem()
    {
        currentNumber -= 1;
    }

    public override void ApplyItem(GameObject player)
    {
        GameObject potion = Instantiate(Resources.Load<GameObject>("Prefabs/HealthPotion"));
        potion.transform.position = new Vector3(-6.3f, -37.3f, 0);
        //player.GetComponent<Player>().inventoryBar.GetComponent<InventoryDisplay>().PickUpItem(potion);
        Debug.Log("Applied health potion item");
    }
}
