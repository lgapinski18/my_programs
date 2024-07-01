using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MultiformSpriteExtractor : ScriptableObject
{
    [SerializeField]
    private MultiformSpriteVariant[] multiformSpriteVariants = null;

    //[SerializeField, Tooltip("This value will be default value if specific variance is missing in MultiformSpriteVariants Array.")]
    //private Sprite defaultValue = null;
    //
    //[SerializeField]
    //private int numberOfFeatures = 8;
    //[SerializeField]
    //private int[] featuresVariantsCounts = null;
    //
    //private Sprite[] spriteSelectionArray = null;


    private void Awake()
    {
        //priteSelectionArray = (Sprite[])Array.CreateInstance(typeof(Sprite), featuresVariantsCounts);
        //
        //rray.Fill<Sprite>(spriteSelectionArray, defaultValue);
        //
        //oreach (MultiformSpriteVariant variant in multiformSpriteVariants)
        //
        //   spriteSelectionArray.SetValue(variant.SpriteVariant, variant.FeatureVector);
        //
    }

    public Sprite ExtractVariant(short[] featuresVector)
    {
        //return (Sprite) spriteSelectionArray.GetValue(featuresVector);
        //Debug.Log("Extractor: " + multiformSpriteVariants.Length + " " + featuresVector.Length);
        foreach (MultiformSpriteVariant variant in multiformSpriteVariants)
        {
            if (variant.CompareFeatureVector(featuresVector))
            {
                //Debug.Log("Sprite: " +  variant.SpriteVariant);
                return variant.SpriteVariant;
            }
        }
        return null;
    }
}
