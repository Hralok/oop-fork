public class MovementDevice
{
    public RouteTypeEnum routeType { get; }
    public int movementPointsConsumption { get; }
    public int force { get; }
    public MovementFieldEnum movementField { get; }

    public MovementDevice(RouteTypeEnum routeType, int movementPointsConsumption, MovementFieldEnum movementField, int saturationCost, int force)
    {
        this.movementField = movementField;
        this.movementPointsConsumption = movementPointsConsumption;
        this.routeType = routeType;
        this.force = force;
    }

}
