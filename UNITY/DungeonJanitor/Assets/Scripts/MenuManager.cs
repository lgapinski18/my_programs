using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private MenuManager instance = null;
    private bool informedAboutConsequences = false;

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


    public MenuManager Instance()
    {
        return instance;
    }


    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }

        GameManager.instance.Pause(true);
        Show(true);
    }


    public void Show(bool show)
    {
        gameObject.SetActive(show);
        Cursor.visible = show;
    }

    public void Continue()
    {
        Show(false);
        GameManager.instance.Pause(false);
    }

    public void ReturnToMainMenu()
    {
        MessageManager.Instance().AddPrioritizedPopUp(PopUp.PopUpType.YESNO, () => {
            GameManager.instance.Pause(false);
            GameManager.instance.StopTimeCoroutine();
            SceneManager.LoadScene("MainScene");
        }, () => {
            GameManager.instance.Pause(true);
            Cursor.visible = true;
        },
        "UWAGA!", "Postêp gry zapisywany jest z pocz¹tkiem zmiany. Pozosta³e dane zostan¹ utracone.", MessageManager.Icons.DEVS_ICON, 400, 400);
        MessageManager.Instance().showPrioritizedPopUp(true);
        /*if (!informedAboutConsequences)
        {
            MessageManager.Instance().AddPrioritizedPopUp(PopUp.PopUpType.YESNO, () => { ReturnToMainMenu(); }, "UWAGA!", "Postêp gry zapisywany jest z pocz¹tkiem zmiany. Pozosta³e dane zostan¹ utracone.", MessageManager.Icons.DEVS_ICON);
            MessageManager.Instance().showPrioritizedPopUp(true);
            //MessageManager.Instance().AddNotification(MessageManager.Icons.DEVS_ICON, "Postêp gry zapisywany jest z pocz¹tkiem zmiany. Pozosta³e dane zostan¹ utracone.");
            informedAboutConsequences = true;
        }
        else
        {
            GameManager.instance.Pause(false);
            GameManager.instance.StopTimeCoroutine();
            SceneManager.LoadScene("MainScene");
        }*/
    }

    public void EndGame()
    {
        MessageManager.Instance().AddPrioritizedPopUp(PopUp.PopUpType.YESNO, () => {
            GameManager.instance.Pause(false);
            GameManager.instance.StopTimeCoroutine();
            Application.Quit(); 
        }, () => {
            GameManager.instance.Pause(true);
            Cursor.visible = true;
        },
        "UWAGA!", "Postêp gry zapisywany jest z pocz¹tkiem zmiany. Pozosta³e dane zostan¹ utracone.", MessageManager.Icons.DEVS_ICON, 400, 400);
        MessageManager.Instance().showPrioritizedPopUp(true);
        /*if (!informedAboutConsequences)
        {
            MessageManager.Instance().AddPrioritizedPopUp(PopUp.PopUpType.YESNO, () => { EndGame(); }, "UWAGA!", "Postêp gry zapisywany jest z pocz¹tkiem zmiany. Pozosta³e dane zostan¹ utracone.", MessageManager.Icons.DEVS_ICON, 260, 260);
            MessageManager.Instance().showPrioritizedPopUp(true);
            //MessageManager.Instance().AddNotification(MessageManager.Icons.DEVS_ICON, "Postêp gry zapisywany jest z pocz¹tkiem zmiany. Pozosta³e dane zostan¹ utracone.");
            informedAboutConsequences = true;
        }
        else
        {
            Application.Quit();
        }*/
    }
}
