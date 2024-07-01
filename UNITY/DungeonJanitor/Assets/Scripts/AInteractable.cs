using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AInteractable : MonoBehaviour
{
    public string objectName = "";
    public SpriteRenderer interactionMarkup;
    private SpriteRenderer interactionMarkupInstance;

    public abstract bool interact();

    public string getObjectName()
    {
        return objectName;
    }

    public void selectInteractable()
    {
        //Debug.Log(string.Format("select: {0}!", getObjectName()));
        if ((interactionMarkup != null) && (interactionMarkupInstance == null))
        {
            interactionMarkupInstance = Instantiate(interactionMarkup);
            interactionMarkupInstance.gameObject.transform.position = transform.position;
            interactionMarkupInstance.transform.position += new Vector3(0.25f, 0.25f, 0.0f);
            interactionMarkupInstance.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        }
    }

    public void unselectInteractable()
    {
        if (interactionMarkupInstance != null)
        {
            Destroy(interactionMarkupInstance.gameObject);
            interactionMarkupInstance = null;
        }
    }
}
