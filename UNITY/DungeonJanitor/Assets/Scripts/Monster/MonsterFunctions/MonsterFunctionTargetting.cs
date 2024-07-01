using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(MonsterController))]
public abstract class MonsterFunctionTargetting : MonsterFunction
{
    #region NESTED_TYPES

    public class MonsterFunctionAction
    {
        private readonly GameObject target;
        private readonly MonsterFunctionTargetting monsterFunction;
        private readonly int priority;
        private readonly int disturbPriority;

        public MonsterFunctionAction(GameObject target, MonsterFunctionTargetting monsterFunction, int priority, int disturbPriority)
        {
            this.target = target;
            this.monsterFunction = monsterFunction;
            this.priority = priority;
            this.disturbPriority = disturbPriority;
        }

        public GameObject Target { get => target; }
        public MonsterFunctionTargetting MonsterFunction { get => monsterFunction; }
        public int Priority { get => priority; }

        public int DisturbPriority { get => disturbPriority; }

        public void ExecuteAction()
        {
            monsterFunction.StartPerformingAction(this);
        }
    }

    #endregion

    #region SCRIPT_PROPERTIES

    [Header("General Targetting Properties"), Space(10)]
    [SerializeField, Tooltip("Maximal distance from target to perform action")]
    private float maxDistance = 0.2f;
    public float MaxDistance { get => maxDistance; }

    [SerializeField, Tooltip("Cooldown time after completing function before, it would be possible to perform this function again.")]
    private float actionCooldown = 1.0f;
    public float ActionCooldown { get => actionCooldown; }

    protected bool inCooldown = false;

    [SerializeField]
    private int priority = 0;
    public int Priority { get => priority; }
    [SerializeField]
    private int overpassPriority = 0;
    public int OverpassPriority { get => overpassPriority; }

    [SerializeField]
    protected readonly MonsterDisruptionBeheviour disruptionBeheviour;

    [SerializeField]
    protected readonly MonsterMissingTargetBeheviour missingTargetBeheviour;
    


    #endregion



    protected LookingTarget lookingTarget;

    //Action target
    protected GameObject actionTarget = null;
    public GameObject ActionTarget { get => actionTarget; }

    public bool IsPerformingAction { get => actionTarget != null; }

    protected ViewComponent viewComponent;

    protected bool isFollowingToTarget = false;


    #region EVENTS_AND_DELEGATES

    public delegate void MonsterFunctionTargettingEvent(MonsterFunctionTargetting monsterFunction);

    public event MonsterFunctionTargettingEvent OnMissingTarget;

    public event MonsterFunctionTargettingEvent OnArrivedAtTargetDestination;

    public event MonsterFunctionTargettingEvent OnStartedPerformingAction;
    public event MonsterFunctionTargettingEvent OnEndedPerformingAction;


    #endregion

    #region MONSTER_FUNCTION_MANAGEMENT


    protected override void SetupMonsterFunction()
    {
        base.SetupMonsterFunction();
        viewComponent = gameObject.GetComponentInChildren<ViewComponent>();
    }

    protected override void ClearOnEndPerforming()
    {
        base.ClearOnEndPerforming();
        actionTarget = null;
        isFollowingToTarget = false;
    }

    protected abstract void StartPerformingAction(MonsterFunctionAction action);

    protected void StartCooldown()
    {
        inCooldown = true;
        StartCoroutine(CooldownCoroutine(ActionCooldown));
    }

    private IEnumerator CooldownCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        inCooldown = false;
    }


    #endregion

    //Informing MonsterController about that monsterFunction found target on which action can be performed
    //public delegate bool OnTargetFound(MonsterFunctionCorpseTargetting function, GameObject target);
    //private OnTargetFound onTargetFound;
    //public void SetOnTargetFoundCallback(OnTargetFound callback)
    //{
    //    onTargetFound = callback;
    //}
    //protected bool InvokeOnTargetFound(MonsterFunctionCorpseTargetting function, GameObject target)
    //{
    //    if (onTargetFound != null)
    //    {
    //        return onTargetFound.Invoke(function, target);
    //    }
    //    return false;
    //}

    protected IEnumerator StartPerformingFunction(float time)
    {
        yield return new WaitForSeconds(time);
        EndPerforming();
    }

    protected void InformControllerAboutPossibleAction(MonsterFunctionAction action)
    {
        controller.InformAboutPossibleAction(action);
    }

    protected void InvokeOnStartedPerformingAction(MonsterFunctionTargetting function)
    {
        OnStartedPerformingAction?.Invoke(function);
    }
    protected void InvokeOnEndedPerformingAction(MonsterFunctionTargetting function)
    {
        OnEndedPerformingAction?.Invoke(function);
    }
    protected void InvokeOnMissingTarget(MonsterFunctionTargetting function)
    {
        //Debug.Log("Invoking Missing INV");
        OnMissingTarget?.Invoke(function);
    }
}
