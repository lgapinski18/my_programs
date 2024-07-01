using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterViewComponent : MonoBehaviour
{
    ViewComponent viewComponent = null;

    // Start is called before the first frame update
    void Awake()
    {
        viewComponent = gameObject.GetComponentInChildren<ViewComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LookTowards(Vector3 direction)
    {
        viewComponent.LookTowards(direction);
    }

}
