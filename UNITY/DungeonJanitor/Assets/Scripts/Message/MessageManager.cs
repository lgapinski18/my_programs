using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MessageManager : MonoBehaviour
{
    private static MessageManager instance = null;

    public Notification notification;
    public Message message;
    public PopUp popUp;
    public PopUp PrioritizedPopUp;
    public int MessageLineLength = 65;
    public int PopUpLineLength = 32;

    [Header("Icons Sprites")]
    public Sprite HoodedIcon;
    public Sprite BookIcon;
    public Sprite DevsIcon;

    public enum Icons { NONE, HOODED_ICON, BOOK_ICON, DEVS_ICON };
    private enum MessageType { DIALOG, POPUP, NONE };
    private MessageType activeMessageType = MessageType.NONE;

    private Queue<KeyValuePair<Icons, string>> NotificationQueue = new Queue<KeyValuePair<Icons, string>>();
    private Queue<Tuple<Icons, string, string, MessageType>> MessageQueue = new Queue<Tuple<Icons, string, string, MessageType>>();
    private Queue<Tuple<PopUp.PopUpType, UnityAction, Texture, int, int>> AdditionalPopUpData = new Queue<Tuple<PopUp.PopUpType, UnityAction, Texture, int, int>>();

    private Queue<Tuple<PopUp.PopUpType, UnityAction, string, string, Texture, int, int>> PrioritizedPopUpQueue = new Queue<Tuple<PopUp.PopUpType, UnityAction, string, string, Texture, int, int>>();
    private Queue<UnityAction> NoActions = new Queue<UnityAction>();


    private bool notificationsAreDisplayed = false;
    //private bool messageIsVisible = false;

    private object notificationLock = new object();


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static MessageManager Instance()
    {
        return instance;
    }

    private Sprite getIconSprite(Icons icon)
    {
        switch (icon)
        {
            case Icons.HOODED_ICON:
                return HoodedIcon;

            case Icons.BOOK_ICON:
                return BookIcon;

            case Icons.DEVS_ICON:
                return DevsIcon;

            default:
                return DevsIcon;
        }
    }

    private IEnumerator DisplayNotification()
    {
        KeyValuePair<Icons, string> ntf;

        while (NotificationQueue.Count > 0)
        {
            lock (notificationLock)
            {
                ntf = NotificationQueue.Dequeue();
            }

            notification.SetImageIcon(getIconSprite(ntf.Key));

            notification.SetNotificationText(ntf.Value);

            notification.gameObject.SetActive(true);
            yield return new WaitForSeconds(2.7f);

            notification.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }

        notificationsAreDisplayed = false;

        yield return null;
    }


    public void AddNotification(Icons icon, string notificationMessage)
    {
        lock (notificationLock)
        {
            NotificationQueue.Enqueue(new KeyValuePair<Icons, string>(icon, notificationMessage));
        }

        if (!notificationsAreDisplayed)
        {
            notificationsAreDisplayed = true;
            StartCoroutine(DisplayNotification());
        }
    }

    #region MESSAGE_AND_POPUP

    public void AddMessage(Icons icon, string author, string message)
    {
        string text = "";
        string line = "";

        int numberOfRows = 0;

        Queue<string> words = new Queue<string>(message.Split(' '));

        while (words.Count > 0)
        {
            line += words.Dequeue();

            while (numberOfRows < 4)
            {
                if (words.Count == 0)
                {
                    break;
                }

                if ((line.Length + words.Peek().Length + 1) <= MessageLineLength)
                {
                    line += " ";
                    line += words.Dequeue();
                }
                else
                {
                    text += line + "\n";
                    numberOfRows += 1;
                    line = "";

                    if (words.Count > 0 && numberOfRows < 4)
                    {
                        line += words.Dequeue();
                    }
                }
            }

            if (line.Length > 0)
            {
                text += line;
            }

            numberOfRows = 0;

            MessageQueue.Enqueue(Tuple.Create<Icons, string, string, MessageType>(icon, author, text, MessageType.DIALOG));

            text = "";
        }
    }

    public void AddPopUp(string header, string msg, Icons icon, int width = 0, int height = 0)
    {
        AddPopUp(PopUp.PopUpType.OK, () => { }, header, msg, getIconSprite(icon).texture, width, height);
    }

    public void AddPopUp(PopUp.PopUpType type, string header, string msg, Icons icon, int width = 0, int height = 0)
    {
        AddPopUp(type, () => { }, header, msg, getIconSprite(icon).texture, width, height);
    }

    public void AddPopUp(PopUp.PopUpType type, UnityAction yesAction, string header, string msg, Icons icon, int width = 0, int height = 0)
    {
        AddPopUp(type, yesAction, header, msg, getIconSprite(icon).texture, width, height);
    }

    public void AddPopUp(string header, string msg, Texture informationImage = null, int width = 0, int height = 0)
    {
        AddPopUp(PopUp.PopUpType.OK, () => { }, header, msg, informationImage, width, height);
    }

    public void AddPopUp(PopUp.PopUpType type, string header, string msg, Texture informationImage = null, int width = 0, int height = 0)
    {
        AddPopUp(type, () => { }, header, msg, informationImage, width, height);
    }

    public void AddPopUp(PopUp.PopUpType type, UnityAction yesAction, string header, string msg, Texture informationImage = null, int width = 0, int height = 0)
    {
        string text = "";
        string line = "";

        int numberOfRows = 0;

        Queue<string> words = new Queue<string>(msg.Split(' '));

        while (words.Count > 0)
        {
            line += words.Dequeue();

            while (numberOfRows < 4)
            {
                if (words.Count == 0)
                {
                    break;
                }

                if ((line.Length + words.Peek().Length + 1) <= PopUpLineLength)
                {
                    line += " ";
                    line += words.Dequeue();
                }
                else
                {
                    text += line + "\n";
                    numberOfRows += 1;
                    line = "";

                    if (words.Count > 0 && numberOfRows < 6)
                    {
                        line += words.Dequeue();
                    }
                }
            }

            if (line.Length > 0)
            {
                text += line;
            }

            numberOfRows = 0;

            MessageQueue.Enqueue(Tuple.Create<Icons, string, string, MessageType>(Icons.NONE, header, text, MessageType.POPUP));
            AdditionalPopUpData.Enqueue(Tuple.Create<PopUp.PopUpType, UnityAction, Texture, int, int>(type, yesAction, informationImage, width, height));

            line = "";
            text = "";
        }
    }

    public void showMessage(bool v)
    {
        if ((activeMessageType == MessageType.NONE) && v)
        {
            NextMessage();
        }

        if (!v)
        {
            activeMessageType = MessageType.NONE;
        }

        showMessage(v, activeMessageType);
    }

    private void showMessage(bool v, MessageType type)
    {
        switch (type)
        {
            case MessageType.DIALOG:
                GameManager.instance.Pause(true);
                message.gameObject.SetActive(v);
                break;

            case MessageType.POPUP:
                GameManager.instance.Pause(true);
                popUp.gameObject.SetActive(v);
                Cursor.visible = v;
                break;

            default:
                break;
        }
    }

    public void OnNextMessage(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }

        NextMessage();
    }

    private bool end = false;
    public void NextMessage()
    {
        GameManager.instance.Pause(false);
        //Cursor.visible = false;
        showPrioritizedPopUp(false);

        if (end)
        {
            end = false;
            return;
        }

        if (PrioritizedPopUpQueue.Count > 0)
        {
            Tuple<PopUp.PopUpType, UnityAction, string, string, Texture, int, int> popup = PrioritizedPopUpQueue.Dequeue();
            UnityAction noAction = NoActions.Dequeue();

            PrioritizedPopUp.SetData(popup.Item1, popup.Item3, popup.Item4, popup.Item5, popup.Item6, popup.Item7);
            PrioritizedPopUp.SetYesAction(popup.Item2);
            PrioritizedPopUp.SetNoAction(noAction);

            //Cursor.visible = true;
            showPrioritizedPopUp(true);

            if (PrioritizedPopUpQueue.Count == 0)
            {
                end = true;
            }
        }
        /*else
        {
            prioritizedPopUpVisible = false;
        }/**/


        if (prioritizedPopUpVisible || GameManager.instance.IsPaused())
        {
            return;
        }


        showMessage(false, activeMessageType);

        if (MessageQueue.Count > 0)
        {
            Tuple<Icons, string, string, MessageType> msg = MessageQueue.Dequeue();
            activeMessageType = msg.Item4;

            Debug.Log(activeMessageType);

            switch (activeMessageType)
            {
                case MessageType.DIALOG:
                    NextDialogMessage(msg);
                    break;

                case MessageType.POPUP:
                    NextPopUpMessage(msg);
                    break;

                default:
                    break;
            }

            showMessage(true, activeMessageType);
        }
        else
        {
            activeMessageType = MessageType.NONE;
            GameManager.instance.Pause(false);
        }
    }

    void NextDialogMessage(Tuple<Icons, string, string, MessageType> msg)
    {
        message.SetImageIcon(getIconSprite(msg.Item1));
        message.SetAuthorText(msg.Item2);
        message.SetMessageText(msg.Item3);
    }

    void NextPopUpMessage(Tuple<Icons, string, string, MessageType> msg)
    {
        Tuple<PopUp.PopUpType, UnityAction, Texture, int, int> spriteTuple = AdditionalPopUpData.Dequeue();

        popUp.SetData(spriteTuple.Item1, msg.Item2, msg.Item3, spriteTuple.Item3, spriteTuple.Item4, spriteTuple.Item5);
        popUp.SetYesAction(spriteTuple.Item2);
    }

    #endregion MESSAGE_AND_POPUP


    #region PRIORITIZED_POPUP

    private bool prioritizedPopUpVisible = false;

    public void AddPrioritizedPopUp(string header, string msg, Icons icon, int width = 0, int height = 0)
    {
        AddPrioritizedPopUp(PopUp.PopUpType.OK, () => { }, () => { }, header, msg, getIconSprite(icon).texture, width, height);
    }

    public void AddPrioritizedPopUp(PopUp.PopUpType type, string header, string msg, Icons icon, int width = 0, int height = 0)
    {
        AddPrioritizedPopUp(type, () => { }, () => { }, header, msg, getIconSprite(icon).texture, width, height);
    }

    public void AddPrioritizedPopUp(PopUp.PopUpType type, UnityAction yesAction, string header, string msg, Icons icon, int width = 0, int height = 0)
    {
        AddPrioritizedPopUp(type, yesAction, () => { }, header, msg, getIconSprite(icon).texture, width, height);
    }

    public void AddPrioritizedPopUp(string header, string msg, Texture informationImage = null, int width = 0, int height = 0)
    {
        AddPrioritizedPopUp(PopUp.PopUpType.OK, () => { }, () => { }, header, msg, informationImage, width, height);
    }

    public void AddPrioritizedPopUp(PopUp.PopUpType type, string header, string msg, Texture informationImage = null, int width = 0, int height = 0)
    {
        AddPrioritizedPopUp(type, () => { }, () => { }, header, msg, informationImage, width, height);
    }

    public void AddPrioritizedPopUp(PopUp.PopUpType type, UnityAction yesAction, UnityAction noAction, string header, string msg, Icons icon, int width = 0, int height = 0)
    {
        AddPrioritizedPopUp(type, yesAction, noAction, header, msg, getIconSprite(icon).texture, width, height);
    }

    public void AddPrioritizedPopUp(PopUp.PopUpType type, UnityAction yesAction, UnityAction noAction, string header, string msg, Texture informationImage = null, int width = 0, int height = 0)
    {
        string text = "";
        string line = "";

        int numberOfRows = 0;

        Queue<string> words = new Queue<string>(msg.Split(' '));

        while (words.Count > 0)
        {
            line += words.Dequeue();

            while (numberOfRows < 4)
            {
                if (words.Count == 0)
                {
                    break;
                }

                if ((line.Length + words.Peek().Length + 1) <= PopUpLineLength)
                {
                    line += " ";
                    line += words.Dequeue();
                }
                else
                {
                    text += line + "\n";
                    numberOfRows += 1;
                    line = "";

                    if (words.Count > 0 && numberOfRows < 6)
                    {
                        line += words.Dequeue();
                    }
                }
            }

            if (line.Length > 0)
            {
                text += line;
            }

            numberOfRows = 0;

            PrioritizedPopUpQueue.Enqueue(Tuple.Create<PopUp.PopUpType, UnityAction, string, string, Texture, int, int>(type, yesAction, header, text, informationImage, width, height));//Tuple.Create<int>(height)
            NoActions.Enqueue(noAction);

            line = "";
            text = "";
        }
    }

    public void showPrioritizedPopUp(bool v)
    {
        if (!prioritizedPopUpVisible && v)
        {
            prioritizedPopUpVisible = v;
            NextMessage();
        }

        prioritizedPopUpVisible = v;

        Cursor.visible = v;
        GameManager.instance.Pause(v);
        PrioritizedPopUp.gameObject.SetActive(v);
    }

    #endregion PRIORITIZED_POPUP
}
