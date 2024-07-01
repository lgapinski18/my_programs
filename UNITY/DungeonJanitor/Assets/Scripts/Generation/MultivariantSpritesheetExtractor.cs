using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.InputSystem.Android.LowLevel.AndroidGameControllerState;

[CreateAssetMenu]
public class MultivariantSpritesheetExtractor : ScriptableObject
{
    #region SCRIPT_PROPERTIES

    //[SerializeField]
    //private Sprite spritesheet = null;

    [SerializeField, Tooltip("Passed Sprites should be set to enable Read/Write, Filter Mode to Point and Format to RGBA32")]
    private Sprite[] variantSpritesSet = null;

    //[SerializeField]
    //private Vector2Int size = Vector2Int.zero;
    //
    //[SerializeField]
    //private int number;

    [SerializeField]
    private SpritesheetVariantFeature[] features = null;

    #endregion

    [SerializeField]
    private List<MultiformSpriteVariant> variants; // = new List<MultiformSpriteVariant>();


    void Awake()
    {
        //Debug.Log("Extractor: OnAwake Awake");
        CreateVariantsBasingOnSpritesheet();
    }

    public Sprite ExtractVariant(short[] featuresVector)
    {
        //return (Sprite) spriteSelectionArray.GetValue(featuresVector);
        //Debug.Log("Extractor: " + variants.Count + " " + featuresVector.Length);
        foreach (MultiformSpriteVariant variant in variants)
        {
            //Debug.Log("Extractor2: " + variants.Count + " " + featuresVector.Length);
            if (variant.CompareFeatureVector(featuresVector))
            {
                //Debug.Log("Sprite: " + variant.SpriteVariant);
                return variant.SpriteVariant;
            }
        }
        return null;
    }

    [ContextMenu(nameof(CreateVariantsBasingOnSpritesheet))]
    public void CreateVariantsBasingOnSpritesheet()
    {

        //Debug.Log("Extractor: OnAwake");
        variants = new List<MultiformSpriteVariant>();
        //variants.Clear();
        
        //
        //Rect extractingRect = Rect.zero;
        //extractingRect.width = size.x;
        //extractingRect.height = size.y;
        //
        ////Vector2 center = extractingRect.center;
        //Vector2 center = new Vector2(size.x * 0.5f, size.y * 0.5f);
        //Texture2D texture = spritesheet.texture;
        //
        //int number = (spritesheet.texture.width / size.x) * (spritesheet.texture.height / size.y);
        //
        //for (int i = 0; i < number; i++)
        //{
        //    Sprite extractedSprite = Sprite.Create(texture, extractingRect, center);
        //    short[] featuresVector = new short[features.Length];
        //    for (int j = 0; j < features.Length; j++)
        //    {
        //        featuresVector[j] = features[j].SpecifyFeatureValue(extractedSprite);
        //    }
        //
        //    variants.Add(MultiformSpriteVariant.Create(extractedSprite, featuresVector));
        //
        //    extractingRect.x += size.x;
        //    if (extractingRect.x >= spritesheet.texture.width)
        //    {
        //        extractingRect.x = 0;
        //        extractingRect.y += size.y;
        //    }
        //    else if (extractingRect.y >= spritesheet.texture.height)
        //    {
        //        break;
        //    }
        //    string content = "";
        //    foreach(var feature in featuresVector)
        //    {
        //        content += feature.ToString() + ", ";
        //    }
        //    Debug.Log("Extractor: FV: " + content);
        //}
        foreach (Sprite sprite in variantSpritesSet)
        {
            short[] featuresVector = new short[features.Length];
            for (int j = 0; j < features.Length; j++)
            {
                featuresVector[j] = features[j].SpecifyFeatureValue(sprite);
            }

            variants.Add(MultiformSpriteVariant.Create(sprite, featuresVector));

            string content = "";
            foreach (var feature in featuresVector)
            {
                content += feature.ToString() + ", ";
            }
            //Debug.Log("Extractor: FV: " + content);
        }
    }
}
