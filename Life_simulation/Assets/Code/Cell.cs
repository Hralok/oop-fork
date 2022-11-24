using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : IComparable
{
    public Vector3Int coords { get; }
    public CellOccupiedStatus occupiedStatus { private set; get; }


    private List<Entity> entitysAtCell = new List<Entity>();
    private HashSet<ResourceSource> resourceSources = new HashSet<ResourceSource>();
    private List<Resource> lyingResources = new List<Resource>();
    private MovingObstructionFromBuilding bestObsruction = new MovingObstructionFromBuilding(0, 0, 0, 0, 0, 0);

    public Cell(int x, int y, int z)
    {
        coords = new Vector3Int(x, y, z);
    }

    public void SetNewOccupiedStatus(bool ground, bool underground)
    {
        CellOccupiedStatus newStatus = new CellOccupiedStatus();
        newStatus.ground = ground;
        newStatus.underground = underground;
        occupiedStatus = newStatus;
    }

    public int GetObstruction(MovementFieldEnum field, MovingObstructionFromBuilding.MovementType movementType)
    {
        return bestObsruction.MovingCost(field, movementType);
    }

    public List<Entity> GetEntitiesAtCell()
    {
        return entitysAtCell;
    }

    public void AddEntityToCell(Entity newEntity)
    {
        entitysAtCell.Add(newEntity);
        newEntity.SetCurrentCell(this);

        RecalculateObstraction();

    }

    private void RecalculateObstraction()
    {
        bestObsruction = new MovingObstructionFromBuilding(0, 0, 0, 0, 0, 0);

        foreach (Entity entity in entitysAtCell)
        {
            switch (entity)
            {
                case Building building:
                    foreach (MovementFieldEnum movementField in Enum.GetValues(typeof(MovementFieldEnum)))
                    {
                        foreach (MovingObstructionFromBuilding.MovementType movementType in Enum.GetValues(typeof(MovingObstructionFromBuilding.MovementType)))
                        {
                            if (
                                (bestObsruction.MovingCost(movementField, movementType) != -1 && building.movingObstruction.MovingCost(movementField, movementType) == -1) ||
                                (bestObsruction.MovingCost(movementField, movementType) != -1 && building.movingObstruction.MovingCost(movementField, movementType) > bestObsruction.MovingCost(movementField, movementType))
                                )
                            {
                                bestObsruction.SetNewCost(movementField, movementType, building.movingObstruction.MovingCost(movementField, movementType));
                            }
                        }
                    }
                    break;
            }

        }
    }

    public void RemoveEntityFromCell(Entity entityToRemove)
    {
        entitysAtCell.Remove(entityToRemove);
        RecalculateObstraction();
    }

    public void DropResorce(Resource resource)
    {
        lyingResources.Add(resource);
    }

    public void AddResource(Resource newResource)
    {
        bool alreadyIn = false;
        foreach (ResourceSource source in resourceSources)
        {
            if (source.resourceType == newResource.type)
            {
                alreadyIn = true;
                source.IncreaseCount(newResource.GetCount());
            }
        }

        if (!alreadyIn)
        {
            ResourceSource newSource;
            switch (newResource.type)
            {
                case ResourceTypeEnum.Mineral:
                    newSource = new MineralResourceSource(newResource.GetCount());
                    resourceSources.Add(newSource);
                    break;
            }
        }
    }

    public ResourceSource GetMineralResourceSource()
    {
        foreach (var i in resourceSources)
        {
            if (i.resourceType == ResourceTypeEnum.Mineral)
            {
                return i;
            }
        }
        return null;
    }

    public int CompareTo(object obj)
    {
        Cell cellToCompare = obj as Cell;

        if (cellToCompare != null)
        {
            if (cellToCompare.coords.x < coords.x)
            {
                return 1;
            }
            else if (cellToCompare.coords.x > coords.x)
            {
                return -1;
            }
            else if (cellToCompare.coords.y < coords.y)
            {
                return 1;
            }
            else if (cellToCompare.coords.y > coords.y)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            throw new Exception("Невозможно сравнить два объекта!");
        }

    }


}
