using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance = null;


    [Header("PopUps Content")]
    private bool[] InformationSent = { };
    public string[] InformationHeaders = { };
    public string[] InformationContent = { };
    public Texture[] InformationImages = { };
    //public Tuple<string, string, Sprite>[] PopUpContent = { }; 

    [Header("Messages Content")]
    private bool[] MessageSent = { };
    public string[] MessageHeaders = { };
    public string[] MessageContent = { };
    public MessageManager.Icons[] MessageImages = { };


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            InformationSent = new bool[InformationHeaders.Length];
            for (int i = 0; i < InformationSent.Length; i++)
            {
                InformationSent[i] = false;
            }

            MessageSent = new bool[MessageHeaders.Length];
            for (int i = 0; i < MessageSent.Length; i++)
            {
                MessageSent[i] = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static TutorialManager Instance()
    {
        return instance;
    }


    public void ShowInformationPopUp(int informationIndex)
    {
        if (informationIndex < InformationSent.Length && informationIndex >= 0)
        {
            if (!InformationSent[informationIndex])
            {
                InformationSent[informationIndex] = true;
                MessageManager.Instance().AddPopUp(InformationHeaders[informationIndex], InformationContent[informationIndex], InformationImages[informationIndex]);
                MessageManager.Instance().showMessage(true);
            }
        }
    }

    public void ShowDialogMessage(int messageIndex)
    {
        if (messageIndex < InformationSent.Length && messageIndex >= 0)
        {
            if (!InformationSent[messageIndex])
            {
                MessageSent[messageIndex] = true;
                MessageManager.Instance().AddMessage(MessageImages[messageIndex], MessageHeaders[messageIndex], MessageContent[messageIndex]);
                MessageManager.Instance().showMessage(true);
            }
        }
    }
}
