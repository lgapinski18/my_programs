using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMagicDoor : AInteractable
{
    public Transform TeleportationTargetPosition;
    public bool isWorking = true;

    public override bool interact()
    {
        if (isWorking)
        {
            GameObject player = GameObject.Find("Player");
            player.transform.position = TeleportationTargetPosition.position;
            GameObject camera = GameObject.Find("Camera");
            camera.transform.position = TeleportationTargetPosition.position;
            camera.transform.position += new Vector3(0, 0, -10);
        }
        else
        {
            MessageManager.Instance().AddNotification(MessageManager.Icons.BOOK_ICON, "Nie mo¿na u¿yæ drzwi!");
        }

        return false;
    }

    public void setWorking(bool b)
    {
        isWorking = b;
    }
}
