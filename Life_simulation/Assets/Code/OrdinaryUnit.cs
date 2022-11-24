using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OrdinaryUnit : Unit
{
    public OrdinaryUnit(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType)
        : base(currentCell, currentFraction, selfEntityType)
    {

    }

}