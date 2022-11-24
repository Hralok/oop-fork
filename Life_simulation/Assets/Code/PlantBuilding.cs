using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantBuilding : Building
{
    protected Resource mineralResourceReserve = new Resource(ResourceTypeEnum.Mineral, 0);
    protected int currentSaturation;
    protected int maximalSaturation;
    protected int mineralConsumption;

    public PlantBuilding(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, MovingObstructionFromBuilding movingObstruction)
        : base(currentCell, currentFraction, selfEntityType, movingObstruction)
    {

    }
}
