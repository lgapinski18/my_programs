using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MaxHealthShopItem : ShopItem
{
    public override void BuyItem()
    {
        currentNumber -= 1;
    }

    public override void ApplyItem(GameObject player)
    {
        player.GetComponent<Player>().maxHealth += 1;
        Debug.Log("Applied max health item");
    }
}
