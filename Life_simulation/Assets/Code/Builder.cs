using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
public static class Builder
{
    private static Dictionary<EntityTypeEnum, TileBase> entityTilesDict;
    private static Dictionary<TileBase, EntityTypeEnum> entityTypesDict;

    private static HashSet<Cell> cellsToRepaint = new HashSet<Cell>();

    private static Tilemap groundTilemap;
    private static Tilemap buildingsTilemap;
    private static Tilemap unitsTilemap;

    public static void InitializeBuilder()
    {
        if (entityTilesDict == null)
        {
            groundTilemap = GameObject.Find("Grid").transform.Find("Ground Tilemap").GetComponent<Tilemap>();
            buildingsTilemap = GameObject.Find("Grid").transform.Find("Mountain Tilemap").GetComponent<Tilemap>();
            unitsTilemap = GameObject.Find("Grid").transform.Find("Units Tilemap").GetComponent<Tilemap>();

            entityTilesDict = new Dictionary<EntityTypeEnum, TileBase>();

            entityTilesDict.Add(EntityTypeEnum.SpiderWorkerMassT1Unit, Resources.Load<TileBase>("Prefabs/Spider"));
            entityTilesDict.Add(EntityTypeEnum.SeedTree, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_0"));
            entityTilesDict.Add(EntityTypeEnum.YoungTree, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_1"));
            entityTilesDict.Add(EntityTypeEnum.YoungDeadTree, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_13"));
            entityTilesDict.Add(EntityTypeEnum.AdultTree, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_2"));
            entityTilesDict.Add(EntityTypeEnum.AdultDeadTree, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_13"));
            entityTilesDict.Add(EntityTypeEnum.Mountain, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_5"));
            entityTilesDict.Add(EntityTypeEnum.Sheep, Resources.Load<TileBase>("Tiles/SF_Monster_7"));
        }
    }

    public static void AddCellToRePaint(Cell cell)
    {
        cellsToRepaint.Add(cell);
    }

    public static void RePaintAll()
    {
        foreach (var i in cellsToRepaint.ToList())
        {
            RepaintCell(i.coords);
            cellsToRepaint.Remove(i);
        }
    }

    public static void RepaintCell(Vector3Int coords)
    {
        TileBase newBuildingTile = null;
        TileBase newUnitTile = null;
        bool buildingTileIsCorrect = false;
        bool unitTileIsCorrect = false;

        TileBase CheckEntity(Tilemap tilemap, Entity entity, ref bool correctMarker)
        {
            if (tilemap.GetTile
                        (SimulationMath.CropVector3ToVector3Int
                            (
                                SimulationMath.ConvertNormalCoordsToUnity(coords)
                            )
                        ) == entityTilesDict[entity.selfEntityType]
                        )
            {
                correctMarker = true;
                return null;
            }
            else
            {
                return entityTilesDict[entity.selfEntityType];
            }
        }

        void MakeDecision(bool correctMarker, Tilemap tilemap, TileBase newTile)
        {
            if (!correctMarker)
            {
                if (newTile != null)
                {
                    tilemap.SetTile(
                        SimulationMath.CropVector3ToVector3Int(
                            SimulationMath.ConvertNormalCoordsToUnity(
                                coords
                                )
                            ),
                        newTile
                        );
                }
                else if (tilemap.GetTile(SimulationMath.CropVector3ToVector3Int(SimulationMath.ConvertNormalCoordsToUnity(coords))) != null)
                {
                    tilemap.SetTile(SimulationMath.CropVector3ToVector3Int(SimulationMath.ConvertNormalCoordsToUnity(coords)), null);
                }
            }
        }

        foreach (Entity i in Map.GetCell(coords).GetEntitiesAtCell())
        {
            switch (i)
            {
                case Building building:
                    if (CheckEntity(buildingsTilemap, building, ref buildingTileIsCorrect) != null)
                    {
                        newBuildingTile = CheckEntity(buildingsTilemap, building, ref buildingTileIsCorrect);
                    }
                    break;
                case Unit unit:
                    if (CheckEntity(unitsTilemap, unit, ref unitTileIsCorrect) != null)
                    {
                        newUnitTile = CheckEntity(unitsTilemap, unit, ref unitTileIsCorrect);
                    }
                    break;
            }
        }

        MakeDecision(unitTileIsCorrect, unitsTilemap, newUnitTile);
        MakeDecision(buildingTileIsCorrect, buildingsTilemap, newBuildingTile);
    }

    public static bool Build(CreateIntention information)
    {
        CellOccupiedStatus currentCellStatus = Map.GetCellOccupiyStatus(information.targetCellCords);

        bool? newUndergroundStatus = null;
        bool? newGroundStatus = null;
        Entity newEntity = null;

        void SetNewEntity()
        {
            switch (information.objectToCreate)
            {
                case EntityTypeEnum.YoungTree:
                    newEntity = new YoungTree
                    (
                        Map.GetCell(information.targetCellCords),
                        information.parentFraction,
                        EntityTypeEnum.YoungTree,
                        information.informationForCreated
                    );
                    break;
                case EntityTypeEnum.Sheep:
                    newEntity = new Sheep
                    (
                        Map.GetCell(information.targetCellCords),
                        information.parentFraction,
                        EntityTypeEnum.Sheep,
                        information.informationForCreated
                    );
                    break;
                case EntityTypeEnum.AdultTree:
                    newEntity = new AdultTree
                    (
                        Map.GetCell(information.targetCellCords),
                        information.parentFraction,
                        EntityTypeEnum.AdultTree,
                        information.informationForCreated
                    );
                    break;
                case EntityTypeEnum.SeedTree:
                    newEntity = new SeedTree
                    (
                        Map.GetCell(information.targetCellCords),
                        information.parentFraction,
                        EntityTypeEnum.SeedTree,
                        information.informationForCreated
                    );
                    break;
                case EntityTypeEnum.YoungDeadTree:
                    newEntity = new YoungDeadTree
                    (
                        Map.GetCell(information.targetCellCords),
                        information.parentFraction,
                        EntityTypeEnum.YoungDeadTree,
                        information.informationForCreated
                    );
                    break;
                case EntityTypeEnum.AdultDeadTree:
                    newEntity = new AdultDeadTree
                    (
                        Map.GetCell(information.targetCellCords),
                        information.parentFraction,
                        EntityTypeEnum.AdultDeadTree,
                        information.informationForCreated
                    );
                    break;
                case EntityTypeEnum.Mountain:
                    newEntity = new Mountain
                    (
                        Map.GetCell(information.targetCellCords),
                        information.parentFraction,
                        EntityTypeEnum.Mountain
                    );
                    break;
            }
        }

        void SetNewStatuses()
        {
            switch (information.objectToCreate)
            {
                case EntityTypeEnum.SeedTree:
                    newGroundStatus = true;
                    break;
                case EntityTypeEnum.YoungTree:
                    newGroundStatus = true;
                    break;
                case EntityTypeEnum.YoungDeadTree:
                    newGroundStatus = true;
                    break;
                case EntityTypeEnum.AdultTree:
                    newGroundStatus = true;
                    break;
                case EntityTypeEnum.AdultDeadTree:
                    newGroundStatus = true;
                    break;
                case EntityTypeEnum.Mountain:
                    newGroundStatus = true;
                    break;
                default:
                    break;
            }
        }


        SetNewStatuses();

        if (information.executor == null)
        {
            SetNewEntity();

            information.parentFraction.AddEntity(newEntity);
            Map.GetCell(information.targetCellCords).AddEntityToCell(newEntity);
            Map.SetNewOccupieStatus(information.targetCellCords, newUndergroundStatus, newGroundStatus);
            RepaintCell(information.targetCellCords);
            return true;
        }
        else
        {

            if (information.executor.currentCell == Map.GetCell(information.targetCellCords) && WorldStatusInformation.IsItReplacebleObject(information.executor.selfEntityType, information.objectToCreate))
            {
                information.executor.currentFraction.DestroyEntity(information.executor);

                SetNewEntity();

                information.parentFraction.AddEntity(newEntity);
                Map.GetCell(information.targetCellCords).AddEntityToCell(newEntity);

                buildingsTilemap.SetTile
                    (
                    SimulationMath.CropVector3ToVector3Int
                    (
                        SimulationMath.ConvertNormalCoordsToUnity(information.targetCellCords)
                        ),
                    entityTilesDict[information.objectToCreate]
                        );
                return true;
            }
            else
            {

                if (newGroundStatus == null && newUndergroundStatus == null)
                {
                    SetNewEntity();
                    information.parentFraction.AddEntity(newEntity);
                    Map.GetCell(information.targetCellCords).AddEntityToCell(newEntity);

                    RepaintCell(information.targetCellCords);
                    return true;
                }
                else
                {
                    if ((currentCellStatus.ground == true && newGroundStatus == true) || (currentCellStatus.underground == true && newUndergroundStatus == true))
                    {
                        return false;
                    }
                    else
                    {
                        SetNewStatuses();
                        Map.SetNewOccupieStatus(information.targetCellCords, newUndergroundStatus, newGroundStatus);

                        SetNewEntity();
                        information.parentFraction.AddEntity(newEntity);
                        Map.GetCell(information.targetCellCords).AddEntityToCell(newEntity);

                        RepaintCell(information.targetCellCords);

                        return true;
                    }
                }
            }
        }
    }


}
