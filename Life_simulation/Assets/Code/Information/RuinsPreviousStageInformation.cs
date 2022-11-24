using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinsPreviousStageInformation : Information
{
    public int age { get; }
    public int healthPoints { get; }

    public RuinsPreviousStageInformation(int age, int healthPoints)
    {
        this.age = age;
        this.healthPoints = healthPoints;
    }
}
