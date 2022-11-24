using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInfo
{
    public ResourceTypeEnum resourceType { get; }
    public byte chance { get; }
    public int minCount { get; }
    public int maxCount { get; }

    public DropInfo(ResourceTypeEnum resourceType, byte chance, int minCount, int maxCount)
    {
        this.resourceType = resourceType;
        this.chance = chance;
        this.minCount = minCount;
        this.maxCount = maxCount;
    }
}
