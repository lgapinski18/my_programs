using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModifiersManagerComponent : MonoBehaviour
{
    private Dictionary<string, List<ModifiersManager>> nameModifiersBindings = new Dictionary<string, List<ModifiersManager>>();

#if UNITY_EDITOR
    [SerializeField]
    private List<string> modifiersNames = new List<string>();

#endif

    public void RegisterModifiersManager(string propertyName, ModifiersManager modifiersManager)
    {
        if (!nameModifiersBindings.ContainsKey(propertyName))
        {
            nameModifiersBindings.Add(propertyName, new List<ModifiersManager>());
        }
        if (!nameModifiersBindings[propertyName].Exists(predicateValue => predicateValue == modifiersManager))
        {
            nameModifiersBindings[propertyName].Add(modifiersManager);
        }


#if UNITY_EDITOR

        if (!modifiersNames.Contains(propertyName))
        {
            modifiersNames.Add(propertyName);
        }

#endif
}

public void UnregisterModifiersManager(string propertyName, ModifiersManager modifiersManager)
    {
        if (nameModifiersBindings.ContainsKey(propertyName))
        {
            nameModifiersBindings[propertyName].Remove(modifiersManager);
        }

#if UNITY_EDITOR

        if (modifiersNames.Contains(propertyName))
        {
            modifiersNames.Remove(propertyName);
        }

#endif
    }

    public void UnregisterModifiersManager(ModifiersManager modifiersManager)
    {
        foreach (var list in nameModifiersBindings.Values)
        {
            list.Remove(modifiersManager);
        }
    }

    //public List<ModifiersManager.ModifierHandle> AddModifierForPropertyName(string propertyName, float modifier)
    public ModifierHandlesGroup AddModifierForPropertyName(string propertyName, float modifier)
    {
        //Debug.Log("Modifier: MMC");
        //Debug.Log("SC: 1" + propertyName + modifier);
        List<ModifiersManager.ModifierHandle> result = new List<ModifiersManager.ModifierHandle>();

        if (nameModifiersBindings.ContainsKey(propertyName))
        {
            //Debug.Log("SC: 2" + propertyName);
            foreach (ModifiersManager modifiersManager in nameModifiersBindings[propertyName])
            {
                //Debug.Log("SC: 3" + propertyName);
                //Debug.Log("Modifier: MMC Applying");
                ModifiersManager.ModifierHandle handle = modifiersManager.AddModifier(modifier);

                result.Add(handle);
            }

        }

        return new ModifierHandlesGroup(result);
    }
    public List<ModifiersManager.ModifierHandle> AddModifierForTimeForPropertyName(string propertyName, float modifier, float time)
    {
        List<ModifiersManager.ModifierHandle> result = new List<ModifiersManager.ModifierHandle>();

        if (nameModifiersBindings.ContainsKey(propertyName))
        {
            foreach (ModifiersManager modifiersManager in nameModifiersBindings[propertyName])
            {
                ModifiersManager.ModifierHandle handle = modifiersManager.AddModifierForTime(modifier, time);

                result.Add(handle);
            }
        }

        return result;
    }

    public void RemoveModifier(ModifiersManager.ModifierHandle handle)
    {
        foreach (var list in nameModifiersBindings.Values)
        {
            foreach (ModifiersManager modifiersManager in list)
            {
                modifiersManager.RemoveModifier(handle);
            }
        }
    }
}
