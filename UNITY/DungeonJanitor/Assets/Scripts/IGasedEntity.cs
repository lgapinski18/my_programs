using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGasedEntity
{
    public bool IsGased { set; get; }
    public void GasEntity();
    public void DecayGas();
}
