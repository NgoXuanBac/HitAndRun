using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSFX : ToggleBase
{
    override protected void Awake()
    {
        base.Awake();
        toggleKey = "SFX";
        if (AudioManager.Instance == null)
            Debug.Log("AudioManager is null");
    }
    protected override void OnToggleChanged(bool newState)
    {
        if (newState)
            AudioManager.Instance.SFXOn();
        else
            AudioManager.Instance.SFXOff();
    }
}

