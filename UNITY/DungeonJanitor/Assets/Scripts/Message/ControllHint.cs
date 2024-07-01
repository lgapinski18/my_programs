using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllHint : MonoBehaviour
{
    public Image image;
    public Text controllDescription;

    public void SetHintData(Sprite sprite, string hintText)
    {
        image.sprite = sprite;
        controllDescription.text = hintText;
    }
}
