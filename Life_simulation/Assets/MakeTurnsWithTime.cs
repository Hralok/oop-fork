using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTurnsWithTime : MonoBehaviour
{
    [SerializeField]
    protected MainWorldController mvc;
    [SerializeField]
    protected float timePerTurns;

    [SerializeField]
    protected float currentTime;

    void Start()
    {
        lastTime = Time.time;
    }

    protected float lastTime;
    void Update()
    {
        currentTime -= lastTime - Time.time;
        lastTime = Time.time;

        if (currentTime >= timePerTurns)
        {
            mvc.MakeTurn();
            currentTime = 0;
        }

    }
}
