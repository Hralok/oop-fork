using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIntention : Intention
{
    public Cell targetCell { get; }

    public MoveIntention(Entity executor, Cell targetCell, int priorityInExecutorIntentions = 0)
        :base(executor, priorityInExecutorIntentions)
    {
        this.targetCell = targetCell;





    }
}
