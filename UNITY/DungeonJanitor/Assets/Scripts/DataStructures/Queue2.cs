using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue2<TElement>
{
    private TElement element_0;
    private TElement element_1;

    public Queue2(TElement element_0, TElement element_1)
    {
        this.element_0 = element_0;
        this.element_1 = element_1;
    }

    public void Push(TElement element)
    {
        this.element_0 = element_1;
        this.element_1 = element;
    }

    public TElement Zero() { return this.element_0; }
    public TElement One() { return this.element_1;}
}
