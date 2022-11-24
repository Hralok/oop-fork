using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System.Collections.Generic;
using System;

public class MainWorldController : MonoBehaviour
{
    [SerializeField]
    private int earthTileWeight;
    [SerializeField]
    private int grassTileWeight;
    [SerializeField]
    private int waterTileWeight;
    [SerializeField]
    private int mountainTileWeight;
    [SerializeField]
    private int smallMountainTileWeight;

    [SerializeField]
    private Tilemap groundTileMap;
    [SerializeField]
    private Tilemap mountainTileMap;

    [SerializeField]
    private int worldSeedx;
    [SerializeField]
    private int worldSeedy;
    [SerializeField]
    private float worldZoom;

    [SerializeField]
    private int map_radius;


    private Fraction neutralFraction;

    [SerializeField]
    private int minimalMineralCount;
    [SerializeField]
    private int maximalMineralCount;


    [SerializeField]
    private GameObject testParent;

    [SerializeField]
    private int minimumPercentageOfTheTerritoryOccupiedByAdultTrees;
    [SerializeField]
    private int minimumPercentageOfTheTerritoryOccupiedByYoungTrees;
    [SerializeField]
    private int minimumPercentageOfTheTerritoryOccupiedSeedTrees;

    [SerializeField]
    private bool generateTrees;

    public int GetWorldSeedX()
    {
        return worldSeedx;
    }

    public int GetWorldSeedY()
    {
        return worldSeedy;
    }

    public float GetWorldZoom()
    {
        return worldZoom;
    }

    public Fraction GetNeutralFraction()
    {
        return neutralFraction;
    }

    [SerializeField]
    private int treeCounter;

    private void Start()
    {
        neutralFraction = new Fraction();


        Builder.InitializeBuilder();
        SimulationMath.InitializeSimulationMath(groundTileMap, this);

        Map.InitializeMap(
            earthTileWeight,
            grassTileWeight,
            waterTileWeight,
            mountainTileWeight,
            smallMountainTileWeight,
            minimumPercentageOfTheTerritoryOccupiedByAdultTrees,
            minimumPercentageOfTheTerritoryOccupiedByYoungTrees,
            minimumPercentageOfTheTerritoryOccupiedSeedTrees,
            generateTrees
            );

        treeCounter = WorldStatusInformation.treeCounter;

        //for (int i = 0; i < 5000; i++)
        //{
        //    Builder.Build(new CreateIntention(null, EntityTypeEnum.Sheep, SimulationMath.CreateRandomCoords(map_radius), null, neutralFraction));
        //}


        //Builder.Build(EntityTypeEnum.SeedTree, new Vector3Int(9, -6, -3), testParent);
    }

    public int GetMinimalMineralCount()
    {
        return minimalMineralCount;
    }

    public int GetMaximalMineralCount()
    {
        return maximalMineralCount;
    }

    public int GetMapRadius()
    {
        return map_radius;
    }


    [SerializeField]
    private int sheepCounter;

    protected float lastTime;


    public void MakeTurn()
    {
        Debug.Log(Time.realtimeSinceStartup - lastTime);
        lastTime = Time.realtimeSinceStartup;

        foreach (Intention i in neutralFraction.CollectIntentions())
        {
            if (!WorldStatusInformation.CheckForBeingDestroyed(i.executor))
            {
                switch (i)
                {
                    case MakeDamageIntention inten:
                        (inten.executor as IDamageDealer).DealDamage(inten);
                        break;
                    case MoveIntention inten:
                        Builder.AddCellToRePaint(inten.executor.currentCell);
                        Builder.AddCellToRePaint(inten.targetCell);
                        inten.executor.MoveRightNow(inten);
                        break;
                    case CreateIntention inten:
                        Builder.Build(inten);
                        Builder.AddCellToRePaint(Map.GetCell(inten.targetCellCords));
                        break;
                    case ExtractionIntention inten:
                        (inten.executor as IExtract).Extract(inten.target);
                        break;
                    case ReproductionIntention inten:
                        WorldStatusInformation.sheepCounter += 1;
                        sheepCounter = WorldStatusInformation.sheepCounter;
                        (inten.executor as Animal).Reproduct(inten);
                        break;
                    case FindInIntention inten:
                        (inten.executor as ISearcher).Search(inten);
                        break;

                }
            }
        }



        neutralFraction.LiveOneTurn();
        neutralFraction.KillEveryoneWhoWants();

        Builder.RePaintAll();





        WorldStatusInformation.CleanDeadList();



    }

}
