using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthPotionItem : PickupItem
{
    public override bool UsePickUpItem()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.currentHealth = player.maxHealth;
        Debug.Log("Player used a health potion");
        player.healthBar.GetComponent<HealthDisplay>().UpdateDisplay(player.currentHealth, player.maxHealth);
        player.inventoryBar.GetComponent<InventoryDisplay>().DestroyItem(0);
        return true;
    }

}
