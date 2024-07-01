using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiersManager
{
    #region NESTED_TYPES

    public class ModifierHandle
    {
        private float modifier;
        private ModifiersManager modifiersManager;

        public float Modifier { get => modifier; }
        public ModifiersManager ModifiersManager { get => modifiersManager; }

        public ModifierHandle(float modifier, ModifiersManager modifiersManager)
        {
            this.modifier = modifier;
            this.modifiersManager = modifiersManager;
        }

        public void RemoveSelf()
        {
            modifiersManager.RemoveModifier(this);
        }

    }

    private class TimedModifierHandle
    {
        public ModifierHandle modifierHandle;
        public IEnumerator coroutine;

    }

    public enum ModifiersProcessingType
    {
        Min,
        Max,
        Multiplication,
        Avarage
    }

    #endregion

    List<ModifierHandle> modifiers = new List<ModifierHandle>();
    //List<ModifierHandle> timedModifiers;
    List<TimedModifierHandle> timedModifiers = new List<TimedModifierHandle>();

    private float defaultModifier = 1.0f;
    private float currentModifier = 1.0f;


    private ModifiersProcessor modifiersProcessor;

    private MonoBehaviour owner;

    #region EVENTS_AND_DELEGATES

    public delegate void ModifiersManagerEvent(ModifiersManager modifiersManager);

    public event ModifiersManagerEvent OnChanged;

    public delegate float ModifiersProcessor();

    #endregion

    #region MODIFIERS_PROCESSORS

    private float Min()
    {
        float modifier;
        if (modifiers.Count > 0)
        {
            modifier = modifiers[0].Modifier;
        }
        else
        {
            modifier = timedModifiers[0].modifierHandle.Modifier;
        }
        //for (int i = 1; i < modifiers.Count; i++)
        //{
        //    if (modifiers[i].Modifier < modifier)
        //    {
        //        modifier = modifiers[i].Modifier;
        //    }
        //}
        foreach (ModifierHandle modifierHandle in modifiers)
        {
            if (modifierHandle.Modifier < modifier)
            {
                modifier = modifierHandle.Modifier;
            }
        }

        foreach (TimedModifierHandle timedModifierHandle in timedModifiers)
        {
            if (timedModifierHandle.modifierHandle.Modifier < modifier)
            {
                modifier = timedModifierHandle.modifierHandle.Modifier;
            }
        }

        return modifier;
    }

    private float Max()
    {
        float modifier;
        if (modifiers.Count > 0)
        {
            modifier = modifiers[0].Modifier;
        }
        else
        {
            modifier = timedModifiers[0].modifierHandle.Modifier;
        }
        

        //for (int i = 1; i < modifiers.Count; i++)
        //{
        //    if (modifiers[i].Modifier > modifier)
        //    {
        //        modifier = modifiers[i].Modifier;
        //    }
        //}
        foreach (ModifierHandle modifierHandle in modifiers)
        {
            if (modifierHandle.Modifier > modifier)
            {
                modifier = modifierHandle.Modifier;
            }
        }

        foreach (TimedModifierHandle timedModifierHandle in timedModifiers)
        {
            if (timedModifierHandle.modifierHandle.Modifier > modifier)
            {
                modifier = timedModifierHandle.modifierHandle.Modifier;
            }
        }

        return modifier;
    }

    private float Product()
    {
        float modifier = 1.0f;

        foreach (ModifierHandle modifierHandle in modifiers)
        {
            modifier *= modifierHandle.Modifier;
        }

        foreach (TimedModifierHandle timedModifierHandle in timedModifiers)
        {
            modifier *= timedModifierHandle.modifierHandle.Modifier;
        }

        return modifier;
    }
    private float Avarage()
    {
        float modifier = 0.0f;

        foreach (ModifierHandle modifierHandle in modifiers)
        {
            modifier += modifierHandle.Modifier;
        }

        foreach (TimedModifierHandle timedModifierHandle in timedModifiers)
        {
            modifier += timedModifierHandle.modifierHandle.Modifier;
        }

        return modifier / (modifiers.Count + timedModifiers.Count);
    }

    #endregion

    #region COROUTINES

    private IEnumerator TimeoutModifierCoroutine(TimedModifierHandle modifierHandle, float timeout)
    {
        yield return new WaitForSeconds(timeout);

        timedModifiers.Remove(modifierHandle);

        RecalculateModifier();
    }

    #endregion

    public ModifiersManager(ModifiersProcessingType processingType, MonoBehaviour owner, float defaultModifier = 1.0f)
    {
        //this.modifiersProcessor = modifiersProcessor;
        this.owner = owner;
        this.defaultModifier = defaultModifier;
        this.currentModifier = defaultModifier;

        switch (processingType)
        {
            case ModifiersProcessingType.Min:
                modifiersProcessor = Min;
                break;

            case ModifiersProcessingType.Max:
                modifiersProcessor = Max;

                break;

            case ModifiersProcessingType.Multiplication:
                modifiersProcessor = Product;

                break;

            case ModifiersProcessingType.Avarage:
                modifiersProcessor = Avarage;
                break;
        }

        this.defaultModifier = defaultModifier;
    }

    public float GetModifier()
    {
        return currentModifier;
    }

    public ModifierHandle AddModifier(float modifier)
    {
        //Debug.Log("Modifier: SpeedAttackMM");
        ModifierHandle newModifier = new ModifierHandle(modifier, this);
        modifiers.Add(newModifier);

        //currentModifier = modifiersProcessor(modifier, currentModifier);
        currentModifier = modifiersProcessor();

        OnChanged?.Invoke(this);

        return newModifier;
    }

    public void RemoveModifier(ModifierHandle toRemove)
    {
        bool removed = modifiers.Remove(toRemove);

        if (!removed)
        {
            TimedModifierHandle modifierHandle = timedModifiers.Find((TimedModifierHandle handle) => { return handle.modifierHandle == toRemove; });
            if (modifierHandle != null)
            {
                owner.StopCoroutine(modifierHandle.coroutine);
                timedModifiers.Remove(modifierHandle);

                removed = true;
            }
        }
        
        if (removed)
        {
            RecalculateModifier();
        }
    }

    public ModifierHandle AddModifierForTime(float modifier, float time)
    {
        ModifierHandle newModifier = new ModifierHandle(modifier, this);
        TimedModifierHandle newTimed = new TimedModifierHandle();
        newTimed.modifierHandle = newModifier;
        newTimed.coroutine = TimeoutModifierCoroutine(newTimed, time);
        
        timedModifiers.Add(newTimed);
        owner.StartCoroutine(newTimed.coroutine);

        //currentModifier = modifiersProcessor(modifier, currentModifier);
        currentModifier = modifiersProcessor();

        OnChanged?.Invoke(this);

        return newModifier;
    }

    private void RecalculateModifier()
    {
        if (modifiers.Count > 0 || timedModifiers.Count > 0)
        {
            //float newCurrentModifier = modifiers[0].Modifier;

            //newCurrentModifier = modifiersProcessor();

            //for (int i = 1; i < modifiers.Count; i++)
            //{
            //    //newCurrentModifier = modifiersProcessor(newCurrentModifier, modifiers[i].Modifier);
            //    newCurrentModifier = modifiersProcessor();
            //}
            //for (int i = 0; i < timedModifiers.Count; i++)
            //{
            //    //newCurrentModifier = modifiersProcessor(newCurrentModifier, timedModifiers[i].modifierHandle.Modifier);
            //    newCurrentModifier = modifiersProcessor();
            //}

            //currentModifier = newCurrentModifier;
            currentModifier = modifiersProcessor();
        }
        else
        {
            currentModifier = defaultModifier;
        }

        OnChanged?.Invoke(this);
    }
}

