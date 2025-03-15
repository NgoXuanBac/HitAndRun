using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBGM : ToggleBase
{
    override protected void Awake()
    {
        base.Awake();
        toggleKey = "BGM";
        if (AudioManager.Instance == null) 
            Debug.Log("AudioManager is null");
    }

    protected override void OnToggleChanged(bool newState)
    {
        if (newState)
            AudioManager.Instance.BGMOn();
        else
            AudioManager.Instance.BGMOff();
    }
    
}
