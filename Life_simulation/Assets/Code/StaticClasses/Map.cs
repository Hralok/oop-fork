using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Map
{
    private static List<List<Cell>> map;
    private static MainWorldController main_controller;

    private static Tilemap groundTilemap;
    private static Tilemap buildingTilemap;
    private static Tilemap markerTilemap;

    private static int earthTileWeight;
    private static int grassTileWeight;
    private static int waterTileWeight;
    private static int mountainTileWeight;

    private static int minimumPercentageOfTheTerritoryOccupiedByAdultTrees;
    private static int minimumPercentageOfTheTerritoryOccupiedByYoungTrees;
    private static int minimumPercentageOfTheTerritoryOccupiedSeedTrees;

    private static Dictionary<MarkerColors, TileBase> markerTilesDict;
    private static Dictionary<GroundTypesEnum, TileBase> groundTilesDict;
    private static Dictionary<TileBase, GroundTypesEnum> groundTypesDict;

    private static HashSet<Cell> markedCells;

    private static bool generateTrees;

    public enum MarkerColors
    {
        Yellow,
        Purple,
        Green,
        Red,
        Blue
    }

    public static void MarkCells(MarkerColors color, List<Cell> cells)
    {
        foreach (var i in cells)
        {
            markerTilemap.SetTile(SimulationMath.CropVector3ToVector3Int(SimulationMath.ConvertNormalCoordsToUnity(i.coords)), markerTilesDict[color]);
            markedCells.Add(i);
        }
    }

    public static void CleanSingleMarker(Vector3Int coords)
    {
        markerTilemap.SetTile(SimulationMath.CropVector3ToVector3Int(SimulationMath.ConvertNormalCoordsToUnity(coords)), null);
    }

    public static void CleanAllMarkers()
    {
        foreach (var i in markedCells)
        {
            markerTilemap.SetTile(SimulationMath.CropVector3ToVector3Int(SimulationMath.ConvertNormalCoordsToUnity(i.coords)), null);
        }
        markedCells.Clear();
    }

    private class ListCellComparer : IComparer<List<Cell>>
    {
        public int Compare(List<Cell> x, List<Cell> y)
        {
            if (x[0].coords.x == y[0].coords.x)
            {
                return 0;
            }
            else if (x[0].coords.x > y[0].coords.x)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }

    public static Cell GetCell(Vector3Int coords)
    {
        return map[main_controller.GetMapRadius() - 1 + coords.x][Mathf.Abs(map[main_controller.GetMapRadius() - 1 + coords.x][0].coords.y) + coords.y];
    }

    public static CellOccupiedStatus GetCellOccupiyStatus(Vector3Int coords)
    {
        int x = main_controller.GetMapRadius() - 1 + coords.x;
        int y;

        if (coords.x < 0)
        {
            y = -(main_controller.GetMapRadius() - 1) - coords.x;
        }
        else
        {
            y = -(main_controller.GetMapRadius() - 1);
        }


        return map[x][coords.y - y].occupiedStatus;
    }

    public static GroundTypesEnum? GetGroundType(Vector3Int coords)
    {
        var i = groundTilemap.GetTile(SimulationMath.CropVector3ToVector3Int(SimulationMath.ConvertNormalCoordsToUnity(coords)));
        if (i != null)
        {
            return groundTypesDict[i];
        }
        else
        {
            return null;
        }
    }

    public static void SetGroundTile(Vector3Int coords, GroundTypesEnum newTileType)
    {
        groundTilemap.SetTile(SimulationMath.CropVector3ToVector3Int(SimulationMath.ConvertNormalCoordsToUnity(coords)), groundTilesDict[newTileType]);
    }

    public static void SetNewOccupieStatus(Vector3Int cellCoords, bool? underground, bool? ground)
    {
        int x = main_controller.GetMapRadius() - 1 + cellCoords.x;
        int y;

        if (cellCoords.x < 0)
        {
            y = -(main_controller.GetMapRadius() - 1) - cellCoords.x;
        }
        else
        {
            y = -(main_controller.GetMapRadius() - 1);
        }

        CellOccupiedStatus newStatus;


        if (underground != null)
        {
            newStatus.underground = (bool)underground;
        }
        else
        {
            newStatus.underground = map[x][cellCoords.y - y].occupiedStatus.underground;
        }

        if (ground != null)
        {
            newStatus.ground = (bool)ground;
        }
        else
        {
            newStatus.ground = map[x][cellCoords.y - y].occupiedStatus.ground;
        }

        map[x][cellCoords.y - y].SetNewOccupiedStatus(newStatus.ground, newStatus.underground);
    }

    public static void InitializeMap(
        int earthTileWeight,
        int grassTileWeight,
        int waterTileWeight,
        int mountainTileWeight,
        int smallMountainTileWeight,
        int minimumPercentageOfTheTerritoryOccupiedByAdultTrees,
        int minimumPercentageOfTheTerritoryOccupiedByYoungTrees,
        int minimumPercentageOfTheTerritoryOccupiedSeedTrees,
        bool generateTrees
        )
    {
        if (map == null)
        {
            Map.generateTrees = generateTrees;

            groundTilemap = GameObject.Find("Grid").transform.Find("Ground Tilemap").GetComponent<Tilemap>();
            buildingTilemap = GameObject.Find("Grid").transform.Find("Mountain Tilemap").GetComponent<Tilemap>();
            markerTilemap = GameObject.Find("Grid").transform.Find("Marker Tilemap").GetComponent<Tilemap>();

            groundTilesDict = new Dictionary<GroundTypesEnum, TileBase>();
            groundTypesDict = new Dictionary<TileBase, GroundTypesEnum>();
            markerTilesDict = new Dictionary<MarkerColors, TileBase>();

            groundTilesDict.Add(GroundTypesEnum.Sand, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_24"));
            groundTilesDict.Add(GroundTypesEnum.Water, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_6"));
            groundTilesDict.Add(GroundTypesEnum.DeadEarth, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_14"));

            groundTypesDict.Add(groundTilesDict[GroundTypesEnum.Sand], GroundTypesEnum.Sand);
            groundTypesDict.Add(groundTilesDict[GroundTypesEnum.Water], GroundTypesEnum.Water);
            groundTypesDict.Add(groundTilesDict[GroundTypesEnum.DeadEarth], GroundTypesEnum.DeadEarth);

            markerTilesDict.Add(MarkerColors.Yellow, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_borderless Ч копи€_2"));
            markerTilesDict.Add(MarkerColors.Blue, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_borderless Ч копи€_4"));
            markerTilesDict.Add(MarkerColors.Green, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_borderless Ч копи€_3"));
            markerTilesDict.Add(MarkerColors.Red, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_borderless Ч копи€_1"));
            markerTilesDict.Add(MarkerColors.Purple, Resources.Load<TileBase>("Tiles/fantasyhextiles_v3_borderless Ч копи€_0"));

            Map.earthTileWeight = earthTileWeight;
            Map.grassTileWeight = grassTileWeight;
            Map.waterTileWeight = waterTileWeight;
            Map.mountainTileWeight = mountainTileWeight;

            main_controller = GameObject.Find("God").GetComponent<MainWorldController>();

            Map.minimumPercentageOfTheTerritoryOccupiedByAdultTrees = minimumPercentageOfTheTerritoryOccupiedByAdultTrees;
            Map.minimumPercentageOfTheTerritoryOccupiedByYoungTrees = minimumPercentageOfTheTerritoryOccupiedByYoungTrees;
            Map.minimumPercentageOfTheTerritoryOccupiedSeedTrees = minimumPercentageOfTheTerritoryOccupiedSeedTrees;

            map = new List<List<Cell>>();
            markedCells = new HashSet<Cell>();

            for (int i = -(main_controller.GetMapRadius() - 1); i < main_controller.GetMapRadius(); i++)
            {
                map.Add(new List<Cell>());

                if (i < 0)
                {
                    for (int j = main_controller.GetMapRadius() - 1; j >= -(main_controller.GetMapRadius() - 1) - i; j--)
                    {
                        map[map.Count - 1].Add(new Cell(i, j, -i - j));
                    }
                }
                else
                {
                    for (int j = main_controller.GetMapRadius() - 1 - i; j >= -(main_controller.GetMapRadius() - 1); j--)
                    {
                        map[map.Count - 1].Add(new Cell(i, j, -i - j));
                    }
                }
            }

            map.Sort(new ListCellComparer());

            foreach (var i in map)
            {
                i.Sort();
            }

            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    int randomCount = Random.Range(main_controller.GetMinimalMineralCount(), main_controller.GetMaximalMineralCount());
                    map[i][j].AddResource(new Resource(ResourceTypeEnum.Mineral, randomCount));
                }
            }

            GenerateVisibleMap();
        }
    }

    private static void GenerateTile(int x, int y)
    {
        float perlinNoise;

        perlinNoise = Mathf.PerlinNoise((x + main_controller.GetWorldSeedX()) / main_controller.GetWorldZoom(), (y + main_controller.GetWorldSeedY()) / main_controller.GetWorldZoom());

        TileBase currentTile = null;

        float totalWeight = waterTileWeight + earthTileWeight + grassTileWeight + mountainTileWeight;


        if (perlinNoise >= 1 - mountainTileWeight / totalWeight)
        {
            currentTile = groundTilesDict[GroundTypesEnum.Sand];
            Builder.Build(new CreateIntention(null, EntityTypeEnum.Mountain, SimulationMath.ConvertUnityCoordsToNormal(new Vector3(x, y, 0)), null, main_controller.GetNeutralFraction()));
        }
        else if (perlinNoise >= 1 - (mountainTileWeight + grassTileWeight) / totalWeight)
        {
            currentTile = groundTilesDict[GroundTypesEnum.DeadEarth];

            if (generateTrees)
            {
                if (perlinNoise >= 1 - (mountainTileWeight + grassTileWeight) / totalWeight + (grassTileWeight / totalWeight) * (minimumPercentageOfTheTerritoryOccupiedByAdultTrees / 100f))
                {
                    WorldStatusInformation.treeCounter += 1;
                    Builder.Build(new CreateIntention(null, EntityTypeEnum.AdultTree, SimulationMath.ConvertUnityCoordsToNormal(new Vector3(x, y, 0)), null, main_controller.GetNeutralFraction()));
                }
                else if (perlinNoise >= 1 - (mountainTileWeight + grassTileWeight) / totalWeight + (grassTileWeight / totalWeight) * (minimumPercentageOfTheTerritoryOccupiedByYoungTrees / 100f))
                {
                    WorldStatusInformation.treeCounter += 1;
                    Builder.Build(new CreateIntention(null, EntityTypeEnum.YoungTree, SimulationMath.ConvertUnityCoordsToNormal(new Vector3(x, y, 0)), null, main_controller.GetNeutralFraction()));
                }
                else if (perlinNoise >= 1 - (mountainTileWeight + grassTileWeight) / totalWeight + (grassTileWeight / totalWeight) * (minimumPercentageOfTheTerritoryOccupiedSeedTrees / 100f))
                {
                    WorldStatusInformation.treeCounter += 1;
                    Builder.Build(new CreateIntention(null, EntityTypeEnum.SeedTree, SimulationMath.ConvertUnityCoordsToNormal(new Vector3(x, y, 0)), null, main_controller.GetNeutralFraction()));
                }
            }

        }
        else if (perlinNoise >= 1 - (mountainTileWeight + grassTileWeight + earthTileWeight) / totalWeight)
        {
            currentTile = groundTilesDict[GroundTypesEnum.Sand];
        }
        else
        {
            currentTile = groundTilesDict[GroundTypesEnum.Water];
        }


        groundTilemap.SetTile(new Vector3Int(x, y, 0), currentTile);

    }

    public static void GenerateVisibleMap()
    {
        int x = 0;
        int y = 0;
        int radius = 1;

        groundTilemap.ClearAllTiles();
        buildingTilemap.ClearAllTiles();


        while (radius < main_controller.GetMapRadius())
        {
            radius += 1;
            x = radius;
            y = 0;

            GenerateTile(0, 0);

            for (int i = 1; i <= radius - 1; i++)
            {
                GenerateTile(x - 1, y);
                y += 1;
                if (y % 2 == 1)
                {
                    x -= 1;
                }
            }
            for (int i = 1; i <= radius - 1; i++)
            {
                GenerateTile(x - 1, y);
                x -= 1;
            }
            for (int i = 1; i <= radius - 1; i++)
            {
                GenerateTile(x - 1, y);
                y -= 1;
                if (y % 2 == 1)
                {
                    x -= 1;
                }
            }
            for (int i = 1; i <= radius - 1; i++)
            {
                GenerateTile(x - 1, y);
                y -= 1;
                if (y % 2 == 0)
                {
                    x += 1;
                }
            }
            for (int i = 1; i <= radius - 1; i++)
            {
                GenerateTile(x - 1, y);
                x += 1;
            }
            for (int i = 1; i <= radius - 1; i++)
            {
                GenerateTile(x - 1, y);
                y += 1;
                if (y % 2 == 0)
                {
                    x += 1;
                }
            }
        }
    }

    public static HashSet<Entity> FindEntity(Vector3Int centerCoords, int radius, EntityTypeEnum? entityType)
    {
        HashSet<Entity> findedEntitys = new HashSet<Entity>();
        Vector3Int currentCoords = new Vector3Int(centerCoords.x, centerCoords.y - 1, centerCoords.z + 1);

        void CheckCell()
        {
            if (SimulationMath.CheckCoordsToInclusionIntoMap(currentCoords))
            {
                foreach (Entity entity in GetCell(currentCoords).GetEntitiesAtCell())
                {
                    if (entityType == null)
                    {
                        findedEntitys.Add(entity);
                    }
                    else if (entity.selfEntityType == entityType)
                    {

                        findedEntitys.Add(entity);
                    }
                }
            }
        }

        for (int i = 1; i <= radius; i++)
        {
            currentCoords.y += 1;
            currentCoords.z -= 1;

            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.x += 1;
                currentCoords.y -= 1;
            }
            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.z += 1;
                currentCoords.y -= 1;
            }
            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.z += 1;
                currentCoords.x -= 1;
            }
            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.y += 1;
                currentCoords.x -= 1;
            }
            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.y += 1;
                currentCoords.z -= 1;
            }
            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.x += 1;
                currentCoords.z -= 1;
            }
        }

        return findedEntitys;
    }

    public static HashSet<Entity> FindEntityWithResource(Vector3Int centerCoords, int radius, HashSet<ResourceTypeEnum?> requiredResourceType, EntityTypeEnum? withoutEntitys = null)
    {
        HashSet<Entity> findedEntitys = new HashSet<Entity>();
        Vector3Int currentCoords = new Vector3Int(centerCoords.x, centerCoords.y - 1, centerCoords.z + 1);

        void CheckCell()
        {
            if (SimulationMath.CheckCoordsToInclusionIntoMap(currentCoords))
            {
                foreach (Entity entity in GetCell(currentCoords).GetEntitiesAtCell())
                {
                    if (requiredResourceType.Contains(entity.resourceFromAttack))
                    {
                        findedEntitys.Add(entity);
                    }
                    else if ((entity is ISearchingSource))
                    {
                        foreach (DropInfo info in ((ISearchingSource)entity).GetDropInfo())
                        {
                            if (requiredResourceType.Contains(info.resourceType))
                            {
                                findedEntitys.Add(entity);
                            }
                        }
                    }
                }
            }
        }

        for (int i = 1; i <= radius; i++)
        {
            currentCoords.y += 1;
            currentCoords.z -= 1;

            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.x += 1;
                currentCoords.y -= 1;
            }
            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.z += 1;
                currentCoords.y -= 1;
            }
            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.z += 1;
                currentCoords.x -= 1;
            }
            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.y += 1;
                currentCoords.x -= 1;
            }
            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.y += 1;
                currentCoords.z -= 1;
            }
            for (int j = 0; j < i - 1; j++)
            {
                CheckCell();
                currentCoords.x += 1;
                currentCoords.z -= 1;
            }
        }


        return findedEntitys;
    }





}
