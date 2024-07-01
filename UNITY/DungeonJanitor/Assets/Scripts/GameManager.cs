using UnityEngine;
using System.Collections;
using System.Collections.Generic;        //Allows us to use Lists. 
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private int SaveSlotId = -1; /////////To save
    public Generator generator = null;
    [Header("Shift data")]
    public GameObject ShiftPanel = null;
    public ShiftTimeManager ShiftTimePanel = null;
    public int DurationOfShiftInSeconds = 600;

    [Header("Player Spawning Data")]
    public Player PlayerInstance = null;
    public Transform SpawnPositionPointer = null;
    public CameraControll Camera_Controll = null;

    [Header("Task Data")]
    public int ShiftNo = 0; /////////To save
    private static int ShiftNoControl = 0;
    public Taskboard taskboard = null;

    //[Header("Message Data")]
    //public MessageManager messageManager = null;

    public int Rank = 0; /////////To save
    private int NumberOfRankPoints = 0; /////////To save
    private int ShiftEarnedPoints = 0;
    public int money = 0;
    public bool[] RankTutorialDone = { false, false, false, true, true, true }; ///////To save
    public int shopCounter = 0;
    public List<ShopItem> boughtItems;

    List<ASummaryEntry> summaryEntries;
    private Coroutine FinishShiftCoroutine = null;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        //InitGame();
    }

    private string realoadSceneName;
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.isLoaded)
        {
            SortedSet<string> GameScaneNames = new SortedSet<string>();
            GameScaneNames.Add("SampleScene");
            GameScaneNames.Add("Rank0Tutorial");
            GameScaneNames.Add("Rank1Tutorial");
            GameScaneNames.Add("Rank2Tutorial");
            //GameScaneNames.Add("Rank3Tutorial");
            //GameScaneNames.Add("Rank4Tutorial");
            //GameScaneNames.Add("Rank5Tutorial");

            if (GameScaneNames.Contains(scene.name) && (ShiftNo == ShiftNoControl))
            {
                InitGame();
            }
            else if (scene.name == "ShiftSummary")
            {
                if (summaryEntries.Count > 0)
                {
                    SetUpSummary();
                }
            }
            else if (scene.name == "ReloadScene")
            {
                //StartCoroutine("ReloadCoroutine");
                SceneManager.LoadScene(realoadSceneName);
            }
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(realoadSceneName);
        yield return null;
    }

    #region TaskMethods
    void InitGame()
    {
        Debug.Log("Initiating shift");
        ShiftPanel = GameObject.Find("Player").GetComponent<Player>().ShiftNumberPanel;
        ShiftTimePanel = GameObject.Find("Player").GetComponent<Player>().TimePanel;
        PlayerInstance = GameObject.Find("Player").GetComponent<Player>();
        SpawnPositionPointer = GameObject.Find("SpawnPlayerPositionPointer").GetComponent<Transform>();
        Camera_Controll = GameObject.Find("Camera").GetComponent<CameraControll>();

        if (GameObject.Find("Map generator") != null)
        {
            generator = GameObject.Find("Map generator").GetComponent<Generator>();
        }
        //messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();
        taskboard = Taskboard.instance;
        /*if (ShiftPanel == null)
        {
            ShiftPanel = GameObject.Find("Player").GetComponent<Player>().ShiftNumberPanel;
            ShiftTimePanel = GameObject.Find("Player").GetComponent<Player>().TimePanel;
            PlayerInstance = GameObject.Find("Player").GetComponent<Player>();
            SpawnPositionPointer = GameObject.Find("SpawnPlayerPositionPointer").GetComponent<Transform>();
            Camera_Controll = GameObject.Find("Camera").GetComponent<CameraControll>();
            //messageManager = GameObject.Find("MessageManager").GetComponent<MessageManager>();
            taskboard = Taskboard.instance;
        }/**/
        ShiftPanel.SetActive(true);

        ShiftNo += 1;
        if (ShiftNo == (ShiftNoControl + 1))
        {
            ShiftNoControl = ShiftNo;
        }
        GameObject.Find("ShiftNumberPanel").GetComponentInChildren<Text>().text = ShiftNo.ToString();
        BeginShift();
    }

    private IEnumerator HideShiftPanel()
    {
        bool paused = IsPaused();
        Pause(false);
        Pauseable = false;
        toPause = paused;

        yield return new WaitForSeconds(3.0f);

        Pauseable = true;

        ShiftPanel.SetActive(false);

        FinishShiftCoroutine = StartCoroutine(CountDownTime());

        yield return null;
    }

    private IEnumerator CountDownTime()
    {
        ShiftTimePanel.NumberOfSeconds = DurationOfShiftInSeconds;

        ShiftTimePanel.SetShiftNumber(ShiftNo);

        switch ((ShiftNo - 1) % 4)
        {
            case 0:
                ShiftTimePanel.SetDayTimeSprite(ShiftTimeManager.DayTime.DAWN);
                break;

            case 1:
                ShiftTimePanel.SetDayTimeSprite(ShiftTimeManager.DayTime.DAY);
                break;

            case 2:
                ShiftTimePanel.SetDayTimeSprite(ShiftTimeManager.DayTime.TWILIGHT);
                break;

            case 3:
                ShiftTimePanel.SetDayTimeSprite(ShiftTimeManager.DayTime.NIGHT);
                break;
        }

        while (ShiftTimePanel.NumberOfSeconds >= 0)
        {
            ShiftTimePanel.UpdateTime();
            yield return new WaitForSeconds(1.0f);
        }

        FinishShiftCoroutine = null;

        FinishShift();

        yield return null;
    }

    private void SaveCheckpoint()
    {
        Debug.Log("Saving Checkpoint");
        SaveData data = new SaveData();
        data.ShiftNo = ShiftNo;
        data.SaveDateTime = DateTime.Now.ToString();
        data.RankNo = Rank;
        data.RankPoints = NumberOfRankPoints;
        data.rankTutorialDone = RankTutorialDone;
        data.gRoom_num = gRoom_num;
        data.gSize = gSize;
        data.gNumberOfStains = gNumberOfStains;
        data.gNumberOfBodyParts = gNumberOfBodyParts;
        data.Money = money;
        SaveSerializer.SaveToFile("SaveSlot" + SaveSlotId, data);
    }

    public void BeginShift()
    {
        Debug.Log("Shift has begun!");

        SaveCheckpoint();

        PlayerInstance.gameObject.transform.position = SpawnPositionPointer.position;
        Camera_Controll.gameObject.transform.position = PlayerInstance.gameObject.transform.position;
        Camera_Controll.gameObject.transform.position += new Vector3(0, 0, -10);

        //shift duration
        switch (Rank)
        {
            case 0:
                DurationOfShiftInSeconds = 480;
                break;

            case 1:
            case 2:
                DurationOfShiftInSeconds = 600;
                break;

            case 3:
            case 4:
            case 5:
                DurationOfShiftInSeconds = 720;
                break;
        }

        StartCoroutine(HideShiftPanel());

        if (generator != null)
        {
            generateDungeon();
        }

        taskboard.InitTasks(Rank);


        /*
        if (RankTutorialDone[Rank])
        {
            MessageManager.Instance().AddMessage(MessageManager.Icons.HOODED_ICON, "Menad¿er", "Witaj spowrotem woŸny! Trochê siê naba³agani³o od ostaniej zmiany. Mam nadziejê, ¿e siê szybko z tym uwiniesz..");
            MessageManager.Instance().showMessage(true);
        }/**/
    }

    private float gRoom_num = 5;
    private float gSize = 3;
    private float gNumberOfBodyParts = 20;
    private float gNumberOfStains = 12;
    private void generateDungeon()
    {
        System.Random rnd = new System.Random();
        generator.InitGenerator();

        generator.Room_num = (int)gRoom_num;
        generator.Size = (int)gSize;
        generator.Min_room_size = generator.Size - 1;
        generator.Max_room_size = generator.Size + 1;
        generator.GenerateRooms();
        
        gRoom_num += 0.6f;
        gSize += 0.4f;

        generator.GenerateCorridors();
        generator.GenerateWalls();
        generator.GenerateMagicDoor();

        //stains generation
        for (int i = 0; i < generator.Stains_counts.Length; i++)
        {
            generator.Stains_counts[i] = 0;
        }

        for (int i = 0; i < gNumberOfStains; i++)
        {
            generator.Stains_counts[rnd.Next(generator.Stains_counts.Length)] += 1;
        }
        generator.GenerateStains();
        gNumberOfStains += 1;

        //monster generation
        if (Rank >= 2)
        {
            generator.Monsters_counts[0] = Rank - 2;
            generator.Monsters_counts[1] = Rank - 2;
            generator.InstancePathFinderNodes();
            generator.GenerateMonsters();
        }

        //body generation
        if (Rank >= 1)
        {
            for (int i = 0; i < generator.Bodies_counts.Length; i++)
            {
                generator.Bodies_counts[i] = 0;
            }

            for (int i = 0; i < gNumberOfBodyParts; i++)
            {
                generator.Bodies_counts[rnd.Next(generator.Bodies_counts.Length)] += 1;
            }

            generator.GenerateBodies();

            gNumberOfBodyParts += 1;

            generator.Traps_counts[0] = generator.Room_num;
            generator.Traps_counts[1] = generator.Room_num;
            generator.GenerateTraps();
        }


        generator.GenerateMinimap();
    }

    public void StopTimeCoroutine()
    {
        if (FinishShiftCoroutine != null)
        {
            StopCoroutine(FinishShiftCoroutine);
        }
    }

    private bool IsDying = false;
    public void FinishShift(bool isDying = false)
    {
        IsDying = isDying;

        Debug.Log("Shift has ended!");
        //Debug.Log(IsDying);

        StopTimeCoroutine();

        if (isDying && !RankTutorialDone[Rank])
        {
            ShiftNo -= 1;
            switch (Rank)
            {
                case 0:
                    realoadSceneName = "Rank0Tutorial";
                    ShiftNoControl -= 1;
                    SceneManager.LoadScene("ReloadScene", LoadSceneMode.Single);
                    break;

                case 1:
                    realoadSceneName = "Rank1Tutorial";
                    ShiftNoControl -= 1;
                    SceneManager.LoadScene("ReloadScene", LoadSceneMode.Single);
                    break;

                case 3:
                    realoadSceneName = "Rank2Tutorial";
                    ShiftNoControl -= 1;
                    SceneManager.LoadScene("ReloadScene", LoadSceneMode.Single);
                    break;

                default:
                    break;
            }
        }
        else
        {
            summaryEntries = taskboard.SummarizeTasks(Rank);

            NumberOfRankPoints = ShiftEarnedPoints;
            /*foreach (ASummaryEntry summaryEntry in summaryEntries)
            {
                ShiftEarnedPoints += summaryEntry.NumberOfPoints;
            }*/
            ShiftEarnedPoints += 51;

            //ShiftEarnedPoints += NumberOfRankPoints;
            if (ShiftEarnedPoints < 0)
            {
                ShiftEarnedPoints = 0;
            }
            money += ShiftEarnedPoints - NumberOfRankPoints;

            if (!IsDying)
            {
                if (!RankTutorialDone[Rank])
                {
                    RankTutorialDone[Rank] = true;
                }
            }

            SceneManager.LoadScene("ShiftSummary");
        }
    }

    public void SetUpSummary()
    {
        Cursor.visible = true;
        foreach (ASummaryEntry summaryEntry in summaryEntries)
        {
            TaskSummaryPanel.Instance().AddTaskSummaryEntry(summaryEntry);
        }

        summaryEntries.Clear();

        int prevRank = Rank;
        Rank = RankPanel.Instance().UpdateProgressData(NumberOfRankPoints, ShiftEarnedPoints);
        if (Rank < prevRank)
        {
            gRoom_num -= 4;
            gSize -= 2;
            gNumberOfStains -= 4;
            gNumberOfBodyParts -= 5;

            if (gRoom_num < 5)
            {
                gRoom_num = 5;
            }

            if (gSize < 3)
            {
                gSize = 3;
            }

            if (gNumberOfStains < 12)
            {
                gNumberOfStains = 12;
            }

            if (gNumberOfBodyParts < 20)
            {
                gNumberOfBodyParts = 20;
            }
        }

        NumberOfRankPoints = ShiftEarnedPoints;
    }

    public void EndShift()
    {
        /*if (!IsDying)
        {
            if (!RankTutorialDone[Rank])
            {
                RankTutorialDone[Rank] = true;
            }
        }*/
        Cursor.visible = true;
        LoadShop();
        //NextShift();
    }

    #endregion TaskMethods

    //public void SetData(int saveSlotId, int shiftNo, int rankNo, int rankPoints, bool[] rankTutorialDone)
    public void SetData(int saveSlotId, SaveData saveData)
    {
        SaveSlotId = saveSlotId;
        ShiftNo = saveData.ShiftNo - 1;
        ShiftNoControl = saveData.ShiftNo - 1;
        Rank = saveData.RankNo;
        NumberOfRankPoints = saveData.RankPoints;

        for (int i = 0; i < saveData.rankTutorialDone.Length; i++)
        {
            RankTutorialDone[i] = saveData.rankTutorialDone[i];
        }

        gRoom_num = saveData.gRoom_num;
        gSize = saveData.gSize;
        gNumberOfStains = saveData.gNumberOfStains;
        gNumberOfBodyParts = saveData.gNumberOfBodyParts;

        money = saveData.Money;
    }

    public void NextShift()
    {
        if (!RankTutorialDone[Rank])
        {
            switch (Rank)
            {
                case 0:
                    SceneManager.LoadScene("Rank0Tutorial", LoadSceneMode.Single);
                    break;

                case 1:
                    //SceneManager.LoadScene("Rank1Tutorial", LoadSceneMode.Single);
                    SceneManager.LoadScene("Rank1Tutorial", LoadSceneMode.Single);
                    break;

                case 2:
                    SceneManager.LoadScene("Rank2Tutorial", LoadSceneMode.Single);
                    //SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
                    break;

                case 3:
                    //SceneManager.LoadScene("Rank3Tutorial", LoadSceneMode.Single);
                    SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
                    break;

                case 4:
                    //SceneManager.LoadScene("Rank4Tutorial", LoadSceneMode.Single);
                    SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
                    break;

                case 5:
                    //SceneManager.LoadScene("Rank5Tutorial", LoadSceneMode.Single);
                    SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
                    break;

                default:
                    break;
            }
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void LoadShop()
    {
        if (shopCounter == 0)
        {
            SceneManager.LoadScene("ShopTutorialScene");
        }
        else
        {
            SceneManager.LoadScene("ShopScene");
        }
        
    }

    #region Pause
    public bool isPaused = false;
    private bool toPause = false;
    private bool pauseable = true;
    public bool Pauseable
    {
        get => pauseable;
        set {
            pauseable = value;
            if (pauseable)
            {
                Pause(toPause);
            }
        }
    }

    public void Pause(bool pause)
    {
        if (pauseable)
        {
            isPaused = pause;
            toPause = pause;

            if (pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        else
        {
            toPause = pause;
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    #endregion Pause
}