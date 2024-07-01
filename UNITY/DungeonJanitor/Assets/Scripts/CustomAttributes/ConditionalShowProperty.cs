using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ConditionalShowProperty : PropertyAttribute
{
    public string property;
    public object expectedValue;
    public ConditionalShowProperty(string property)
    {
        this.property = property;
        this.expectedValue = true;
    }

    public ConditionalShowProperty(string property, object expectedValue)
    {
        this.property = property;
        this.expectedValue = expectedValue;
    }
}
