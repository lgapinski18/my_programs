using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public enum PopUpType { YESNO, OK };

    public Text Header = null;
    public Text TextContent = null;
    public RawImage InformationImage = null;

    public Button YesButton;
    public Button NoButton;
    public Button OkButton;

    private UnityAction yesAction = () => { };
    private UnityAction noAction = () => { };


    public void YesAction()
    {
        GameManager.instance.Pause(false);
        Cursor.visible = false;
        yesAction();
        //InformationImage.sprite.texture.GetPixel(0, 0).;
    }

    public void NoAction()
    {
        GameManager.instance.Pause(false);
        Cursor.visible = false;
        MessageManager.Instance().NextMessage();
        noAction();
        //InformationImage.sprite.texture.GetPixel(0, 0).;
    }

    public void DoNothing()
    {
        Cursor.visible = false;
        MessageManager.Instance().NextMessage();
    }

    public void SetYesAction(UnityAction yes_action)
    {
        yesAction = yes_action;
    }

    public void SetNoAction(UnityAction no_action)
    {
        noAction = no_action;
    }

    public void SetData(PopUpType type, string header, string text, Texture informationTexture, int width, int height)
    {
        Header.text = header;
        TextContent.text = text;

        switch (type)
        {
            case PopUpType.YESNO:
                YesButton.gameObject.SetActive(true);
                NoButton.gameObject.SetActive(true);
                OkButton.gameObject.SetActive(false);
                break;

            case PopUpType.OK:
                YesButton.gameObject.SetActive(false);
                NoButton.gameObject.SetActive(false);
                OkButton.gameObject.SetActive(true);
                break;
        }

        if (informationTexture != null)
        {
            InformationImage.gameObject.SetActive(true);

            if (width == 0 || height == 0)
            {
                //float w = 260 * informationTexture.height / informationTexture.width;// / informationTexture.height;
                float h = InformationImage.rectTransform.rect.width * informationTexture.height / informationTexture.width;// / informationTexture.height;
                InformationImage.rectTransform.sizeDelta = new Vector2(InformationImage.rectTransform.rect.width, h);
                //InformationImage.rectTransform.sizeDelta = new Vector2(informationImage.rect.width, informationImage.rect.height);
            }
            else
            {
                InformationImage.rectTransform.sizeDelta = new Vector2(width, height);
            }

            InformationImage.texture = informationTexture;
        }
        else
        {
            InformationImage.gameObject.SetActive(false);
        }
    }
}
