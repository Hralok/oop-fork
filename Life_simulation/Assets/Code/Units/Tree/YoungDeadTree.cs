using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoungDeadTree : DeadStage
{
    public YoungDeadTree(Cell currentCell, Fraction currentFraction, EntityTypeEnum selfEntityType, Information initializationInformation)
        : base(currentCell, currentFraction, selfEntityType, initializationInformation, new MovingObstructionFromBuilding(0, 0, 0, 0, 0, 0))
    {
        helthFromPreviousStageMultipler = 2;
        mineralsFromAsimilation = 30;
        damagePerTurn = 10;
        selfEntityType = EntityTypeEnum.YoungDeadTree;

        switch (initializationInformation)
        {
            case RuinsPreviousStageInformation info:
                age = info.age;
                currentHelthPoints = info.healthPoints * helthFromPreviousStageMultipler;
                break;
            case null:
                age = 15;
                currentHelthPoints = 60;
                break;
            default:
                throw new Exception("При создании сущности был передан неверный подкласс информации");
        }
    }
}
