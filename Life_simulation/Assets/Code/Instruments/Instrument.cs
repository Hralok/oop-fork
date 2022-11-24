using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instrument
{
    public TierEnum tier { get; }
    public Instrument(TierEnum tier)
    {
        this.tier = tier;
    }
    public abstract int CalculationOfEstimatedResourceProduction(ResourceSourceTypeEnum sourceType, ResourceTypeEnum resourceType);
    


}
