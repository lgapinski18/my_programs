using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiersAffector : MonoBehaviour
{
    #region NESTED_TYPES

    [Serializable]
    private struct Modifier
    {
        public string propertyName;
        public float modifier;
    }

    private class AffectedRecord
    {
        public GameObject affected;
        public int count;
        //ListHandle
        //public List<ModifiersManager.ModifierHandle> handles = new List<ModifiersManager.ModifierHandle>();
        public List<ModifierHandlesGroup> handles = new List<ModifierHandlesGroup>();

        public AffectedRecord(GameObject affected, int count)
        {
            this.affected = affected;
            this.count = count;
        }
    }

    #endregion

    #region SCRIPT_TYPES

    [SerializeField]
    private Modifier[] modifiers;

    #endregion

    #region SCRIPT_FIELDS

    List<AffectedRecord> affectedList = new List<AffectedRecord>();

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ModifiersManagerComponent modifiersManager = collision.GetComponentInParent<ModifiersManagerComponent>();
        if (modifiersManager != null)
        {
            AffectedRecord record = affectedList.Find(record => record.affected == collision.gameObject);
            if (record == null)
            {
                AffectedRecord newRecord = new AffectedRecord(collision.gameObject, 1);
                affectedList.Add(newRecord);
                //ListHandle
                //List<ModifiersManager.ModifierHandle> newHandles = new List<ModifiersManager.ModifierHandle>();
                List<ModifierHandlesGroup> newHandles = new List<ModifierHandlesGroup>();
                //Debug.Log("Modifier: InArea");
                foreach (Modifier modifier in modifiers)
                {
                    //Debug.Log("Modifier: ForArea");
                    //ListHandle
                    //newHandles.AddRange(modifiersManager.AddModifierForPropertyName(modifier.propertyName, modifier.modifier));
                    newHandles.Add(modifiersManager.AddModifierForPropertyName(modifier.propertyName, modifier.modifier));
                }
                newRecord.handles = newHandles;
            }
            else
            {
                record.count++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        AffectedRecord record = affectedList.Find(record => record.affected == collision.gameObject);
        if (record != null)
        {
            record.count--;
            //Debug.Log("Decrementing: " + record.count);
            if (record.count == 0)
            {
                affectedList.Remove(record);


                ModifiersManagerComponent modifiersManager = record.affected.GetComponentInParent<ModifiersManagerComponent>();
                //ListHandle
                //foreach (ModifiersManager.ModifierHandle handle in record.handles) 
                //{
                //    modifiersManager.RemoveModifier(handle);
                //}

                foreach (ModifierHandlesGroup handlesGroup in record.handles)
                {
                    handlesGroup.RemoveSelf();
                }

            }

        }
    }

    //public void OnChildTriggerEnter2D(Collider2D collision)
    //{
    //    //Debug.Log("Modifier: Area");
    //    ModifiersManagerComponent modifiersManager = collision.GetComponentInParent<ModifiersManagerComponent>();
    //    if (modifiersManager != null)
    //    {
    //        AffectedRecord record = affectedList.Find(record => record.affected == collision.gameObject);
    //        if (record == null)
    //        {
    //            AffectedRecord newRecord = new AffectedRecord(collision.gameObject, 1);
    //            affectedList.Add(newRecord);
    //            List<ModifiersManager.ModifierHandle> newHandles = new List<ModifiersManager.ModifierHandle>();
    //            //Debug.Log("Modifier: InArea");
    //            foreach (Modifier modifier in modifiers)
    //            {
    //                //Debug.Log("Modifier: ForArea");
    //                newHandles.AddRange( modifiersManager.AddModifierForPropertyName(modifier.propertyName, modifier.modifier) );
    //            }
    //            newRecord.handles = newHandles;
    //        }
    //        else
    //        {
    //            record.count++;
    //        }
    //    }
    //}

    //public void OnChildTriggerExit2D(Collider2D collision)
    //{
    //    AffectedRecord record = affectedList.Find(record => record.affected == collision.gameObject);
    //    if (record != null)
    //    {
    //        record.count--;
    //        Debug.Log("Decrementing: " + record.count);
    //        if (record.count == 0)
    //        {
    //            affectedList.Remove(record);
    //
    //
    //            ModifiersManagerComponent modifiersManager = record.affected.GetComponentInParent<ModifiersManagerComponent>();
    //            foreach (ModifiersManager.ModifierHandle handle in record.handles)
    //            {
    //                modifiersManager.RemoveModifier(handle);
    //            }
    //            
    //        }
    //
    //    }
    //
    //    //Debug.Log("Modifier: Area");
    //    //ModifiersManagerComponent modifiersManager = collision.GetComponent<ModifiersManagerComponent>();
    //    //if (modifiersManager != null)
    //    //{
    //    //    AffectedRecord record = affectedList.Find(record => record.affected == collision.gameObject);
    //    //    if (record == null)
    //    //    {
    //    //        affectedList.Add(new AffectedRecord(collision.gameObject, 1));
    //    //
    //    //        Debug.Log("Modifier: InArea");
    //    //        foreach (Modifier modifier in modifiers)
    //    //        {
    //    //            Debug.Log("Modifier: ForArea");
    //    //            modifiersManager.AddModifierForPropertyName(modifier.propertyName, modifier.modifier);
    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        record.count++;
    //    //    }
    //    //}
    //}

    private void Update()
    {
        
    }

}
