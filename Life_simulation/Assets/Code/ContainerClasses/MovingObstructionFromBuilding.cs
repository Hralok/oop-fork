using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstructionFromBuilding
{
    protected int UndergroundEnterCost;
    protected int UndergroundLeaveCost;

    protected int GroundEnterCost;
    protected int GroundLeaveCost;

    protected int AirEnterCost;
    protected int AirLeaveCost;

    public MovingObstructionFromBuilding(
        int UndergroundEnterCost, 
        int UndergroundLeaveCost, 
        int GroundEnterCost, 
        int GroundLeaveCost,
        int AirEnterCost,
        int AirLeaveCost
        )
    {
        this.AirEnterCost = AirEnterCost;
        this.AirLeaveCost = AirLeaveCost;
        this.GroundEnterCost = GroundEnterCost;
        this.GroundLeaveCost = GroundLeaveCost;
        this.UndergroundEnterCost = UndergroundEnterCost;
        this.UndergroundLeaveCost = UndergroundLeaveCost;
    }

    public void SetNewCost(MovementFieldEnum field, MovementType movementType, int newCost)
    {
        switch (field)
        {
            case MovementFieldEnum.Underground:
                switch (movementType)
                {
                    case MovementType.Enter:
                        UndergroundEnterCost = newCost;
                        break;
                    case MovementType.Leave:
                        UndergroundLeaveCost = newCost;
                        break;
                }
                break;
            case MovementFieldEnum.Ground:
                switch (movementType)
                {
                    case MovementType.Enter:
                        GroundEnterCost = newCost;
                        break;
                    case MovementType.Leave:
                        GroundLeaveCost = newCost;
                        break;
                }
                break;
            case MovementFieldEnum.Air:
                switch (movementType)
                {
                    case MovementType.Enter:
                        AirEnterCost = newCost;
                        break;
                    case MovementType.Leave:
                        AirLeaveCost = newCost;
                        break;
                }
                break;
        }
    }

    public enum MovementType
    {
        Enter,
        Leave
    }

    public int MovingCost(MovementFieldEnum field, MovementType movementType)
    {
        int cost = 0;
        switch(field)
        {
            case MovementFieldEnum.Underground:
                switch(movementType)
                {
                    case MovementType.Enter:
                        cost = UndergroundEnterCost;
                        break;
                    case MovementType.Leave:
                        cost = UndergroundLeaveCost;
                        break;
                }
                break;
            case MovementFieldEnum.Ground:
                switch (movementType)
                {
                    case MovementType.Enter:
                        cost = GroundEnterCost;
                        break;
                    case MovementType.Leave:
                        cost = GroundLeaveCost;
                        break;
                }
                break;
            case MovementFieldEnum.Air:
                switch (movementType)
                {
                    case MovementType.Enter:
                        cost = AirEnterCost;
                        break;
                    case MovementType.Leave:
                        cost = AirLeaveCost;
                        break;
                }
                break;
        }
        return cost;
    }
/*
    public static int GetObstraction(EntityTypeEnum buildingType, MovementType movementType, MovementFieldEnum field, RouteTypeEnum routeType)
    {
        switch(buildingType)
        {
            case EntityTypeEnum.Mountain:
                switch(field)
                {
                    case MovementFieldEnum.Ground:

                        switch(routeType)
                        {
                            case RouteTypeEnum.ShortestPath:

                                switch(movementType)
                                {
                                    case MovementType.Enter:
                                        return 7;
                                    case MovementType.Leave:
                                        return 6;
                                    default:
                                        return 0;
                                }



                            default:
                                return 0;
                        }




                    default:
                        return 0;
                }

            default:
                return 0;
        }
    }

*/



}
