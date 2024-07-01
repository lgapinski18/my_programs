using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static MonsterMovementComponent;

public class MonsterPursuitComponent : MonoBehaviour
{
    #region NESTED_TYPES

    public enum PursuitEffectType
    {
        SPEED,
        LOSSING_INTEREST
    }

    public class PursuitModificatingEffect
    {
        private PursuitEffectType effectType;
        private float modificator = 1.0f;
        private float time = -1.0f;

        public PursuitEffectType EffectType { get => effectType; }
        public float Modificator { get => modificator; }
        public float Time { get => time; }

        public PursuitModificatingEffect(PursuitEffectType effectType, float modificator, float time)
        {
            this.effectType = effectType;
            this.modificator = modificator;
            this.time = time;
        }
    }

    public class EffectController
    {
        private List<PursuitModificatingEffect> effects = new List<PursuitModificatingEffect>();

        private MonoBehaviour owningMonoBehaviour = null;

        public delegate void EffectControllerEvent(EffectController effectController);
        public event EffectControllerEvent OnChangedEvent;

        public EffectController(MonoBehaviour owningMonoBehaviour)
        {
            this.owningMonoBehaviour = owningMonoBehaviour;
        }

        public void AddEffect(PursuitModificatingEffect effect)
        {
            effects.Add(effect);

            OnChangedEvent?.Invoke(this);

            owningMonoBehaviour.StartCoroutine(RemoveEffect(effect));
        }

        public void RemoveAllEffects()
        {
            effects.Clear();
            OnChangedEvent?.Invoke(this);
        }

        public float CalculateResultMoficator()
        {
            float result = 1.0f;
            foreach (PursuitModificatingEffect effect in effects)
            {
                result *= effect.Modificator;
            }

            return result;
        }

        private IEnumerator RemoveEffect(PursuitModificatingEffect effect)
        {
            yield return new WaitForSeconds(effect.Time);

            effects.Remove(effect);
            OnChangedEvent?.Invoke(this);
        }

        public void Recalculate()
        {
            OnChangedEvent?.Invoke(this);
        }
    }

    private class CoroutineGuard
    {
        private bool notAchieved = true;
        public bool NotAchieved { get => notAchieved; set => notAchieved = value; }
        public CoroutineGuard()
        {
        }
    }

    public class CurveInterpolatingCourutine
    {
        [SerializeField]
        private AnimationCurve curve = new();
        [SerializeField]
        private float interpolationInterval = 0.05f;
        [SerializeField]
        private float interpolationTimeout = 1.0f;

        private bool canInterpolate = true;
        private float interpolationCurrentTime = 0.0f;

        private IEnumerator interpolationCoroutine = null;
        private IEnumerator stopInterpolationCoroutine = null;


        private MonoBehaviour owner = null;
        //public MonoBehaviour Owner { get => owner; set => owner = value; }

        #region EVENTS_AND_DELEGATES
        public delegate void InterpolationResultEvent(float value);
        public event InterpolationResultEvent InterpolationResult;

        public delegate void InterpolationEndedEvent();
        public event InterpolationEndedEvent InterpolationEnded;

        #endregion

        public CurveInterpolatingCourutine()
        {

        }

        public CurveInterpolatingCourutine(AnimationCurve curve, float interpolationInterval, float interpolationTimeout, MonoBehaviour owner)
        {
            this.curve = curve;
            this.interpolationInterval = interpolationInterval;
            this.interpolationTimeout = interpolationTimeout;
            this.owner = owner;
        }

        public void StartInterpolation()
        {
            interpolationCurrentTime = 0.0f;
            canInterpolate = true;

            interpolationCoroutine = InterpolationCoroutine(interpolationInterval);
            stopInterpolationCoroutine = EndInterpolationCoroutine(interpolationTimeout);




            owner.StartCoroutine(interpolationCoroutine);
            owner.StartCoroutine(stopInterpolationCoroutine);
        }

        public void StopInterpolation()
        {
            owner.StopCoroutine(interpolationCoroutine);
            owner.StopCoroutine(stopInterpolationCoroutine);
            canInterpolate = false;
        }

        private IEnumerator InterpolationCoroutine(float interval)
        {
            while (canInterpolate)
            {
                yield return new WaitForSeconds(interval);

                interpolationCurrentTime += interval;

                InterpolationResult?.Invoke(curve.Evaluate(interpolationCurrentTime));
            }
        }

        private IEnumerator EndInterpolationCoroutine(float timeout)
        {
            yield return new WaitForSeconds(timeout);

            StopInterpolation();

            InterpolationEnded?.Invoke();
        }

    }

    #endregion

    #region SCRIPT_PROPERTIES

    [SerializeField]
    private float originSpeed = 1.0f;

    [SerializeField]
    private float smoothFactor = 0.5f;

    [SerializeField]
    private float pursuitRadius = 2.0f;

    [SerializeField]
    private float trackingTime = 5.0f;
    [SerializeField]
    private float trackingNodeCreationInterval = 0.1f;

    [SerializeField]
    private float trackingRefreshInterval = 0.1f;
    [SerializeField]
    private float originLoosingInterestTime = 5.0f;
    private float loosingInterestTime = 5.0f;


    //[SerializeField]
    //private float softSlowDownRadius = 0.2f;
    //[SerializeField]
    //private float hardSlowDownRadius = 0.1f;


    [SerializeField]
    private LayerMask pursuitViewBlockingLayers;
    //private LayerMask pursuitViewBlockingLayers = 0;

    [SerializeField]
    private ReactionsController reactionsController;

    [Header("Start pursuit behaviour"), Space(10)]
    [SerializeField]
    private AnimationCurve startPursuitSpeedChange = new();
    [SerializeField]
    private float startPursuitSamplingInterval = 0.05f;
    [SerializeField]
    private float startPursuitTimeOut = 1.0f;

    [Header("Out of reach"), Space(10)]
    [SerializeField]
    private AnimationCurve outOfRangeSpeedUpCurve = new();
    [SerializeField]
    private float outOfReachSamplingInterval = 0.05f;
    [SerializeField]
    private float outOfReachSpeedUpTime = 1.0f;

    [SerializeField]
    private float outOfReachSpeedUpCooldown = 10.0f;



    #endregion

    #region SCRIPT_FIELDS
    private GameObject pursuitTarget = null;
    public GameObject PursuitTarget { get => pursuitTarget; }


    private Vector2 targetVelocity = Vector2.zero;
    private Vector3 targetPosition = Vector3.zero;

    private IEnumerator trackingCoroutine = null;
    //private IEnumerator trackingNodeCreationCoroutine = null;

    private Rigidbody2D rgbody2D = null;
    private ViewComponent viewComponent = null;
    //private MonsterMovementComponent monsterMovementComponent = null;

    private bool isSlowedDown = false;

    //SPEED UP
    private CurveInterpolatingCourutine startPursuitSpeedUp = new CurveInterpolatingCourutine();
    private CurveInterpolatingCourutine outOfReachSpeedUp = new CurveInterpolatingCourutine();
    private float speed = 5.0f;
    private IEnumerator outOfReachCooldownCoroutine = null;
    private bool inOutOfReachCooldown = false;

    //private bool activatedDirectPursuit = false;
    //private bool isDirectlyPursuiting = false;
    private bool activatedTracking = true;
    private bool isTracking = false;

    private readonly float positionAchievedDelta = 0.1f;

    //LOOSING INTEREST
    private IEnumerator loosingInterestCoroutine = null;

    //EFFECTS
    //private List<PursuitModificatingEffect> speedEffects = new List<PursuitModificatingEffect>();
    //private List<PursuitModificatingEffect> loosingInterestEffects = new List<PursuitModificatingEffect>();

    private EffectController speedEffects = null;
    private EffectController loosingInterestEffects = null;


    private PathfindingPath path = null;

    [SerializeField]
    private float distanceDifference = 0.1f;

    private Vector2 lastMovingDirection = Vector2.zero;
    public Vector2 LastMovingDirection { get => lastMovingDirection; }

    //private Vector2 targetVelocity = Vector2.zero;
    //
    //private Vector3 targetPosition = Vector3.zero;
    //
    //private Rigidbody2D rgbody2D = null;

    private MonsterViewComponent monsterViewComponent = null;

    private bool canMove = true;


    #endregion

    #region EVENTS_AND_DELEGATES

    public delegate void MonsterPursuitEvent(MonsterPursuitComponent monsterPursuit);

    //public event MonsterPursuitEvent OnOutOfPursuitViewZone;
    public event MonsterPursuitEvent OnPursuitEnded;
    //public event MonsterPursuitEvent OnArrivedAtEndPoint;

    #endregion

    // Start is called before the first frame update
    void Awake()
    {

        rgbody2D = GetComponent<Rigidbody2D>();
        viewComponent = GetComponentInChildren<ViewComponent>();
        //monsterMovementComponent = GetComponent<MonsterMovementComponent>();

        speed = originSpeed;
        loosingInterestTime = originLoosingInterestTime;

        startPursuitSpeedUp = new CurveInterpolatingCourutine(startPursuitSpeedChange, startPursuitSamplingInterval, startPursuitTimeOut, this);
        startPursuitSpeedUp.InterpolationResult += ChangeSpeedValue;
        startPursuitSpeedUp.InterpolationEnded += SpeedInterpolationEnded;

        outOfReachSpeedUp = new CurveInterpolatingCourutine(outOfRangeSpeedUpCurve, outOfReachSamplingInterval, outOfReachSpeedUpTime, this);
        outOfReachSpeedUp.InterpolationResult += ChangeSpeedValue;
        outOfReachSpeedUp.InterpolationEnded += SpeedInterpolationEnded;

        trackingCoroutine = CreateTrackingCoroutine(trackingRefreshInterval);
        loosingInterestCoroutine = CreateLoosingInterestCoroutine(loosingInterestTime);

        //Effects
        speedEffects = new EffectController(this);
        speedEffects.OnChangedEvent += RecalculateSpeed;

        loosingInterestEffects = new EffectController(this);
        loosingInterestEffects.OnChangedEvent += RecalculateLoosingInterestTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && path != null)
        {
            if (pursuitTarget != null)
            {
                Vector3 direction = pursuitTarget.transform.position - transform.position;
                RaycastHit2D resultHit = Physics2D.Raycast(transform.position, direction, pursuitRadius,
                                        pursuitViewBlockingLayers | LayerMask.GetMask(LayerMask.LayerToName(pursuitTarget.layer)));

                if (resultHit.collider?.gameObject == pursuitTarget)
                {
                    if (!activatedTracking)
                    {
                        activatedTracking = true;

                        StopLoosingInterest();
                    }
                }
                else
                {
                    if (activatedTracking)
                    {
                        activatedTracking = false;

                        StartLoosingInterest();
                    }
                }
            }

            //Debug.Log("Achieved end: " + path.AchievedEnd());
            if (!path.AchievedEnd())
            {
                if (Vector3.Distance(transform.position, targetPosition) < distanceDifference)
                {
                    GoToNextPathNode();
                }
            }
            else if (Vector3.Distance(transform.position, targetPosition) < distanceDifference)
            {
                //Debug.Log("ETarget Position: " + targetPosition);
                //Debug.Log("Achieved ECurrent Position: " + transform.position);
                targetVelocity = Vector3.zero;
            }
        }

        targetVelocity = targetVelocity.normalized * speed;
        rgbody2D.velocity = Vector2.Lerp(rgbody2D.velocity, targetVelocity, smoothFactor);


    }

    private void GoToNextPathNode()
    {
        lastMovingDirection = targetVelocity;
        targetPosition = path.NextNode();
        //Debug.Log("Pursuit.Count: " + path.Count);
        //Debug.Log("Can move: " + canMove);
        targetVelocity = (targetPosition - transform.position).normalized * speed;
        if (path.AchievedEnd())
        {
            //viewComponent.LookTowards(viewComponent.FacingDirection);
        }
        else
        {
            //viewComponent.LookTowards(targetVelocity);
            viewComponent.LookTowards(pursuitTarget.transform.position - transform.position);
            //viewComponent.LookTowards((targetPosition - transform.position).normalized);
        }
        //Debug.Log("Target Position: " + targetPosition);
        //Debug.Log("Current Position: " + transform.position);
    }

    public void SetDestination(Vector3 destination)
    {
        //Debug.Log("Pursuit.SetDest");
        //ContinueMoving();
        canMove = true;

        //Debug.Log("Setting new destination1 " + path);
        //if (path == null)
        //{
        //    path = PathfindingController.Instance.GetPath(transform.position, destination);
        //}
        //else
        //{
        //    //path = path.GetSubPath(0, 3);
        //    //path += PathfindingController.Instance.GetPath(path.Destination(), destination);
        //    path = PathfindingController.Instance.GetPath(transform.position, destination);
        //}
        path = PathfindingController.Instance.GetPath(transform.position, destination);

        //Debug.Log("Count: " + path.Count);
        //Debug.Log("Setting new destination2 " + path);

        GoToNextPathNode();
    }
    public void SetPursuitTarget(GameObject pursuitTarget)
    {
        //Debug.Log("Pursuit.SetTarg");
        inOutOfReachCooldown = true;
        StartCoroutine(CreateOutOfReachCoolDownCoroutine(startPursuitTimeOut));

        this.pursuitTarget = pursuitTarget;

        reactionsController.MakeReaction("NoticingTarget");

        startPursuitSpeedUp.StartInterpolation();

        SetDestination(pursuitTarget.transform.position);

        StartTracking();
    }

    public void StopMoving()
    {
        //Debug.Log("Pursuit.StopMov");
        startPursuitSpeedUp.StopInterpolation();

        StopTracking();
        StopLoosingInterest();
        canMove = false;
        activatedTracking = false;
        path = null;
        targetVelocity = Vector3.zero;
    }


    #region SPEED_UP

    private void ChangeSpeedValue(float value)
    {
        speed = value;
        targetVelocity.Normalize();
        targetVelocity *= speed;
    }

    private void SpeedInterpolationEnded()
    {
        ChangeSpeedValue(originSpeed);
    }


    private void StartOutOfReachSpeedUp()
    {
        if (!inOutOfReachCooldown)
        {
            outOfReachSpeedUp.StartInterpolation();

            StartOutOfReachCooldown();
        }
    }

    private void StartOutOfReachCooldown()
    {
        inOutOfReachCooldown = true;
        outOfReachCooldownCoroutine = CreateOutOfReachCoolDownCoroutine(outOfReachSpeedUpCooldown + outOfReachSpeedUpTime); ;
        StartCoroutine(outOfReachCooldownCoroutine);
    }

    private IEnumerator CreateOutOfReachCoolDownCoroutine(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        inOutOfReachCooldown = false;
    }

    #endregion 

    #region TRACKING

    private void StartTracking()
    {
        //Debug.Log("Pursuit.StartTack");
        isTracking = true;
        trackingCoroutine = CreateTrackingCoroutine(trackingRefreshInterval);
        //monsterMovementComponent.enabled = true;
        StartCoroutine(trackingCoroutine);
    }

    private IEnumerator CreateTrackingCoroutine(float refreshInterval)
    {
        while (isTracking)
        {
            yield return new WaitForSeconds(refreshInterval);

            //Debug.Log("Pursuit.RefTack");
            //monsterMovementComponent.SetDestination(pursuitTarget.transform.position);
            //SetDestination(pursuitTarget.transform.position);
            path = PathfindingController.Instance.GetPath(transform.position, pursuitTarget.transform.position);
            //Debug.Log("Count: " + path.Count);
            //Debug.Log("Setting new destination2 " + path);

            GoToNextPathNode();
        }
    }

    private void StopTracking()
    {
        //monsterMovementComponent.enabled = false;
        //monsterMovementComponent.StopMoving();
        //Debug.Log("Pursuit.StopTack");
        isTracking = false;
        StopCoroutine(trackingCoroutine);
    }

    #endregion

    #region LOOSING_INTEREST

    private void StartLoosingInterest()
    {
        //Debug.Log("Pursuit.StartLoosingInt");
        loosingInterestCoroutine = CreateLoosingInterestCoroutine(loosingInterestTime);
        StartCoroutine(loosingInterestCoroutine);
    }

    private IEnumerator CreateLoosingInterestCoroutine(float loosingInterestTime)
    {
        yield return new WaitForSeconds(loosingInterestTime);
        reactionsController.MakeReaction("LoosingInterest");

        LooseInterest();
    }

    private void StopLoosingInterest()
    {
        //Debug.Log("Pursuit.StopLoosingInt");

        StopCoroutine(loosingInterestCoroutine);
    }

    private void LooseInterest()
    {
        //StopMoving();
        //Debug.Log("Pursuit.LoosingInt");
        OnPursuitEnded?.Invoke(this);
    }

    #endregion

    #region PURSUIT_MODIFICATION_EFFECTS

    public void AddEffect(PursuitEffectType effectType, float modificator, float time)
    {
        PursuitModificatingEffect effect = new PursuitModificatingEffect(effectType, modificator, time);

        switch (effectType)
        {
            case PursuitEffectType.SPEED:
                {
                    speedEffects.AddEffect(effect);
                }
                break;
            case PursuitEffectType.LOSSING_INTEREST:
                {
                    loosingInterestEffects.AddEffect(effect);
                }
                break;
        }
    }

    private void RecalculateSpeed(EffectController effectController)
    {
        speed = originSpeed * effectController.CalculateResultMoficator();
    }
    private void RecalculateLoosingInterestTime(EffectController effectController)
    {
        loosingInterestTime = originLoosingInterestTime * effectController.CalculateResultMoficator();
    }

    #endregion
}
