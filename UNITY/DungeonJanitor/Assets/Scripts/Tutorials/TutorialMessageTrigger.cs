using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessageTrigger : MonoBehaviour
{
    public enum InfoType { POPUP, MSG };
    private bool sent = false;

    public string[] HeadersOrAuthors = { };
    [TextAreaAttribute]
    public string[] Contents = { };

    [Header("Choose Show Message Option Type")]
    public InfoType[] InformationTypes = { };
    public Texture[] InformationImages = { };
    private int InformationImageId = 0;
    public MessageManager.Icons[] MessageImages = { };
    private int MessageImageId = 0;


    public bool ShowPopUpViaTutorialManager = false;
    public int[] PopUpInformationIds = { };

    public bool ShowMessageViaTutorialManager = false;
    public int[] MessageInformationIds = { };

    //public bool T

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("TMT");
        //Debug.Log(other.tag);
        //Debug.Log(!sent);


        if ((other.tag == "Player") && !sent)
        {
            //Debug.Log("!1");
            InformationImageId = 0;
            MessageImageId = 0;

            for (int i = 0; i < HeadersOrAuthors.Length; i++)
            {
                //Debug.Log("!2");
                if (InformationTypes[i] == InfoType.POPUP)
                {
                    MessageManager.Instance().AddPopUp(HeadersOrAuthors[i], Contents[i], InformationImages[InformationImageId++]);
                    MessageManager.Instance().showMessage(true);
                }
                else
                {
                    //Debug.Log("!3");
                    MessageManager.Instance().AddMessage(MessageImages[MessageImageId++], HeadersOrAuthors[i], Contents[i]);
                    MessageManager.Instance().showMessage(true);
                }
            }


            if (ShowPopUpViaTutorialManager)
            {
                foreach (int i in PopUpInformationIds)
                {
                    TutorialManager.Instance().ShowInformationPopUp(i);
                }
                
            }


            if (ShowMessageViaTutorialManager)
            {
                foreach (int i in MessageInformationIds)
                {
                    TutorialManager.Instance().ShowDialogMessage(i);
                }
            }

            sent = true;

            if (sent)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TutorialManualTrigger()
    {
        //Debug.Log("TMT");
        //Debug.Log(other.tag);
        //Debug.Log(!sent);


        if (!sent)
        {
            //Debug.Log("!1");
            InformationImageId = 0;
            MessageImageId = 0;

            for (int i = 0; i < HeadersOrAuthors.Length; i++)
            {
                //Debug.Log("!2");
                if (InformationTypes[i] == InfoType.POPUP)
                {
                    MessageManager.Instance().AddPopUp(HeadersOrAuthors[i], Contents[i], InformationImages[InformationImageId++]);
                    MessageManager.Instance().showMessage(true);
                }
                else
                {
                    //Debug.Log("!3");
                    MessageManager.Instance().AddMessage(MessageImages[MessageImageId++], HeadersOrAuthors[i], Contents[i]);
                    MessageManager.Instance().showMessage(true);
                }
            }


            if (ShowPopUpViaTutorialManager)
            {
                foreach (int i in PopUpInformationIds)
                {
                    TutorialManager.Instance().ShowInformationPopUp(i);
                }

            }


            if (ShowMessageViaTutorialManager)
            {
                foreach (int i in MessageInformationIds)
                {
                    TutorialManager.Instance().ShowDialogMessage(i);
                }
            }

            sent = true;

            if (sent)
            {
                Destroy(gameObject);
            }
        }
    }
}
