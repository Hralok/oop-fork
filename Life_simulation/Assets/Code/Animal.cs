using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Animal : OrdinaryUnit, IReproductive
{
    public int saturationSufficientQuantity { protected set; get; }
    public int birthSaturetionCost { protected set; get; }
    public int turnsFromLastReproduction { protected set; get; } = 0;
    public int turnsToRestAfterReproduction { protected set; get; }
    public int marriageSearchRadius { protected set; get; }
    public Gender partnerGender { protected set; get; }
    public Gender mainReproductionGender { protected set; get; }
    public Brain brain { protected set; get; }
    public bool alreadyMadeTurn { protected set; get; } = false;

    public bool CheckReadyForReplacementStatus()
    {
        if (currentSaturation > saturationSufficientQuantity && turnsFromLastReproduction >= turnsToRestAfterReproduction)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Animal(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information info)
        : base(currentCell, currentFraction, selfEntityType)
    {
        switch (info)
        {
            case AnimalBirthInformation anInfo:
                gender = anInfo.gender;
                age = anInfo.age;

                break;
            case null:
                break;
            default:
                throw new Exception("Неверный тип информации для создаваемого существа!");
        }
    }

    public override void LiveOneTurn()
    {
        if (turnsFromLastReproduction < turnsToRestAfterReproduction)
        {
            turnsFromLastReproduction += 1;
        }

        EatSomeFood();

        if (currentSaturation > 0)
        {
            currentSaturation -= saturationConsumption;
            EatSomeFood();
        }
        else
        {
            TakeDamage(damageFromHunger);
        }

        alreadyMadeTurn = false;
    }

    public override List<Intention> MakeIntention()
    {
        alreadyMadeTurn = true;
        return brain.Think();
    }

    protected override void Die()
    {
        currentFraction.DeclareDeath(this);
    }

    public virtual void Reproduct(ReproductionIntention intention)
    {
        if (intention.executor.currentCell == ((Entity)intention.partner).currentCell)
        {
            turnsFromLastReproduction = 0;
            currentSaturation -= birthSaturetionCost;

            if (gender == mainReproductionGender)
            {
                Builder.Build(new CreateIntention(this, EntityTypeEnum.Sheep, currentCell.coords, null, currentFraction));
            }

            if (intention.executor == this && intention.executor != intention.partner)
            {
                intention.partner.Reproduct(intention);
            }
        }
    }

    protected void EatSomeFood()
    {
        foreach (var foodPref in foodThatUnitCanEat)
        {
            foreach (Resource potentialFood in inventory)
            {
                if (foodPref.foodType == potentialFood.type)
                {
                    while (potentialFood.GetCount() > 0 && currentSaturation < maximalSaturation)
                    {
                        potentialFood.DecreaseCount(1);
                        currentSaturation += foodPref.saturationFromOne;
                    }

                    if (currentSaturation >= maximalSaturation)
                    {
                        break;
                    }
                }
            }

            if (currentSaturation >= maximalSaturation)
            {
                break;
            }
        }
    }

}
