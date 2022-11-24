using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedTree : SeedStage, ISearchingSource
{
    protected List<DropInfo> dropInfoList = new List<DropInfo>();
    protected HashSet<ResourceTypeEnum?> fastDropInfoList = new HashSet<ResourceTypeEnum?>();

    public SeedTree(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information initializationInformation)
        : base(currentCell, currentFraction, selfEntityType, initializationInformation, new MovingObstructionFromBuilding(0, 0, 0, 0, 0, 0))
    {
        maximalHelthPoints = 10;
        currentHelthPoints = maximalHelthPoints;
        currentSaturation = 0;
        maximalSaturation = 0;
        mineralConsumption = 0;
        ageCountToGrow = 15;
        olderVersion = EntityTypeEnum.YoungTree;
        dropInfoList.Add(new DropInfo(ResourceTypeEnum.Grass, 70, 2, 5));
        fastDropInfoList.Add(ResourceTypeEnum.Grass);
    }


    public Resource FindSomeResource(Instrument instrument)
    {
        if (Random.Range(1, 101) <= dropInfoList[0].chance)
        {
            return new Resource(dropInfoList[0].resourceType, Random.Range(dropInfoList[0].minCount, dropInfoList[0].maxCount + 1));
        }
        else
        {
            TakeDamage(1);
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
