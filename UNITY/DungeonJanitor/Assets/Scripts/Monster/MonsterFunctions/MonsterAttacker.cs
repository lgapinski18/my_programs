using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterAttacker;

public class AttackArgs
{
    public readonly GameObject source;
    public readonly AttackEffect[] attackEffects;

    public AttackArgs(GameObject source, AttackEffect[] attackEffects)
    {
        this.source = source;
        this.attackEffects = attackEffects;
    }
}

public interface IAttackable
{

    public void Attack(AttackArgs args);
}

public class MonsterAttacker : MonsterFunctionTargetting, IMonsterPursuitUser
{

    #region SCRIPT_PROPERTIES

    [Header("Attack function settings")]
    [SerializeField]
    private float attackTime = 0.1f;

    [SerializeField]
    private float angerRadius = 1f;

    [SerializeField]
    private bool slowOnAttack = false;
    [SerializeField, ConditionalShowProperty("slowOnAttack"), Range(0.0f, 1.0f)]
    public float slownModificator = 1.0f;
    [SerializeField, ConditionalShowProperty("slowOnAttack")]
    public float slownEffectTime = 1.0f;

    [SerializeField]
    private bool shortenLoosingInterest = false;
    [SerializeField, ConditionalShowProperty("shortenLoosingInterest"), Range(0.0f, 1.0f)]
    public float loosingInterestModificator = 1.0f;
    [SerializeField, ConditionalShowProperty("shortenLoosingInterest")]
    public float loosingInterestEfectTime = 10.0f;

    [SerializeField]
    private AttackEffect[] attackEffects = new AttackEffect[0];

    #endregion


    #region SCRIPT_FIELDS

    private IAttackable attackableTarget = null;

    private MonsterPursuitComponent pursuitComponent = null;

    private bool isAttacking = false;

    #endregion


    #region EVENTS AND DELEGATES

    public delegate void AttackEvent();
    public event AttackEvent attackEevent;

    #endregion


    #region MONSTER_FUNCTION_MANAGEMENT

    protected override void SetupMonsterFunction()
    {
        base.SetupMonsterFunction();
    }

    protected override void StartPerformingAction(MonsterFunctionAction action)
    {
        actionTarget = action.Target;
        attackableTarget = actionTarget.GetComponent<IAttackable>();

        pursuitComponent.enabled = true;
        pursuitComponent.SetPursuitTarget(actionTarget);
    }

    protected override void ClearOnEndPerforming()
    {
        base.ClearOnEndPerforming();

        attackableTarget = null;

        if (pursuitComponent != null)
        {
            pursuitComponent.StopMoving();
            pursuitComponent.enabled = false;
        }
        pursuitComponent = null;
    }

    #endregion



    // Start is called before the first frame update
    void Awake()
    {
        SetupMonsterFunction();

        lookingTarget = new LookingTarget(ValidateIsTarget, Callback);

        viewComponent.AddLookingTarget(lookingTarget);

        controller.RegisterFunction(this, MonsterDisruptionBeheviour.RemeberActionTargetPosition, MonsterMissingTargetBeheviour.DoNothing);
    }

    // Update is called once per frame
    void Update()
    {
        if (actionTarget != null && !inCooldown)
        {
            if (Vector3.Distance(transform.position, actionTarget.transform.position) < MaxDistance && !isAttacking)
            {
                //AttackTarget();
                StartCoroutine(StartPerformingFunction(StartPerforming()));
            }
        }
    }

    //#region ATTACKING
    //
    //private void AttackTarget()
    //{
    //    attackableTarget.Attack();
    //    StartCooldown();
    //}
    //
    //#endregion

    #region VIEW_PART
    public bool ValidateCanPerform(ViewComponent viewComponent, GameObject target)
    {
        //Debug.Log("ValidatingCanPerform");
        return (!inCooldown) && controller.CanPerformAction;
    }

    public bool ValidateIsTarget(GameObject potentailTarget)
    {
        return potentailTarget.GetComponent<IAttackable>() != null;
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
            int bonus = (int) (angerRadius - Vector3.Distance(transform.position, target.transform.position)) * 3;
            if (bonus < 0)
            {
                bonus = 0;
            }

            controller.InformAboutPossibleAction(new MonsterFunctionAction(target, this, Priority + bonus, OverpassPriority + bonus));
        }   
    }

    #endregion


    public override float StartPerforming()
    {
        isAttacking = true;
        return attackTime;
    }
    public override void EndPerforming()
    {
        Debug.Log("Attacking: " + attackableTarget + " " + actionTarget);
        if (attackableTarget != null)
        {
            isAttacking = false;
            Debug.Log("Attack!");
            //attackableTarget.Attack(gameObject);
            if (actionTarget.GetComponent<Player>().isDamageable)
            {
                float shakex = Mathf.Abs(actionTarget.transform.position.x - this.transform.position.x);
                float shakey = Mathf.Abs(actionTarget.transform.position.y - this.transform.position.y);
                CameraShake.Shake(1, new Vector3(shakex / (shakex + shakey), shakey / (shakex + shakey), 0));
            }
            attackableTarget.Attack(new AttackArgs(gameObject, (AttackEffect[]) attackEffects.Clone()));
            
            attackEevent?.Invoke();

            pursuitComponent.AddEffect(MonsterPursuitComponent.PursuitEffectType.SPEED, slownModificator, slownEffectTime);
            pursuitComponent.AddEffect(MonsterPursuitComponent.PursuitEffectType.LOSSING_INTEREST, loosingInterestModificator, loosingInterestEfectTime);
        }
        StartCooldown();
    }

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
            pursuitComponent.StopMoving();
            ClearOnEndPerforming();
        }
    }

    public override void DisturbPerformedAction()
    {
        if (IsPerformingAction)
        {
            pursuitComponent.StopMoving();
            ClearOnEndPerforming();
        }
    }

    #region PURSUIT

    public void OnPursuitEndedCallback(MonsterPursuitComponent pursuitComponent)
    {
        pursuitComponent.OnPursuitEnded -= OnPursuitEndedCallback;
        //Debug.Log("PursueEnded");
        ClearOnEndPerforming();

        InvokeOnEndedPerformingAction(this);
    }

    public void SetMovementComponent(MonsterPursuitComponent movementComponent)
    {
        pursuitComponent = movementComponent;
        pursuitComponent.OnPursuitEnded += OnPursuitEndedCallback;
    }

    #endregion
}
