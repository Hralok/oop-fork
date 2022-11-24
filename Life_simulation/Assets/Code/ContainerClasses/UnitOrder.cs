using System.Collections;
using System.Collections.Generic;

public abstract class UnitOrder
{
    private PriorityEnum priority;

    public UnitOrder(PriorityEnum priority)
    {
        this.priority = priority;
    }

    public PriorityEnum GetPriority()
    {
        return priority;
    }

    public void SetPriority(PriorityEnum new_priority)
    {
        priority = new_priority;
    }
}
