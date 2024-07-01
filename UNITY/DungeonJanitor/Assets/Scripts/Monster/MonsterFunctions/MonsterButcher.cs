using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MonsterButcher;
using static MonsterController;

public class MonsterButcher : MonsterFunctionCorpseTargetting, IMonsterMovementUser
{
    // Start is called before the first frame update
    //private LookingTarget lookingTarget;

    private MonsterMovementComponent movementComponent = null;

    #region MONSTER_FUNCTION_MANAGEMENT

    protected override void SetupMonsterFunction()
    {
        base.SetupMonsterFunction();
    }
    protected override void ClearOnEndPerforming()
    {
        OnButcherEndedEvent?.Invoke();

        base.ClearOnEndPerforming();
        //if (IsPerformingAction)
        //{
        //}
        //movementComponent.StopMoving();
        //movementComponent.enabled = false;
        movementComponent = null;
    }

    #endregion

    #region EVENTS AND DELEGATES

    public delegate void ButcherEvent();
    public event ButcherEvent OnButcherStartEvent;
    public event ButcherEvent OnButcherEndedEvent;

    #endregion

    void Awake()
    {
        SetupMonsterFunction();

        controller.RegisterFunction(this, disruptionBeheviour, missingTargetBeheviour);

        lookingTarget = new LookingTarget(ValidateIsTarget, Callback);
        viewComponent.AddLookingTarget(lookingTarget);

    }

    // Update is called once per frame
    void Update()
    {
        if (actionTarget != null)
        {
            if (!actionTarget.IsDestroyed())
            {
                if (!isStartedPerforming && Vector3.Distance(transform.position, actionTarget.transform.position) <= MaxDistance)
                {
                    //Debug.Log("FollowingReached");
                    movementComponent.StopMoving();
                    InvokeOnStartedPerformingAction(this);
                    //controllerState = MonsterControllerState.PerfomingFunction;
                    //float time = performedFunction.StartPerforming();

                    StartCoroutine(StartPerformingFunction(StartPerforming()));

                }
            }
        }
        else
        {
            if (isFollowingToTarget)
            {
                ClearOnEndPerforming();
                InvokeOnMissingTarget(this);
            }

        }
    }
    public override float StartPerforming()
    {
        OnButcherStartEvent.Invoke();
        //Debug.Log("StartPerforming");
        isStartedPerforming = true;
        Corpse corpse = actionTarget.GetComponent<Corpse>();
        float performanceTime = corpse.ButcheringTime;

        return performanceTime;
    }

    public override void EndPerforming()
    {
        //Debug.Log("EndPerforming");

        if (actionTarget != null)
        {
            actionTarget.GetComponent<Corpse>().ButcherCorpse();
        }
        //actionTarget = null;
        ClearOnEndPerforming();

        isStartedPerforming = false;

        StartCooldown();

        InvokeOnEndedPerformingAction(this);
    }


    #region VIEW_TARGET_METHODS

    public bool ValidateCanPerform(ViewComponent viewComponent, GameObject target)
    {
        //Debug.Log("ValidatingCanPerform");
        return (!inCooldown) && controller.CanPerformAction;
    }

    public bool ValidateIsTarget(GameObject potentailTarget)
    {
        Corpse corpse = potentailTarget.GetComponent<Corpse>();
        //Debug.Log("C1: " + ((corpse.Size & ResultCorpseSizeMask) != 0));
        //Debug.Log("C2: " + ((corpse.Type & ResultCorpseTypeMask) != 0));
        //Debug.Log("TM: " + ResultCorpseTypeMask);
        bool result = corpse != null
                    && ((corpse.Size & ResultCorpseSizeMask) != 0)
                    && ((corpse.Type & ResultCorpseTypeMask) != 0);

        return result;
    }

    private void Callback(ViewComponent viewComponent, GameObject target)
    {
        //Debug.Log("Function Callback");
        if (ValidateCanPerform(viewComponent, target))
        {
            //OnCanPerform(this, target);
            //if (InvokeOnTargetFound(this, target))
            //{
            //    actionTarget = target;
            //}
            InformControllerAboutPossibleAction(new MonsterFunctionAction(target, this, Priority, OverpassPriority));
        }
    }

    #endregion


    public override bool CanBeCanceled()
    {
        return true;
    }

    public override bool CanBeDisturbed()
    {
        return true;
    }

    public override void CancelPerformedAction()
    {
        if (IsPerformingAction)
        {
            ClearOnEndPerforming();

        }
    }

    public override void DisturbPerformedAction()
    {
        if (IsPerformingAction)
        {
            ClearOnEndPerforming();

        }
    }

    protected override void StartPerformingAction(MonsterFunctionAction action)
    {
        actionTarget = action.Target;
        isFollowingToTarget = true;
        movementComponent.enabled = true;
        //movementComponent.SetDestination(actionTarget.transform.position);

        reactionsController.MakeReaction("NoticingCorpse");
        movementComponent.SetTarget(actionTarget);
    }

    public void SetMovementComponent(MonsterMovementComponent movementComponent)
    {
        this.movementComponent = movementComponent;
    }
}
