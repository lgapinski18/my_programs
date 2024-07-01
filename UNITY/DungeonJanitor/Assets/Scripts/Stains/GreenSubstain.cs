using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGreenSubstainData
{
    float StickinessFactor { get; }
}

public class GreenSubstain : ASubstainType
{
    #region NESTED_TYPES

    [Serializable]
    public class GreenSubstainData : IGreenSubstainData
    {
        [SerializeField]
        private float stickinessFactor = 1.0f;

        public float StickinessFactor => stickinessFactor;

        public GreenSubstainData(float stickinessFactor)
        {
            this.stickinessFactor = stickinessFactor;
        }

    }

    #endregion

    #region SCRIPT_PROPERTIES

    [Header("GREEN STAIN DATA"), Space(10)]
    [SerializeField]
    private GreenSubstainData greenSubstainData;

    #endregion

    //public override float MovementFactor => throw new System.NotImplementedException();
    //
    //public override float ActionSpeedFactor => throw new System.NotImplementedException();
    //
    //public override float SmoothFactor => throw new System.NotImplementedException();
    //
    //public override float CleaningDamageFactor => throw new System.NotImplementedException();

    public override void ApplyDetergent(RedDetergent detergent)
    {
        base.ApplyDetergent(detergent);
    }

    public override void ApplyDetergent(GreenDetergent detergent)
    {
        base.ApplyDetergent(detergent);
    }

    public override void ApplyDetergent(BlueDetergent detergent)
    {
        base.ApplyDetergent(detergent);
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
            //currentCommonStainTypeData.MovementFactor *= currentGreenDetergent.CorrectUseData.MovementFactorModfier;
            //currentCommonStainTypeData.ActionSpeedFactor *= currentGreenDetergent.CorrectUseData.ActionSpeedFactorModifier;
            //currentCommonStainTypeData.SmoothFactor *= currentGreenDetergent.CorrectUseData.SmoothFactorModifier;
            currentCommonStainTypeData.CleaningDamageFactor *= currentGreenDetergent.CorrectUseData.CleaningDamageModifier;

            currentCommonStainTypeData.MovementFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentGreenDetergent.CorrectUseData.MovementFactorModfier);
            currentCommonStainTypeData.ActionSpeedFactor = CalculteModifer(currentCommonStainTypeData.ActionSpeedFactor, currentGreenDetergent.CorrectUseData.ActionSpeedFactorModifier);
            currentCommonStainTypeData.SmoothFactor = CalculteModifer(currentCommonStainTypeData.SmoothFactor, currentGreenDetergent.CorrectUseData.SmoothFactorModifier);
            //currentCommonStainTypeData.CleaningDamageFactor = CalculteModifer(currentCommonStainTypeData.CleaningDamageFactor, currentGreenDetergent.CorrectUseData.CleaningDamageModifier);
        }

        if (currentBlueDetergent != null)
        {
            //currentCommonStainTypeData.MovementFactor *= currentBlueDetergent.WrongUseData.MovementFactorModfier;
            //currentCommonStainTypeData.ActionSpeedFactor *= currentBlueDetergent.WrongUseData.ActionSpeedFactorModifier;
            //currentCommonStainTypeData.SmoothFactor *= currentBlueDetergent.WrongUseData.SmoothFactorModifier;
            currentCommonStainTypeData.CleaningDamageFactor *= currentBlueDetergent.WrongUseData.CleaningDamageModifier;

            currentCommonStainTypeData.MovementFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentBlueDetergent.WrongUseData.MovementFactorModfier);
            currentCommonStainTypeData.ActionSpeedFactor = CalculteModifer(currentCommonStainTypeData.ActionSpeedFactor, currentBlueDetergent.WrongUseData.ActionSpeedFactorModifier);
            currentCommonStainTypeData.SmoothFactor = CalculteModifer(currentCommonStainTypeData.SmoothFactor, currentBlueDetergent.WrongUseData.SmoothFactorModifier);
            //currentCommonStainTypeData.CleaningDamageFactor = CalculteModifer(currentCommonStainTypeData.CleaningDamageFactor, currentBlueDetergent.WrongUseData.CleaningDamageModifier);
        }
    }

    protected override void SetupSubstain()
    {
        base.SetupSubstain();
    }

    void Start()
    {
        SetupSubstain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
