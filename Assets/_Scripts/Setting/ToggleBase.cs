using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBase : MonoBehaviour
{
    public RectTransform circleHandle;
    public float distance = 100f;
    private Vector2 onPosition;
    private Vector2 offPosition;
    protected bool isOn = false;
    public string toggleKey;

    protected virtual void Awake()
    {
        isOn = LoadSetting();
        Debug.Log("isOn: " + isOn);
    }
    protected virtual void Start()
    {
        offPosition = circleHandle.anchoredPosition;
        onPosition = offPosition + new Vector2(distance, 0);
        UpdateUI();
        OnToggleChanged(isOn);
    }

    public void Toggle()
    {
        isOn = !isOn;
        UpdateUI();
        PlayerPrefs.SetInt(toggleKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
        OnToggleChanged(isOn);

        ClickSound();
    }

    protected virtual void OnToggleChanged(bool newState) 
    { 
    }

    void UpdateUI()
    {
        circleHandle.anchoredPosition = isOn ? onPosition : offPosition;
    }

    bool LoadSetting(bool defaultValue = true)
    {
        return PlayerPrefs.GetInt(toggleKey, defaultValue ? 1 : 0) == 1;
    }

    public void ClickSound()
    {
        if(PlayerPrefs.GetInt("SFX", 1) == 1)
        {
            AudioManager.Instance.PlaySFX(SFXType.ButtonClick);
        }
    }
}
