using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        bgmSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

        if (AudioManager.Instance != null) // Kiểm tra AudioManager.Instance
        {
            if (bgmSlider.value != 0f)
            {
                AudioManager.Instance.PlayBGM();
            }
            else
            {
                AudioManager.Instance.StopBGM();
            }
        }
    }
}