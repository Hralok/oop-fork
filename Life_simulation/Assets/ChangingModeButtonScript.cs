using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangingModeButtonScript : MonoBehaviour
{
    [SerializeField]
    private int currentMode = 1;

    [SerializeField]
    private int maximalMode = 2;

    [SerializeField]
    private Color[] colorList;

    public int GetMode()
    {
        return currentMode;
    }

    public void ChangeMode()
    {
        currentMode += 1;

        if (currentMode > maximalMode)
        {
            currentMode = 1;
        }

        gameObject.GetComponent<Image>().color = colorList[currentMode - 1];
    }
}
