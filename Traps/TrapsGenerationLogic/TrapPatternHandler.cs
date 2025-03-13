using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Traps.TrapsGenerationLogic
{
    public class TrapPatternHandler : ITickable, IDisposable
    {
        public Action<TrapPattern> OnPatternSpawned;

        private readonly TrapGenerationSettings _settings;
        private readonly CameraPositionService _cameraPositionService;
        private readonly TrapPatternProvider _trapProvider;
        private readonly Transform _planeTransform;
        private readonly List<TrapPattern> _createdPatterns;
        private bool _zoneIsReadyToSpawn = false;
        private CancellationTokenSource _cancellationTokenSource;

        public TrapPatternHandler(Transform planeTransform, TrapGenerationSettings settings,
            CameraPositionService cameraPositionService, TrapPatternProvider trapPatternFactory)
        {
            _createdPatterns = new List<TrapPattern>();
            _settings = settings;
            _cameraPositionService = cameraPositionService;
            _planeTransform = planeTransform;
            _trapProvider = trapPatternFactory;
            _cancellationTokenSource = new CancellationTokenSource();
            CheckForFirstTrapSpawning();
        }

        private async UniTaskVoid CheckForFirstTrapSpawning()
        {
            await UniTask.WaitUntil(() => _planeTransform.position.x > _settings.StartGenerationXCoordinate,
                cancellationToken: _cancellationTokenSource.Token);
            _zoneIsReadyToSpawn = true;
        }

        public void Tick()
        {
            if (_zoneIsReadyToSpawn)
            {
                TrapPatternDifficulty difficulty = GetDifficulty();
                TrapPattern trapPattern = _trapProvider.GetPatternByDifficulty(difficulty);
                if (trapPattern)
                {
                    _zoneIsReadyToSpawn = false;
                    SpawnPattern(trapPattern);
                }
            }
        }

        private void SpawnPattern(TrapPattern trapPattern)
        {
            float cameraRightBorderPoint = _cameraPositionService.Camera.transform.position.x +
                                           _cameraPositionService.GetCameraHalfWidth();
            var positionX = cameraRightBorderPoint + trapPattern.Width / 2;
            Vector3 position =
                new Vector2(positionX, 0);
            position.y =
                Mathf.Clamp(_planeTransform.position.y,
                    _settings.MinYPosition + trapPattern.Height / 2,
                    Mathf.Infinity);
            position.z = 0;

            trapPattern.transform.position = position;
            trapPattern.MoveToCenter();
            _createdPatterns.Add(trapPattern);
            
            OnPatternSpawned?.Invoke(trapPattern);
            trapPattern.OnAllTrapsDeactivated += DeactivatePattern;
        }

        private TrapPatternDifficulty GetDifficulty()
        {
            float value = Random.Range(0, 100);
            if (value < _settings.HardTrapChance)
                return TrapPatternDifficulty.Hard;
            if (value < _settings.HardTrapChance + _settings.EasyTrapChance)
                return TrapPatternDifficulty.Easy;
            if (value < _settings.HardTrapChance + _settings.EasyTrapChance + _settings.BoostChance)
                return TrapPatternDifficulty.Booster;
            return TrapPatternDifficulty.Easy;
        }

        private async void DeactivatePattern(TrapPattern trapPattern)
        {
            Debug.Log("Pattern was deactivated " + trapPattern.gameObject.name);
            trapPattern.OnAllTrapsDeactivated -= DeactivatePattern;
            _trapProvider.ReleasePattern(trapPattern);
            await UniTask.Delay(TimeSpan.FromSeconds(_settings.DelayBetweenPatterns));
            _zoneIsReadyToSpawn = true;
        }

        public void Dispose() => _cancellationTokenSource?.Cancel();
    }
}