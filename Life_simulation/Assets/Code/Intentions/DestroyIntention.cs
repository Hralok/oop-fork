using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIntention : Intention
{
    private Entity target { get; } 

    public DestroyIntention(Entity executor, Entity target, int priorityInExecutorIntentions = 0)
        : base(executor, priorityInExecutorIntentions)
    {
        this.target = target;
    }

}
