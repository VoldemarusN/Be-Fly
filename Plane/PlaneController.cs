using System;
using Services.Input;
using UI.LaunchForce;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;


namespace Plane
{
    public class PlaneController : IFixedTickable, IDisposable
    {
        public PlaneView View => _view;
        private readonly IPlaneMovement _planeMovement;
        private readonly PlaneView _view;
        private readonly Oil _planeOil;
        private readonly AudioManager _audioManager;
        private Vector3 _startCenterOfMass;
        private InputService _inputService;
        private PlaneState _planeState;


        public PlaneController(IPlaneMovement planeMovement, PlaneView view,
            InputService inputService, Oil planeOil, AudioManager audioManager)
        {
            _planeOil = planeOil;
            _audioManager = audioManager;
            _planeMovement = planeMovement;
            _view = view;
            _view.OnTouchedGround += Ragdoll;
            _planeOil.OnOilEnded += () =>
            {
                DisposeInput();
                _planeState = PlaneState.Falling;
                _view.StopPlane();
            };
            _view.OnSlowed += Slow;
            _view.OnSlowed += PlayCollisionEffect;

            InitInput(inputService);
            if (view.Type != PlaneType.Paper)
                planeOil.OnOilEnded += Ragdoll;
        }

        public void Slow(float force) =>
            _planeMovement.AddForceFromTrap(CalculateSlowForce(force));

        public void AddForce(float force) => _planeMovement.AddForce(force);

        public void Launch()
        {
            _planeOil.Start();
            _planeState = PlaneState.Launched;
            _planeMovement.EnablePhysic();
        }

        private float CalculateSlowForce(float force)
        {
            return -force;
        }

        private void PlayCollisionEffect(float _)
        {
            _audioManager.PlayOneShotEffect(_view.TouchTrapSound);
        }


        // удары - 0 соурс

        // пропеллер - 1 соурс


        private void Ragdoll()
        {
            if (_audioManager.GetEffectSource(0).isPlaying == false)
                _audioManager.PlayRandomEffect(_view.TouchGroundSounds);

            if (_planeState == PlaneState.Ragdolling) return;
            _audioManager.StopEffect(1);
            DisposeInput();
            _planeState = PlaneState.Ragdolling;
            _planeMovement.Ragdoll();
            _view.StopPlane();
            _view.SetTransparent();
        }


        public void FixedTick()
        {
            if (_planeState == PlaneState.Ragdolling) return;
            if (_inputService.HoldOnScreen.IsPressed() && _planeState == PlaneState.Launched)
            {
                _planeMovement.ActiveMove();
                _view.StartPropeller();
                if (_audioManager.GetEffectSource(1).isPlaying == false)
                    _audioManager.PlayEffect(_view.ActiveSound, 1);
            }
            else
            {
                if (_audioManager.GetEffectSource(1).isPlaying)
                    _audioManager.StopEffect(1);
                _planeMovement.PassiveMove();
                _view.StopPropeller();
            }

            _planeMovement.ClampPlaneSpeed();
        }

        private void DisposeInput()
        {
            _inputService.HoldOnScreen.performed -= HoldOnScreen_Changed;
            _inputService.HoldOnScreen.canceled -= HoldOnScreen_Changed;
            _inputService.Dispose();
        }

        private void InitInput(InputService inputService)
        {
            _inputService = inputService;
            _inputService.HoldOnScreen.performed += HoldOnScreen_Changed;
            _inputService.HoldOnScreen.canceled += HoldOnScreen_Changed;
        }

        private void HoldOnScreen_Changed(InputAction.CallbackContext obj)
        {
            _planeMovement.ResetAngularSpeed();
        }


        //  private void ViewWasSlowed(float force) => _planeMovement.AddForce(force, -_view.transform.right);

        public void Dispose()
        {
            DisposeInput();
            _inputService.Dispose();
        }
    }

    internal enum PlaneState
    {
        Launched,
        Falling,
        Ragdolling
    }
}