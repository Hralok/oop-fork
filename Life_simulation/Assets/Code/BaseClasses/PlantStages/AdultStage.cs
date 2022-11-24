using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdultStage : PlantBuilding, IExtract
{
    protected EntityTypeEnum seed;
    protected int turnsToRestAfterSeedCreation;
    protected int turnsFromTheLastSeedCreation;
    protected int seedCreationSaturationCost;
    protected int saturationSufficientQuantity;
    protected int plantingRadius;

    protected int helthGrowFromPreviousStageMultipler;
    protected int healthPointsRegeneration;

    protected EntityTypeEnum driedUpVersion;

    public AdultStage(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information initializationInformation, MovingObstructionFromBuilding movingObstruction)
    : base(currentCell, currentFraction, selfEntityType, movingObstruction)
    {
        
    }

    public override void LiveOneTurn()
    {
        age += 1;
        turnsFromTheLastSeedCreation += 1;
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
                if (currentHelthPoints + healthPointsRegeneration <= maximalHelthPoints)
                {
                    currentHelthPoints += healthPointsRegeneration;
                }
                else
                {
                    currentHelthPoints = maximalHelthPoints;
                }
            }
        }
    }

    protected Intention PlantSeed()
    {
        currentSaturation -= seedCreationSaturationCost;

        turnsFromTheLastSeedCreation = 0;

        Vector3Int randCoords;

        do
        {
            randCoords = SimulationMath.CreateRandomCoords(currentCell.coords, plantingRadius);
        }
        while (!SimulationMath.CheckCoordsToInclusionIntoMap(randCoords));

        return new CreateIntention(
            this, 
            seed, 
            randCoords, 
            null, 
            currentFraction
            );
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
            if (turnsFromTheLastSeedCreation >= turnsToRestAfterSeedCreation)
            {
                if (currentSaturation >= saturationSufficientQuantity + seedCreationSaturationCost)
                {
                    intentionList.Add(PlantSeed());
                }
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
        currentCell.AddResource(new Resource(ResourceTypeEnum.Mineral, maximalHelthPoints + mineralResourceReserve.GetCount()));
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
