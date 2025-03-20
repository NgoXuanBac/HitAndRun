using System;
using System.Collections.Generic;
using UnityEngine;

namespace HitAndRun.Audio
{

    public class MbAudioManager : MbSingleton<MbAudioManager>
    {

        [Header("Audio Sources")]
        private AudioSource _bgmSource;
        private AudioSource _sfxSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip _bgmClip;

        private void Awake()
        {
            if (Instance != null) DontDestroyOnLoad(Instance);
            _bgmSource = gameObject.AddComponent<AudioSource>();
            _sfxSource = gameObject.AddComponent<AudioSource>();

            _bgmSource.clip = _bgmClip;
            _bgmSource.loop = true;

            _sfxSource.playOnAwake = false;

            float savedBgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
            float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

            SetBGMVolume(savedBgmVolume);
            SetSFXVolume(savedSfxVolume);

            if (savedBgmVolume != 0f && !_bgmSource.isPlaying)
            {
                PlayBGM();
            }

        }
        public void PlayBGM()
        {
            if (!_bgmSource.isPlaying)
            {
                _bgmSource.Play();
            }
        }

        public void StopBGM()
        {
            _bgmSource.Stop();
        }

        public void PlaySFX(AudioClip clip)
        {
            _sfxSource.PlayOneShot(clip);
        }

        public void SetBGMVolume(float volume)
        {
            _bgmSource.volume = volume;
            PlayerPrefs.SetFloat("BGMVolume", volume);
        }

        public void SetSFXVolume(float volume)
        {
            _sfxSource.volume = volume;
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }
    }

}
