using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : PieacefulAnimals
{
    public Sheep(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information info)
        : base(currentCell, currentFraction, selfEntityType, info)
    {
        maximalHelthPoints = 45;
        maximalSaturation = 50;

        movementPointsForTurn = 3;

        if (info == null)
        {
            switch (UnityEngine.Random.Range(1, 3))
            {
                case 1:
                    this.gender = Gender.Female;
                    
                    break;
                case 2:
                    this.gender = Gender.Male;
                    
                    break;
            }

            currentSaturation = 15;
            currentHelthPoints = maximalHelthPoints;

        }
        else
        {
            switch (info)
            {
                case AnimalBirthInformation anInfo:
                    if (currentSaturation <= 0)
                    {
                        currentSaturation = maximalSaturation;
                    }
                    else
                    {
                        currentSaturation = anInfo.currentSaturation;
                    }

                    if (currentHelthPoints <= 0)
                    {
                        currentHelthPoints = maximalHelthPoints;
                    }
                    else
                    {
                        currentHelthPoints = anInfo.healthPoints;
                    }
                    break;
                case null:
                    break;
                default:
                    throw new Exception("Неверный тип информации для создаваемого существа!");
            }
        }

        if (gender == Gender.Female)
        {
            partnerGender = Gender.Male;
            saturationSufficientQuantity = 20;
            birthSaturetionCost = 5;
            turnsToRestAfterReproduction = 5;
        }
        else
        {
            partnerGender = Gender.Female;
            saturationSufficientQuantity = 25;
            birthSaturetionCost = 7;
            turnsToRestAfterReproduction = 20;
        }

        mainReproductionGender = Gender.Female;

        
        
        vision = 5;
        unitType.Add(UnitTypeEnum.Living);
        initiative = 5;
        selfEntityType = EntityTypeEnum.Sheep;
        resourceFromAttack = ResourceTypeEnum.FreshMeat;

        brain = new PrimitiveAnimalBrain(this);

        damageFromHunger = 3;

        foodThatUnitCanEat.Add(new FoodPreference(ResourceTypeEnum.Grass, 4, null));
        fastFoodThatUnitCanEat.Add(ResourceTypeEnum.Grass);

        saturationConsumption = 1;
        marriageSearchRadius = 20;

        movementDevices.Add(new MovementDevice(RouteTypeEnum.ShortestPath, 1, MovementFieldEnum.Ground, 1, 1));
    }
}
