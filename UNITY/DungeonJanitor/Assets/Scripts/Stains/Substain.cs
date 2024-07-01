using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubstain
{
    void Init(Sprite sprite, Vector2 colliderSize);
    void Init(Vector2 colliderSize, Rect rectInPixels);
}

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Substain : MonoBehaviour, ISubstain
{
    #region SCRIPT_PROPERTIES

    //[SerializeField]
    //private float dirtness = 0.0f;
    private int totalDirtness = 2;
    private int dirtness = 2;


    #endregion

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    [SerializeField]
    private MainStain.StainType type;
    public MainStain.StainType Type { get => type; }
    private int cleaningFactor = 1;

    #region EVENTS_AND_DELEGATES

    public delegate void SubstainEvent(Substain substain);
    public event SubstainEvent OnCleaned;

    #endregion

    #region COROUTINES

    private IEnumerator coroutineEndBonusCleaningFactor = null;

    private void StartBonusCleaningFactor(int factor, float time)
    {
        if (coroutineEndBonusCleaningFactor != null)
        {
            StopCoroutine(coroutineEndBonusCleaningFactor);
        }

        cleaningFactor = factor;
        coroutineEndBonusCleaningFactor = EndBonusCleaningFactor(time);
        StartCoroutine(coroutineEndBonusCleaningFactor);
    }

    private IEnumerator EndBonusCleaningFactor(float time)
    {
        yield return new WaitForSeconds(time);

        cleaningFactor = 1;
    }

    #endregion

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public void Init(MainStain.StainType type, Sprite sprite, Vector2 colliderSize)
    public void Init(Sprite sprite, Vector2 colliderSize)
    {
        //spriteRenderer.sprite = sprite;
        //boxCollider.size = colliderSize;
        this.type = MainStain.StainType.Red;
        GetComponent<SpriteRenderer>().sprite = sprite;
        GetComponent<BoxCollider2D>().size = colliderSize;
    }


    public void Clean()
    {
        dirtness -= 1 * cleaningFactor;
        spriteRenderer.color = new Color(1f, 1f, 1f, (dirtness / (float) totalDirtness) * 0.8f + 0.2f); //w alfie may * 0.8f + 0.2f, aby mimo niskiej wartoœci dirtiness/bliskoœæ do wyczyszczenia plama ca³y czas by³a widoczna

        if (dirtness <= 0)
        {
            OnCleaned?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void AddCleaningBonusFactor(int factor, float time)
    {
        //Debug.Log("RD AddedFactor");
        StartBonusCleaningFactor(factor, time);
    }

    public void Init(Vector2 colliderSize, Rect rectInPixels)
    {
        //throw new System.NotImplementedException();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //Debug.Log("SubstainParent: " + transform.parent.gameObject);
    //    transform.parent.gameObject.SendMessage("OnChildTriggerEnter2D", collision, SendMessageOptions.DontRequireReceiver);
    //}
    //
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    //Debug.Log("SubstainParent: " + transform.parent.gameObject);
    //    transform.parent.gameObject.SendMessage("OnChildTriggerExit2D", collision, SendMessageOptions.DontRequireReceiver);
    //}
}
