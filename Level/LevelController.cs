using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Plane;
using SavingSystem;
using Scriptable_Objects;
using TMPro;
using Traps.TrapsGenerationLogic;
using Traps.TrapsGenerationLogic;
using UI.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class LevelController : ITickable, IDisposable, IInitializable
    {
        private float PassedDistanceNormalized => PassedDistance / _levelConfig.LevelDistance;

        public float PassedDistance
        {
            get => _passedDistance;
            set
            {
                if (value < _passedDistance) return;
                _passedDistance = value;
            }
        }


        private float _passedDistance;
        private float _handledPassedDistance;

        private readonly Transform _planeTransform;
        private readonly LevelConfig _levelConfig;
        private readonly float _planeStartPositionX;
        private readonly LevelProgressView _progressView;
        private readonly PlaneController _planeController;
        private readonly PlaneView _view;
        private readonly StorageData _storageData;
        private readonly LooseWindow _gameOverWindow;
        private readonly WinWindow _winWindow;
        private readonly AudioManager _audioManager;
        private readonly CancellationTokenSource _source;
        private readonly TrapGenerationSettings _trapGenerationSettings;
        private bool _won;
        private int _currentAward;
        private int _money;
        private float _moneyProgress;


        private LevelController(LevelConfig levelConfig,
            LevelProgressView progressView, Storage storageData,
            PlaneController planeController, LooseWindow gameOverWindow,
            WinWindow winWindow, AudioManager audioManager,
            TrapGenerationSettings settings)
        {
            _planeTransform = planeController.View.transform;
            _levelConfig = levelConfig;
            _progressView = progressView;
            _planeController = planeController;
            _view = planeController.View;
            _storageData = storageData.StorageData;
            _gameOverWindow = gameOverWindow;
            _winWindow = winWindow;
            _audioManager = audioManager;
            _planeStartPositionX = _planeTransform.position.x;
            _view.OnTouchedGround += LooseGame;
            _source = new CancellationTokenSource();
            _trapGenerationSettings = settings;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Level 1":
                    _storageData.LastLevel = 0;
                    break;
                case "Level 2":
                    _storageData.LastLevel = 1;
                    break;
                case "Level 3":
                    _storageData.LastLevel = 2;
                    break;
            }

            SetCurrentAward();
        }

        private void SetCurrentAward()
        {
            _currentAward = _storageData.Levels[_storageData.LastLevel].NextAwardIndex;
        }

        private void LooseGame()
        {
            _view.OnTouchedGround -= LooseGame;
            EndGameTask();
        }

        private void WinGame()
        {
            _view.OnTouchedGround -= LooseGame;
            _view.StopPlane();
            _audioManager.PlayWinEffect();
            _storageData.Money += _money;
            _planeController.Dispose();
            SetupWinWindow();
        }


        private async UniTask EndGameTask()
        {
            _money = (int)_moneyProgress;
            _storageData.Money += _money;

            Queue<float> middleSpeedList = new Queue<float>();

            do
            {
                await UniTask.Delay(1000, cancellationToken: _source.Token);
                middleSpeedList.Enqueue(_view.Speed);
                if (middleSpeedList.Count > 3) middleSpeedList.Dequeue();
            } while (middleSpeedList.Average() > 10f);

            _audioManager.PlayLooseEffect();
            SetupGameOverWindow();
            _storageData.Levels[0].PassedDistance = _passedDistance;
        }

        private void SetupGameOverWindow()
        {
            _gameOverWindow.gameObject.SetActive(true);
            _gameOverWindow.SetCurrentProgressNormalized(PassedDistanceNormalized);
            _gameOverWindow.SetLastProgressNormalized(_storageData.Levels[0].PassedDistance / _levelConfig.LevelDistance);
            _gameOverWindow.SetMoney(_money);
            _gameOverWindow.SetDistance((int)_passedDistance);
        }

        private void SetupWinWindow()
        {
            _won = true;
            _winWindow.gameObject.SetActive(true);
            _winWindow.SetDistance((int)_passedDistance);
            _winWindow.SetMoney(_money);
        }

        public void Tick()
        {
            PassedDistance = _planeTransform.position.x - _planeStartPositionX;
            UpdateLevelIndicator();
            TryAddMoney();
            TryAddAward();
            if (PassedDistanceNormalized >= 1 && !_won) WinGame();
        }

        private void TryAddAward()
        {
            if (_currentAward >= _levelConfig.LevelAwards.Length) return;
            if (_passedDistance >= _levelConfig.LevelAwards[_currentAward].Distance)
            {
                _moneyProgress += _levelConfig.LevelAwards[_currentAward].Amount;
                _currentAward++;
            }
        }

        private void TryAddMoney()
        {
            float delta = _passedDistance - _handledPassedDistance;
            _moneyProgress += delta * _levelConfig.MoneyAmountForMeter;
            _handledPassedDistance = _passedDistance;
#if UNITY_EDITOR
            CustomDebugConsole.Instance.ShowString(0, "Money: " + _money);
            CustomDebugConsole.Instance.ShowString(1, "delta: " + delta);

#endif
        }


        private void UpdateLevelIndicator()
        {
            _progressView.SetProgress(PassedDistanceNormalized, (int)PassedDistance);
        }

        public void Dispose()
        {
            _source.Cancel();
            _storageData.Levels[_storageData.LastLevel].NextAwardIndex = _currentAward;
        }

        public void Initialize()
        {
            foreach (var trap in _trapGenerationSettings.StaticTraps)
            {
                _progressView.SetPoint(trap.Distance, PointType.Trap, _levelConfig.LevelDistance);
            }

            for (var i = _storageData.Levels[_storageData.LastLevel].NextAwardIndex; i < _levelConfig.LevelAwards.Length; i++)
            {
                var award = _levelConfig.LevelAwards[i];
                _progressView.SetPoint(award.Distance, PointType.Income, _levelConfig.LevelDistance);
            }
        }
    }
}