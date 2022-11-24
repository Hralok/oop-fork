using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Intention
{
    public Entity executor { get; }

    protected int priorityInExecutorIntentions = 0;

    public int GetPriority()
    {
        return priorityInExecutorIntentions;
    }

    public void SetPriority(int newPriority)
    {
        priorityInExecutorIntentions = newPriority;
    }

    public Intention(Entity executor, int priorityInExecutorIntentions)
    {
        this.executor = executor;
        this.priorityInExecutorIntentions = priorityInExecutorIntentions;
    }

    
}
