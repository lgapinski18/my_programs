using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SpritesheetVariantFeature
{
    #region NESTED_TYPES

    [Serializable]
    public class SpritesheetVariantFeatureValueProber
    {
        [SerializeField]
        private Vector2Int probingOffset = Vector2Int.zero;
        [SerializeField]
        private Color32[] expectedColors = null;
        [SerializeField]
        private short value = 0;

        public short Value { get => value; }

        public bool HasThisValue(Sprite sprite)
        {
            int posX = (int) sprite.rect.x + probingOffset.x;
            int posY = (int) sprite.rect.y + probingOffset.y;
            //Texture2D texture = duplicateTexture(sprite.texture);
            Texture2D texture = sprite.texture;
            //Color32 color = texture.GetPixel(probingOffset.x, probingOffset.y);
            Color32 color = texture.GetPixel(posX, posY);
            //color = texture.GetPixels32()[probingOffset.x + probingOffset.y * texture.width];
            //color = texture.GetPixels32()[posX + posY * texture.width];
            //Debug.Log("Extractor: TSizeW: " + texture.width);
            //Debug.Log("Extractor: TSizeH: " + texture.height);
            //Debug.Log("Extractor: TSizeX: " + probingOffset.x);
            //Debug.Log("Extractor: TSizeY: " + probingOffset.y);
            //Debug.Log("Extractor: TPosX: " + posX);
            //Debug.Log("Extractor: TPosY: " + posY);

            foreach (Color32 expectedColor in expectedColors)
            {

                //Debug.Log("Extractor: ValueE: " + expectedColor);
                //Debug.Log("Extractor: ValueC: " + color);
                //Debug.Log("Extractor: ValueP: " + (expectedColor.Equals(color)));
                //if (ColorClose(expectedColor, color, 3))
                if (ColorClose(expectedColor, color, 5))
                {
                    return true;
                }
            }
            return false;
        }
        private Texture2D duplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.sRGB);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
        private bool ColorClose(Color32 color1, Color32 color2, int treshold)
        {
            int rd = color2.r - color1.r;
            int gd = color2.g - color1.g;
            int bd = color2.b - color1.b;

            return (rd * rd + gd * gd + bd * bd) <= (treshold * treshold);
        }
    }

    #endregion

    #region SCRIPT_PROPERTIES

    [SerializeField]
    private SpritesheetVariantFeatureValueProber[] probers = null;
    [SerializeField]
    private short defaultValue = 0;

    #endregion


    public short SpecifyFeatureValue(Sprite sprite)
    {
        foreach (SpritesheetVariantFeatureValueProber prober in probers)
        {
            if (prober.HasThisValue(sprite))
            {
                return prober.Value;
            }
        }

        return defaultValue;
    }
}
