using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mop : AbstractTool
{

    public override void EndUsing()
    {
        //Debug.Log("End Using Mop");
        used = false;
    }

    public override void Use()
    {
        //Debug.Log("Using Mop");
        used = true;
    }

    protected override void SetupTool()
    {
        base.SetupTool();
    }

    private void Awake()
    {
        SetupTool();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Try Mop Used");
        if (used)
        {
            //Debug.Log("Mop Used");
            Substain substain = collision.GetComponent<Substain>();
            if (substain != null)
            {
                substain.Clean();
            }
        }
    }
}
