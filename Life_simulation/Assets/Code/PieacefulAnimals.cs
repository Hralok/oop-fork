using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieacefulAnimals : Animal, ISearcher
{
    public PieacefulAnimals(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information info)
        : base(currentCell, currentFraction, selfEntityType, info)
    {
        
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
