using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractMovementComponent : MonoBehaviour
{
    #region NESTED_TYPES

    public class EffectVariant
    {
        private float modifier = 1.0f;
        private float time = -1.0f;
        private GameObject source = null;
    }

    private class EffectGroupController
    {

    }

    #endregion


    #region SCRIPT_PROPERTIES

    [Header("General Movement Settings")]
    protected float originSpeed = 2.0f;

    #endregion

    protected float speed = 0.0f;

    protected Rigidbody2D rgbody2D = null;

    protected void SetupMovement()
    {
        speed = originSpeed;
        rgbody2D = GetComponent<Rigidbody2D>();

        if (rgbody2D == null)
        {
            rgbody2D = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
