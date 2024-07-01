using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance = null;

    public GameManager GameManagerPrefab;

    public GameObject MainMenuPanel;
    public GameObject SaveLoadPanel;

    [Header("SaveSlots")]
    public SaveSlot[] SaveSlots = new SaveSlot[3];

    public int SelectedSaveSlot = -1;

    public string GameSceneName = "SampleScene";


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Cursor.visible = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static MainMenuManager Instance()
    {
        return instance;
    }


    public void LoadSaves()
    {
        SaveSlots[0].LoadSaveData();
        SaveSlots[1].LoadSaveData();
        SaveSlots[2].LoadSaveData();

        /*SaveSlots[0].SetSaveData();
        SaveSlots[1].SetSaveData(2, "22.11.2023  23:15", 1, 250, rankTutorialDone);
        SaveSlots[2].SetSaveData();*/
    }


    public void NewGame()
    {
        bool[] rankTutorialDone = { false, false, false, true, true, true };
        SaveData saveData = new SaveData();
        saveData.ShiftNo = 1;
        saveData.SaveDateTime = DateTime.Now.ToString();
        saveData.RankNo = 0;
        saveData.RankPoints = 0;
        saveData.rankTutorialDone = rankTutorialDone;

        SaveSlots[SelectedSaveSlot].SetSaveData(saveData);

        LoadGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        Instantiate(GameManagerPrefab);
        SaveSlots[SelectedSaveSlot].SetGameManagerData();

        Cursor.visible = false;

        Debug.Log("Load Scene");
        //SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
        GameManager.instance.NextShift();
    }

    public void GoToSaveLoadMenu()
    {
        LoadSaves();

        SaveLoadPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SaveLoadPanel.SetActive(false);

        if (SelectedSaveSlot >= 0)
        {
            SaveSlots[SelectedSaveSlot].SetSelected(false);
        }
        SelectedSaveSlot = -1;
    }

    public void SetSelectedSlot(int slotId)
    {
        SelectedSaveSlot = slotId;
    }
}
