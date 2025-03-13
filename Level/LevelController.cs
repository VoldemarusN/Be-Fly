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
        public event Action OnWin;
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
        private int _gottenAwardAmount;


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
            _storageData.LastLevel = SceneManager.GetActiveScene().name;
            _trapGenerationSettings = settings;
        }

        private void SetCurrentAward() =>
            _currentAward = _storageData.Levels.FirstOrDefault(x => x.Name == _storageData.LastLevel).NextAwardIndex;

        private void UpdateSavedAwardIndex() =>
            _storageData.Levels.FirstOrDefault(x => x.Name == _storageData.LastLevel).NextAwardIndex = _currentAward;

        private void LooseGame()
        {
            _view.OnTouchedGround -= LooseGame;
            EndGameTask();
        }

        private void WinGame()
        {
            OnWin?.Invoke();
            _money = (int)_moneyProgress;
            _storageData.Money += _money;
            _view.OnTouchedGround -= LooseGame;
            _view.StopPlane();
            _audioManager.PlayWinEffect();
            _storageData.Money += _money;
            _planeController.DisableInput();
            SetupWinWindow();
            SaveDistance();
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
            SetupLooseWindow();
            SaveDistance();
        }

        private void SaveDistance()
        {
            _storageData.Levels.FirstOrDefault(x => x.Name == _storageData.LastLevel).PassedDistance =
                Mathf.Max(_passedDistance,
                    _storageData.Levels.FirstOrDefault(x => x.Name == _storageData.LastLevel).PassedDistance);
        }

        private void SetupLooseWindow()
        {
            _gameOverWindow.gameObject.SetActive(true);
            _gameOverWindow.SetCurrentProgressNormalized(PassedDistanceNormalized);
            _gameOverWindow.SetLastProgressNormalized(
                _storageData.Levels[0].PassedDistance / _levelConfig.LevelDistance);
            _gameOverWindow.SetMoney(_money);
            _gameOverWindow.SetDistance((int)_passedDistance);
            if (_gottenAwardAmount > 0)
                _gameOverWindow.SetAchievement(_gottenAwardAmount);
        }

        private void SetupWinWindow()
        {
            _won = true;
            _winWindow.gameObject.SetActive(true);
            _winWindow.SetDistance((int)_passedDistance);
            _winWindow.SetMoney(_money);
            if (_gottenAwardAmount > 0)
                _winWindow.SetAchievement(_gottenAwardAmount);
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
                int amount = _levelConfig.LevelAwards[_currentAward].Amount;
                _moneyProgress += amount;
                _currentAward++;
                _gottenAwardAmount += amount;
                UpdateSavedAwardIndex();
                _audioManager.PlayOneShotEffect(_audioManager.GrabbedFlag);
                //UpdateAwardPoints();
                _progressView.ClearFirstPoint(PointType.Income);
                _progressView.ShowAwardEffect();
            }
        }

        private void TryAddMoney()
        {
            float delta = _passedDistance - _handledPassedDistance;
            _moneyProgress += delta * _levelConfig.MoneyAmountForMeter;
            _handledPassedDistance = _passedDistance;
        }


        private void UpdateLevelIndicator()
        {
            _progressView.SetProgress(PassedDistanceNormalized, (int)PassedDistance);
        }

        public void Dispose()
        {
            _source.Cancel();
            UpdateSavedAwardIndex();
        }

        public void Initialize()
        {
            SetCurrentAward();
            UpdateAwardPoints();
            foreach (var trap in _trapGenerationSettings.StaticTraps)
            {
                _progressView.SetPoint(trap.Distance / _levelConfig.LevelDistance, PointType.Trap);
            }
        }

        private void UpdateAwardPoints()
        {
            _progressView.ClearPoints(PointType.Income);
            for (var i = _currentAward;
                 i < _levelConfig.LevelAwards.Length;
                 i++)
            {
                var award = _levelConfig.LevelAwards[i];
                _progressView.SetPoint(award.Distance / _levelConfig.LevelDistance, PointType.Income);
            }
        }
    }
}