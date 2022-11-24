using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    public int age { protected set; get; } = 0;
    public int currentHelthPoints { protected set; get; }
    public int maximalHelthPoints { protected set; get; }
    public int initiative { protected set; get; } = 1;
    public Cell currentCell { protected set; get; }
    public Fraction currentFraction { protected set; get; }
    public EntityTypeEnum selfEntityType { get; }
    public ResourceTypeEnum? resourceFromAttack { protected set; get; }
    public List<Effect> effectsList { protected set; get; } = new List<Effect>();

    public void MoveRightNow(MoveIntention intention)
    {
        intention.executor.currentCell.RemoveEntityFromCell(intention.executor);
        intention.executor.currentCell = intention.targetCell;
        intention.executor.currentCell.AddEntityToCell(intention.executor);
    }

    public virtual Resource TakeDamage(int damage)
    {
        if (currentHelthPoints > 0)
        {
            int realDamage;
            if (damage > currentHelthPoints)
            {
                realDamage = currentHelthPoints;
            }
            else
            {
                realDamage = damage;
            }

            currentHelthPoints -= damage;

            if (currentHelthPoints <= 0)
            {
                Die();
            }

            if (resourceFromAttack == null)
            {
                return null;
            }
            else
            {
                return new Resource((ResourceTypeEnum)resourceFromAttack, realDamage);
            };
        }
        else
        {
            return null;
        }
    }

    public abstract List<Intention> MakeIntention();
    public abstract void LiveOneTurn();

    public Entity(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType)
    {
        this.currentCell = currentCell;
        this.currentFraction = currentFraction;
        this.selfEntityType = selfEntityType;
    }

    public void SetCurrentCell(Cell newCurrentCell)
    {
        currentCell = newCurrentCell;
    }

    protected virtual void Initialization()
    {

    }
    protected abstract void Die();
}
