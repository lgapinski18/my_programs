using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BootsShopItem : ShopItem
{
    public override void BuyItem()
    {
        currentNumber -= 1;
    }

    public override void ApplyItem(GameObject player)
    {
        player.GetComponent<Player>().movementComponent.originSpeed *= 1.5f;
        player.GetComponent<Player>().movementComponent.speed *= 1.5f;
        Debug.Log("Applied boots upgrade");
    }
}
