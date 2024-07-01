using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceItemContainer : AInteractable
{
    public ItemInstance item;
    protected bool push = false;
    protected bool change = false;
    public ItemInstance TakeItem()
    {
        Destroy(gameObject);
        return item;
    }

    public override bool interact()
    {
        UnityEngine.Debug.Log("Interact with dropped item - " + gameObject);
        item.itemType.Interaction();
        return false;
    }

    private void Update()
    {
        if(push)
        {
            GameObject player = GameObject.Find("Player");
            gameObject.GetComponent<Transform>().position = player.GetComponent<Transform>().position;

        }
        if (change && push)
        {
            GameObject.Find("Player").GetComponent<MovementComponent>().speed *= 0.5f;
            change = false;
        }
        if(change == true && push == false)
        {
            GameObject.Find("Player").GetComponent<MovementComponent>().speed *= 2.0f;
            change = false;
        }
    }

    public void Push()
    {
        push ^= true;
        change = true;
    }
}
