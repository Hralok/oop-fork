using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBuildingPreviousStageInformation : Information
{
    public int age { get; }
    public int healthPoints { get; }
    public Resource mineralResource { get; }

    public PlantBuildingPreviousStageInformation(int age, int healthPoints, Resource mineralResource)
    {
        this.age = age;
        this.healthPoints = healthPoints;
        this.mineralResource = mineralResource;
    }
}
