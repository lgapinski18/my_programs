using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public interface IBlueSubstainData
{
    float ProgressionFactor { get; }
    float GassingFactor { get; }
}

public class BlueSubstain : ASubstainType, IBlueSubstainData
{
    #region NESTED_TYPES

    [Serializable]
    public class BlueSubstainData : IBlueSubstainData
    {
        [SerializeField]
        private float progressionFactor = 1.0f;
        [SerializeField]
        private float gassingFactor = 1.0f;
        public float ProgressionFactor { get => progressionFactor; set => progressionFactor = value; }
        public float GassingFactor { get => gassingFactor; set => gassingFactor = value; }

        public BlueSubstainData(float progressionFactor, float gassingFactor)
        {
            this.progressionFactor = progressionFactor;
            this.gassingFactor = gassingFactor;
        }

    }

    #endregion

    #region SCRIPT_PROPERTIES

    [Header("BLUE STAIN DATA"), Space(10)]
    [SerializeField]
    protected BlueSubstainData blueSubstainData;
    protected BlueSubstainData currentBueSubstainData;

    [Header("GASING"), Space(10)]
    [SerializeField]
    private float gasingPhase0 = 0.0f;
    [SerializeField]
    private float gasingPhase1 = 0.0f;
    [SerializeField]
    private float gasingPhase2 = 0.0f;
    [SerializeField]
    private float gasingPhase3 = 0.0f;
    [Header("ZONE RADIUS"), Space(10)]
    [SerializeField]
    private float radiusPhase0 = 0.04f;
    [SerializeField]
    private float radiusPhase1 = 0.06f;
    [SerializeField]
    private float radiusPhase2 = 0.08f;
    [SerializeField]
    private float radiusPhase3 = 0.1f;

    private float currentGasing = 0.0f;

    [SerializeField]
    private float correctCleaningValue = -10.0f;
    [SerializeField]
    private float wrongCleaningValue = 5.0f;

    #endregion

    #region SCRIPT_FIELDS

    private BlueMainStain blueMainStain;
    private int phase = 0;
    private float progress = 0.0f;
    private BlueGasingZone blueGasingZone;

    //private bool progressionAffected = false;
    //private bool gasingAffected = false;
    //private Collider2D substainCollider;
    //private Collider2D gasingArea;

    #endregion

    //public override float MovementFactor => throw new System.NotImplementedException();
    //
    //public override float ActionSpeedFactor => throw new System.NotImplementedException();
    //
    //public override float SmoothFactor => throw new System.NotImplementedException();
    //
    //public override float CleaningDamageFactor => throw new System.NotImplementedException();

    public float ProgressionFactor => currentBueSubstainData.ProgressionFactor;

    public float GassingFactor => currentBueSubstainData.GassingFactor;

    public override void ApplyDetergent(RedDetergent detergent)
    {
        base.ApplyDetergent(detergent);

        progress += wrongCleaningValue * ProgressionFactor;
        blueMainStain.Progress(wrongCleaningValue * ProgressionFactor);
        UpdatePhase();
    }

    public override void ApplyDetergent(GreenDetergent detergent)
    {
        base.ApplyDetergent(detergent);

        progress += wrongCleaningValue * ProgressionFactor;
        blueMainStain.Progress(wrongCleaningValue * ProgressionFactor);
        UpdatePhase();
    }

    public override void ApplyDetergent(BlueDetergent detergent)
    {
        base.ApplyDetergent(detergent);

        progress += correctCleaningValue * ProgressionFactor;
        blueMainStain.Progress(correctCleaningValue * ProgressionFactor);
        UpdatePhase();
    }

    protected override void RecalculateDetergentsInfluence()
    {
        currentCommonStainTypeData = commonStainTypeData.Clone();

        if (currentRedDetergent != null)
        {
            //currentCommonStainTypeData.MovementFactor *= currentRedDetergent.WrongUseData.MovementFactorModfier;
            //currentCommonStainTypeData.ActionSpeedFactor *= currentRedDetergent.WrongUseData.ActionSpeedFactorModifier;
            //currentCommonStainTypeData.SmoothFactor *= currentRedDetergent.WrongUseData.SmoothFactorModifier;
            currentCommonStainTypeData.CleaningDamageFactor *= currentRedDetergent.WrongUseData.CleaningDamageModifier;

            currentCommonStainTypeData.MovementFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentRedDetergent.WrongUseData.MovementFactorModfier);
            currentCommonStainTypeData.ActionSpeedFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentRedDetergent.WrongUseData.ActionSpeedFactorModifier);
            currentCommonStainTypeData.SmoothFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentRedDetergent.WrongUseData.SmoothFactorModifier);
            //currentCommonStainTypeData.CleaningDamageFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentRedDetergent.WrongUseData.CleaningDamageModifier);
        }

        if (currentGreenDetergent != null)
        {
            //currentCommonStainTypeData.MovementFactor *= currentGreenDetergent.WrongUseData.MovementFactorModfier;
            //currentCommonStainTypeData.ActionSpeedFactor *= currentGreenDetergent.WrongUseData.ActionSpeedFactorModifier;
            //currentCommonStainTypeData.SmoothFactor *= currentGreenDetergent.WrongUseData.SmoothFactorModifier;
            currentCommonStainTypeData.CleaningDamageFactor *= currentGreenDetergent.WrongUseData.CleaningDamageModifier;

            currentCommonStainTypeData.MovementFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentGreenDetergent.WrongUseData.MovementFactorModfier);
            currentCommonStainTypeData.ActionSpeedFactor = CalculteModifer(currentCommonStainTypeData.ActionSpeedFactor, currentGreenDetergent.WrongUseData.ActionSpeedFactorModifier);
            currentCommonStainTypeData.SmoothFactor = CalculteModifer(currentCommonStainTypeData.SmoothFactor, currentGreenDetergent.WrongUseData.SmoothFactorModifier);
            //currentCommonStainTypeData.CleaningDamageFactor = CalculteModifer(currentCommonStainTypeData.CleaningDamageFactor, currentGreenDetergent.WrongUseData.CleaningDamageModifier);
        }

        if (currentBlueDetergent != null)
        {
            //currentCommonStainTypeData.MovementFactor *= currentBlueDetergent.CorrectUseData.MovementFactorModfier;
            //currentCommonStainTypeData.ActionSpeedFactor *= currentBlueDetergent.CorrectUseData.ActionSpeedFactorModifier;
            //currentCommonStainTypeData.SmoothFactor *= currentBlueDetergent.CorrectUseData.SmoothFactorModifier;
            currentCommonStainTypeData.CleaningDamageFactor *= currentBlueDetergent.CorrectUseData.CleaningDamageModifier;

            currentCommonStainTypeData.MovementFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentBlueDetergent.CorrectUseData.MovementFactorModfier);
            currentCommonStainTypeData.ActionSpeedFactor = CalculteModifer(currentCommonStainTypeData.ActionSpeedFactor, currentBlueDetergent.CorrectUseData.ActionSpeedFactorModifier);
            currentCommonStainTypeData.SmoothFactor = CalculteModifer(currentCommonStainTypeData.SmoothFactor, currentBlueDetergent.CorrectUseData.SmoothFactorModifier);
            //currentCommonStainTypeData.CleaningDamageFactor = CalculteModifer(currentCommonStainTypeData.CleaningDamageFactor, currentBlueDetergent.CorrectUseData.CleaningDamageModifier);
        }

        RecalculateProgresionFactor();

        RecalculateGasingFactor();
    }

    private void RecalculateProgresionFactor()
    {
        currentBueSubstainData.ProgressionFactor = blueSubstainData.ProgressionFactor;

        if (currentRedDetergent != null)
        {
            currentBueSubstainData.ProgressionFactor *= currentRedDetergent.WrongUseData.StainPaceModifier;
        }

        if (currentGreenDetergent != null)
        {
            currentBueSubstainData.ProgressionFactor *= currentGreenDetergent.WrongUseData.StainPaceModifier;
        }

        if (currentBlueDetergent != null)
        {
            currentBueSubstainData.ProgressionFactor *= currentBlueDetergent.CorrectUseData.StainPaceModifier;
        }
    }

    private void RecalculateGasingFactor()
    {
        currentBueSubstainData.GassingFactor = blueSubstainData.GassingFactor;

        if (currentRedDetergent != null)
        {
            currentBueSubstainData.GassingFactor *= currentRedDetergent.WrongUseData.StainSpecialModifier;
        }

        if (currentGreenDetergent != null)
        {
            currentBueSubstainData.GassingFactor *= currentGreenDetergent.WrongUseData.StainSpecialModifier;
        }

        if (currentBlueDetergent != null)
        {
            currentBueSubstainData.GassingFactor *= currentBlueDetergent.CorrectUseData.StainSpecialModifier;
        }

    }

    protected override void SetupSubstain()
    {
        //Debug.Log("SettingUpBlueStain");
        base.SetupSubstain();
        //Debug.Log("SettingUpBlueStain2");
        blueGasingZone = GetComponentInChildren<BlueGasingZone>();
        blueMainStain = transform.parent.GetComponent<BlueMainStain>();

        blueGasingZone.InZone += GaseInZone;
        blueGasingZone.OutZone += LeaveGasingZone;
    }

    void Start()
    {
        currentBueSubstainData = new BlueSubstainData(blueSubstainData.ProgressionFactor, blueSubstainData.GassingFactor);
        SetupSubstain();
    }

    // Update is called once per frame
    void Update()
    {
        blueMainStain = transform.parent.GetComponent<BlueMainStain>();
        blueMainStain.Progress(ProgressionFactor * Time.deltaTime);
        progress += ProgressionFactor * Time.deltaTime;
        UpdatePhase();
    }


    void UpdatePhase()
    {
        if (progress >= blueMainStain.MaxProgress)
        {
            progress = blueMainStain.MaxProgress;
            phase = 3;
            currentGasing = gasingPhase3;
            blueGasingZone.Radius = radiusPhase3;
        }
        else if (progress >= blueMainStain.Phase3)
        {
            phase = 3;
            currentGasing = gasingPhase3;
            blueGasingZone.Radius = radiusPhase3;
        }
        else if (progress >= blueMainStain.Phase2)
        {
            phase = 2;
            currentGasing = gasingPhase2;
            blueGasingZone.Radius = radiusPhase2;
        }
        else if (progress >= blueMainStain.Phase1)
        {
            phase = 1;
            currentGasing = gasingPhase1;
            blueGasingZone.Radius = radiusPhase1;
        }
        else
        {
            phase = 0;
            currentGasing = gasingPhase0;
            blueGasingZone.Radius = radiusPhase0;
        }
    }



    private void GaseInZone(Collider2D collision)
    {
        //Collider2D[] contacts = null;
        //
        //substainCollider.GetContacts(contacts);
        //
        //if (contacts != null)
        //{
        //    if (contacts.Contains(collision))
        //    {
        //
        //    }
        //}
        //
        IGasedEntity[] gasedEntities = collision.GetComponents<IGasedEntity>();

        if (gasedEntities != null)
        {
            foreach (IGasedEntity gasedEntity in gasedEntities)
            {
                gasedEntity.IsGased = true;
            }
        }
    }    

    private void LeaveGasingZone(Collider2D collision)
    {
        IGasedEntity[] gasedEntities = collision.GetComponents<IGasedEntity>();

        if (gasedEntities != null)
        {
            foreach (IGasedEntity gasedEntity in gasedEntities)
            {
                gasedEntity.IsGased = false;
            }
        }
    }

    #region PROGRESSION_FACTOR_MANAGING


    //protected BlueDetergent currentRedDetergentProgression = null;
    //protected BlueDetergent currentGreenDetergentProgression = null;
    //protected BlueDetergent currentBlueDetergentProgression = null;

    #endregion
}
