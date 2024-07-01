using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletionTrigger : MonoBehaviour
{
    public GameObject[] GameObjects = { };
    public bool DestroyOnTrigger = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            foreach (var obj in GameObjects)
            {
                Destroy(obj);
            }

            if (DestroyOnTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}
