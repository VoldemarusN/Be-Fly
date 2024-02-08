using System;
using SavingSystem;
using Scriptable_Objects;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.LevelMenu
{
    public class LevelMenuController : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Button[] _levels;
        [SerializeField] private Image[] _levelImages;
        [SerializeField] private LevelConfig[] _configs;
        [SerializeField] private ComicShower _comicShower;
        [SerializeField] private ComicData _comicData;


        private SceneLoader _sceneLoader;
        private Storage _storage;


        [Inject]
        private void Construct(SceneLoader sceneLoader, Storage storage)
        {
            _sceneLoader = sceneLoader;
            _storage = storage;
        }

        private void Awake()
        {
            SetLevelProgress(_storage.StorageData.Levels, _configs);
            InitializeButtonsButtons();
            if (_storage.StorageData.CutSceneWasPlayed == false)
            {
                /*Sprite[] comicSprites;
                switch (_storage.)
                {
                    
                }
                
                _comicShower.SetFrame();*/
            }
        }

        private void InitializeButtonsButtons()
        {
            _exitButton.onClick.AddListener(() => _sceneLoader.LoadScene(SceneType.MainMenu));

            for (int i = 0; i < _levels.Length; i++)
            {
                var i1 = i;
                _levels[i].onClick.AddListener(() =>
                {
                    _storage.StorageData.LastLevel = i1;
                    _sceneLoader.LoadScene(SceneType.Garage);
                });
            }
        }


        private void SetLevelProgress(LevelData[] storageDataLevels, LevelConfig[] configs)
        {
            bool isLevelLocked = false;
            for (int i = 0; i < _configs.Length; i++)
            {
                if (isLevelLocked)
                {
                    _levels[i].interactable = false;
                    _levels[i].GetComponent<CanvasGroup>().alpha = 0.4f;
                    continue;
                }
                else
                {
                    _levels[i].interactable = true;
                    _levels[i].GetComponent<CanvasGroup>().alpha = 1;
                    
                }

                float normalizedDistance = (storageDataLevels[i].PassedDistance / configs[i].LevelDistance);

                _levelImages[i].fillAmount = normalizedDistance;
                if (Mathf.Approximately(normalizedDistance, 1) == false)
                {
                    _progressText.text = "Progress: " + (int)(normalizedDistance * 100) + "%";
                    isLevelLocked = true;
                }
            }
        }
    }
}