using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrimitiveAnimalBrain : Brain
{
    private Animal owner;
    private Vector3Int? randCoords = null;

    public bool randomMoving;

    public PrimitiveAnimalBrain(Animal owner)
    {
        this.owner = owner;
    }

    private List<Intention> whatToDo = new List<Intention>();


    public override List<Intention> Think()
    {
        List<Intention> intentionsList = new List<Intention>();
        bool alreadyMoved = false;

    Begining:

        if (whatToDo.Count > 0)
        {
            Intention currentIntention = whatToDo[0];

            switch (currentIntention)
            {
                case WaitIntention intention:
                    if (intention.turns > 1)
                    {
                        intention.ReduceTurnsCount();
                    }
                    else
                    {
                        intention.ReduceTurnsCount();
                        whatToDo.Remove(intention);
                    }
                    break;
                case MoveIntention intention:
                    if (alreadyMoved)
                    {
                        break;
                    }
                    else
                    {
                        alreadyMoved = true;
                        intentionsList.Add(intention);
                        whatToDo.Remove(intention);
                        goto Begining;
                    }
                default:
                    intentionsList.Add(currentIntention);
                    whatToDo.Remove(currentIntention);
                    break;
            }
        }
        else
        {
            Sup();
            goto Begining;
        }







        return intentionsList;
    }

    private Entity partner;

    public void CatchTheMatingCall(List<Intention> meetingPlan)
    {
        whatToDo.Clear();
        for (int i = 0; i < meetingPlan.Count; i++)
        {
            whatToDo.Add(meetingPlan[i]);
        }
    }

    public void Sup()
    {
        //List<Entity> interestingEntitys = null;
        //Entity nearestEntity = null;
        //int priority = 0;

        List<Intention> FindBestWay(Vector3Int target)
        {
            List<Intention> way = null;
            foreach (var i in owner.movementDevices)
            {
                List<Intention> currentWay = Move.PlanTheWay(Move.FindTheWay(i, owner.currentCell.coords, target), owner);

                if ((currentWay != null) && (way == null || currentWay.Count < way.Count))
                {
                    way = currentWay;
                }
            }
            return way;
        }

        (List<Intention>, Entity, bool) FindWayToNearestInteresting(HashSet<Entity> interestingEntitys)
        {
        //Entity targetEntity = null;
        //List<Intention> targetWay = null;

        Begining:
            var nearestEntity = SimulationMath.FindTheNearest(interestingEntitys, owner.currentCell.coords);
            interestingEntitys.Remove(nearestEntity);

            List<Intention> currentWay = FindBestWay(nearestEntity.currentCell.coords);

            if (currentWay == null && nearestEntity.currentCell != owner.currentCell)
            {
                if (interestingEntitys.Count != 0)
                {
                    goto Begining;
                }
                else
                {
                    return (null, null, false);
                }
            }
            else
            {
                return (currentWay, nearestEntity, true);
            }
        }

        if (owner.saturationSufficientQuantity > owner.currentSaturation)
        {
            var interestingEntitys = Map.FindEntityWithResource(owner.currentCell.coords, owner.vision, owner.fastFoodThatUnitCanEat);

            foreach (Entity i in interestingEntitys.ToList()) // Очищаем список возможных целей от неподходящих
            {
                if (!(this is IDamageDealer))
                {
                    var resourceIntersection = new HashSet<ResourceTypeEnum?>(((ISearchingSource)i).GetFastDropInfo());
                    resourceIntersection.IntersectWith(owner.fastFoodThatUnitCanEat);

                    if (!(i is ISearchingSource) || resourceIntersection.Count == 0)
                    {
                        interestingEntitys.Remove(i);
                    }
                }
                else if (!(this is ISearcher))
                {
                    if (!owner.fastFoodThatUnitCanEat.Contains(i.resourceFromAttack))
                    {
                        interestingEntitys.Remove(i);
                    }
                }
            }

            var wayInfo = FindWayToNearestInteresting(interestingEntitys);

            if (wayInfo.Item3)
            {
                if (wayInfo.Item1 != null)
                {
                    for (int i = 0; i < wayInfo.Item1.Count; i++)
                    {
                        whatToDo.Add(wayInfo.Item1[i]);
                    }
                }



                if (!(owner is ISearcher))
                {
                    owner = (Animal)(IDamageDealer)owner;
                    whatToDo.Add(new MakeDamageIntention(owner, wayInfo.Item2, 1));
                }
                else if (!(owner is IDamageDealer))
                {
                    owner = (Animal)(ISearcher)owner;
                    whatToDo.Add(new FindInIntention(owner, (ISearchingSource)wayInfo.Item2, 1));
                }
                else
                {
                    if (owner.fastFoodThatUnitCanEat.Contains(wayInfo.Item2.resourceFromAttack))
                    {
                        whatToDo.Add(new MakeDamageIntention(owner, wayInfo.Item2, 1));
                    }
                    else
                    {
                        whatToDo.Add(new FindInIntention(owner, (ISearchingSource)wayInfo.Item2, 1));
                    }
                }
            }
        }
        else if (owner.turnsFromLastReproduction >= owner.turnsToRestAfterReproduction) //Поиск ближайшего доступного подходящего партнёра для размножения
        {
            var interestingEntitys = Map.FindEntity(owner.currentCell.coords, owner.marriageSearchRadius, owner.selfEntityType);
            List<Intention> way = null;

            foreach (Unit entity in interestingEntitys.ToList()) //Исключение из списка возможных целей особей неподходящего пола и особей не готовых к размножению
            {
                if (entity.gender != owner.partnerGender || !((Animal)entity).CheckReadyForReplacementStatus())
                {
                    interestingEntitys.Remove(entity);
                }
            }

            var nearestEntity = SimulationMath.FindTheNearest(interestingEntitys, owner.currentCell.coords);
            interestingEntitys.Remove(nearestEntity);













            var wayInfo = FindWayToNearestInteresting(interestingEntitys);

            if (wayInfo.Item3)
            {
                if (wayInfo.Item1 != null)
                {
                    for (int i = 0; i < wayInfo.Item1.Count; i++)
                    {
                        whatToDo.Add(wayInfo.Item1[i]);
                    }
                }

                whatToDo.Add(new ReproductionIntention(owner, (IReproductive)wayInfo.Item2, 1));
            }
        }







        if (whatToDo.Count == 0)
        {
            List<Intention> way = null;

            if (randCoords == null || owner.currentCell.coords == randCoords)
            {
                bool findedNiceCoords = false;
                while (!findedNiceCoords)
                {
                    do
                    {
                        randCoords = SimulationMath.CreateRandomCoords(owner.currentCell.coords, owner.vision);
                    }
                    while (!SimulationMath.CheckCoordsToInclusionIntoMap((Vector3Int)randCoords) || randCoords == owner.currentCell.coords);

                    way = FindBestWay((Vector3Int)randCoords);

                    if (way != null)
                    {
                        findedNiceCoords = true;
                    }

                }
            }
            else
            {
                way = FindBestWay((Vector3Int)randCoords);
            }

            if (way != null)
            {
                for (int i = 0; i < way.Count; i++)
                {
                    whatToDo.Add(way[i]);
                }

            }

            randomMoving = true;
        }
        else
        {
            randomMoving = false;
        }
    }


}
