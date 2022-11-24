using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Fraction
{
    protected List<Entity> allFractionEntitys = new List<Entity>();

    protected List<Entity> wantsToBeDestroyed = new List<Entity>();
    protected List<Intention> firstQueueOfIntentions = new List<Intention>();
    protected List<Intention> secondQueueOfIntentions = new List<Intention>();

    public List<Intention> CollectIntentions()
    {
        foreach (Entity entity in allFractionEntitys.ToList())
        {
            List<Intention> intentionListFromEntity = entity.MakeIntention();

            if (intentionListFromEntity != null)
            {
                foreach (Intention entityIntention in intentionListFromEntity)
                {
                    firstQueueOfIntentions.Add(entityIntention);
                }
            }
        }
        List<Intention> copyOfFirstQueue = firstQueueOfIntentions.ToList();
        firstQueueOfIntentions.Clear();

        return copyOfFirstQueue;
    }

    public void LiveOneTurn()
    {
        foreach (Entity i in allFractionEntitys.ToList())
        {
            i.LiveOneTurn();
        }
    }

    public void DeclareDeath(Entity dying)
    {
        wantsToBeDestroyed.Add(dying);
    }

    public void KillEveryoneWhoWants()
    {
        foreach (Entity i in wantsToBeDestroyed.ToList())
        {
            DestroyEntity(i);
            wantsToBeDestroyed.Remove(i);
        }
    }

    public void DestroyEntity(Entity i)
    {
        allFractionEntitys.Remove(i);
        Builder.AddCellToRePaint(i.currentCell);
        i.currentCell.RemoveEntityFromCell(i);
        WorldStatusInformation.AddDestroyedEntity(i);

    }

    public void AddEntity(Entity newEntity)
    {
        allFractionEntitys.Add(newEntity);
    }

}
