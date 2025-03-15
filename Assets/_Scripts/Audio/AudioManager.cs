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
    private AudioSource bgmSource;  // Nguồn nhạc nền
    private AudioSource sfxSource;  // Nguồn phát âm thanh hiệu ứng

    [Header("Audio Clips")]
    public AudioClip bgmClip;  // Nhạc nền
    public List<SFXMapping> sfxClip;  // Âm thanh hiệu ứng

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
            
            sfxSource.playOnAwake = true;
            sfxSource.playOnAwake = false;

            bgmSource.volume = 0.5f;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void BGMOn()
    {
        if (bgmSource.isPlaying) return;
        if (bgmClip != null)
        {
            bgmSource.Play();
        }
    }

    public void BGMOff()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
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

    public void SFXOff()
    {
        if (sfxSource.isPlaying)
        {
            sfxSource.Stop();
        }
    }

    public void SFXOn()
    {
        if (sfxSource.isPlaying)
        {
            sfxSource.Play();
        }
    }

}
