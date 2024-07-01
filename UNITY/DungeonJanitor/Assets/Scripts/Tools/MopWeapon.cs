using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

[RequireComponent(typeof(BoxCollider2D))]
public class MopWeapon : AbstractTool, IStainAffected
{

    #region SCRIPT_PROPERTIES

    [SerializeField]
    private float cleaningDamage = 1.0f;
    [SerializeField]
    private float minRange = 0.1f;
    [SerializeField]
    private float maxRange = 0.4f;
    //[SerializeField]
    //private float width = 0.4f;
    [SerializeField]
    private uint steps = 3;
    [SerializeField]
    private float interpolationTime = 0.2f;

    [SerializeField]
    private Vector2 mopSize = new Vector2(0.1f, 0.4f);
    [SerializeField]
    private float actionMovementSlowFactor = 0.2f;

    #endregion


    #region SCRIPT_FIELDS

    private ModifiersManager actionSpeedModifierManager = null;
    private float actionSpeedModifier = 1.0f;
    //INTERPOLATION
    private float deltaRange;
    private float interpolationInterval;
    //private Vector2 originColliderSize = Vector2.zero;
    //private Vector2 deltaColliderSize = Vector2.zero;
    private Vector2 originColliderOffset = Vector2.zero;
    private Vector2 deltaColliderOffset = Vector2.zero;

    //COMPONENTS
    private BoxCollider2D boxCollider;

    //MODIFING
    //ListHandle
    //List<ModifiersManager.ModifierHandle> handles = new List<ModifiersManager.ModifierHandle>();
    private ModifierHandlesGroup handles = new ModifierHandlesGroup();

    #endregion

    #region COROUTINES

    private IEnumerator coroutineRangeInterpolation = null;
    private IEnumerator RangeInterpolatingCoroutine(float interval)
    {
        for (int i = 0; i < steps; i++)
        {
            //Debug.Log("Mod: Interval: " + interval);
            //Debug.Log("Mod: Interval: " + interpolationInterval);
            //Debug.Log("Mod: actionSpeedModifier: " + actionSpeedModifier);
            yield return new WaitForSeconds(interval * actionSpeedModifier);
            //boxCollider.size += deltaColliderSize;
            boxCollider.offset += deltaColliderOffset;
        }
        yield return new WaitForSeconds(interval * actionSpeedModifier);
        EndSweeping();
    }

    #endregion

    protected override void SetupTool()
    {
        base.SetupTool();

        boxCollider = GetComponent<BoxCollider2D>();
        //animator = GetComponent<Animator>();
        actionSpeedModifierManager = new ModifiersManager(ModifiersManager.ModifiersProcessingType.Max, this);
        actionSpeedModifierManager.OnChanged += SetActionSpeedModifier;
        transform.parent.GetComponent<ModifiersManagerComponent>().RegisterModifiersManager("actionSpeed", actionSpeedModifierManager);
        GetComponent<ModifiersManagerComponent>().RegisterModifiersManager("actionSpeed", actionSpeedModifierManager);

        if (steps != 0)
        {
            deltaRange = (maxRange - minRange) / steps;
            interpolationInterval = interpolationTime / steps;
        }
        else
        {
            deltaRange = maxRange - minRange;
            interpolationInterval = interpolationTime;
        }
        //originColliderSize = boxCollider.size;
        //deltaColliderSize = new Vector2(deltaRange, 0.0f);
        //deltaColliderOffset = new Vector2(deltaRange * 0.5f, 0.0f);

        //originColliderOffset = boxCollider.offset;

        boxCollider.size = mopSize;
        boxCollider.offset = new Vector2(minRange, 0.0f);
        originColliderOffset = boxCollider.offset;

        deltaColliderOffset = new Vector2(deltaRange, 0.0f);
    }

    private void SetActionSpeedModifier(ModifiersManager modifiersManager)
    {
        //Debug.Log("SettgingModifier: ActionSpeedMW: " + modifiersManager.GetModifier());
        actionSpeedModifier = modifiersManager.GetModifier();
        animator.speed = 1 / actionSpeedModifier;
    }

    void Start()
    {
        //Debug.Log("Mod: StartingMopWaepon");
        SetupTool();
    }

    public override void EndUsing()
    {

    }

    public override void Use()
    {
        StartSweeping();
    }

    private void StartSweeping()
    {
        if (!used)
        {
            used = true;
            boxCollider.enabled = true;
            CallOnBeginUsing();
            animator.Play("Using");
            //Debug.Log("Isung mop");

            coroutineRangeInterpolation = RangeInterpolatingCoroutine(interpolationInterval);
            StartCoroutine(coroutineRangeInterpolation);

            //ListHandle
            //handles.AddRange(transform.parent.GetComponent<ModifiersManagerComponent>().AddModifierForPropertyName("actionMovementSlow", actionMovementSlowFactor));
            handles = transform.parent.GetComponent<ModifiersManagerComponent>().AddModifierForPropertyName("actionMovementSlow", actionMovementSlowFactor);
        }
    }

    private void EndSweeping()
    {
        //boxCollider.size = originColliderSize;
        boxCollider.offset = originColliderOffset;
        CallOnEndUsing();

        //ListHandle
        //foreach (var modifier in handles)
        //{
        //    transform.parent.GetComponent<ModifiersManagerComponent>().RemoveModifier(modifier);
        //}
        //handles.Clear();
        handles.RemoveSelf();
        boxCollider.enabled = false;
        used = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log("Try Mop Used");
        if (used)
        {
            //Debug.Log("Mop Used");
            Substain substain = collision.GetComponent<Substain>();
            if (substain != null)
            {
                substain.Clean();
            }


            SubstainController substainController = collision.GetComponent<SubstainController>();
            if (substainController != null)
            {
                substainController.Clean(cleaningDamage);
            }
        }
    }
}
