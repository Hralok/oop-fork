using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateIntention : Intention
{
    public EntityTypeEnum objectToCreate { get; }
    public Vector3Int targetCellCords { get; }
    public Fraction parentFraction { get; }
    public Information informationForCreated { get; }

    public CreateIntention(
        Entity executor, 
        EntityTypeEnum objectToCreate, 
        Vector3Int targetCellCords, 
        Information informationForCreated, 
        Fraction parentFraction,
        int priorityInExecutorIntentions = 0
        )
        :base(executor, priorityInExecutorIntentions)
    {
        this.objectToCreate = objectToCreate;
        this.targetCellCords = targetCellCords;
        this.informationForCreated = informationForCreated;
        this.parentFraction = parentFraction;
    }
}
