using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierHandlesGroup
{
    private readonly List<ModifiersManager.ModifierHandle> handles;

    public ModifiersManager.ModifierHandle this[int index] => handles[index];
    public int Count => handles.Count;

    public ModifierHandlesGroup()
    {
        handles = new List<ModifiersManager.ModifierHandle>();
    }

    public ModifierHandlesGroup(List<ModifiersManager.ModifierHandle> handles)
    {
        this.handles = handles;
    }

    public void RemoveSelf()
    {
        foreach (ModifiersManager.ModifierHandle handle in handles)
        {
            handle.RemoveSelf();
        }
        handles.Clear();
    }
}
