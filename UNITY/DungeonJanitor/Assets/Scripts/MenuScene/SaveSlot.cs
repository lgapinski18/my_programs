using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public int SaveSlotId = -1;

    public GameObject EmptySavePanel;
    public GameObject SaveDataPanel;

    public Button NewGameButton;
    public Button LoadGameButton;

    private bool containsData = false;

    [Header("Ribbons")]
    public GameObject LeftRibbon;
    public GameObject RightRibbon;

    private bool isSelected = false;

    [Header("Texts")]
    public Text DateText;
    public Text ShiftNumberText;
    public Text RankNumberText;

    //Save data
    private SaveData saveData;
    /*private int shiftNo = 1;
    private int rankNo = 0;
    private int rankPoints = 0;
    bool[] RankTutorialDone = { false, false, true, false, true, true };*/


    public void LoadSaveData()
    {
        string filepath = Path.Combine(Application.persistentDataPath, "SaveSlot" + SaveSlotId);

        if (File.Exists(filepath))
        {
            SaveData data = SaveSerializer.LoadFromFile(filepath);
            SetSaveData(data);
        }
        else
        {
            SetSaveData();
        }
    }

    public void SetSaveData(SaveData savedata)
    {
        saveData = savedata;
        containsData = true;
        EmptySavePanel.SetActive(false);
        SaveDataPanel.SetActive(true);

        DateText.text = saveData.SaveDateTime;
        ShiftNumberText.text = saveData.ShiftNo.ToString();
        RankNumberText.text = saveData.RankNo.ToString();
        //SetSaveData(savedata.ShiftNo, savedata.SaveDateTime, savedata.RankNo, savedata.RankPoints, savedata.rankTutorialDone);
    }

    /*public void SetSaveData(int ShiftNo, string SaveDateTime, int RankNo, int RankPoints, bool[] rankTutorialDone)
    {
        containsData = true;
        EmptySavePanel.SetActive(false);
        SaveDataPanel.SetActive(true);

        DateText.text = SaveDateTime;
        ShiftNumberText.text = ShiftNo.ToString();
        RankNumberText.text = RankNo.ToString();

        shiftNo = ShiftNo;
        rankNo = RankNo;
        rankPoints = RankPoints;

        for (int i = 0; i < rankTutorialDone.Length; i++)
        {
            RankTutorialDone[i] = rankTutorialDone[i];
        }
    }*/

    public void SetSaveData()
    {
        saveData = new SaveData();
        containsData = false;
        EmptySavePanel.SetActive(true);
        SaveDataPanel.SetActive(false);
    }

    public void SetGameManagerData()
    {
        //GameManager.instance.SetData(SaveSlotId, shiftNo, rankNo, rankPoints, RankTutorialDone);
        GameManager.instance.SetData(SaveSlotId, saveData);
    }

    public bool IsSelected()
    {
        return isSelected;
    }


    public void SetSelected(bool value)
    {
        LeftRibbon.gameObject.SetActive(value);
        RightRibbon.gameObject.SetActive(value);

        NewGameButton.gameObject.SetActive(value);
        LoadGameButton.gameObject.SetActive(containsData && value);
    }
}
