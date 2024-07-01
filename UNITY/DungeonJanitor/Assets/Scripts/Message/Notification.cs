using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public Image Icon;
    public Text NotificationText;

    public void SetImageIcon(Sprite image)
    {
        Icon.sprite = image;
    }

    public void SetNotificationText(string text) 
    {
        NotificationText.text = text;
    }
}
