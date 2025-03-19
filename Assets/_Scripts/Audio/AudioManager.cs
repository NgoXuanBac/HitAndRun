using System;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    ButtonClick,
    PlayerWin,
    PlayerLose
}

[Serializable]
public class SFXMapping
{
    public SFXType sfxType;
    public AudioClip sfxClip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    private AudioSource bgmSource;  
    private AudioSource sfxSource;  

    [Header("Audio Clips")]
    public AudioClip bgmClip;  
    public List<SFXMapping> sfxClip;  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgmSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            bgmSource.clip = bgmClip;
            bgmSource.loop = true;

            sfxSource.playOnAwake = false;

            float savedBgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
            float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

            SetBGMVolume(savedBgmVolume);
            SetSFXVolume(savedSfxVolume);

            if (savedBgmVolume != 0f && !bgmSource.isPlaying)
            {
                PlayBGM();
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Update()
    {
        float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        if (Input.GetMouseButtonDown(0) && sfxSource.volume != 0)
        {
            PlaySFX(SFXType.ButtonClick);
        }
    }
    public void PlayBGM()
    {
        if(!bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
    public void PlaySFX(SFXType type)
    {
        SFXMapping mapping = sfxClip.Find(sfx => sfx.sfxType == type);
        if (mapping != null && mapping.sfxClip != null)
        {
            sfxSource.PlayOneShot(mapping.sfxClip);
        }
        else
        {
            Debug.LogWarning($"SFXType {type} không có âm thanh tương ứng!");
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
