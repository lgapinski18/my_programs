using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewComponent : MonoBehaviour
{
    private MonsterViewComponent monsterViewComponent = null;


    public float viewRadius = 1.5f;

    [SerializeField]
    private LayerMask viewBlockingLayers;

    //private LayerMask viewBlockingLayers;

    private List<GameObject> inViewField = new List<GameObject>();

    private Vector2 facingDirection = Vector2.zero;
    public Vector2 FacingDirection { get => facingDirection; }

    private List<LookingTarget> lookingTargetsList = new List<LookingTarget>();

    private Animator parentAnimator = null;

    private struct LookingTargetEntry
    {
        public LookingTarget target;
        public GameObject targetObject;
    }

    private List<LookingTargetEntry> lookingTargetsInViewField = new List<LookingTargetEntry>();


    [SerializeField]
    private float lookingAroundSpeed = 72.0f;
    private bool lookingAround = false;
    private float rotatedAngle = 0.0f;
    private float startAngle = 0.0f;

    public delegate void OnEndedLookingAround(ViewComponent viewComponent);
    public event OnEndedLookingAround onEndedLookingAround;


    // Start is called before the first frame update
    void Awake()
    {
        monsterViewComponent = transform.parent.GetComponent<MonsterViewComponent>();
        parentAnimator = transform.parent.gameObject.GetComponent<Animator>();
        if (parentAnimator == null)
        {
            parentAnimator = transform.parent.gameObject.AddComponent<Animator>();
        }

        GetComponent<SpriteRenderer>().enabled = false;

        //foreach (LayerMask layer in viewBlockingLayers)
        //{
        //    viewBlockingLayers |= layer;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        foreach (LookingTargetEntry entry in lookingTargetsInViewField)
        {
            Vector3 direction = entry.targetObject.transform.position - transform.parent.transform.position;
            RaycastHit2D raycastResult2D = Physics2D.Raycast(transform.parent.transform.position, direction, direction.magnitude, viewBlockingLayers);

            if (raycastResult2D.collider?.gameObject == null)
            {
                entry.target.Callback(this, entry.targetObject);
            }
        }
        if (lookingAround)
        {
            rotatedAngle += lookingAroundSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, startAngle + rotatedAngle);
            SetInAnimatorFacingDirection();
            if (rotatedAngle > 360.0f)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, startAngle);
                lookingAround = false;
                onEndedLookingAround?.Invoke(this);
            }
        }
    }

    private void SetInAnimatorFacingDirection()
    {
        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        float sinAngle = Mathf.Sin(angle);
        float cosAngle = Mathf.Cos(angle);

        if (sinAngle > 0.5f)
        {
            parentAnimator.SetInteger("Vertical", 1);
        }
        else if (sinAngle < -0.5f)
        {
            parentAnimator.SetInteger("Vertical", -1);
        }
        else
        {
            parentAnimator.SetInteger("Vertical", 0);
        }

        if (cosAngle > 0.5f)
        {
            parentAnimator.SetInteger("Horizontal", 1);
        }
        else if (cosAngle < -0.5f)
        {
            parentAnimator.SetInteger("Horizontal", -1);
        }
        else
        {
            parentAnimator.SetInteger("Horizontal", 0);
        }
    }

    public void LookTowards(Vector2 direction)
    {
        Vector2 baseDirection = Vector2.right;
        //Vector2 baseDirection = new Vector2(1.0f, 0.0f);
        direction.Normalize();

        facingDirection = direction;

        float cosAngle = Vector2.Dot(baseDirection, direction);

        float angle = Mathf.Rad2Deg * Mathf.Acos(cosAngle);

        if (direction.y < 0.0f)
        {
            angle = -angle;
        }

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

        SetInAnimatorFacingDirection();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (LookingTarget target in lookingTargetsList)
        {
            //Debug.Log("Validation result in View: " + target.Validator(collision.gameObject));
            if (target.Validator(collision.gameObject))
            {
                lookingTargetsInViewField.Add(new LookingTargetEntry { target = target, targetObject = collision.gameObject });
            }
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.GetComponent<Corpse>() != null)
        //{
        //    lookingTargetsInViewField.RemoveAll(entry => entry.targetObject == collision.gameObject);
        //}
        lookingTargetsInViewField.RemoveAll(entry => entry.targetObject == collision.gameObject);
    }

    public void AddLookingTarget(LookingTarget target)
    {
        if (!lookingTargetsList.Contains(target))
        {
            lookingTargetsList.Add(target);
        }
    }

    public void RemoveLookingTarget(LookingTarget target)
    {
        lookingTargetsList.Remove(target);

    }

    public void StartLookingAround()
    {
        lookingAround = true;
        startAngle = transform.rotation.eulerAngles.z;
        rotatedAngle = 0.0f;
    }

    public void StopLookingAround()
    {
        lookingAround = false;
    }


}
