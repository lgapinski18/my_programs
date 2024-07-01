using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryUpgradeShopItem : ShopItem
{
    public override void BuyItem()
    {
        currentNumber -= 1;
    }

    public override void ApplyItem(GameObject player)
    {
        player.GetComponent<Player>().inventorySize += 1;
        Debug.Log("Applied inventory upgrade item");
    }
}
