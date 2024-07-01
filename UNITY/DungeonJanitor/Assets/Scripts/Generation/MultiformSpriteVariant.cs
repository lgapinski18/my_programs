using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MultiformSpriteVariant : ScriptableObject
{
    [SerializeField]
    private Sprite spriteVariant = null;
    public Sprite SpriteVariant { get => spriteVariant; }

    [SerializeField]
    private short[] featureVector = null;
    public short[] FeatureVector { get => (short[]) featureVector.Clone(); }

    public bool CompareFeatureVector(short[] featureVector)
    {
        //Debug.Log("Cond1: " + (this.featureVector));
        //Debug.Log("Cond1: " + (featureVector));
        //Debug.Log("Cond1: " + (this.featureVector.Length != featureVector.Length));
        //Debug.Log("Cond1: " + this.featureVector.Length + " " + featureVector.Length);
        if (this.featureVector.Length != featureVector.Length)
        {
            return false;
        }

        for (int i = 0; i < featureVector.Length; i++)
        {
            //Debug.Log("Cond2: " + (this.featureVector[i] != featureVector[i]));
            if (this.featureVector[i] != featureVector[i])
            {
                return false;
            }
        }

        return true;
    }
    public static bool operator ==(MultiformSpriteVariant lhs, MultiformSpriteVariant rhs)
    {
        return lhs.CompareFeatureVector(rhs.FeatureVector);
    }
    public static bool operator !=(MultiformSpriteVariant lhs, MultiformSpriteVariant rhs)
    {
        return !(lhs == rhs);
    }

    public static MultiformSpriteVariant Create(Sprite sprite, short[] featureVector)
    {
        MultiformSpriteVariant variant = new MultiformSpriteVariant();  
        variant.spriteVariant = sprite;
        variant.featureVector = featureVector;
        //Debug.Log("Variant: " + variant);
        return variant;
    }

    public override bool Equals(object obj)
    {
        return obj is MultiformSpriteVariant variant &&
               base.Equals(obj) &&
               EqualityComparer<Sprite>.Default.Equals(spriteVariant, variant.spriteVariant) &&
               EqualityComparer<short[]>.Default.Equals(featureVector, variant.featureVector);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), spriteVariant, featureVector);
    }
}
