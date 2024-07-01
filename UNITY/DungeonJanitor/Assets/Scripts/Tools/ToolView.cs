using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ToolView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private bool flipX = false;
    [SerializeField] 
    private bool flipY = false;
    [SerializeField, Tooltip("When enabled allows sprite to rotate according to facing ")]
    private bool rotate = false;
    [SerializeField, Tooltip("When enabled allows sprite to rotate according to facing to achieve stablized rotation.")]
    private bool stabilizeRotate = false;

    void Start()
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
            spriteRenderer.flipX = flipX;
            spriteRenderer.flipY = flipY;
        }
        else
        {
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }

        if (rotate)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        if (stabilizeRotate)
        {
            //transform.localEulerAngles = Quaternion.Euler(0f, 0f, -angle);
            transform.localEulerAngles = new Vector3(0f, 0f, -angle);
        }

    }

    
}
