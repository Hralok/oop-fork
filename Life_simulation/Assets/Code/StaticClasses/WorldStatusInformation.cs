using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldStatusInformation
{
    private static HashSet<Entity> alreadyDestroyed = new HashSet<Entity>();
    private static int currentTurn = 0;
    private static int pathFindingRadius = 40;

    public static int treeCounter = 0;
    public static int sheepCounter = 0;

    public static int GetPathFindingRadius()
    {
        return pathFindingRadius;
    }

    public static int GetTurn()
    {
        return currentTurn;
    }

    public static void IncreaseTurnCounter()
    {
        currentTurn += 1;
    }

    public static void AddDestroyedEntity(Entity victim)
    {
        alreadyDestroyed.Add(victim);
    }

    public static bool CheckForBeingDestroyed(Entity suspect)
    {
        return alreadyDestroyed.Contains(suspect);
    }

    public static void CleanDeadList()
    {
        alreadyDestroyed.Clear();
    }

    public static bool IsItReplacebleObject(EntityTypeEnum entityToReplace, EntityTypeEnum replacementEntity)
    {
        if (entityToReplace == EntityTypeEnum.SeedTree && replacementEntity == EntityTypeEnum.YoungTree ||
            entityToReplace == EntityTypeEnum.YoungTree && replacementEntity == EntityTypeEnum.AdultTree ||
            entityToReplace == EntityTypeEnum.YoungTree && replacementEntity == EntityTypeEnum.YoungDeadTree ||
            entityToReplace == EntityTypeEnum.AdultTree && replacementEntity == EntityTypeEnum.AdultDeadTree
            )
        {
            return true;
        }
        else
        {
            return false;
        }

    }



}
