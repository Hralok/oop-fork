using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : PieacefulAnimals
{
    public Deer(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information info, Gender gender = Gender.Neutral)
        : base(currentCell, currentFraction, selfEntityType, info)
    {
        if (gender == Gender.Neutral)
        {
            switch (Random.Range(1, 3))
            {
                case 1:
                    this.gender = Gender.Female;
                    saturationSufficientQuantity = 20;
                    birthSaturetionCost = 5;
                    turnsToRestAfterReproduction = 20;
                    break;
                case 2:
                    this.gender = Gender.Male;
                    saturationSufficientQuantity = 25;
                    birthSaturetionCost = 7;
                    turnsToRestAfterReproduction = 5;
                    break;
            }
        }

        if (gender == Gender.Female)
        {
            partnerGender = Gender.Male;
        }
        else
        {
            partnerGender = Gender.Female;
        }

        mainReproductionGender = Gender.Female;

        maximalSaturation = 55;
        currentSaturation = 20;
        maximalHelthPoints = 90;
        vision = 7;
        unitType.Add(UnitTypeEnum.Living);
        initiative = 10;
        selfEntityType = EntityTypeEnum.Deer;
        resourceFromAttack = ResourceTypeEnum.FreshMeat;

        foodThatUnitCanEat.Add(new FoodPreference(ResourceTypeEnum.Grass, 4, null));
        fastFoodThatUnitCanEat.Add(ResourceTypeEnum.Grass);

        saturationConsumption = 1;
        marriageSearchRadius = 13;

        movementDevices.Add(new MovementDevice(RouteTypeEnum.ShortestPath, 9, MovementFieldEnum.Ground, 1, 1));
        movementDevices.Add(new MovementDevice(RouteTypeEnum.ShortestPath, 5, MovementFieldEnum.Ground, 1, 4));
    }
}
