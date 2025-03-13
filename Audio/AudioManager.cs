using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Audio;
using DG.Tweening;
using Services;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public AudioClip PassiveUpgradeSound;
    public AudioClip[] ActiveUpgradeSounds;
    public AudioClip AchievementSound;
    public AudioClip ComicMusic;
    public AudioClip MenuMusic;
    public AudioClip GrabbedFlag;
    public AudioClip SadMusic;
    public AudioClip EpicMusic;

    [SerializeField] private AudioSource[] _effectSources;
    [SerializeField] private AudioSource _backgroundSource;
    [SerializeField] private AudioClip _looseSound;
    [SerializeField] private AudioClip _winSound;

    [SerializeField, ReadOnly] private float[] _startEffectVolumes;


    private void Start()
    {
        GlobalSettings.OnChange += OnSettingsChanged;
    }

    private void OnDestroy()
    {
        GlobalSettings.OnChange -= OnSettingsChanged;
    }

    private void OnSettingsChanged()
    {
        foreach (var source in _effectSources)
            source.enabled = GlobalSettings.SFX;
        
        _backgroundSource.enabled = GlobalSettings.Music;
    }

    private void OnValidate()
    {
        _startEffectVolumes = new float[_effectSources.Length];
        for (int i = 0; i < _effectSources.Length; i++)
            _startEffectVolumes[i] = _effectSources[i].volume;
    }


    public void PlayBackgroundMusic(AudioClip backgroundMusic, bool smooth = false)
    {
        if (_backgroundSource.clip == backgroundMusic) return;
        if (smooth)
        {
            _backgroundSource.DOFade(0f, 0.2f).OnComplete(() =>
            {
                _backgroundSource.clip = backgroundMusic;
                _backgroundSource.Play();
                _backgroundSource.DOFade(0.1f, 0.2f);
            });
        }
        else
        {
            _backgroundSource.clip = backgroundMusic;
            _backgroundSource.Play();
        }
    }

    public AudioSource GetEffectSource(int layer = 0) => _effectSources[layer];
    public float GetStartEffectSourceVolume(int layer = 0) => _startEffectVolumes[layer];

    public void PlayEffect(AudioClip clip, int layer = 0)
    {
        if (clip == null)
        {
            _effectSources[layer].Stop();
        }
        
        _effectSources[layer].clip = clip;
        _effectSources[layer].Play();
    }

    public void PlayOneShotEffect(AudioClip clip, float pitchVariance = 0f)
    {
        _effectSources[0].pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
        _effectSources[0].PlayOneShot(clip);
    }

    public void PlayOneShotEffect(AudioClip[] clips) =>
        PlayEffect(clips[Random.Range(0, clips.Length)]);


    public void PlayRandomEffect(AudioClip[] clips) =>
        PlayEffect(clips[Random.Range(0, clips.Length)]);

    public void StopEffect(int layer = 0) => _effectSources[layer].Stop();


    public void PlayLooseEffect() => PlayEffect(_looseSound);

    public void PlayWinEffect() => PlayEffect(_winSound);

    public void SetBackgroundMusicVolume(float volume) =>
        _backgroundSource.volume = volume;
}