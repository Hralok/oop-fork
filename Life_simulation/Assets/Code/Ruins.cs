using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ruins : Building
{
    protected int ageToDestract;
    protected int helthFromPreviousStageMultipler;
    protected int damagePerTurn;

    public Ruins(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, MovingObstructionFromBuilding movingObstruction)
        : base(currentCell, currentFraction, selfEntityType, movingObstruction)
    {

    }

}
