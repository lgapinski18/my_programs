using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableChest : AInteractable
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool interact()  
    {
        Debug.Log(string.Format("Chest: {0} - interaction!", getObjectName()));

        return false;
    }
}
