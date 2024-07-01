using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    public float originSpeed = 5.0f;
    public float speed = 5.0f;
    [SerializeField]
    public float originSmoothFactor = 0.5f;


    private Rigidbody2D rgbody2D = null;
    private Vector2 targetVelocity = Vector2.zero;
    private Vector2 rememberedTargetVelocityDirection = Vector2.zero;

    private Animator animator = null;

    private Vector2 normalizedMovementDirection = Vector2.zero;

    public Vector2 NormalizedMovementDirection { get => normalizedMovementDirection; } //set => normalizedMovementDirection = value;

    private Vector2Int facingDirection = new Vector2Int();
    public Vector2Int FacingDirection { get => facingDirection; }
    public Vector2 TargetVelocity { get => targetVelocity; }

    private bool canMove = true;

    [SerializeField]
    private float knockbackImmunityTime = 0.5f;

    private bool knockbackImmune = false;

    #region MODIFIERS_MANAGERS

    private ModifiersManager mmEnviromentSpeedFactor;
    private ModifiersManager mmOtherSpeedFactor;
    private float movementSpeedFactor = 1.0f;
    //private float otherSpeedFactor = 1.0f;
    private ModifiersManager actionMovementSlow;
    private float actionMovementSlowFactor = 1.0f;
    private ModifiersManager smoothFactorMM;
    private float smoothFactor = 1.0f;

    #endregion

    #region EVENTS_AND_DELEGATES

    public delegate void OnFacingDirectionChangedEvent(MovementComponent movement, Vector2Int facingDirection);
    public event OnFacingDirectionChangedEvent OnFacingDirectionChanged;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        speed = originSpeed;
        smoothFactor = originSmoothFactor;

        rgbody2D = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();
        }

        mmEnviromentSpeedFactor = new ModifiersManager(ModifiersManager.ModifiersProcessingType.Avarage, this);
        mmEnviromentSpeedFactor.OnChanged += UpdateSpeedFactor;
        //GetComponent<ModifiersManagerComponent>().RegisterModifiersManager("movementSpeedSlow", enviromentSlowDown);
        GetComponent<ModifiersManagerComponent>().RegisterModifiersManager("movementSpeed", mmEnviromentSpeedFactor);

        mmOtherSpeedFactor = new ModifiersManager(ModifiersManager.ModifiersProcessingType.Avarage, this);
        mmOtherSpeedFactor.OnChanged += UpdateSpeedFactor;
        GetComponent<ModifiersManagerComponent>().RegisterModifiersManager("otherMovementSpeed", mmOtherSpeedFactor);

        actionMovementSlow = new ModifiersManager(ModifiersManager.ModifiersProcessingType.Avarage, this);
        actionMovementSlow.OnChanged += UpdateActionMovementSlowFactor;
        GetComponent<ModifiersManagerComponent>().RegisterModifiersManager("actionMovementSlow", actionMovementSlow);

        smoothFactorMM = new ModifiersManager(ModifiersManager.ModifiersProcessingType.Avarage, this, smoothFactor);
        smoothFactorMM.OnChanged += UpdateSmoothFactor;
        GetComponent<ModifiersManagerComponent>().RegisterModifiersManager("smoothFactor", smoothFactorMM);
    }

    // Update is called once per frame
    void Update()
    {
        rgbody2D.velocity = (targetVelocity * (1 - smoothFactor)) + (rgbody2D.velocity * smoothFactor);
    }

    private void UpdateTargetVelocitySpeed()
    {
        targetVelocity = targetVelocity.normalized * speed * movementSpeedFactor * actionMovementSlowFactor;// * GetActualEnvironemtSpeedModificationFactor();
        //targetVelocity = targetVelocity.normalized * speed * GetActualEnvironemtSpeedModificationFactor();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        direction.Normalize();

        if (canMove)
        {
            //targetVelocity = context.ReadValue<Vector2>();
            //targetVelocity.Normalize();

            //normalizedMovementDirection = targetVelocity;
            normalizedMovementDirection = direction;

            //targetVelocity *= speed;
            targetVelocity = direction;

            UpdateTargetVelocitySpeed();

            int vertical = 0;
            int horizontal = 0;

            if (targetVelocity.x > 0.0f)
            {
                horizontal = 1;
            }
            else if (targetVelocity.x < 0.0f)
            {
                horizontal = -1;
            }

            if (targetVelocity.y > 0.0f)
            {
                vertical = 1;
            }
            else if (targetVelocity.y < 0.0f)
            {
                vertical = -1;
            }

            Vector2Int newFacingDirection = new Vector2Int(horizontal, vertical);

            animator.SetInteger("Vertical", vertical);
            animator.SetInteger("Horizontal", horizontal); //(int) Mathf.Sign(targetVelocity.x)

            if (newFacingDirection != facingDirection && newFacingDirection != Vector2Int.zero)
            {
                facingDirection = newFacingDirection;
                OnFacingDirectionChanged?.Invoke(this, facingDirection);

            }


            //Debug.Log("Vertical: " + animator.GetInteger("Vertical"));
            //Debug.Log("Horizontal: " + animator.GetInteger("Horizontal"));
        }
        else
        {
            rememberedTargetVelocityDirection = targetVelocity;
        }
    }

    #region KNOCKBACK

    public void ApplyKnockback(Vector2 direction, float distance, float time)
    {
        //Debug.Log("Knockback: " + !knockbackImmune);
        if (!knockbackImmune)
        {
            canMove = false;
            speed = distance / time;
            targetVelocity = direction.normalized * speed;

            StartCoroutine(StartKnockback(time));

            knockbackImmune = true;
            StartCoroutine(StartKnockbackImmune(time + knockbackImmunityTime));
        }
    }

    private IEnumerator StartKnockback(float time)
    {
        yield return new WaitForSeconds(time);

        speed = originSpeed;
        //targetVelocity = Vector2.zero;
        targetVelocity = rememberedTargetVelocityDirection;
        UpdateTargetVelocitySpeed();
        rememberedTargetVelocityDirection = Vector2.zero;

        canMove = true;
    }

    private IEnumerator StartKnockbackImmune(float time)
    {
        yield return new WaitForSeconds(time);

        knockbackImmune = false;
    }

    #endregion

    #region ENVIRONMENT_SPEED_MODIFICATION
    private class EnvironmentalSpeedEffectRecord
    {
        private GameObject source = null;
        private float speedFactor = 1.0f;

        public EnvironmentalSpeedEffectRecord(GameObject source, float speedFactor)
        {
            this.source = source;
            this.speedFactor = speedFactor;
        }

        public GameObject Source { get => source; }
        public float SpeedFactor { get => speedFactor; }
    }

    private List<EnvironmentalSpeedEffectRecord> environmentalSpeedFactors = new List<EnvironmentalSpeedEffectRecord>();
    private float environmentSpeedModificationFactor = 1.0f;

    public void ApplyEnvironmentSpeedEffect(GameObject source, float factor)
    {
        environmentalSpeedFactors.Add(new EnvironmentalSpeedEffectRecord(source, factor));
        CalculateEnvironmentSpeedModification();
    }

    public void RemoveEnvironmentalSpeedEffect(GameObject source)
    {
        environmentalSpeedFactors.RemoveAll(record => record.Source == source);
        CalculateEnvironmentSpeedModification();
    }

    private void CalculateEnvironmentSpeedModification()
    {
        float sum = 0.0f;
        foreach (var record in environmentalSpeedFactors)
        {
            sum += record.SpeedFactor;
        }
        environmentSpeedModificationFactor = sum / environmentalSpeedFactors.Count;

        UpdateTargetVelocitySpeed();
    }

    protected float GetActualEnvironemtSpeedModificationFactor()
    {
        return environmentSpeedModificationFactor;
    }

    #endregion

    #region MODIFIERS

    private void UpdateSpeedFactor(ModifiersManager modifiersManager)
    {
        movementSpeedFactor = mmEnviromentSpeedFactor.GetModifier() * mmOtherSpeedFactor.GetModifier();
        //Debug.Log("SC: MS" + movementSpeedFactor);
        UpdateTargetVelocitySpeed();
    }

    private void UpdateActionMovementSlowFactor(ModifiersManager modifiersManager)
    {
        //Debug.Log("SC: AMSF");
        actionMovementSlowFactor = modifiersManager.GetModifier();
        UpdateTargetVelocitySpeed();
    }

    private void UpdateSmoothFactor(ModifiersManager modifiersManager)
    {
        //Debug.Log("SC: SF" + modifiersManager.GetModifier());
        smoothFactor = Mathf.Clamp(originSmoothFactor * modifiersManager.GetModifier(), 0.0f, 0.99f); ;
    }
    #endregion

}