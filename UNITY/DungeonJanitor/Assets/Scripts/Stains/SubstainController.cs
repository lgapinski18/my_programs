using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Obecnie ten interfejs musi byæ implementowany przez Player
 */
public interface IStainAffected
{
}

//[RequireComponent(typeof(ModifiersManagerComponent), typeof(SpriteRenderer), typeof(BoxCollider2D))]
[RequireComponent(typeof(ModifiersManagerComponent), typeof(BoxCollider2D))]
public class SubstainController : MonoBehaviour, ISubstain
{
    #region NESTED_TYPES

    #endregion

    #region SCRIPT_PROPERTIES

    [SerializeField]
    private float maxDirtiness = 1.0f;
    public float MaxDirtiness { get => maxDirtiness; }

    #endregion


    #region SCRIPT_FIELDS

    private float dirtiness = 1.0f;
    public float Dirtiness { get => dirtiness; }

    //private SpriteRenderer spriteRenderer;
    private ModifiersManagerComponent modifiersManagerComponent;
    private MainStain parentMainStain;
    private Vector2 sizeInPixels;
    private Vector2 positionInPixels;
    private Rect rectInPixels;

    #endregion


    #region MODIFIERS

    private float movementSpeed = 1.0f;
    private float actionSpeed = 1.0f;
    private float smoothFactor = 0.5f;
    private float cleaningDamageFactor = 1.0f;

   //private float currentMovementSpeeedModifier = 1.0f;
   //private float currentActionSpeeedModifier = 1.0f;
   //private float currentSmoothFactorModifier = 1.0f;
   //private float currentCleaningDamageFactorModifier = 1.0f;

    private ModifiersManager mmMovementSpeed;
    private ModifiersManager mmActionSpeed;
    private ModifiersManager mmSmoothFactor;
    private ModifiersManager mmCleaningDamageFactor;

    private bool updateAffected = false;

    private void OnChangedMovementSpeedModifier(ModifiersManager modifiersManager)
    {
        movementSpeed = modifiersManager.GetModifier();
        updateAffected = true;
    }

    private void OnChangedActionSpeedModifier(ModifiersManager modifiersManager)
    {
        actionSpeed = modifiersManager.GetModifier();
        updateAffected = true;
    }

    private void OnChangedSmoothFactorModifier(ModifiersManager modifiersManager)
    {
        smoothFactor = modifiersManager.GetModifier();
        updateAffected = true;
    }

    private void OnChangedCleaningDamageFactorModifier(ModifiersManager modifiersManager)
    {
        cleaningDamageFactor = modifiersManager.GetModifier();
        updateAffected = true;
    }

    #endregion


    #region EVENTS_AND_DELEGATES

    public delegate void SubstainControllerEvent(SubstainController substain);
    public event SubstainControllerEvent OnCleaned;
    public event SubstainControllerEvent OnPartlyCleaned;

    #endregion

    public void Init(Sprite sprite, Vector2 colliderSize)
    {
        //GetComponent<SpriteRenderer>().sprite = sprite;
        GetComponent<BoxCollider2D>().size = colliderSize;
    }
    public void Init(Vector2 colliderSize, Rect rectInPixels)
    {
        GetComponent<BoxCollider2D>().size = colliderSize;
        this.rectInPixels = rectInPixels;
        //this.sizeInPixels = rectInPixels.size;
        //this.positionInPixels = rectInPixels.position;
    }

    void Awake()
    {
        dirtiness = maxDirtiness;

        //spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.enabled = false;
        //spriteRenderer.allowOcclusionWhenDynamic = false;
        //spriteRenderer.enabled = true;
        //spriteRenderer.isPartOfStaticBatch = true;
        modifiersManagerComponent = GetComponent<ModifiersManagerComponent>();

        parentMainStain = transform.parent.GetComponent<MainStain>();

        mmMovementSpeed = new ModifiersManager(ModifiersManager.ModifiersProcessingType.Avarage, this);
        mmMovementSpeed.OnChanged += OnChangedMovementSpeedModifier;
        modifiersManagerComponent.RegisterModifiersManager("movementSpeed", mmMovementSpeed);

        mmActionSpeed = new ModifiersManager(ModifiersManager.ModifiersProcessingType.Avarage, this);
        mmActionSpeed.OnChanged += OnChangedActionSpeedModifier;
        modifiersManagerComponent.RegisterModifiersManager("actionSpeed", mmActionSpeed);

        mmSmoothFactor = new ModifiersManager(ModifiersManager.ModifiersProcessingType.Avarage, this);
        mmSmoothFactor.OnChanged += OnChangedSmoothFactorModifier;
        modifiersManagerComponent.RegisterModifiersManager("smoothFactor", mmSmoothFactor);

        mmCleaningDamageFactor = new ModifiersManager(ModifiersManager.ModifiersProcessingType.Avarage, this);
        mmCleaningDamageFactor.OnChanged += OnChangedCleaningDamageFactorModifier;
        modifiersManagerComponent.RegisterModifiersManager("cleaningDamageFactor", mmCleaningDamageFactor);


    }

    void Start()
    {

        //spriteRenderer.allowOcclusionWhenDynamic = false;
        //spriteRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (updateAffected)
        {
            UpdateModifiersOfAllAffected();
            updateAffected = false;
        }

        //spriteRenderer.enabled = true;
        //if (!spriteRenderer.isVisible)
        //{
        //    spriteRenderer.enabled = false;
        //    Debug.Log("SubVisible");
        //}
    }

    public void Clean(float cleaningDamage)
    {
        dirtiness -= cleaningDamageFactor * cleaningDamage;


        //spriteRenderer.color = new Color(1f, 1f, 1f, (dirtiness / (float)maxDirtiness) * 0.8f + 0.2f); //w alfie may * 0.8f + 0.2f, aby mimo niskiej wartoœci dirtiness/bliskoœæ do wyczyszczenia plama ca³y czas by³a widoczna
        

        Debug.Log("Cleaning Substain");

        if (dirtiness <= 0)
        {
            parentMainStain.CleanStain(rectInPixels, 0.0f);
            OnCleaned?.Invoke(this);
            Destroy(gameObject);
        }
        else
        {
            parentMainStain.CleanStain(rectInPixels, (dirtiness / (float)maxDirtiness) * 0.8f + 0.2f);
            OnPartlyCleaned?.Invoke(this);
        }
    }

    #region AFFECTING

    private class MHLStruct //ModifiersHandlesListsSturcture
    {
        //ListHandle
        //public List<ModifiersManager.ModifierHandle> mlMovementSpeedFactor;
        //public List<ModifiersManager.ModifierHandle> mlActionSpeedFactor;
        //public List<ModifiersManager.ModifierHandle> mlSmoothFactorFactor;

        public ModifierHandlesGroup mlMovementSpeedFactor;
        public ModifierHandlesGroup mlActionSpeedFactor;
        public ModifierHandlesGroup mlSmoothFactorFactor;

        public MHLStruct()
        {
            //ListHandle
            //mlMovementSpeedFactor = new List<ModifiersManager.ModifierHandle>();
            //mlActionSpeedFactor = new List<ModifiersManager.ModifierHandle>();
            //mlSmoothFactorFactor = new List<ModifiersManager.ModifierHandle>();
            mlMovementSpeedFactor = new ModifierHandlesGroup();
            mlActionSpeedFactor = new ModifierHandlesGroup();
            mlSmoothFactorFactor = new ModifierHandlesGroup();
        }

        //ListHandle
        //public MHLStruct(List<ModifiersManager.ModifierHandle> mlMovementSpeedFactor, 
        //                 List<ModifiersManager.ModifierHandle> mlActionSpeedFactor, 
        //                 List<ModifiersManager.ModifierHandle> mlSmoothFactorFactor)
        //{
        //    this.mlMovementSpeedFactor = mlMovementSpeedFactor;
        //    this.mlActionSpeedFactor = mlActionSpeedFactor;
        //    this.mlSmoothFactorFactor = mlSmoothFactorFactor;
        //}

        public MHLStruct(ModifierHandlesGroup mlMovementSpeedFactor, ModifierHandlesGroup mlActionSpeedFactor, ModifierHandlesGroup mlSmoothFactorFactor)
        {
            this.mlMovementSpeedFactor = mlMovementSpeedFactor;
            this.mlActionSpeedFactor = mlActionSpeedFactor;
            this.mlSmoothFactorFactor = mlSmoothFactorFactor;
        }
    }
    protected void RemoveModifiersHandles(ModifiersManagerComponent modifiersManagerComponent, List<ModifiersManager.ModifierHandle> modifiersHandles)
    {
        foreach (ModifiersManager.ModifierHandle handle in modifiersHandles)
        {
            modifiersManagerComponent.RemoveModifier(handle);
        }

        modifiersHandles.Clear();
    }
    private void RemoveModifiersForGameObject(GameObject gameObject)
    {
        MHLStruct mhlStruct = affectedBySubstain[gameObject];
        //ModifiersManagerComponent modifiersManagerComponent = gameObject.GetComponent<ModifiersManagerComponent>();
        if (mhlStruct != null)
        {
            //ListHandle
            //RemoveModifiersHandles(modifiersManagerComponent, mhlStruct.mlMovementSpeedFactor);
            //RemoveModifiersHandles(modifiersManagerComponent, mhlStruct.mlActionSpeedFactor);
            //RemoveModifiersHandles(modifiersManagerComponent, mhlStruct.mlSmoothFactorFactor);

            mhlStruct.mlMovementSpeedFactor.RemoveSelf();
            mhlStruct.mlActionSpeedFactor.RemoveSelf();
            mhlStruct.mlSmoothFactorFactor.RemoveSelf();
        }
    }

    private void ApplyEffectsForGameObject(GameObject gameObject)
    {
        ModifiersManagerComponent modifiersManagerComponent = gameObject.GetComponent<ModifiersManagerComponent>();

        MHLStruct mhlStruct = new()
        {
            mlMovementSpeedFactor = modifiersManagerComponent.AddModifierForPropertyName("movementSpeed", movementSpeed),
            mlActionSpeedFactor = modifiersManagerComponent.AddModifierForPropertyName("actionSpeed", actionSpeed),
            mlSmoothFactorFactor = modifiersManagerComponent.AddModifierForPropertyName("smoothFactor", smoothFactor)
        };

        if (affectedBySubstain.ContainsKey(gameObject))
        {
            affectedBySubstain[gameObject] = mhlStruct;
        }
        else
        {
            affectedBySubstain.Add(gameObject, mhlStruct);
        }
    }

    private void UpdateEffectsForGameObject(GameObject gameObject)
    {
        RemoveModifiersForGameObject(gameObject);
        ApplyEffectsForGameObject(gameObject);
    }

    private void UpdateModifiersOfAllAffected()
    {
        GameObject[] keys = new GameObject[affectedBySubstain.Count];
        affectedBySubstain.Keys.CopyTo(keys, 0);
        foreach (var key in keys)
        {
            UpdateEffectsForGameObject(key);
        }
    }

    private Dictionary<GameObject, MHLStruct> affectedBySubstain = new Dictionary<GameObject, MHLStruct>();




    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonoBehaviour component = (MonoBehaviour)collision.GetComponent<IStainAffected>();

        if (component != null)
        {
            ModifiersManagerComponent modifiersManagerComponent = component.GetComponent<ModifiersManagerComponent>();

            if (modifiersManagerComponent != null)
            {
                //Debug.Log("SC: Appling");
                ApplyEffectsForGameObject(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (affectedBySubstain.ContainsKey(collision.gameObject))
        {
            //Debug.Log("SC: EndingAppling");
            RemoveModifiersForGameObject(collision.gameObject);
            affectedBySubstain.Remove(collision.gameObject);
        }
    }

    //void OnBecameVisible()
    //{
    //    spriteRenderer.enabled = true;
    //    Debug.Log("BecamVisible: " + spriteRenderer.enabled);
    //}
    //
    //void OnBecameInvisible()
    //{
    //    spriteRenderer.enabled = false;
    //    Debug.Log("BecamInvisible: " + spriteRenderer.enabled);
    //}

}
