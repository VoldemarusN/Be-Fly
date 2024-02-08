using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Scriptable_Objects;
using Services.Input;
using UI.LaunchForce;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Plane
{
    public class PlaneLauncher : IDisposable
    {
        private readonly InputService _inputService;
        private readonly PlaneController _planeController;
        private readonly LaunchScrollController _launchScrollController;
        private readonly Blur _blur;
        private readonly LaunchConfig _launchConfig;
        private UniTaskVoid _uniTask;


        public PlaneLauncher(InputService inputService, PlaneController planeController,
            LaunchScrollController launchScrollViewController, Blur blur, LaunchConfig launchConfig)
        {
            _inputService = inputService;
            _inputService.Initialize();
            _planeController = planeController;
            _launchScrollController = launchScrollViewController;
            _blur = blur;
            _launchConfig = launchConfig;
            _inputService.Launch.performed += Launch;
        }

        private void Launch(InputAction.CallbackContext _)
        {
            _inputService.Launch.performed -= Launch;
            _planeController.Launch();
            _launchScrollController.Complete();
            _planeController.AddForce(_launchScrollController.Force);
            if (_launchScrollController.IsScrollInBustZone()) _uniTask = Bust();
        }

        private async UniTaskVoid Bust()
        {
            _blur.ToggleBlur();
            float timer = _launchConfig.BoostDuration;
            while (timer > 0)
            {
                _planeController.AddForce(_launchConfig.BoostForce);
                timer -= Time.deltaTime;
                await UniTask.Yield();
            }

            _blur.ToggleBlur();
        }

        public void Dispose()
        {
            _uniTask.Forget();
            _inputService.Dispose();
        }
    }
}