using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Audio;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public AudioClip PassiveUpgradeSound;
    public AudioClip[] ActiveUpgradeSounds;
    public AudioClip AchievementSound;

    [SerializeField] private AudioSource[] _effectSources;
    [SerializeField] private AudioSource _backgroundSource;
    [SerializeField] private AudioClip _looseSound;
    [SerializeField] private AudioClip _winSound;


    public void PlayBackgroundMusic(AudioClip backgroundMusic)
    {
        if (_backgroundSource.clip == backgroundMusic) return;
        _backgroundSource.clip = backgroundMusic;
        _backgroundSource.Play();
    }

    public AudioSource GetEffectSource(int layer = 0) => _effectSources[layer];

    public void PlayEffect(AudioClip clip, int layer = 0)
    {
        _effectSources[layer].clip = clip;
        _effectSources[layer].Play();
    }

    public void PlayOneShotEffect(AudioClip clip) => _effectSources[0].PlayOneShot(clip);

    public void PlayOneShotEffect(AudioClip[] clips) => 
        PlayEffect(clips[Random.Range(0, clips.Length)]);


    public void PlayRandomEffect(AudioClip[] clips) =>
        PlayEffect(clips[Random.Range(0, clips.Length)]);

    public void StopEffect(int layer = 0) => _effectSources[layer].Stop();


    public void PlayLooseEffect() => PlayEffect(_looseSound);

    public void PlayWinEffect() => PlayEffect(_winSound);
}