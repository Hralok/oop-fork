using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : Building
{
    public Mountain(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType)
        : base(currentCell, currentFraction, selfEntityType, new MovingObstructionFromBuilding(0, 0, 7, 6, 1, 1))
    {
        maximalHelthPoints = 1000;
        currentHelthPoints = maximalHelthPoints;
        age = 0;
        selfEntityType = EntityTypeEnum.Mountain;
        resourceFromAttack = ResourceTypeEnum.Stone;
    }

    public override void LiveOneTurn()
    {
        return;
    }

    public override List<Intention> MakeIntention()
    {
        return null;
    }

    protected override void Die()
    {
        Map.SetNewOccupieStatus(currentCell.coords, null, false);
        currentFraction.DeclareDeath(this);
    }
}
