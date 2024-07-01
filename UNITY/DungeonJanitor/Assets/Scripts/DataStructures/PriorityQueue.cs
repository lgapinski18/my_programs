using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PriorityQueue<TElement, TPriority>
{
    List<Tuple<TElement, TPriority>> elements = new List<Tuple<TElement, TPriority>>();

    public delegate bool Comparator(TPriority priority1, TPriority priority2);

    Comparator comparator;

    public int Count {  get => elements.Count; }

    public PriorityQueue(Comparator comparator)
    {
        this.comparator = comparator;
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        if (elements.Count == 0)
        {
            elements.Add(new Tuple<TElement, TPriority>(element, priority));
        }
        else
        {
            int index = elements.FindIndex(0, elem => comparator(priority, elem.Item2));
            if (index ==  -1)
            {
                elements.Add(new Tuple<TElement, TPriority>(element, priority));
            }
            else
            {
                elements.Insert(index, new Tuple<TElement, TPriority>(element, priority));
            }
            //Debug.Log("Index pq: " + index);
            //Debug.Log("Count pq: " + elements.Count);
        }
    }

    public TElement Dequeue()
    {
        if (elements.Count == 0)
        {
            return default(TElement);
        }
        Tuple<TElement, TPriority> element = elements[0];

        elements.RemoveAt(0);

        return element.Item1;
    }

}
