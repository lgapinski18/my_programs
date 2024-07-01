using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BlueGasingZone : MonoBehaviour
{

    #region SCRIPT_PROPERTIES

    private float radius = 0.0f;

    #endregion


    #region SCRIPT_FIELDS

    private CircleCollider2D gasingZone;

    public float Radius { get => radius; 
        set {
            radius = value;
            gasingZone.radius = radius;
        }
    }

    #endregion

    #region EVENTS_AND_DELEGATES

    public delegate void InZoneEvent(Collider2D collision);
    public event InZoneEvent InZone;
    
    public delegate void OutZoneEvent(Collider2D collision);
    public event OutZoneEvent OutZone;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gasingZone = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("InGasingZone");
        //transform.parent.SendMessage(nameof(OnTriggerStay2D), collision);
        InZone?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("OutGasingZone");
        //transform.parent.SendMessage(nameof(OnTriggerStay2D), collision);
        OutZone?.Invoke(collision);
    }
}
