using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductionIntention : Intention
{
    public IReproductive partner { get; }
    public ReproductionIntention(Entity executor, IReproductive partner, int priorityInExecutorIntentions = 0)
        :base(executor, priorityInExecutorIntentions)
    {
        this.partner = partner;
    }
}
