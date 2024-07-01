using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MonsterMovementComponent : MonoBehaviour
{
    private PathfindingPath path = null;

    public float speed = 2.0f;
    public float smoothFactor = 0.5f;
    public float distanceDifference = 0.1f;

    private Vector2 lastMovingDirection = Vector2.zero;
    public Vector2 LastMovingDirection { get => lastMovingDirection; }

    private Vector2 targetVelocity = Vector2.zero;

    private Vector3 targetPosition = Vector3.zero;
    private Vector3? destination = null;

    private Rigidbody2D rgbody2D = null;

    private MonsterViewComponent monsterViewComponent = null;

    private bool canMove = true;

    public delegate void AchievedDestination();
    public event AchievedDestination onAchievedDestination;

    private GameObject targetObject = null;
    private IEnumerator trackingCoroutine = null;
    private bool isTracking = true;
    private float trackingRefreshInterval = 0.2f;

    // Start is called before the first frame update
    void Awake()
    {
        rgbody2D = GetComponent<Rigidbody2D>();
        if (rgbody2D == null)
        {
            rgbody2D = gameObject.AddComponent<Rigidbody2D>();
        }

        monsterViewComponent = GetComponent<MonsterViewComponent>();

        trackingCoroutine = CreateTrackingCoroutine(trackingRefreshInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && path != null)
        {
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
                //targetVelocity = Vector3.zero;
                //path = null;
                StopMoving();

                onAchievedDestination?.Invoke();
            }
        }
        //rgbody2D.velocity = (targetVelocity * (1 - smoothFactor)) + (rgbody2D.velocity * smoothFactor);
        rgbody2D.velocity = Vector2.Lerp(rgbody2D.velocity, targetVelocity, smoothFactor);


    }

    private void GoToNextPathNode()
    {
        lastMovingDirection = targetVelocity;
        targetPosition = path.NextNode();
        //Debug.Log("Count: " + path.Count);
        //Debug.Log("Can move: " + canMove);
        targetVelocity = (targetPosition - transform.position).normalized * speed;
        if (targetObject == null)
        {
            monsterViewComponent.LookTowards((targetPosition - transform.position).normalized);
        }
        else
        {
            monsterViewComponent.LookTowards((targetObject.transform.position - transform.position).normalized);
        }
        //Debug.Log("Target Position: " + targetPosition);
        //Debug.Log("Current Position: " + transform.position);
    }

    public void SetDestination(Vector3 destination)
    {
        //ContinueMoving();
        canMove = true;
        this.destination = destination;
        targetObject = null;
        //Debug.Log("Setting new destination1 " + path);

        path = PathfindingController.Instance.GetPath(transform.position, destination);
        //Debug.Log("Count: " + path.Count);
        //Debug.Log("Setting new destination2 " + path);

        GoToNextPathNode();
    }

    public void SetTarget(GameObject target)
    {
        //ContinueMoving();
        canMove = true;

        //Debug.Log("Setting new destination1 " + path);
        destination = null;
        targetObject = target;


        path = PathfindingController.Instance.GetPath(transform.position, target.transform.position);
        //Debug.Log("Count: " + path.Count);
        //Debug.Log("Setting new destination2 " + path);
        StartTracking();

        GoToNextPathNode();
    }

    public void StopMoving()
    {
        canMove = false;
        path = null;
        targetVelocity = Vector3.zero;

        StopTracking();
    }

    public void ContinueMoving()
    {
        //canMove = true;
        //targetVelocity = (targetPosition - transform.position).normalized * speed;
        //monsterViewComponent.LookTowards((targetPosition - transform.position).normalized);
        if (destination != null)
        {
            SetDestination((Vector3)destination);
        }
        else if (targetObject != null)
        {
            SetTarget(targetObject);
        }
    }


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
            //SetTarget(targetObject);
            path = PathfindingController.Instance.GetPath(transform.position, targetObject.transform.position);
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
}
