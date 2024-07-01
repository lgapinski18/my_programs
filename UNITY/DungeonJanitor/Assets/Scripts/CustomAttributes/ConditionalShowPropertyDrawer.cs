
#if (UNITY_EDITOR) 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalShowProperty))]
public class ConditionalShowPropertyDrawer : PropertyDrawer
{
    bool ShouldShow(SerializedProperty property)
    {
        var conditionAttribute = (ConditionalShowProperty)attribute;
        string propertyPath = conditionAttribute.property;

        // If this property is defined inside a nested type 
        // (like a struct inside a MonoBehaviour), look for
        // our condition field inside the same nested instance.
        string thisPropertyPath = property.propertyPath;
        int last = thisPropertyPath.LastIndexOf('.');
        if (last > 0)
        {
            string containerPath = thisPropertyPath.Substring(0, last + 1);
            propertyPath = containerPath + propertyPath;
        }

        var conditionProperty = property.serializedObject.FindProperty(propertyPath);

        if (conditionProperty == null)
        {
            return false;
        }

        return conditionProperty.GetUnderlyingValue().Equals(conditionAttribute.expectedValue);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property))
        {
            // Provision the normal vertical spacing for this control.
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        else
        {
            // Collapse the unseen derived property.
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}

#endif