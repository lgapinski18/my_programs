using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class AttackEffect
{
    #region NESTED_TYPES

    public enum AttackType
    {
        DAMAGE,
        KNOCKBACK
    }
    #endregion

    [SerializeField]
    private AttackType type;
    [SerializeField]
    private float magnitude = 0.0f;
    [SerializeField]
    private float time = 0.0f;

    public AttackEffect(AttackType type, float magnitude, float time)
    {
        this.type = type;
        this.magnitude = magnitude;
        this.time = time;
    }

    public AttackType Type { get => type; }
    public float Magnitude { get => magnitude; }
    public float Time { get => time; }



}

