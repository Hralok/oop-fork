using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdultDeadTree : DeadStage
{
    public AdultDeadTree(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information initializationInformation)
        : base(currentCell, currentFraction, selfEntityType, initializationInformation, new MovingObstructionFromBuilding(1, 1, 1, 1, 0, 0))
    {
        helthFromPreviousStageMultipler = 2;
        mineralsFromAsimilation = 300;
        damagePerTurn = 20;
        selfEntityType = EntityTypeEnum.AdultDeadTree;



        switch (initializationInformation)
        {
            case RuinsPreviousStageInformation info:
                age = info.age;
                currentHelthPoints = info.healthPoints * helthFromPreviousStageMultipler;
                break;
            case null:
                age = 30;
                currentHelthPoints = 600;
                break;
            default:
                throw new Exception("При создании сущности был передан неверный подкласс информации");
        }
    }
}
