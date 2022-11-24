using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdultTree : AdultStage, ISearchingSource
{
    protected List<DropInfo> dropInfoList = new List<DropInfo>();
    protected HashSet<ResourceTypeEnum?> fastDropInfoList = new HashSet<ResourceTypeEnum?>();

    public AdultTree(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information initializationInformation)
    : base(currentCell, currentFraction, selfEntityType, initializationInformation, new MovingObstructionFromBuilding(1, 1, 2, 2, 0, 0))
    {
        maximalHelthPoints = 300;
        maximalSaturation = 20;
        currentSaturation = 1;
        mineralConsumption = 1;
        helthGrowFromPreviousStageMultipler = 6;
        healthPointsRegeneration = 3;
        driedUpVersion = EntityTypeEnum.AdultDeadTree;
        seed = EntityTypeEnum.SeedTree;
        turnsToRestAfterSeedCreation = 25;
        turnsFromTheLastSeedCreation = 0;
        seedCreationSaturationCost = 5;
        saturationSufficientQuantity = 10;
        plantingRadius = 3;

        dropInfoList.Add(new DropInfo(ResourceTypeEnum.Grass, 70, 1, 2));
        dropInfoList.Add(new DropInfo(ResourceTypeEnum.Wood, 15, 1, 3));

        fastDropInfoList.Add(ResourceTypeEnum.Grass);
        fastDropInfoList.Add(ResourceTypeEnum.Wood);

        switch (initializationInformation)
        {
            case PlantBuildingPreviousStageInformation info:
                age = info.age;
                currentHelthPoints = info.healthPoints * helthGrowFromPreviousStageMultipler;
                mineralResourceReserve.IncreaseCount(info.mineralResource.GetCount());
                break;
            case null:
                age = 30;
                currentHelthPoints = maximalHelthPoints;
                break;
            default:
                throw new Exception("При создании сущности был передан неверный подкласс информации");
        }
    }

    public Resource FindSomeResource(Instrument instrument)
    {
        int randInt = UnityEngine.Random.Range(1, 101);

        if (randInt >= dropInfoList[0].chance)
        {
            return new Resource(dropInfoList[0].resourceType, UnityEngine.Random.Range(dropInfoList[0].minCount, dropInfoList[0].maxCount + 1));
        }
        else if (randInt <= dropInfoList[0].chance + dropInfoList[1].chance)
        {
            return new Resource(dropInfoList[1].resourceType, UnityEngine.Random.Range(dropInfoList[1].minCount, dropInfoList[1].maxCount + 1));
        }
        else
        {
            return null;
        }
    }

    public List<DropInfo> GetDropInfo()
    {
        return dropInfoList;
    }

    public HashSet<ResourceTypeEnum?> GetFastDropInfo()
    {
        return fastDropInfoList;
    }
}
