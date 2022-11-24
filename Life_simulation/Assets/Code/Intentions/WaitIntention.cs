using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitIntention : Intention
{
    public int turns { private set; get; }

    public WaitIntention(Entity executor, int priorityInExecutorIntentions, int turns)
        :base(executor, priorityInExecutorIntentions)
    {
        this.turns = turns;
    }

    public void ReduceTurnsCount()
    {
        turns -= 1;
    }
}
