using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : AngryAnimals
{
    public Wolf(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information info, Gender gender = Gender.Neutral)
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

        maximalSaturation = 120;
        currentSaturation = 15;
        maximalHelthPoints = 45;
        vision = 5;
        unitType.Add(UnitTypeEnum.Living);
        initiative = 5;
        selfEntityType = EntityTypeEnum.Wolf;
        resourceFromAttack = ResourceTypeEnum.FreshMeat;

        foodThatUnitCanEat.Add(new FoodPreference(ResourceTypeEnum.FreshMeat, 4, null));
        fastFoodThatUnitCanEat.Add(ResourceTypeEnum.FreshMeat);

        damage = 4;

        saturationConsumption = 3;
        marriageSearchRadius = 20;

        movementDevices.Add(new MovementDevice(RouteTypeEnum.ShortestPath, 15, MovementFieldEnum.Ground, 1, 1));
    }
}
