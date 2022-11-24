using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaoticAnimals : Animal, ISearcher, IDamageDealer
{
    protected int damage;

    public ChaoticAnimals(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information info)
        : base(currentCell, currentFraction, selfEntityType, info)
    {

    }

    public void DealDamage(MakeDamageIntention intention)
    {
        if (SimulationMath.FindDistance(intention.executor.currentCell.coords, intention.target.currentCell.coords) <= intention.damageDealDistance)
        {

            Resource findedResource = intention.target.TakeDamage(damage);

            if (findedResource != null)
            {
                foreach (Resource i in inventory)
                {
                    if (i.type == findedResource.type)
                    {
                        i.IncreaseCount(findedResource.DecreaseCount(findedResource.GetCount()));
                    }
                }

                if (findedResource.GetCount() != 0)
                {
                    inventory.Add(findedResource);
                }

                EatSomeFood();
            }
        }


    }



    public void Search(FindInIntention intention)
    {
        Resource findedResource = intention.target.FindSomeResource(null);

        if (findedResource != null)
        {
            foreach (Resource i in inventory)
            {
                if (i.type == findedResource.type)
                {
                    i.IncreaseCount(findedResource.DecreaseCount(findedResource.GetCount()));
                }
            }

            if (findedResource.GetCount() != 0)
            {
                inventory.Add(findedResource);
            }

            EatSomeFood();
        }
    }





}
