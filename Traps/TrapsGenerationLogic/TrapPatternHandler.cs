using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using UnityEngine;
using Zenject;

namespace Traps.TrapsGenerationLogic
{
    public class TrapPatternHandler : ITickable
    {
        public Action<TrapPattern> OnPatternSpawned;

        private readonly TrapGenerationSettings _settings;
        private readonly CameraPositionService _cameraPositionService;
        private Transform _planeTransform;
        private int _currentZone;
        private List<TrapPattern> _createdPatterns;
        private TrapPatternProvider _trapProvider;

        public TrapPatternHandler(Transform planeTransform, TrapGenerationSettings settings,
            CameraPositionService cameraPositionService, TrapPatternProvider trapPatternFactory)
        {
            _createdPatterns = new List<TrapPattern>();
            _settings = settings;
            _cameraPositionService = cameraPositionService;
            _planeTransform = planeTransform;
            _trapProvider = trapPatternFactory;
        }

        public void Tick()
        {
            if (ZoneIsReached())
            {
                TrapGenerationSettings.Zone currentZone = _settings.Zones[_currentZone];
                TrapPattern trapPattern = _trapProvider.GetPatternByDifficulty(currentZone.Difficulty);
                if (trapPattern)
                {
                    trapPattern.OnAllTrapsDeactivated += DeactivatePattern;
                    float cameraRightBorderPoint = _cameraPositionService.Camera.transform.position.x + _cameraPositionService.GetCameraHalfWidth();
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
                }

                _currentZone++;
            }
        }

        private void DeactivatePattern(TrapPattern trapPattern)
        {
            trapPattern.OnAllTrapsDeactivated -= DeactivatePattern;
            _trapProvider.ReleasePattern(trapPattern);
        }


        private bool ZoneIsReached()
        {
            if (_currentZone >= _settings.Zones.Count) return false;
            return _planeTransform.position.x >= _settings.Zones[_currentZone].XPosition;
        }
    }
}