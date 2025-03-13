using System;
using System.Linq;
using ModestTree;
using SavingSystem;
using Scriptable_Objects;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace UI.LevelMenu
{
    public class LevelMenuController : MonoBehaviour
    {
        [SerializeField] private Sprite[] _beginnerComicFrames;
        [SerializeField] private Sprite[] _endComicFrames;


        [SerializeField] private Button _exitButton;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private LevelView[] _levels;
        [SerializeField] private ComicController _comicController;
        [SerializeField] private Sprite _level1CompleteSprite;
        [SerializeField] private Sprite _level2CompleteSprite;
        [SerializeField] private LocalizedString _progressKey;


        private SceneLoader _sceneLoader;
        private Storage _storage;
        private AudioManager _audioManager;


        [Inject]
        private void Construct(SceneLoader sceneLoader, Storage storage, AudioManager audioManager)
        {
            _audioManager = audioManager;
            _sceneLoader = sceneLoader;
            _storage = storage;
        }

        private void Start()
        {
            bool levelShouldBeBlocked = false;
            for (var i = 0; i < _levels.Length; i++)
            {
                var levelView = _levels[i];
                var levelProgress = levelView.SetProgress(_storage.StorageData.Levels[i]);
                
                levelView.SetInteractivity(true);
                if (levelShouldBeBlocked) levelView.SetInteractivity(false);
                else if (levelProgress < 1f)
                {
                    UpdateProgressText(levelProgress);
                    levelShouldBeBlocked = true;
                }
                else levelView.ShowReward();
            }

        
            
            
            foreach (var t in _levels)
            {
                t.OnClick += () =>  LoadLevel(t.Config.SceneName);
            }


            ShowComic();
            _comicController.OnComicEnded += () =>
                _audioManager.PlayBackgroundMusic(_audioManager.MenuMusic, true);

            _exitButton.onClick.AddListener(() => _sceneLoader.LoadScene(SceneType.MainMenu));
        }

        private void LoadLevel(string levelName)
        {
            _storage.StorageData.LastLevel = levelName;
            _sceneLoader.LoadScene(SceneType.Garage);
        }

        private void ShowComic()
        {
            for (int i = _levels.Length - 1; i >= 0; i--)
            {
                var levelData = _storage.StorageData.Levels[i];
                float normalizedDistance = levelData.PassedDistance / _levels[i].Config.LevelDistance;

                if (normalizedDistance <= 1f) continue;

                if (levelData.ComicWasShown == false)
                {
                    if (i == 2)
                    {
                        _comicController.ShowMultiComic(_endComicFrames);
                        PlayerEpicMusic();
                        _comicController.OnFrameSwitched += frameIndex =>
                        {
                            switch (frameIndex)
                            {
                                case 2:
                                    _audioManager.SetBackgroundMusicVolume(0.05f);
                                    break;
                                case 3:
                                    PlaySadMusic();
                                    _audioManager.SetBackgroundMusicVolume(0.05f);
                                    break;
                                case 4:
                                    _audioManager.SetBackgroundMusicVolume(0.1f);
                                    break;
                            }

                            //_audioManager.SetBackgorundMusicVolume(0.5f);
                            if (frameIndex == 3)
                            {
                            }
                        };
                    }
                    else
                    {
                        _comicController.ShowFrame(i == 0 ? _level1CompleteSprite : _level2CompleteSprite);
                        _audioManager.PlayBackgroundMusic(_audioManager.ComicMusic);
                    }


                    levelData.ComicWasShown = true;
                    return;
                }
            }


            if (_storage.StorageData.CutSceneWasPlayed == false)
            {
                _comicController.ShowMultiComic(_beginnerComicFrames);
                _audioManager.PlayBackgroundMusic(_audioManager.ComicMusic);
                _storage.StorageData.CutSceneWasPlayed = true;
            }
        }

        private void UpdateProgressText(float normalizedDistance) =>
            _progressText.text = _progressKey.GetLocalizedString() + " " + (int)(normalizedDistance * 100) + "%";

        private void PlayerEpicMusic()
        {
            _audioManager.PlayBackgroundMusic(_audioManager.EpicMusic);
        }

        private void PlaySadMusic()
        {
            _audioManager.PlayBackgroundMusic(_audioManager.SadMusic, smooth: true);
        }
    }
}