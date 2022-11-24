using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Entity
{
    public MovingObstructionFromBuilding movingObstruction { get; }



    public Building(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, MovingObstructionFromBuilding movingObstruction)
        :base(currentCell, currentFraction, selfEntityType)
    {
        this.movingObstruction = movingObstruction;
    }

}
