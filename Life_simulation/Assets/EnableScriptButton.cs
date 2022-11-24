using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableScriptButton : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour victim;



    public void ChangeStatus()
    {
        victim.enabled = !victim.enabled;

        if (victim.enabled)
        {
            gameObject.GetComponent<Image>().color = Color.green;
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.red;
        }
    }

}
