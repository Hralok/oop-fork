using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionIntention : Intention
{
    public ResourceSource target { get; }
    public Instrument instrument { get; }

    public ExtractionIntention(Entity executor, ResourceSource target, Instrument instrument, int priorityInExecutorIntentions = 0)
    :base(executor, priorityInExecutorIntentions)
    {
        this.instrument = instrument;
        this.target = target;
    }
}
