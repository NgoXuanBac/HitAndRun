using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MbDisable : MonoBehaviour
{
    public float delayTime = 0.2f; 

    public void HideSetting()
    {
        Invoke("DisablePanel", delayTime); 
    }

    void DisablePanel()
    {
        gameObject.SetActive(false); 
    }
}
