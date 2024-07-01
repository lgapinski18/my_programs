using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;


public class RedSubstain : ASubstainType
{
    #region NESTED_TYPE

    private class RedSubstainTypeData
    {

    }

    #endregion

    //public override float MovementFactor => currentCommonStainTypeData.MovementFactor;
    //public override float ActionSpeedFactor => currentCommonStainTypeData.ActionSpeedFactor;
    //public override float SmoothFactor => currentCommonStainTypeData.SmoothFactor;
    //public override float CleaningDamageFactor => currentCommonStainTypeData.CleaningDamageFactor;



    //public override void ApplyDetergent(RedDetergent detergent)
    //{
    //    //Usuwanie starej korutyny
    //    if (currentRedDetergent != null)
    //    {
    //        StopCoroutine(coroutineRedDetergent);
    //    }
    //
    //    //Zapisywanie nowego detergentu oraz tworzenie nowej korutyny
    //    currentRedDetergent = detergent;
    //
    //    coroutineRedDetergent = EndRedDetergentInfluence(currentRedDetergent.EffectSustainingTime);
    //
    //    StartCoroutine(coroutineRedDetergent);
    //
    //    RecalculateDetergentsInfluence();
    //}
    //
    //public override void ApplyDetergent(GreenDetergent detergent)
    //{
    //    //Usuwanie starej korutyny
    //    if (currentGreenDetergent != null)
    //    {
    //        StopCoroutine(coroutineGreenDetergent);
    //    }
    //
    //    //Zapisywanie nowego detergentu oraz tworzenie nowej korutyny
    //    currentGreenDetergent = detergent;
    //
    //    coroutineGreenDetergent = EndRedDetergentInfluence(currentGreenDetergent.EffectSustainingTime);
    //
    //    StartCoroutine(coroutineGreenDetergent);
    //
    //    RecalculateDetergentsInfluence();
    //}
    //
    //public override void ApplyDetergent(BlueDetergent detergent)
    //{
    //    //Usuwanie starej korutyny
    //    if (currentBlueDetergent != null)
    //    {
    //        StopCoroutine(coroutineBlueDetergent);
    //    }
    //
    //    //Zapisywanie nowego detergentu oraz tworzenie nowej korutyny
    //    currentBlueDetergent = detergent;
    //
    //    coroutineBlueDetergent = EndRedDetergentInfluence(currentBlueDetergent.EffectSustainingTime);
    //
    //    StartCoroutine(coroutineBlueDetergent);
    //
    //    RecalculateDetergentsInfluence();
    //}
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
            //currentCommonStainTypeData.MovementFactor *= currentRedDetergent.CorrectUseData.MovementFactorModfier;
            //currentCommonStainTypeData.ActionSpeedFactor *= currentRedDetergent.CorrectUseData.ActionSpeedFactorModifier;
            //currentCommonStainTypeData.SmoothFactor *= currentRedDetergent.CorrectUseData.SmoothFactorModifier;
            currentCommonStainTypeData.CleaningDamageFactor *= currentRedDetergent.CorrectUseData.CleaningDamageModifier;

            currentCommonStainTypeData.MovementFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentRedDetergent.CorrectUseData.MovementFactorModfier);
            currentCommonStainTypeData.ActionSpeedFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentRedDetergent.CorrectUseData.ActionSpeedFactorModifier);
            currentCommonStainTypeData.SmoothFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentRedDetergent.CorrectUseData.SmoothFactorModifier);
            //currentCommonStainTypeData.CleaningDamageFactor = CalculteModifer(currentCommonStainTypeData.MovementFactor, currentRedDetergent.CorrectUseData.CleaningDamageModifier);
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
