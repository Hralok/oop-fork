using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedStage : PlantBuilding
{
    protected EntityTypeEnum olderVersion;

    protected int ageCountToGrow;

    public SeedStage(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information initializationInformation, MovingObstructionFromBuilding movingObstruction)
        : base(currentCell, currentFraction, selfEntityType, movingObstruction)
    {
        age = 0;
    }

    public override void LiveOneTurn()
    {
        age += 1;
    }

    public override List<Intention> MakeIntention()
    {
        if (age >= ageCountToGrow)
        {
            List<Intention> intentionList = new List<Intention>();
            intentionList.Add(
                new CreateIntention(
                    this, 
                    olderVersion, 
                    currentCell.coords, 
                    new PlantBuildingPreviousStageInformation(
                        age, 
                        currentHelthPoints, 
                        mineralResourceReserve
                        ), 
                    currentFraction
                    )
                );
            return intentionList;
        }
        else
        {
            return null;
        }
    }

    protected override void Die()
    {
        currentCell.AddResource(new Resource(ResourceTypeEnum.Mineral, ageCountToGrow - age));
        Map.SetNewOccupieStatus(currentCell.coords, null, false);
        currentFraction.DeclareDeath(this);
    }
}
