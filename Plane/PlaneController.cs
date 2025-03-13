using System;
using DG.Tweening;
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

        public bool DestinateTorqueToRightDir
        {
            get => _planeMovement.DestinateTorqueToRightDir;
            set => _planeMovement.DestinateTorqueToRightDir = value;
        }

        private readonly IPlaneMovement _planeMovement;
        private readonly PlaneView _view;
        private readonly Oil _planeOil;
        private readonly AudioManager _audioManager;
        private Vector3 _startCenterOfMass;
        private InputService _inputService;
        private PlaneState _planeState = PlaneState.Stopped;
        private Tween _activeSoundGrowTween;
        private int _propellerSoundLayer;

        public PlaneController(IPlaneMovement planeMovement, PlaneView view,
            InputService inputService, Oil planeOil, AudioManager audioManager)
        {
            _propellerSoundLayer = 1;
            _planeOil = planeOil;
            _audioManager = audioManager;
            _planeMovement = planeMovement;
            _view = view;
            _inputService = inputService;
        }

        public void Slow(float force)
        {
            _planeMovement.AddForceFromTrap(CalculateSlowForce(force));
        }


        public void AddForce(float force) => _planeMovement.AddForce(force);

        public void Launch()
        {
            _view.OnTouchedGround += Ragdoll;
            _view.OnTouchedGround += PlayGroundTouchMusic;
            _planeOil.OnOilEnded += () =>
            {
                _planeState = PlaneState.Falling;
                _view.StopPlane();
                DisableInput();
            };
            _view.OnSlowed += Slow;
            _view.OnSlowed += PlayCollisionEffect;

            _audioManager.GetEffectSource(_propellerSoundLayer).volume = 0;
            _inputService.HoldOnScreen.performed += HoldOnScreen_Changed;
            _inputService.HoldOnScreen.canceled += HoldOnScreen_Changed;
            _planeState = PlaneState.Launched;
            _planeMovement.EnablePhysic();
        }

        private float CalculateSlowForce(float force)
        {
            return -force;
        }

        private void PlayCollisionEffect(float _)
        {
            _audioManager.PlayOneShotEffect(_view.TouchTrapSound, _view.TrapPitchVariance);
        }


        // удары - 0 соурс
        // пропеллер - 1 соурс


        private void Ragdoll()
        {
            DisableInput();
            ProceedPropellerSound(false);
            _view.OnTouchedGround -= Ragdoll;
            _planeState = PlaneState.Ragdolling;

            _audioManager.StopEffect(1);
            _planeMovement.Ragdoll();
            _view.StopPlane();
            _view.SetTransparent();
        }


        private void PlayGroundTouchMusic()
        {
            if (_audioManager.GetEffectSource(0).isPlaying == false)
                _audioManager.PlayRandomEffect(_view.TouchGroundSounds);
        }

        public void FixedTick()
        {
            if (_planeState == PlaneState.Falling)
            {
                ProcessPassive();
                _planeMovement.ClampPlaneSpeed();
            }
            if (_planeState != PlaneState.Launched) return;

            if (_inputService.HoldOnScreen.IsPressed())
                ProcessActive();
            else
                ProcessPassive();

            _planeMovement.ClampPlaneSpeed();
        }

        private void ProcessActive()
        {
            _planeMovement.ActiveMove();
            _view.StartPropeller();
            _planeOil.ProcessOil();
            ProceedPropellerSound(isActive: true);
        }

        private void ProcessPassive()
        {
            _planeMovement.PassiveMove();
            _view.StopPropeller();
            ProceedPropellerSound(isActive: false);
        }


        private bool _propellerIsActive;

        private void ProceedPropellerSound(bool isActive)
        {
            if (_propellerIsActive == isActive)
                return;
            _activeSoundGrowTween?.Kill();

            if (isActive)
            {
                _audioManager.PlayEffect(_view.ActiveSound, _propellerSoundLayer);
                _activeSoundGrowTween = DOVirtual.Float(
                    _audioManager.GetEffectSource(_propellerSoundLayer).volume,
                    _audioManager.GetStartEffectSourceVolume(_propellerSoundLayer),
                    _view.ActiveSoundGrowDuration, value => { _audioManager.GetEffectSource(_propellerSoundLayer).volume = value; });
            }
            else
            {
                _activeSoundGrowTween = DOVirtual.Float(
                    _audioManager.GetEffectSource(_propellerSoundLayer).volume,
                    0,
                    _view.ActiveSoundGrowDuration,
                    value => { _audioManager.GetEffectSource(_propellerSoundLayer).volume = value; });

                _activeSoundGrowTween.onComplete += () => { _audioManager.StopEffect(_propellerSoundLayer); };
            }

            _propellerIsActive = isActive;
        }

        private void HoldOnScreen_Changed(InputAction.CallbackContext _)
        {
            _planeMovement.ResetAngularSpeed();
        }

        public void DisableInput()
        {
            _inputService.HoldOnScreen.performed -= HoldOnScreen_Changed;
            _inputService.HoldOnScreen.canceled -= HoldOnScreen_Changed;
        }


        public void Dispose() => DisableInput();
    }

    internal enum PlaneState
    {
        Stopped = 0,
        Launched,
        Falling,
        Ragdolling
    }
}