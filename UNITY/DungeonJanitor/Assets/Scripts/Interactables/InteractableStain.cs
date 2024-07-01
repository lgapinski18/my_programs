using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STAIN_TYPES
{
    RED,
    GREEN,
    BLUE,
    YELLOW,
    SKY,
    PURPLE
};

public class InteractableStain : AInteractable
{
    public static readonly Vector3[] stain_types = 
    {
    new Vector3(2.0f, 0.0f, 0.0f),
    new Vector3(0.0f, 2.0f, 0.0f),
    new Vector3(0.0f, 0.0f, 2.0f),
    new Vector3(1.0f, 1.0f, 0.0f),
    new Vector3(0.0f, 1.0f, 1.0f),
    new Vector3(1.0f, 0.0f, 1.0f)
    };

    [SerializeField] public GameObject[] stain_phases;
    [SerializeField] public int difficulty;
    [SerializeField] public GameObject minigame_prefab;
    [SerializeField] public STAIN_TYPES type;

    [HideInInspector] public int stain_phase;

    private GameObject cleaningProductsInventory;

    private int initial_health;
    private int health;
    private GameObject minigame_ui;

    private int HealthPercentage
    {
        get { return health * 100 / initial_health; }
    }
    public void Awake()
    {
        StainsCleaningTaskManager.Instance().IncrementNumberOfSubstains();

        initial_health = difficulty * 10;
        health = initial_health;
        minigame_ui = null;
    }

    public void DealDmg()
    {
        int dmg = 0;
        if (cleaningProductsInventory.GetComponent<CleaningProductsInventory>().cleaning_power == stain_types[(int)type])
        {
            dmg += 10;
        }
        else
        {
            dmg += 5;
        }
        health -= dmg;
    }

    public override bool interact()
    {
        if (minigame_ui == null) 
        {
            minigame_ui = Instantiate(minigame_prefab);
            minigame_ui.transform.GetComponent<CleaningUI>().inventory = cleaningProductsInventory.GetComponent<CleaningProductsInventory>();
            minigame_ui.layer = 5;
            minigame_ui.transform.parent = transform;
        }
        //health -= Damage;
        return false;
    }

    public void Start()
    {
        cleaningProductsInventory = GameObject.Find("CurrentCleanersUI");
    }

    public void Update()
    {
        for (int i = 0; i < stain_phases.Length; i++)
        {
            stain_phases[i].SetActive(false);
        }
        
        if(HealthPercentage > 75)
        {
            stain_phase = 3;
        }
        else if((HealthPercentage <= 75) && (HealthPercentage > 50))
        {
            stain_phase = 2;
        }
        else if((HealthPercentage <= 50) && (HealthPercentage > 25))
        {
            stain_phase = 1;
        }
        else if((HealthPercentage <= 25) && (HealthPercentage > 1))
        {
            stain_phase = 0;
        }
        else
        {
            StainsCleaningTaskManager.Instance().IncrementNumberOfCleaned();
            Destroy(minigame_ui.gameObject);
            Destroy(gameObject);
        }

        stain_phases[stain_phase].SetActive(true);
    }
}
