using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindInIntention : Intention
{
    public ISearchingSource target { get; }
    public FindInIntention(Entity executor, ISearchingSource target, int priorityInExecutorIntentions = 0)
        :base(executor, priorityInExecutorIntentions)
    {
        this.target = target;
    }
}
