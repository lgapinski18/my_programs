using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

//[ExecuteAlways]
public class MainStain : MonoBehaviour
{
    #region NESTED_TYPES

    [System.Flags]
    public enum StainType
    {
        Red     =   0b100, 
        Green   =   0b010,
        Blue    =   0b001
        //Yellow  =   0b110,
        //Magenta =   0b101,
        //Cyan    =   0b011
    }

    #endregion


    [Header("Stain Settings"), Space(10)]
    [SerializeField] 
    StainType type;
    [SerializeField]
    private Texture2D stainTexture;
    [SerializeField]
    private Sprite stainSprite;

    [SerializeField]
    private int pixelsPerUnit = 0;
    //[SerializeField]
    //private Vector2Int numberOfElements;
    [SerializeField]
    private Vector2Int substainSize = Vector2Int.zero;

    [SerializeField]
    private float minScale = 1.0f;
    [SerializeField]
    private float maxScale = 1.5f;

    [SerializeField]
    private float factorToClean = 0.65f;
    //[SerializeField]
    //private Sprite[] slicedStainTexture;
    //[SerializeField]
    //private int columnsNumber;
    //[SerializeField]
    //private int rowsNumber;

    [SerializeField]
    private GameObject substainPrefab = null;

    [SerializeField, Tooltip("These are layers which stain shouldn't overlap.")]
    private LayerMask collisionLayers;

    private int totalSubstains = 0;
    public int TotalSubstains { get => totalSubstains; }

    private int cleanedSubstains = 0;
    public int CleanedSubstains { get => cleanedSubstains; }

    private SpriteRenderer spriteRenderer;


    #region EVENTS_AND_DELEGATES

    public delegate void StainEvent(MainStain substain);
    public event StainEvent OnCleaned;

    public event SubstainController.SubstainControllerEvent OnSubstainCleaned;

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        
        CreateSubtains();

    }

    void Start()
    {
        Reposition();


        StainsCleaningTaskManager.Instance().AddToNumberOfSubstains(totalSubstains);
        OnSubstainCleaned += (SubstainController SubstainController) =>
        {
            StainsCleaningTaskManager.Instance().IncrementNumberOfCleaned();
        };
    }


    [ContextMenu(nameof(RemoveSubstains))]
    private void RemoveSubstains()
    {
        SubstainController[] substains = GetComponentsInChildren<SubstainController>();
        if (substains != null)
        {
            foreach (var sub in substains)
            {
                DestroyImmediate(sub.gameObject);
            }
        }
    }

    [ContextMenu(nameof(CreateSubtains))]
    public void CreateSubtains()
    {
        //foreach (Substain substain in GetComponentsInChildren<Substain>())
        //{
        //    DestroyImmediate(substain.gameObject);
        //}
        RemoveSubstains();

        totalSubstains = 0;


        Texture2D stainTexture = new Texture2D(this.stainTexture.width, this.stainTexture.height, this.stainTexture.format, false);

        // Get the pixels from the source texture
        //Color[] sourcePixels = this.stainTexture.GetPixels();

        // Set the pixels of the copied texture to match the source texture
        stainTexture.SetPixels32(this.stainTexture.GetPixels32());

        // Apply changes to the copied texture
        stainTexture.Apply();

        // Create a Sprite using the copied texture
        Sprite stainSprite = Sprite.Create(stainTexture, new Rect(0, 0, stainTexture.width, stainTexture.height), new Vector2(0.5f, 0.5f));

        // Assign the copied sprite to a SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = stainSprite;

















        //float pixelsPerUnit = stainSprite.pixelsPerUnit;
        Rect currentRect = new Rect();
        currentRect.width = substainSize.x;
        currentRect.height = substainSize.y;

        Vector2 newPivot = new Vector2(0.5f, 0.5f);
        Vector2 center = new Vector2(stainTexture.width / 2, stainTexture.height / 2);

        GameObject newSubstain = null;
        Sprite substainSprite = null;
        for (; currentRect.y < (stainTexture.height - substainSize.y); currentRect.y += substainSize.y)
        //for (int row = 1; row <= rowsNumber; currentRect.y += substainSize.y, row++)
        {
            currentRect.x = 0;
            for (; currentRect.x < (stainTexture.width - substainSize.x); currentRect.x += substainSize.x)
            //for (int col = 0; col < columnsNumber; currentRect.x += substainSize.x, col++)
            {
                substainSprite = Sprite.Create(stainTexture, new Rect(currentRect.position, substainSize), newPivot, pixelsPerUnit, 0, SpriteMeshType.FullRect, new Vector4());
                //substainSprite = slicedStainTexture[(rowsNumber - row) * columnsNumber + col];
                //substainSprite.texture.filterMode = FilterMode.Point;
                //Sprite substainSprite = Sprite.Create(stainSprite.texture, stainSprite.textureRect, stainSprite.pivot, pixelsPerUnit);

                if (SpriteIsFullyInvisible(substainSprite))
                {
                    // newSubstain = Instantiate(substainPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
                    // //newSubstain.transform.parent = transform;
                    // //newSubstain.transform.localPosition = (currentRect.position - stainSprite.pivot) / pixelsPerUnit;
                    // newSubstain.transform.localPosition = (currentRect.position - center) / pixelsPerUnit;
                    // 
                    // //newSubstain.GetComponent<ISubstain>().Init(type, substainSprite, (Vector2)substainSize / pixelsPerUnit);
                    // newSubstain.GetComponent<ISubstain>().Init(substainSprite, (Vector2)substainSize / pixelsPerUnit);
                    // newSubstain.GetComponent<SubstainController>().OnCleaned += SubstainCleaned;

                    InstantiateSubstain(currentRect, center);
                }
                //newSubstain.GetComponent<Substain>().Init(substainSprite, (Vector2) substainSize);
            }

            substainSprite = Sprite.Create(stainTexture, new Rect(currentRect.position,
                new Vector2(stainTexture.width - currentRect.x, stainTexture.height - currentRect.y)), newPivot, pixelsPerUnit, 0, SpriteMeshType.FullRect, new Vector4());

            if (SpriteIsFullyInvisible(substainSprite))
            {

                // newSubstain = Instantiate(substainPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
                // //newSubstain.transform.parent = transform;
                // //newSubstain.transform.localPosition = (currentRect.position - stainSprite.pivot) / pixelsPerUnit;
                // newSubstain.transform.localPosition = (currentRect.position - center) / pixelsPerUnit;
                // 
                // //newSubstain.GetComponent<ISubstain>().Init(type, substainSprite, (Vector2)substainSize / pixelsPerUnit);
                // newSubstain.GetComponent<ISubstain>().Init(substainSprite, (Vector2)substainSize / pixelsPerUnit);
                // newSubstain.GetComponent<SubstainController>().OnCleaned += SubstainCleaned;

                InstantiateSubstain(currentRect, center);

                //totalSubstains++;
            }
        }

        currentRect.x = 0;
        for (; currentRect.x < (stainTexture.width - substainSize.x); currentRect.x += substainSize.x)
        {
            substainSprite = Sprite.Create(stainTexture, new Rect(currentRect.position,
                    new Vector2(substainSize.x, stainTexture.height - currentRect.y)), newPivot, pixelsPerUnit, 0, SpriteMeshType.FullRect, new Vector4());
            //Sprite substainSprite = Sprite.Create(stainSprite.texture, stainSprite.textureRect, stainSprite.pivot, pixelsPerUnit);

            if (SpriteIsFullyInvisible(substainSprite))
            {
                //newSubstain = Instantiate(substainPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
                ////newSubstain.transform.parent = transform;
                ////newSubstain.transform.localPosition = (currentRect.position - stainSprite.pivot) / pixelsPerUnit;
                //newSubstain.transform.localPosition = (currentRect.position - center) / pixelsPerUnit;
                //
                ////newSubstain.GetComponent<ISubstain>().Init(type, substainSprite, (Vector2)substainSize / pixelsPerUnit);
                //newSubstain.GetComponent<ISubstain>().Init(substainSprite, (Vector2)substainSize / pixelsPerUnit);
                //newSubstain.GetComponent<SubstainController>().OnCleaned += SubstainCleaned;

                InstantiateSubstain(currentRect, center);

                //totalSubstains++;
            }
            //newSubstain.GetComponent<Substain>().Init(substainSprite, (Vector2) substainSize);
        }

        substainSprite = Sprite.Create(stainTexture, new Rect(currentRect.position,
            new Vector2(stainTexture.width - currentRect.x, stainTexture.height - currentRect.y)), newPivot, pixelsPerUnit, 0, SpriteMeshType.FullRect, new Vector4());

        if (SpriteIsFullyInvisible(substainSprite))
        {
            //newSubstain = Instantiate(substainPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
            ////newSubstain.transform.parent = transform;
            ////newSubstain.transform.localPosition = (currentRect.position - stainSprite.pivot) / pixelsPerUnit;
            //newSubstain.transform.localPosition = (currentRect.position - center) / pixelsPerUnit;
            ////newSubstain.GetComponent<ISubstain>().Init(type, substainSprite, (Vector2)substainSize / pixelsPerUnit);
            //newSubstain.GetComponent<ISubstain>().Init(substainSprite, (Vector2)substainSize / pixelsPerUnit);
            //newSubstain.GetComponent<SubstainController>().OnCleaned += SubstainCleaned;
            InstantiateSubstain(currentRect, center);
            //totalSubstains++;
        }


        float s = Random.Range(minScale, maxScale);
        //Debug.Log("Scale: " + s);
        transform.localScale = new Vector3(s, s, s);
        //transform.Rotate(0.0f, 0.0f, Random.Range(0, 360));

    }

    private void InstantiateSubstain(Rect rectInPixels, Vector2 stainSpriteCenter)
    {
        GameObject newSubstain = Instantiate(substainPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
        //newSubstain.transform.parent = transform;
        //newSubstain.transform.localPosition = (currentRect.position - stainSprite.pivot) / pixelsPerUnit;
        newSubstain.transform.localPosition = (rectInPixels.position - stainSpriteCenter + new Vector2(2.0f, 2.0f)) / pixelsPerUnit;


        //newSubstain.GetComponent<ISubstain>().Init(type, substainSprite, (Vector2)substainSize / pixelsPerUnit);
        newSubstain.GetComponent<ISubstain>().Init((Vector2)substainSize / pixelsPerUnit, rectInPixels);
        newSubstain.GetComponent<SubstainController>().OnCleaned += SubstainCleaned;
        totalSubstains++;
    }

    private void Reposition()
    {
        float width = spriteRenderer.sprite.textureRect.width / pixelsPerUnit * transform.localScale.x;
        float height = spriteRenderer.sprite.textureRect.height / pixelsPerUnit * transform.localScale.y;

        float deltaX = 0.0f;
        float deltaY = 0.0f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, height * 0.5f, collisionLayers);
        //Debug.DrawRay(transform.position, Vector2.up * height * 0.5f, Color.yellow, 40);
        if (hit.collider != null)
        {
            //Debug.DrawRay(transform.position, Vector2.up * hit.distance, Color.red, 40);
            //Debug.Log("Origin: " + transform.position + "Distance: " + hit.distance + " " + (height * 0.5f));

            deltaY -= (height * 0.5f) - hit.distance;
        }

        hit = Physics2D.Raycast(transform.position, Vector2.down, height * 0.5f, collisionLayers);
        //Debug.DrawRay(transform.position, Vector2.down * height * 0.5f, Color.yellow, 40);
        if (hit.collider != null)
        {
            //Debug.DrawRay(transform.position, Vector2.down * hit.distance, Color.red, 40);
            //Debug.Log("Origin: " + transform.position + "Distance: " + hit.distance + " " + (height * 0.5f));

            deltaY += (height * 0.5f) - hit.distance;
        }

        hit = Physics2D.Raycast(transform.position, Vector2.left, width * 0.5f, collisionLayers);
        //Debug.DrawRay(transform.position, Vector2.left * width * 0.5f, Color.yellow, 40);
        if (hit.collider != null)
        {
            //Debug.DrawRay(transform.position, Vector2.left * hit.distance, Color.red, 40);
            //Debug.Log("Origin: " + transform.position + "Distance: " + hit.distance + " " + (width * 0.5f));

            deltaX += (width * 0.5f) - hit.distance;
        }

        hit = Physics2D.Raycast(transform.position, Vector2.right, width * 0.5f, collisionLayers);
        //Debug.DrawRay(transform.position, Vector2.right * width * 0.5f, Color.yellow, 40);
        if (hit.collider != null)
        {
            //Debug.DrawRay(transform.position, Vector2.right * hit.distance, Color.red, 40);
            //Debug.Log("Origin: " + transform.position + "Distance: " + hit.distance + " " + (width * 0.5f));

            deltaX -= (width * 0.5f) - hit.distance;
        }
        //Debug.Log("Origin: " + transform.position + " dx: " + deltaX + " dy " + deltaY);

        transform.Translate(new Vector3(deltaX, deltaY));

        Vector3 direction = new Vector3(width * 0.5f, height * 0.5f);
        float length = direction.magnitude;
        direction.Normalize();

        
        Vector3 deltaPos = new Vector3();

        hit = Physics2D.Raycast(transform.position, direction, length, collisionLayers);
        if (hit.collider != null)
        {
            deltaPos -= direction * (length - hit.distance);
        }

        direction.x = -direction.x;
        hit = Physics2D.Raycast(transform.position, direction, length, collisionLayers);
        if (hit.collider != null)
        {
            deltaPos -= direction * (length - hit.distance);
        }

        direction.y = -direction.y;
        hit = Physics2D.Raycast(transform.position, direction, length, collisionLayers);
        if (hit.collider != null)
        {
            deltaPos -= direction * (length - hit.distance);
        }

        direction.x = -direction.x;
        hit = Physics2D.Raycast(transform.position, direction, length, collisionLayers);
        if (hit.collider != null)
        {
            deltaPos -= direction * (length - hit.distance);
        }

        transform.Translate(deltaPos);
    }

    //[ContextMenu(nameof(CreateSubstainsUsingELementCount))]
    //public void CreateSubstainsUsingELementCount()
    //{
    //    foreach (Substain substain in GetComponentsInChildren<Substain>())
    //    {
    //        DestroyImmediate(substain.gameObject);
    //    }
    //    float pixelsPerUnit = stainSprite.pixelsPerUnit;
    //    Rect currentRect = new Rect();
    //    currentRect.width = substainSize.x;
    //    currentRect.height = substainSize.y;
    //
    //    Vector2 newPivot = new Vector2(0.5f, 0.5f);
    //    Vector2 center = new Vector2(stainTexture.width / 2, stainTexture.height / 2);
    //
    //    Vector2 size = new Vector2(stainTexture.width / numberOfElements.x, stainTexture.height / numberOfElements.y);
    //
    //    GameObject newSubstain = null;
    //    Sprite substainSprite = null;
    //    for (; currentRect.y < (stainTexture.height - size.y); currentRect.y += size.y)
    //    {
    //        currentRect.x = 0;
    //        for (; currentRect.x < (stainTexture.width - size.x); currentRect.x += size.x)
    //        {
    //            newSubstain = Instantiate(substainPrefab);
    //            newSubstain.transform.parent = transform;
    //            //newSubstain.transform.localPosition = (currentRect.position - stainSprite.pivot) / pixelsPerUnit;
    //            newSubstain.transform.localPosition = (currentRect.position - center) / pixelsPerUnit;
    //
    //            substainSprite = Sprite.Create(stainSprite.texture, new Rect(currentRect.position, size), newPivot, pixelsPerUnit, 0, SpriteMeshType.FullRect, new Vector4());
    //            //Sprite substainSprite = Sprite.Create(stainSprite.texture, stainSprite.textureRect, stainSprite.pivot, pixelsPerUnit);
    //
    //            if (SpriteIsFullyInvisible(substainSprite))
    //            {
    //                newSubstain.GetComponent<Substain>().Init(type, substainSprite, size / pixelsPerUnit);
    //                newSubstain.GetComponent<Substain>().OnCleaned += SubstainCleaned;
    //                totalSubstains++;
    //            }
    //            //newSubstain.GetComponent<Substain>().Init(substainSprite, (Vector2) substainSize);
    //        }
    //
    //        newSubstain = Instantiate(substainPrefab);
    //        newSubstain.transform.parent = transform;
    //
    //        newSubstain.transform.localPosition = (currentRect.position - center) / pixelsPerUnit;
    //
    //        substainSprite = Sprite.Create(stainSprite.texture, new Rect(currentRect.position,
    //            new Vector2(stainTexture.width - currentRect.x, stainSprite.texture.height - currentRect.y)), newPivot, pixelsPerUnit, 0, SpriteMeshType.FullRect, new Vector4());
    //
    //        if (SpriteIsFullyInvisible(substainSprite))
    //        {
    //            newSubstain.GetComponent<Substain>().Init(type, substainSprite, size / pixelsPerUnit);
    //            newSubstain.GetComponent<Substain>().OnCleaned += SubstainCleaned;
    //            totalSubstains++;
    //        }
    //    }
    //
    //    currentRect.x = 0;
    //    for (; currentRect.x < (stainTexture.width - size.x); currentRect.x += size.x)
    //    {
    //        newSubstain = Instantiate(substainPrefab);
    //        newSubstain.transform.parent = transform;
    //        //newSubstain.transform.localPosition = (currentRect.position - stainSprite.pivot) / pixelsPerUnit;
    //        newSubstain.transform.localPosition = (currentRect.position - center) / pixelsPerUnit;
    //
    //        substainSprite = Sprite.Create(stainSprite.texture, new Rect(currentRect.position, 
    //                new Vector2(size.x, stainSprite.texture.height - currentRect.y)), newPivot, pixelsPerUnit, 0, SpriteMeshType.FullRect, new Vector4());
    //        //Sprite substainSprite = Sprite.Create(stainSprite.texture, stainSprite.textureRect, stainSprite.pivot, pixelsPerUnit);
    //
    //        if (SpriteIsFullyInvisible(substainSprite))
    //        {
    //            newSubstain.GetComponent<Substain>().Init(type, substainSprite, size / pixelsPerUnit);
    //            newSubstain.GetComponent<Substain>().OnCleaned += SubstainCleaned;
    //            totalSubstains++;
    //        }
    //        //newSubstain.GetComponent<Substain>().Init(substainSprite, (Vector2) substainSize);
    //    }
    //
    //    newSubstain = Instantiate(substainPrefab);
    //    newSubstain.transform.parent = transform;
    //
    //    newSubstain.transform.localPosition = (currentRect.position - center) / pixelsPerUnit;
    //
    //    substainSprite = Sprite.Create(stainSprite.texture, new Rect(currentRect.position,
    //        new Vector2(stainTexture.width - currentRect.x, stainSprite.texture.height - currentRect.y)), newPivot, pixelsPerUnit, 0, SpriteMeshType.FullRect, new Vector4());
    //
    //    if (SpriteIsFullyInvisible(substainSprite))
    //    {
    //        newSubstain.GetComponent<Substain>().Init(type, substainSprite, size / pixelsPerUnit);
    //        newSubstain.GetComponent<Substain>().OnCleaned += SubstainCleaned;
    //        totalSubstains++;
    //    }
    //}


    private bool SpriteIsFullyInvisible(Sprite sprite)
    {
        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);

        //if (pixels.All(p => p.a < 0.5f))
        if (pixels.All(p => Mathf.Approximately(p.a, 0.0f)))
        {
            return false;
        }
        return true;
    }

    //private void SubstainCleaned(Substain substain)
    //{
    //    //cleanedSubstains++;
    //    //
    //    //
    //    //if (cleanedSubstains == totalSubstains)
    //    //{
    //    //    OnCleaned?.Invoke(this);
    //    //}
    //}

    private void SubstainCleaned(SubstainController substain)
    {

        cleanedSubstains++;

        OnSubstainCleaned?.Invoke(substain);

        if (cleanedSubstains >= (factorToClean * totalSubstains))
        {
            OnCleaned?.Invoke(this);

            Destroy(gameObject);
        }
    }

    public void CleanStain(Rect targetRect, float alpha)
    {
        Debug.Log("Cleaning Main Stain");

        int startX = (int)(targetRect.x);
        int endX = (int)(targetRect.x + targetRect.width);

        int startY = (int)(targetRect.y);
        int endY = (int)(targetRect.y + targetRect.height);

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                Color color = spriteRenderer.sprite.texture.GetPixel(x, y);
                if (color.a > alpha)
                {
                    color.a = alpha;
                    spriteRenderer.sprite.texture.SetPixel(x, y, color);
                }
            }
        }
        spriteRenderer.sprite.texture.Apply();
    }
}
