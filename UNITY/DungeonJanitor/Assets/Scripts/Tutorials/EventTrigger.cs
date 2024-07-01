using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent Events;
    public bool DestroyOnTrigger = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Events.Invoke();

            if (DestroyOnTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}
