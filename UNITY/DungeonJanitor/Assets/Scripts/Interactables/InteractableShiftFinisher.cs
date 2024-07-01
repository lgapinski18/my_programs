using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableShiftFinisher : AInteractable
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
        MessageManager.Instance().AddPrioritizedPopUp(PopUp.PopUpType.YESNO, () => { GameManager.instance.FinishShift(); }, "UWAGA!", "Czy jesteœ pewien aby zakoñczyæ tê zmianê?", MessageManager.Icons.DEVS_ICON, 260, 260);
        MessageManager.Instance().showPrioritizedPopUp(true);
        //GameManager.instance.FinishShift();

        return false;
    }
}
