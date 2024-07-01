using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public enum CorpseSize
    {
        Small   = 0b0000_0001,
        Normal  = 0b0000_0010,
        Huge    = 0b0000_0100
    }

    [SerializeField]
    private CorpseSize corpseSize;
    public CorpseSize Size { get => corpseSize; }


    public enum SmallCorpseSubType
    {
        None,
        Head,
        Arm,
        Leg,
        Torso,
        SmallBody
    }

    [SerializeField, ConditionalShowProperty("corpseSize", CorpseSize.Small)]
    private SmallCorpseSubType smallCorpseSubType;
    public SmallCorpseSubType SmallSubType { get => smallCorpseSubType; }


    public enum CorpseType
    {
        Flesh   = 0b0000_0001,
        Bone    = 0b0000_0010,
        Rock    = 0b0000_0100
    }

    [SerializeField]
    private CorpseType corpseType;

    public CorpseType Type { get => corpseType; }

    #region EATING
    [Header("Eating"), Space(10)]

    [SerializeField]
    private bool canBeEaten = false;
    public bool CanBeEaten { get => canBeEaten; }

    [SerializeField]
    private float eatingTime = 0.0f;
    public float EatingTime { get => eatingTime; }

    [SerializeField]
    private GameObject[] eatingRemains = null;

    [SerializeField, Tooltip("Describe how much spread should be remains after eating.")]
    private float eatingRemainsSpawnRadius = 0.2f;
    public float EatingRemainsSpawnRadius { get => eatingRemainsSpawnRadius; }

    #endregion

    #region BUTCHERING
    [Header("Butchering"), Space(10)]

    [SerializeField]
    private bool canBeButchered = false;
    public bool CanBeButchered { get => canBeButchered; }

    [SerializeField]
    private float butcheringTime = 0.0f;
    public float ButcheringTime { get => butcheringTime; }

    [SerializeField]
    private GameObject[] corpseFragments;

    [SerializeField, Tooltip("Describe how much spread should be corpse fragments after butchering.")]
    private float spawnCorpseFragmentsRadius = 0.2f;
    public float SpawnCorpseFragmentsRadius { get => spawnCorpseFragmentsRadius; }

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        //this.enabled = false;
    }

    void Start()
    {
        CorpseCleaningTaskManager.Instance().IncrementNumberOfCorpses();
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Eat(CorpseType eaterPreferences)
    {
        Debug.Log("Eater eat: " + eaterPreferences);
        if (CanBeEaten)
        {
            Vector3 deltaSpawnPosition = Vector3.zero;
            foreach (GameObject eatingRemain in eatingRemains)
            {
                if ((eatingRemain.GetComponent<Corpse>().Type & eaterPreferences) != 0)
                {
                    deltaSpawnPosition = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
                    deltaSpawnPosition.Normalize();
                    deltaSpawnPosition *= eatingRemainsSpawnRadius;

                    Object.Instantiate(eatingRemain, transform.position + deltaSpawnPosition, Quaternion.identity);
                }
            }

            CorpseCleaningTaskManager.Instance().IncrementNumberOfDisposed();

            Destroy(gameObject);
        }
    }
    public void ButcherCorpse()
    {
        Debug.Log("Butcher butchered.");
        if (CanBeButchered)
        {
            Vector3 deltaSpawnPosition = Vector3.zero;
            foreach (GameObject corpseFragment in corpseFragments)
            {
                deltaSpawnPosition = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
                deltaSpawnPosition.Normalize();
                deltaSpawnPosition *= spawnCorpseFragmentsRadius;

                Object.Instantiate(corpseFragment, transform.position + deltaSpawnPosition, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
