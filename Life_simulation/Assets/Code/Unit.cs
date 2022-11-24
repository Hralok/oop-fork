using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public abstract class Unit : Entity
{
    public Unit(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType)
        :base(currentCell, currentFraction, selfEntityType)
    {

    }

    public int movementPointsForTurn { protected set; get; }
    public int vision { protected set; get; }
    public int currentSaturation { protected set; get; }
    public int maximalSaturation { protected set; get; }
    public int capacity { protected set; get; }
    public Gender gender { protected set; get; }
    public int saturationConsumption { protected set; get; }
    public int damageFromHunger { protected set; get; }

    public HashSet<FoodPreference> foodThatUnitCanEat { protected set; get; } = new HashSet<FoodPreference>();
    public HashSet<ResourceTypeEnum?> fastFoodThatUnitCanEat { protected set; get; } = new HashSet<ResourceTypeEnum?>();

    public List<Object> inventory { protected set; get; } = new List<Object>();
    public List<UnitOrder> queueOfOrders { protected set; get; } = new List<UnitOrder>();

    public HashSet<UnitTypeEnum> unitType { protected set; get; } = new HashSet<UnitTypeEnum>();
    public HashSet<MovementDevice> movementDevices { protected set; get; } = new HashSet<MovementDevice>();

    public HashSet<UnitTypeEnum> GetUnitType()
    {
        return unitType;
    }
}
