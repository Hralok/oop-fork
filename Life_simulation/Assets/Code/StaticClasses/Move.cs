using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Move
{
    public static List<(List<Cell> way, MovementDevice device)> FindTheWay(MovementDevice device, Vector3Int startPoint, Vector3Int finishPoint)
    {
        var answer = new List<(List<Cell> way, MovementDevice device)>();
        switch (device.routeType)
        {
            case RouteTypeEnum.ShortestPath:
                answer.Add((SimpleAStar(startPoint, finishPoint, device.force, device.movementField), device));
                return answer;
            case RouteTypeEnum.Teleport:
                answer.Add((Teleport(startPoint, finishPoint), device));
                return answer;
            default:
                return null;
        }
    }

    public static List<Intention> PlanTheWay(List<(List<Cell> way, MovementDevice device)> targetWay, Unit owner)
    {
        List<Intention> gratePlan = new List<Intention>();

        int currentMovementPoints;

        while (targetWay.Count != 0)
        {
            currentMovementPoints = owner.movementPointsForTurn;

            while (currentMovementPoints > 0)
            {
                int cellIndex = 0;

                currentMovementPoints -= targetWay[0].way[cellIndex].
                    GetObstruction(targetWay[0].device.movementField, MovingObstructionFromBuilding.MovementType.Enter);

                while (currentMovementPoints > 0 && cellIndex < targetWay[0].way.Count - 1)
                {
                    cellIndex += 1;
                    currentMovementPoints -= 1 +
                        targetWay[0].way[cellIndex - 1].GetObstruction(targetWay[0].device.movementField, MovingObstructionFromBuilding.MovementType.Leave) +
                        targetWay[0].way[cellIndex].GetObstruction(targetWay[0].device.movementField, MovingObstructionFromBuilding.MovementType.Enter);
                }

                if (cellIndex < targetWay[0].way.Count - 1)
                {
                    gratePlan.Add(new MoveIntention(owner, targetWay[0].way[cellIndex]));

                    for (int j = 0; j <= cellIndex; j++)
                    {
                        targetWay[0].way.RemoveAt(0);
                    }
                }
                else
                {
                    if (currentMovementPoints <= 0 || targetWay.Count == 1)
                    {
                        gratePlan.Add(new MoveIntention(owner, targetWay[0].way[cellIndex]));

                        if (targetWay.Count == 1)
                        {
                            targetWay.RemoveAt(0);
                        }
                        break;
                    }
                    else
                    {
                        targetWay.RemoveAt(0);
                    }
                }
            }
        }

        return gratePlan;
    }

    public static List<Intention> PlanTheWayForPair(List<(List<Cell> way, MovementDevice device)> targetWay, Unit firstUnit, Unit secondUnit)
    {
        List<Intention> firstGratePlan = new List<Intention>();
        List<Intention> secondGratePlan = new List<Intention>();

        Cell currentFCell = targetWay[0].way[0];
        Cell currentSCell = targetWay[targetWay.Count - 1].way[targetWay[targetWay.Count - 1].way.Count - 1];




        int fcurrentMovementPoints;
        int scurrentMovementPoints;


        while (currentFCell != currentSCell)
        {
            fcurrentMovementPoints = firstUnit.movementPointsForTurn;
            scurrentMovementPoints = secondUnit.movementPointsForTurn;
























            while (currentMovementPoints > 0)
            {
                int cellIndex = 0;

                currentMovementPoints -= targetWay[0].way[cellIndex].
                    GetObstruction(targetWay[0].device.movementField, MovingObstructionFromBuilding.MovementType.Enter);

                while (currentMovementPoints > 0 && cellIndex < targetWay[0].way.Count - 1)
                {
                    cellIndex += 1;
                    currentMovementPoints -= 1 +
                        targetWay[0].way[cellIndex - 1].GetObstruction(targetWay[0].device.movementField, MovingObstructionFromBuilding.MovementType.Leave) +
                        targetWay[0].way[cellIndex].GetObstruction(targetWay[0].device.movementField, MovingObstructionFromBuilding.MovementType.Enter);
                }

                if (cellIndex < targetWay[0].way.Count - 1)
                {
                    gratePlan.Add(new MoveIntention(owner, targetWay[0].way[cellIndex]));

                    for (int j = 0; j <= cellIndex; j++)
                    {
                        targetWay[0].way.RemoveAt(0);
                    }
                }
                else
                {
                    if (currentMovementPoints <= 0 || targetWay.Count == 1)
                    {
                        gratePlan.Add(new MoveIntention(owner, targetWay[0].way[cellIndex]));

                        if (targetWay.Count == 1)
                        {
                            targetWay.RemoveAt(0);
                        }
                        break;
                    }
                    else
                    {
                        targetWay.RemoveAt(0);
                    }
                }
            }
        }

        return gratePlan;
    }



    private static List<Cell> Teleport(Vector3Int startPoint, Vector3Int finishPoint)
    {
        List<Cell> way = new List<Cell>();
        way.Add(Map.GetCell(startPoint));
        way.Add(Map.GetCell(finishPoint));
        return way;
    }


    private class CellWithWeight
    {
        private Cell cell;

        private CellWithWeight previous;


        private int finalWeight;

        private int movedWeight;

        private int distanceToTarget;

        public int GetFinakWeight()
        {
            return finalWeight;
        }

        public int GetMovedWeight()
        {
            return movedWeight;
        }

        public int GetDistanceToTarget()
        {
            return distanceToTarget;
        }

        public CellWithWeight GetPreviousCellWithWeight()
        {
            return previous;
        }

        public Cell GetCell()
        {
            return cell;
        }

        public CellWithWeight(Cell cell, CellWithWeight previous, int movedWeight, Vector3Int finishPoint)
        {
            this.cell = cell;
            this.previous = previous;
            this.movedWeight = movedWeight;
            distanceToTarget = SimulationMath.FindDistance(cell.coords, finishPoint);
            finalWeight = distanceToTarget + movedWeight;
        }

        public void ReSetValues(CellWithWeight previous, int movedWeight)
        {
            this.previous = previous;
            this.movedWeight = movedWeight;
            finalWeight = distanceToTarget + movedWeight;
        }

    }
    /*
    private static List<CellWithWeight> FindNeighboring(Vector3Int startPoint, Vector3Int currentPoint, int streight)
    {
        Vector3Int suspectCoords = new Vector3Int(currentPoint.x, currentPoint.y, currentPoint.z);

        suspectCoords.y += streight;
        suspectCoords.z -= streight;

        if (
            !fastClosedList.c &&
            SimulationMath.FindDistance(startPoint, suspectCoords)<= WorldStatusInformation.GetPathFindingRadius() && 
            SimulationMath.CheckCoordsToInclusionIntoMap(suspectCoords)
            )
        {
            
        }
        return null;
    }*/

    private static List<Cell> SimpleAStar(Vector3Int startPoint, Vector3Int finishPoint, int streight, MovementFieldEnum movementField)
    {
        HashSet<Cell> fastClosedList = new HashSet<Cell>();
        HashSet<Cell> fastOpenList = new HashSet<Cell>();

        List<CellWithWeight> openList = new List<CellWithWeight>();
        openList.Add(new CellWithWeight(Map.GetCell(startPoint), null, 0, finishPoint));

        List<Cell> way = null;

        CellWithWeight currentCWW;

        while (openList.Count != 0)
        {
            currentCWW = openList[0];
            foreach (var i in openList)
            {
                if (i.GetFinakWeight() < currentCWW.GetFinakWeight())
                {
                    currentCWW = i;
                }
            }

            Vector3Int suspectCoords = new Vector3Int(currentCWW.GetCell().coords.x, currentCWW.GetCell().coords.y, currentCWW.GetCell().coords.z);
            suspectCoords.y += streight;
            suspectCoords.z -= streight;

            for (int xModifier = -1; xModifier <= 1; xModifier++)
            {
                for (int yModifier = -1; yModifier <= 1; yModifier++)
                {
                    for (int zModifier = -1; zModifier <= 1; zModifier++)
                    {
                        if (!(xModifier == 0 & yModifier == 0 & zModifier == 0) & (xModifier + zModifier + yModifier == 0))
                        {
                            suspectCoords = new Vector3Int(currentCWW.GetCell().coords.x, currentCWW.GetCell().coords.y, currentCWW.GetCell().coords.z);

                            suspectCoords.x += xModifier * streight;
                            suspectCoords.y += yModifier * streight;
                            suspectCoords.z += zModifier * streight;

                            if (
                                SimulationMath.CheckCoordsToInclusionIntoMap(suspectCoords) &&
                                !fastClosedList.Contains(Map.GetCell(suspectCoords)) &&
                                SimulationMath.FindDistance(startPoint, suspectCoords) <= WorldStatusInformation.GetPathFindingRadius()
                                )
                            {
                                if (suspectCoords == finishPoint)
                                {
                                    way = new List<Cell>();
                                    way.Insert(0, Map.GetCell(suspectCoords));

                                    while (currentCWW.GetPreviousCellWithWeight() != null)
                                    {
                                        way.Insert(0, currentCWW.GetCell());
                                        currentCWW = currentCWW.GetPreviousCellWithWeight();
                                    }
                                    return way;
                                }


                                bool canReach = true;

                                for (int i = 1; i < streight; i++)
                                {
                                    Cell currentCell = Map.GetCell(new Vector3Int(currentCWW.GetCell().coords.x + i * xModifier, currentCWW.GetCell().coords.y + i * yModifier, currentCWW.GetCell().coords.z + i * zModifier));

                                    if (currentCell.GetObstruction(movementField, MovingObstructionFromBuilding.MovementType.Enter) == -1 || currentCell.GetObstruction(movementField, MovingObstructionFromBuilding.MovementType.Leave) == -1)
                                    {
                                        canReach = false;
                                    }
                                }

                                if (canReach)
                                {
                                    if (fastOpenList.Contains(Map.GetCell(suspectCoords)))
                                    {
                                        CellWithWeight alreadyIn = openList[0];

                                        foreach (CellWithWeight i in openList)
                                        {
                                            if (i.GetCell().coords == suspectCoords)
                                            {
                                                alreadyIn = i;
                                                break;
                                            }
                                        }

                                        if (currentCWW.GetMovedWeight() +
                                            Map.GetCell(suspectCoords).GetObstruction(movementField, MovingObstructionFromBuilding.MovementType.Enter) +
                                            currentCWW.GetCell().GetObstruction(movementField, MovingObstructionFromBuilding.MovementType.Leave) < alreadyIn.GetMovedWeight())
                                        {
                                            alreadyIn.ReSetValues(currentCWW,
                                                currentCWW.GetMovedWeight() +
                                                Map.GetCell(suspectCoords).GetObstruction(movementField, MovingObstructionFromBuilding.MovementType.Enter) +
                                                currentCWW.GetCell().GetObstruction(movementField, MovingObstructionFromBuilding.MovementType.Leave));
                                        }
                                    }
                                    else
                                    {
                                        CellWithWeight newCWW = new CellWithWeight(
                                            Map.GetCell(suspectCoords),
                                            currentCWW,
                                            currentCWW.GetMovedWeight() +
                                            Map.GetCell(suspectCoords).GetObstruction(movementField, MovingObstructionFromBuilding.MovementType.Enter) +
                                            currentCWW.GetCell().GetObstruction(movementField, MovingObstructionFromBuilding.MovementType.Leave),
                                            finishPoint
                                            );

                                        fastOpenList.Add(Map.GetCell(suspectCoords));
                                        openList.Add(newCWW);
                                    }
                                }
                            }
                        }
                    }
                }
            }



            openList.Remove(currentCWW);
            fastClosedList.Add(currentCWW.GetCell());
        }



        return way;
    }




}
