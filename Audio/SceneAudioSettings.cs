using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace.Audio
{
    public class SceneAudioSettings : MonoBehaviour
    {
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private ButtonSound[] _buttonSounds;
        private AudioManager _audioManager;


        [Inject]
        private void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        private void Awake()
        {
            foreach (var buttonSound in _buttonSounds)
                buttonSound.Button.onClick.AddListener(() => _audioManager.PlayRandomEffect(buttonSound.Clips));
            _audioManager.PlayBackgroundMusic(_backgroundMusic);
        }
    }

    [Serializable]
    internal class ButtonSound
    {
        public Button Button;
        public AudioClip[] Clips;
    }
}