using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CorpseManager : ScriptableObject
{


    public bool GenerateCorpse(float posX, float posY, Corpse.CorpseSize size, Corpse.CorpseType type, [Optional] Corpse.SmallCorpseSubType smallCorpseType)
    {
        GameObject corpse = Instantiate(Resources.Load<GameObject>("Prefabs/" + size.ToString() + type.ToString() + "Corpse"));
        ItemInstance corpseItem = corpse.GetComponent<InstanceItemContainer>().item;
        corpse.GetComponent<InstanceItemContainer>().item.itemType.isOnMap = true;
        corpse.GetComponent<InstanceItemContainer>().interactionMarkup.sortingLayerName = "Furniture";

        corpse.tag = "Interactable";

        // Set position for the new corpse
        corpse.transform.position = new Vector3(posX, posY, 0);
        return true;
    }
}
