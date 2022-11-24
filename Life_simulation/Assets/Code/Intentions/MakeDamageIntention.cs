using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDamageIntention : Intention
{
    public Entity target { get; }
    public int damageDealDistance { get; }
    public MakeDamageIntention(Entity executor, Entity target, int damageDealDistance, int priorityInExecutorIntentions = 0)
        :base(executor, priorityInExecutorIntentions)
    {
        this.target = target;
        this.damageDealDistance = damageDealDistance;
    }

}
