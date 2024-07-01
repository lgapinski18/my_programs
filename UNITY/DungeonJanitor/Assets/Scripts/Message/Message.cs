using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public Image Icon;
    public Text AuthorText;
    public Text MessageText;


    public void SetImageIcon(Sprite image)
    {
        Icon.sprite = image;
    }

    public void SetAuthorText(string text)
    {
        AuthorText.text = text;
    }

    public void SetMessageText(string text)
    {
        MessageText.text = text;
    }
}
