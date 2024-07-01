using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommonSubstainData
{
    float MovementFactor { get; }
    float ActionSpeedFactor { get; }
    float SmoothFactor { get; }
    float CleaningDamageFactor { get; }
}

[RequireComponent(typeof(ModifiersManagerComponent))]
public abstract class ASubstainType : MonoBehaviour, IRedDetergentTarget, IGreenDetergentTarget, IBlueDetergentTarget, ICommonSubstainData
{
    #region NESTED_TYPES



    [Serializable]
    public class CommonSubstainTypeData : ICommonSubstainData
    {
        [SerializeField]
        private float movementFactor = 1.0f;
        [SerializeField]
        private float actionSpeedFactor = 1.0f;
        [SerializeField]
        private float smoothFactor = 1.0f;
        [SerializeField]
        private float cleaningDamageFactor = 1.0f;

        //public CommonSubstainTypeData(float movementFactor, float actionSpeedFactor, float smoothFactor, float cleaningDamageFactor)
        //{
        //    this.movementFactor = movementFactor;
        //    this.actionSpeedFactor = actionSpeedFactor;
        //    this.smoothFactor = smoothFactor;
        //    this.cleaningDamageFactor = cleaningDamageFactor;
        //}

        public float MovementFactor { get => movementFactor; set => movementFactor = value; }
        public float ActionSpeedFactor { get => actionSpeedFactor; set => actionSpeedFactor = value; }
        public float SmoothFactor { get => smoothFactor; set => smoothFactor = value; }
        public float CleaningDamageFactor { get => cleaningDamageFactor; set => cleaningDamageFactor = value; }

        public CommonSubstainTypeData Clone()
        {
            CommonSubstainTypeData copy = new CommonSubstainTypeData();
            copy.movementFactor = MovementFactor;
            copy.actionSpeedFactor = ActionSpeedFactor;
            copy.smoothFactor = SmoothFactor;
            copy.cleaningDamageFactor = CleaningDamageFactor;

            return copy;
        }
    }

    #endregion

    [Header("COMMON STAIN TYPE DATA"), Space(10)]
    [SerializeField]
    protected CommonSubstainTypeData commonStainTypeData;

    protected CommonSubstainTypeData currentCommonStainTypeData;

    protected ModifiersManagerComponent modifiersManagerComponent;

    //public abstract float MovementFactor { get; }
    //public abstract float ActionSpeedFactor { get; }
    //public abstract float SmoothFactor { get; }
    //public abstract float CleaningDamageFactor { get; }

    //ListHandle
    //private List<ModifiersManager.ModifierHandle> mlMovementSpeedFactor = new List<ModifiersManager.ModifierHandle>();
    //private List<ModifiersManager.ModifierHandle> mlActionSpeedFactor = new List<ModifiersManager.ModifierHandle>();
    //private List<ModifiersManager.ModifierHandle> mlSmoothFactorFactor = new List<ModifiersManager.ModifierHandle>();
    //private List<ModifiersManager.ModifierHandle> mlCleaningDamageFactor = new List<ModifiersManager.ModifierHandle>();
    private ModifierHandlesGroup mlMovementSpeedFactor;
    private ModifierHandlesGroup mlActionSpeedFactor;
    private ModifierHandlesGroup mlSmoothFactorFactor;
    private ModifierHandlesGroup mlCleaningDamageFactor;


    #region EVENTS_AND_DELEGATES

    public delegate void OnAppliedDetergentEvent(ASubstainType substain, ADetergent detergent);
    public event OnAppliedDetergentEvent OnAppliedDetergent;
    protected void CallOnAppliedDetergentEvent(ASubstainType substain, ADetergent detergent)
    {
        OnAppliedDetergent?.Invoke(substain, detergent);
    }

    public delegate void OnDetergentEffectEndedEvent(ASubstainType substain, ADetergent detergent);
    public event OnDetergentEffectEndedEvent OnDetergentEffectEnded;
    protected void CallOnDetergentEffectEndedEvent(ASubstainType substain, ADetergent detergent)
    {
        OnDetergentEffectEnded?.Invoke(substain, detergent);
    }

    #endregion

    protected virtual void SetupSubstain()
    {
        modifiersManagerComponent = GetComponent<ModifiersManagerComponent>();
        RecalculateDetergentsInfluence();

        //ListHandle - brak tego co poni¿ej
        mlMovementSpeedFactor = modifiersManagerComponent.AddModifierForPropertyName("movementSpeed", MovementFactor);
        mlActionSpeedFactor = modifiersManagerComponent.AddModifierForPropertyName("actionSpeed", ActionSpeedFactor);
        mlSmoothFactorFactor = modifiersManagerComponent.AddModifierForPropertyName("smoothFactor", SmoothFactor);
        mlCleaningDamageFactor = modifiersManagerComponent.AddModifierForPropertyName("cleaningDamageFactor", CleaningDamageFactor);
        ////


        UpdateModifiersManagers();
    }

    protected void RemoveModifiersHandles(List<ModifiersManager.ModifierHandle> modifiersHandles)
    {
        foreach (ModifiersManager.ModifierHandle handle in modifiersHandles)
        {
            modifiersManagerComponent.RemoveModifier(handle);
        }

        modifiersHandles.Clear();
    }

    protected virtual void UpdateModifiersManagers()
    {
        //ListHandle
        //RemoveModifiersHandles(mlMovementSpeedFactor);
        //RemoveModifiersHandles(mlActionSpeedFactor);
        //RemoveModifiersHandles(mlSmoothFactorFactor);
        //RemoveModifiersHandles(mlCleaningDamageFactor);
        mlMovementSpeedFactor.RemoveSelf();
        mlActionSpeedFactor.RemoveSelf();
        mlSmoothFactorFactor.RemoveSelf();
        mlCleaningDamageFactor.RemoveSelf();

        mlMovementSpeedFactor = modifiersManagerComponent.AddModifierForPropertyName("movementSpeed", MovementFactor);
        mlActionSpeedFactor = modifiersManagerComponent.AddModifierForPropertyName("actionSpeed", ActionSpeedFactor);
        mlSmoothFactorFactor = modifiersManagerComponent.AddModifierForPropertyName("smoothFactor", SmoothFactor);
        mlCleaningDamageFactor = modifiersManagerComponent.AddModifierForPropertyName("cleaningDamageFactor", CleaningDamageFactor);
    }

    public float MovementFactor => currentCommonStainTypeData.MovementFactor;
    public float ActionSpeedFactor => currentCommonStainTypeData.ActionSpeedFactor;
    public float SmoothFactor => currentCommonStainTypeData.SmoothFactor;
    public float CleaningDamageFactor => currentCommonStainTypeData.CleaningDamageFactor;

    //public abstract void ApplyDetergent(RedDetergent detergent);
    //public abstract void ApplyDetergent(GreenDetergent detergent);
    //public abstract void ApplyDetergent(BlueDetergent detergent);
    public virtual void ApplyDetergent(RedDetergent detergent)
    {
        bool notApplied = true;
        //Usuwanie starej korutyny
        if (currentRedDetergent != null)
        {
            StopCoroutine(coroutineRedDetergent);
            notApplied = false;
        }

        //Zapisywanie nowego detergentu oraz tworzenie nowej korutyny
        currentRedDetergent = detergent;

        coroutineRedDetergent = EndRedDetergentInfluence(currentRedDetergent.EffectSustainingTime);

        StartCoroutine(coroutineRedDetergent);

        RecalculateDetergentsInfluence();

        UpdateModifiersManagers();
        if (notApplied)
        {
            CallOnAppliedDetergentEvent(this, detergent);
        }
    }

    public virtual void ApplyDetergent(GreenDetergent detergent)
    {
        bool notApplied = true;
        //Usuwanie starej korutyny
        if (currentGreenDetergent != null)
        {
            StopCoroutine(coroutineGreenDetergent);
            notApplied = false;
        }

        //Zapisywanie nowego detergentu oraz tworzenie nowej korutyny
        currentGreenDetergent = detergent;

        coroutineGreenDetergent = EndGreenDetergentInfluence(currentGreenDetergent.EffectSustainingTime);

        StartCoroutine(coroutineGreenDetergent);

        RecalculateDetergentsInfluence();

        UpdateModifiersManagers();
        if (notApplied)
        {
            CallOnAppliedDetergentEvent(this, detergent);
        }
    }

    public virtual void ApplyDetergent(BlueDetergent detergent)
    {
        bool notApplied = true;
        //Usuwanie starej korutyny
        if (currentBlueDetergent != null)
        {
            StopCoroutine(coroutineBlueDetergent);
            notApplied = false;
        }

        //Zapisywanie nowego detergentu oraz tworzenie nowej korutyny
        currentBlueDetergent = detergent;

        coroutineBlueDetergent = EndBlueDetergentInfluence(currentBlueDetergent.EffectSustainingTime);

        StartCoroutine(coroutineBlueDetergent);

        RecalculateDetergentsInfluence();

        UpdateModifiersManagers();
        if (notApplied)
        {
            CallOnAppliedDetergentEvent(this, detergent);
        }
    }

    protected abstract void RecalculateDetergentsInfluence();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    protected float CalculteModifer(float factor, float modifier)
    {
        return factor + modifier * Mathf.Abs(factor - 1);
    }


    #region MANAGING_RED

    protected RedDetergent currentRedDetergent = null;
    protected IEnumerator coroutineRedDetergent = null;

    protected IEnumerator EndRedDetergentInfluence(float timeout)
    {
        yield return new WaitForSeconds(timeout);

        Debug.Log("Ending red effect");
        CallOnDetergentEffectEndedEvent(this, currentRedDetergent);

        currentRedDetergent = null;

        RecalculateDetergentsInfluence();
        UpdateModifiersManagers();
    }

    #endregion

    #region MANAGING_GREEN

    protected GreenDetergent currentGreenDetergent = null;
    protected IEnumerator coroutineGreenDetergent = null;

    protected IEnumerator EndGreenDetergentInfluence(float timeout)
    {
        yield return new WaitForSeconds(timeout);

        CallOnDetergentEffectEndedEvent(this, currentGreenDetergent);

        currentGreenDetergent = null;

        RecalculateDetergentsInfluence();
        UpdateModifiersManagers();
    }

    #endregion

    #region MANAGING_BLUE

    protected BlueDetergent currentBlueDetergent = null;
    protected IEnumerator coroutineBlueDetergent = null;

    protected IEnumerator EndBlueDetergentInfluence(float timeout)
    {
        yield return new WaitForSeconds(timeout);

        CallOnDetergentEffectEndedEvent(this, currentBlueDetergent);

        currentBlueDetergent = null;

        RecalculateDetergentsInfluence();
        UpdateModifiersManagers();
    }

    #endregion
}
