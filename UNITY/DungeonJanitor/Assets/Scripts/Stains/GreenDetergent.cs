using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IGreenDetergentTarget
{
    void ApplyDetergent(GreenDetergent detergent);
}

public class GreenDetergent : ADetergent
{
    #region SCRIPT_PROPERTIES

    [Header("GREEN DETERGENT"), Space(10)]
    [SerializeField]
    private float affectingTime = 0.5f;
    [SerializeField]
    private int factor = 2;

    #endregion


    #region SCRIPT_FIELDS


    private PolygonCollider2D affectedArea = null;
    [SerializeField]
    private float actionMovementSlowFactor = 0.2f;
    private ModifierHandlesGroup handles = new ModifierHandlesGroup();

    //private Animator animator;



    #endregion

    #region COROUTINES

    private IEnumerator coroutineEndAffecting = null;

    float timer = 0.0f;


    private IEnumerator EndAffecting()
    {
        yield return new WaitForSeconds(affectingTime);

        StopSpraying();
    }


    private void StopSpraying()
    {
        //Debug.Log("SprayRedEnd1");
        coroutineEndAffecting = null;
        affectedArea.enabled = false;
        used = false;
        handles.RemoveSelf();
        CallOnEndUsing();
        //Debug.Log("SprayRedEnd2");
    }

    #endregion


    public override void EndUsing()
    {
    }

    public override void Use()
    {
        if (!used)
        {
            used = true;
            affectedArea.enabled = true;

            CallOnBeginUsing();
            //Debug.Log("Using GD");
            if (coroutineEndAffecting != null)
            {
                StopCoroutine(coroutineEndAffecting);
            }
            coroutineEndAffecting = EndAffecting();
            //StartCoroutine(coroutineEndAffecting);

            handles = transform.parent.GetComponent<ModifiersManagerComponent>().AddModifierForPropertyName("actionMovementSlow", actionMovementSlowFactor);
            StartAnimation();
        }
    }

    protected override void SetupTool()
    {
        base.SetupTool();
        affectedArea = GetComponent<PolygonCollider2D>();
        //animator = GetComponent<Animator>();
    }

    void Awake()
    {
        SetupTool();
    }
    private void FixedUpdate()
    {
        if (coroutineEndAffecting != null)
        {
            timer += Time.deltaTime;

            if (timer >= affectingTime)
            {
                timer = 0.0f;
                StopSpraying();
            }
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (used)
        {
            ASubstainType[] substains = collision.GetComponents<ASubstainType>();

            //Debug.Log("NRD obj:" + (substains != null));
            if (substains != null)
            {
                //Debug.Log("NRD type:" + substains);

                foreach (ASubstainType substain in substains)
                {
                    substain.ApplyDetergent(this);
                }
            }
        }
    }

    #region ANIMATION_CONTROL

    private void StartAnimation()
    {
        animator.Play("Spraying.Spraying", 0, 0.0f);
    }

    #endregion
}
