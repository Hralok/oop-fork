using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Object
{
    public ResourceTypeEnum type {get;}
    private int count;

    public Resource(ResourceTypeEnum type, int count)
    {
        this.type = type;
        this.count = count;
    }

    public int GetCount()
    {
        return count;
    }

    public void IncreaseCount(int delta)
    {
        if (delta > 0)
        {
            count += delta;
        }
    }

    public int DecreaseCount(int delta)
    {
        if (delta < count)
        {
            count -= delta;
            return delta;
        }
        else
        {
            int crutch = count;
            count = 0;
            return crutch;
        }
    }
}
