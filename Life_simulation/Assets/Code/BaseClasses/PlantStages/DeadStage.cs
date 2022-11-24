using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStage : Ruins
{
    protected int mineralsFromAsimilation;
    protected int appearance;

    protected GameObject[] appearanceOptions;

    public DeadStage(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information initializationInformation, MovingObstructionFromBuilding movingObstruction)
        : base(currentCell, currentFraction, selfEntityType, movingObstruction)
    {
        
    }

    public override void LiveOneTurn()
    {
        age += 1;

        TakeDamage(damagePerTurn);
    }

    public override List<Intention> MakeIntention()
    {
        return null;
    }

    protected override void Die()
    {
        currentCell.AddResource(new Resource(ResourceTypeEnum.Mineral, mineralsFromAsimilation));
        Map.SetNewOccupieStatus(currentCell.coords, null, false);
        currentFraction.DeclareDeath(this);
    }
}
