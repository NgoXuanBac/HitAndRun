using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;

        bgmSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

        if (bgmVolume > 0f)
            AudioManager.Instance.PlayBGM();
        else
            AudioManager.Instance.StopBGM();
    }
}
