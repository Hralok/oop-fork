using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitToUnitOrder : UnitOrder
{
    private UnitToUnitOrderEnum order;
    private GameObject target;

    public UnitToUnitOrder(UnitToUnitOrderEnum order, GameObject target, PriorityEnum priority)
    :base (priority)
    {
        this.order = order;
        this.target = target;
    }

    public UnitToUnitOrderEnum GetOrder()
    {
        return order;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    
}
