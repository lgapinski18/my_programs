using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ADetergent : AbstractTool
{
    #region NESTED_TYPES

    [Serializable]
    public class DetergentData
    {
        [SerializeField]
        private float movementFactorModfier = 1.0f;
        [SerializeField]
        private float actionSpeedFactorModifier = 1.0f;
        [SerializeField]
        private float smoothFactorModifier = 1.0f;
        [SerializeField]
        private float cleaningDamageModifier = 1.0f;
        [SerializeField]
        private float stainPaceModifier = 1.0f;
        [SerializeField]
        private float stainSpecialModifier = 1.0f;
        public float MovementFactorModfier { get => movementFactorModfier; }
        public float ActionSpeedFactorModifier { get => actionSpeedFactorModifier; }
        public float SmoothFactorModifier { get => smoothFactorModifier; }
        public float CleaningDamageModifier { get => cleaningDamageModifier; }
        public float StainPaceModifier { get => stainPaceModifier; }
        public float StainSpecialModifier { get => stainSpecialModifier; }

        public DetergentData(float movementFactorModfier, float actionSpeedFactorModifier, float smoothFactorModifier, float cleaningDamageModifier, float stainPaceModifier, float stainSpecialModifier)
        {
            this.movementFactorModfier = movementFactorModfier;
            this.actionSpeedFactorModifier = actionSpeedFactorModifier;
            this.smoothFactorModifier = smoothFactorModifier;
            this.cleaningDamageModifier = cleaningDamageModifier;
            this.stainPaceModifier = stainPaceModifier;
            this.stainSpecialModifier = stainSpecialModifier;
        }

    }

    #endregion

    #region SCRIPT_PROPERTIES

    [Header("COMMON DETERGENT DATA"), Space(10)]
    [SerializeField]
    private DetergentData correctUseData;
    [SerializeField]
    private DetergentData wrongUseData;
    [SerializeField]
    protected float effectSustainingTime = 5f;
    public float EffectSustainingTime { get => effectSustainingTime; }

    public DetergentData CorrectUseData { get => correctUseData; }
    public DetergentData WrongUseData { get => wrongUseData; }

    #endregion
    protected void SettupTool()
    {
        base.SetupTool();
    }
   
}
