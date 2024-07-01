using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LookingTarget
{
    public delegate bool LookingTargetValidator(GameObject potentialTarget);

    private LookingTargetValidator validator;
    public LookingTargetValidator Validator { get => validator; }

    public delegate void LookingTargetCallback(ViewComponent viewComponent, GameObject target);

    private LookingTargetCallback callback;
    public LookingTargetCallback Callback { get => callback; }


    public LookingTarget(LookingTargetValidator validator, LookingTargetCallback callback)
    {
        this.validator = validator;
        this.callback = callback;
    }
}