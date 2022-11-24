using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceSource
{
    public ResourceTypeEnum resourceType { get; }
    public ResourceSourceTypeEnum sourceType { get; }
    protected int count;
    /*{ 
        get 
        { 
            return count; 
        } 
        private set 
        { 
            count = value; 
        } 
    }*/
    protected HashSet<InstrumentTypeEnum> bestSuitableInstruments { get; }
    protected HashSet<InstrumentTypeEnum> suitableInstruments { get; }

    public ResourceSource(int newCount, ResourceTypeEnum type)
    {
        bestSuitableInstruments = new HashSet<InstrumentTypeEnum>();
        suitableInstruments = new HashSet<InstrumentTypeEnum>();
        resourceType = type;

        if (count >= 0)
        {
            count = newCount;
        }
        else
        {
            count = 0;
        }
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

    public int ExtractResource(Instrument instrument)
    {
        int base_count = instrument.CalculationOfEstimatedResourceProduction(sourceType, resourceType);
        if (base_count <= count)
        {
            count -= base_count;
            return base_count;
        }
        else
        {
            base_count = count;
            count = 0;
            return base_count;
        }
    }
}
