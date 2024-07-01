using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class AbstractTool : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;


    protected SpriteRenderer spriteRenderer;

    protected bool used = false;

    #region EVENTS_AND_DELEGATES

    public delegate void ToolEvent(AbstractTool tool);
    public event ToolEvent OnBeginUsingEvent;
    public event ToolEvent OnEndUsingEvent;

    protected void CallOnBeginUsing()
    {
        OnBeginUsingEvent?.Invoke(this);
    }

    protected void CallOnEndUsing()
    {
        OnEndUsingEvent?.Invoke(this);
    }

    #endregion


    public bool IsUsed { get => used; }

    protected virtual void SetupTool()
    {
        GetComponentInParent<MovementComponent>().OnFacingDirectionChanged += UpdateRotationAccordingToFacing;
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void UpdateRotationAccordingToFacing(MovementComponent movement, Vector2Int facingDirection)
    {
        float magnitude = facingDirection.magnitude;
        float angle = Mathf.Acos(facingDirection.x / magnitude);
        angle *= Mathf.Rad2Deg;
        if (facingDirection.y < 0)
        {
            angle = -angle;
        }

        if (angle < -120 || angle > 120)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public abstract void Use();
    public abstract void EndUsing();
}
