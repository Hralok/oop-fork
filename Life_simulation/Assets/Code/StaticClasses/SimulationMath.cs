using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class SimulationMath
{
    private static Tilemap tileMap;
    private static MainWorldController mvc;

    public static Vector3 ConvertWorldCoordsToUnity(Vector3 coords)
    {
        return tileMap.WorldToCell(coords);
    }

    public static Vector3Int ConvertWorldCoordsToNormal(Vector3 coords)
    {
        return ConvertUnityCoordsToNormal(tileMap.WorldToCell(coords));
    }

    public static void InitializeSimulationMath(Tilemap tilemap, MainWorldController MVC)
    {
        if (tileMap == null)
        {
            tileMap = tilemap;
            mvc = MVC;
        }
    }

    public static Entity FindTheNearest(HashSet<Entity> entityList, Vector3Int startPoint)
    {
        Entity nearestEntity = null;

        foreach (Entity i in entityList)
        {
            if (nearestEntity == null || FindDistance(startPoint, nearestEntity.currentCell.coords) > FindDistance(startPoint, i.currentCell.coords))
            {
                nearestEntity = i;
            }
        }

        return nearestEntity;
    }

    public static bool CheckCoordsToInclusionIntoMap(Vector3Int coords)
    {
        bool inMap = true;

        if (Mathf.Abs(coords.x) > mvc.GetMapRadius() - 1)
        {
            inMap = false;
        }
        else if (Mathf.Abs(coords.y) > mvc.GetMapRadius() - 1)
        {
            inMap = false;
        }
        else if (Mathf.Abs(coords.z) > mvc.GetMapRadius() - 1)
        {
            inMap = false;
        }

        return inMap;
    }

    public static Vector3Int CropVector3ToVector3Int(Vector3 victim)
    {
        return new Vector3Int((int)victim.x, (int)victim.y, (int)victim.z);
    }

    public static Vector3Int ConvertUnityCoordsToNormal(Vector3 coordsToConvert)
    {
        int some_coef = 1;

        if (((int)coordsToConvert.y & 1) == 1)
        {
            some_coef = -1;
        }

        Vector3Int convertedCoords = new Vector3Int();
        convertedCoords.x = (int)coordsToConvert.y;
        convertedCoords.z = ((int)coordsToConvert.x - ((int)coordsToConvert.y + some_coef * ((int)coordsToConvert.y & 1)) / 2);
        convertedCoords.y = -convertedCoords.x - convertedCoords.z;
        return convertedCoords;
    }

    public static Vector3 ConvertNormalCoordsToUnity(Vector3Int coordsToConvert)
    {
        int some_coef = 1;

        if ((coordsToConvert.x & 1) == 1)
        {
            some_coef = -1;
        }

        Vector3 convertedCoords = new Vector3();
        convertedCoords.x = (coordsToConvert.z + (coordsToConvert.x + some_coef * (coordsToConvert.x & 1)) / 2);
        convertedCoords.y = coordsToConvert.x;
        convertedCoords.z = 0;
        return convertedCoords;
    }

    public static Vector3Int CreateRandomCoords(int map_radius)
    {
        Vector3Int newCords = new Vector3Int();
        newCords.x = Random.Range(-(map_radius - 1), (map_radius));
        if (newCords.x > 0)
        {
            newCords.y = Random.Range(-(map_radius - 1), (map_radius) - newCords.x);
        }
        else
        {
            newCords.y = Random.Range(-(map_radius - 1) - newCords.x, (map_radius));
        }

        newCords.z = -newCords.y - newCords.x;

        return newCords;
    }

    public static Vector3Int CreateRandomCoords(Vector3Int basePosition, int radius)
    {
        Vector3Int newCords = new Vector3Int();
        newCords.x = Random.Range(-(radius - 1), (radius));
        if (newCords.x > 0)
        {
            newCords.y = Random.Range(-(radius - 1), (radius) - newCords.x);
        }
        else
        {
            newCords.y = Random.Range(-(radius - 1) - newCords.x, (radius));
        }

        newCords.z = -newCords.y - newCords.x;

        newCords.x += basePosition.x;
        newCords.y += basePosition.y;
        newCords.z += basePosition.z;

        return newCords;
    }

    public static int FindDistance(Vector3Int firstPoint, Vector3Int secondPoint)
    {
        return Mathf.Max(Mathf.Abs(firstPoint.x - secondPoint.x), Mathf.Abs(firstPoint.y - secondPoint.y), Mathf.Abs(firstPoint.z - secondPoint.z));
    }

    public static Vector3 ConvertNormalCoordsToWorld(Vector3Int coords)
    {
        Vector3Int someCoords = new Vector3Int((int)ConvertNormalCoordsToUnity(coords).x, (int)ConvertNormalCoordsToUnity(coords).y, (int)ConvertNormalCoordsToUnity(coords).z);
        return tileMap.CellToWorld(someCoords);
    }

}
