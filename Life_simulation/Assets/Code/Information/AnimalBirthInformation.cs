using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBirthInformation : Information
{
    public int age { get; }
    public int healthPoints { get; }
    public Gender gender { get; }
    public int currentSaturation { get; }

    public AnimalBirthInformation(int age, int healthPoints, Gender gender, int currentSaturation)
    {
        this.age = age;
        this.healthPoints = healthPoints;
        this.gender = gender;
        this.currentSaturation = currentSaturation;
    }


}
