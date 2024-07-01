using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

#region ENUM_TYPES

public enum MonsterDisruptionBeheviour
{
    DoNothing,
    RemeberActionTargetPosition
}

public enum MonsterMissingTargetBeheviour
{
    DoNothing   = 0b0000_0000,
    GoStraight  = 0b0000_0001,
    LookAround  = 0b0000_0010
}

#endregion

//[ExecuteInEditMode]
public class MonsterController : MonoBehaviour
{
    #region NESTED_SCRIPT_TYPES
    public enum MonsterControllerState
    {
        Walking,
        LookingAround,
        Following,
        PerfomingFunction,
        MissingFunctionTarget
    }

    public enum MonsterFunctionMovementType
    {
        NoMovement,
        NormalMovement,
        Pursuit
    }

    private class RegisterFunctionEntry
    {
        public MonsterFunctionTargetting monsterFunction = null;
        public MonsterDisruptionBeheviour disruptionBeheviour;
        public MonsterMissingTargetBeheviour missingTargetBeheviour;
        public MonsterFunctionMovementType movementType;

        public RegisterFunctionEntry(MonsterFunctionTargetting monsterFunction, 
                                        MonsterDisruptionBeheviour disruptionBeheviour,
                                        MonsterMissingTargetBeheviour missingTargetBeheviour,
                                        MonsterFunctionMovementType movementType)
        {
            this.monsterFunction = monsterFunction;
            this.disruptionBeheviour = disruptionBeheviour;
            this.missingTargetBeheviour = missingTargetBeheviour;
            this.movementType = movementType;
        }
    }


    #endregion

    #region SCRIPT_PROPETIES

    [Header("Patrol Area"), Space(10)]
    [SerializeField]
    float maxDistance = 10.0f;
    [SerializeField]
    int hullParts = 5;
    [SerializeField]
    float middleFactor = 0.4f;
    [SerializeField]
    private float outerIgnoreRadius = 5.0f;
    [SerializeField]
    private float middleIgnoreRadius = 3.0f;

    [Header("Looking Around"), Space(10)]
    [SerializeField]
    private float lookingAroundMinInterval = 10.0f;
    [SerializeField]
    private float lookingAroundMaxInterval = 10.0f;

    #endregion

    private List<RegisterFunctionEntry> registerOfTargetingFunctions = new List<RegisterFunctionEntry>();


    private bool canPerformAction = true;
    public bool CanPerformAction { get => canPerformAction; }

    private MonsterControllerState controllerState = MonsterControllerState.Walking;
    public MonsterControllerState ControllerState { get => controllerState; }

    private MonsterFunctionTargetting performedFunction = null;
    private GameObject functionTarget = null;
    private Vector3 functionTargetPosition = Vector3.zero;

    private float distanceOnMissing = 5.0f;

    private PatrolArea patrolArea = null;

    //Components
    private MonsterMovementComponent movementComponent = null;
    private MonsterPursuitComponent pursuitComponent = null;
    private ViewComponent viewComponent = null;



    private float newDestinationDistance = 0.4f;





    private List<MonsterFunctionTargetting.MonsterFunctionAction> possibleActionList = new List<MonsterFunctionTargetting.MonsterFunctionAction>();

    private MonsterFunctionTargetting.MonsterFunctionAction performedAction = null;

    #region DISRUPTION_BEHEVIOUR
    private Vector3? disruptionRemeberedPosition = null;

    #endregion


    // Start is called before the first frame update
    void Start()
    {

        movementComponent = GetComponent<MonsterMovementComponent>();
        movementComponent.onAchievedDestination += OnAchievedDestination;

        pursuitComponent = GetComponent<MonsterPursuitComponent>();
        if (pursuitComponent != null)
        {
            pursuitComponent.enabled = false;
        }

        viewComponent = gameObject.GetComponentInChildren<ViewComponent>();

        controllerState = MonsterControllerState.Walking;


        //Starting movement by setting up first destination point
        CreateNewPatrolingArea();
        CreateNewDestinationPoint();

        //Setting up looking around for this controller
        viewComponent.onEndedLookingAround += EndedLookingAround;
        StartCoroutine(LookingAroundCoroutine(lookingAroundMinInterval, lookingAroundMaxInterval));
    }

    // Update is called once per frame
    void Update()
    {
        switch (controllerState)
        {
            case MonsterControllerState.Walking:
            case MonsterControllerState.LookingAround:
                {
                    ProcessPossibleActions();
                }
            break;

        }

        //Debug.Log("Following " + (State == MonsterControllerState.Following));
        //if (ControllerState == MonsterControllerState.Following)
        //{
        //    //Debug.Log("Following");
        //    if (!functionTarget.IsDestroyed())
        //    {
        //        if (Vector3.Distance(transform.position, functionTarget.transform.position) <= performedFunction.MaxDistance)
        //        {
        //            //Debug.Log("FollowingReached");
        //            StopMoving();
        //            controllerState = MonsterControllerState.PerfomingFunction;
        //            float time = performedFunction.StartPerforming();
        //
        //            StartCoroutine(StartPerformingFunction(performedFunction, time));
        //
        //        }
        //    }
        //    else
        //    {
        //        controllerState = MonsterControllerState.MissingFunctionTarget;
        //    }
        //}
        //else if (ControllerState == MonsterControllerState.MissingFunctionTarget)
        //{
        //    //Debug.Log("Missing target");
        //    Vector2 direction = viewComponent.FacingDirection * distanceOnMissing;
        //    controllerState = MonsterControllerState.Walking;
        //    movementComponent.SetDestination(transform.position + new Vector3(direction.x, direction.y, 0.0f));
        //}
    }

    #region CONTROLLER_STATE_MACHINE_OPERATION

    private void ProcessPossibleActions()
    {
        if (possibleActionList.Count > 0)
        {
            viewComponent.StopLookingAround();
            MonsterFunctionTargetting.MonsterFunctionAction highestPriorityAction = possibleActionList.First();

            foreach (var action in possibleActionList)
            {
                if (action.Priority > highestPriorityAction.Priority)
                {
                    highestPriorityAction = action;
                }
            }

            possibleActionList.Clear();


            StartPerformingAction(highestPriorityAction);
        }
    }


    private void StartPerformingAction(MonsterFunctionTargetting.MonsterFunctionAction action)
    {
        controllerState = MonsterControllerState.Following;


        performedAction = action;

        switch (registerOfTargetingFunctions.Find(entry => entry.monsterFunction == action.MonsterFunction).movementType)
        {
            case MonsterFunctionMovementType.NoMovement: break;

            case MonsterFunctionMovementType.NormalMovement:
                {
                    (action.MonsterFunction as IMonsterMovementUser).SetMovementComponent(movementComponent);
                }
                break;

            case MonsterFunctionMovementType.Pursuit:
                {
                    movementComponent.StopMoving();
                    movementComponent.enabled = false;
                    (action.MonsterFunction as IMonsterPursuitUser).SetMovementComponent(pursuitComponent);

                }
                break;
        }

        performedAction.ExecuteAction();
    }

    private void MonsterFunctionOnStartedPerforming(MonsterFunctionTargetting monsterFunction)
    {
        controllerState = MonsterControllerState.PerfomingFunction;
    }
    private void MonsterFunctionOnEndedPerforming(MonsterFunctionTargetting monsterFunction)
    {

        controllerState = MonsterControllerState.Walking;
        if (monsterFunction is IMonsterPursuitUser)
        {
            movementComponent.enabled = true;
        }
        EndPerformingAction();
    }
    private void EndPerformingAction()
    {
        performedAction = null;
        if (disruptionRemeberedPosition == null)
        {
            CreateNewDestinationPoint();
        }
        else
        {
            movementComponent.SetDestination((Vector3) disruptionRemeberedPosition);
            disruptionRemeberedPosition = null;
        }
    }

    private void MonsterFunctionTargetMissing(MonsterFunctionTargetting monsterFunction)
    {
        //Debug.Log("Invoking Missing Contr");
        if (monsterFunction != null)
        {

            controllerState = MonsterControllerState.Walking;
            RegisterFunctionEntry registerEntry = registerOfTargetingFunctions.Find(registerEntry => { return registerEntry.monsterFunction == monsterFunction; });
            switch (registerEntry.missingTargetBeheviour)
            {
                case MonsterMissingTargetBeheviour.DoNothing:
                    CreateNewDestinationPoint();
                    break;
                case MonsterMissingTargetBeheviour.GoStraight:
                    {
                        controllerState = MonsterControllerState.Walking;
                        //Vector2 direction = viewComponent.FacingDirection * distanceOnMissing;
                        WalkStright(viewComponent.FacingDirection, distanceOnMissing);
                    }
                    break;
                case MonsterMissingTargetBeheviour.LookAround:
                    {
                        controllerState = MonsterControllerState.Walking;
                        CreateNewDestinationPoint();
                        LookAround();
                    }
                    break;
            }
        }
    }
    private void WalkStright(Vector3 direction, float distance)
    {
        direction.Normalize();
        RaycastHit2D result = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Walls", "Furnitures"));

        if (result.collider == null)
        {
            movementComponent.SetDestination(transform.position + direction * distance);
        }
        else
        {
            movementComponent.SetDestination(transform.position + direction * result.distance);
        }

        
    }

    #endregion


    #region MONSTER_FUNCTION_MANAGEMENT
    public void RegisterFunction(MonsterFunctionTargetting function, MonsterDisruptionBeheviour disruptionBeheviour, MonsterMissingTargetBeheviour missingTargetBeheviour)
    {
        if (!registerOfTargetingFunctions.Exists(register => register.monsterFunction == function))
        {
            MonsterFunctionMovementType movementType = MonsterFunctionMovementType.NoMovement;
            if (function is IMonsterMovementUser)
            {
                movementType = MonsterFunctionMovementType.NormalMovement;
            }
            else if (function is IMonsterPursuitUser)
            {
                movementType = MonsterFunctionMovementType.Pursuit;
            }

            registerOfTargetingFunctions.Add(new RegisterFunctionEntry(function, disruptionBeheviour, missingTargetBeheviour, movementType));


            //SettingCallbacks
            //function.SetOnTargetFoundCallback(OnTargetFoundCallback);
            function.OnMissingTarget += MonsterFunctionTargetMissing;
            function.OnStartedPerformingAction += MonsterFunctionOnStartedPerforming;
            function.OnEndedPerformingAction += MonsterFunctionOnEndedPerforming;
        }
    }

    public void UnregisterFunction(MonsterFunctionCorpseTargetting function)
    {
        registerOfTargetingFunctions.RemoveAll(register => register.monsterFunction == function);
    }
    //private bool OnTargetFoundCallback(MonsterFunctionCorpseTargetting function, GameObject target)
    //{
    //    //Debug.Log("Controller.FoundTarget");
    //    if (canPerformAction && (controllerState == MonsterControllerState.Walking || controllerState == MonsterControllerState.LookingAround))
    //    {
    //        //Debug.Log("Controller.FoundTarget.CanPerform");
    //        canPerformAction = false;
    //
    //        performedFunction = function;
    //        functionTarget = target;
    //        functionTargetPosition = target.transform.position;
    //
    //        if (controllerState == MonsterControllerState.LookingAround)
    //        {
    //            viewComponent.StopLookingAround();
    //        }
    //
    //        controllerState = MonsterControllerState.Following;
    //
    //        movementComponent.SetDestination(functionTargetPosition);
    //
    //        return true;
    //    }
    //
    //    return false;
    //}
    //private IEnumerator StartPerformingFunction(MonsterFunctionTargetting function, float performingTime)
    //{
    //    yield return new WaitForSeconds(performingTime);
    //
    //    function.EndPerforming();
    //    controllerState = MonsterControllerState.Walking;
    //    canPerformAction = true;
    //    OnAchievedDestination();
    //}

    #endregion


    #region FUNCTION_ACTION_MANAGEMENT

    public void InformAboutPossibleAction(MonsterFunctionTargetting.MonsterFunctionAction action)
    {
        switch (controllerState)
        {
            case MonsterControllerState.Walking:
            case MonsterControllerState.LookingAround:
                {
                    possibleActionList.Add(action);
                }
                break;

            case MonsterControllerState.Following:
                {
                    CancelChosenAction(action);
                }
                break;

            case MonsterControllerState.PerfomingFunction:
                {
                    DisturbChosenAction(action);
                }
                break;
        }
    }


    private void CancelChosenAction(MonsterFunctionTargetting.MonsterFunctionAction newAction)
    {
        if (performedAction.MonsterFunction.CanBeCanceled() &&  newAction.Priority > performedAction.Priority)
        {
            movementComponent.enabled = true;

            disruptionRemeberedPosition = null;

            performedAction.MonsterFunction.CancelPerformedAction();

            StartPerformingAction(newAction);
        }
    }

    private void DisturbChosenAction(MonsterFunctionTargetting.MonsterFunctionAction newAction)
    {
        if (performedAction.MonsterFunction.CanBeDisturbed() && newAction.Priority > performedAction.DisturbPriority)
        {
            movementComponent.enabled = true;

            RegisterFunctionEntry registerEntry = registerOfTargetingFunctions.Find(registerEntry => { return registerEntry.monsterFunction == newAction.MonsterFunction; });

            switch (registerEntry.disruptionBeheviour)
            {
                case MonsterDisruptionBeheviour.DoNothing: 
                    disruptionRemeberedPosition = null;
                    break;
                case MonsterDisruptionBeheviour.RemeberActionTargetPosition:
                    {
                        disruptionRemeberedPosition = performedAction.Target.transform.position;
                    }
                    break;

            }

            performedAction.MonsterFunction.DisturbPerformedAction();

            StartPerformingAction(newAction);
        }
    }

    private void FunctionStartedPerformingAction(MonsterFunctionTargetting monsterFunction)
    {

    }
    #endregion


    #region MOVEMENT
    private void StopMoving()
    {
        movementComponent.StopMoving();
    }

    private void CreateNewPatrolingArea()
    {
        patrolArea = PathfindingController.Instance.GetPatrolingArea(transform.position, maxDistance, hullParts, middleFactor, outerIgnoreRadius, middleIgnoreRadius);
        patrolArea.DrawDebug();
    }

    private void CreateNewDestinationPoint()
    {
        
        //Vector3 newDestination = PathfindingController.Instance.GetRandomPosition();
        Vector3 newDestination = patrolArea.GetDestination();
        movementComponent.SetDestination(newDestination);
        //do {
        //    newDestination = PathfindingController.Instance.GetRandomPosition();
        //} while (Vector3.Distance(transform.position, newDestination) < newDestinationDistance);

        //Debug.Log("Starting new destination: " + newDestination);
    }

    private void OnAchievedDestination()
    {
        //Debug.Log("AchivedDestination Controller: " + state);
        if (controllerState == MonsterControllerState.Walking || controllerState == MonsterControllerState.LookingAround)
        {
            //Debug.Log("AchivedDestination Controller Can new");
            CreateNewDestinationPoint();
        }
        
    }

    #endregion MOVEMENT

    #region LOOKING_AROUND

    private void LookAround()
    {
        //Debug.Log("Looking around");
        controllerState = MonsterControllerState.LookingAround;
        StopMoving();

        viewComponent.StartLookingAround();
    }

    private void EndedLookingAround(ViewComponent viewComponent)
    {

        controllerState = MonsterControllerState.Walking;
        movementComponent.ContinueMoving();
    }

    private IEnumerator LookingAroundCoroutine(float minInterval, float maxInterval)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            if (controllerState == MonsterControllerState.Walking)
            {
                LookAround();
            }
        }
    }

    #endregion
}
