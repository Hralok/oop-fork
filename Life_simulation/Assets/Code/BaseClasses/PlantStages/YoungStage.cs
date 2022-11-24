using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoungStage : PlantBuilding, IExtract
{
    protected EntityTypeEnum olderVersion;
    protected int ageCountToGrow;

    protected int helthGrowFromPreviousStageMultipler;
    protected int healthPointsRegeneration;

    protected EntityTypeEnum driedUpVersion;

    public YoungStage(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information initializationInformation, MovingObstructionFromBuilding movingObstruction)
    : base(currentCell, currentFraction, selfEntityType, movingObstruction)
    {
        
    }

    public override void LiveOneTurn()
    {

        age += 1;
        currentSaturation -= 1;

        if (mineralResourceReserve.DecreaseCount(mineralConsumption) == mineralConsumption)
        {
            if (currentSaturation + 2 <= maximalSaturation)
            {
                currentSaturation += 2;
            }
            else
            {
                currentSaturation = maximalSaturation;
            }
        }

        if (currentHelthPoints < maximalHelthPoints)
        {
            if (mineralResourceReserve.DecreaseCount(1) != 0)
            {
                currentHelthPoints += healthPointsRegeneration;
            }
        }
    }

    public override List<Intention> MakeIntention()
    {
        List<Intention> intentionList = new List<Intention>();

        if (currentCell.GetMineralResourceSource().GetCount() > 0)
        {
            intentionList.Add(
                new ExtractionIntention
                (
                    this,
                    currentCell.GetMineralResourceSource(),
                    new PlantRootsInstrument
                    (
                        TierEnum.First
                        )
                    )
                );
        }

        if (currentSaturation <= 0)
        {
            intentionList.Add(
                    new CreateIntention(
                        this,
                        driedUpVersion,
                        currentCell.coords,
                        new RuinsPreviousStageInformation(
                            age,
                            currentHelthPoints
                            ),
                        currentFraction
                        )
                    );
        }
        else
        {
            if (currentHelthPoints >= maximalHelthPoints && age >= ageCountToGrow)
            {
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
            }
        }

        if (intentionList.Count != 0)
        {
            return intentionList;
        }
        else
        {
            return null;
        }
    }

    protected override void Die()
    {
        currentCell.AddResource(new Resource(ResourceTypeEnum.Mineral, age + mineralResourceReserve.GetCount()));
        Map.SetNewOccupieStatus(currentCell.coords, null, false);
        currentFraction.DeclareDeath(this);
    }

    public void Extract(ResourceSource target)
    {
        if (target.resourceType == ResourceTypeEnum.Mineral && target.sourceType == ResourceSourceTypeEnum.Underground)
        {
            mineralResourceReserve.IncreaseCount(
            currentCell.
            GetMineralResourceSource().
            ExtractResource(
                new PlantRootsInstrument(TierEnum.First)
            )
        );
        }
    }
}
