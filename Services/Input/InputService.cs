using System;
using Player.Input;
using UnityEngine.InputSystem;
using Zenject;

namespace Services.Input
{
    public class InputService
    {
        public InputAction Launch => _inputActions.Player.Launch;
        public InputAction HoldOnScreen => _inputActions.Player.Hold;
        private readonly InputActions _inputActions;

        public InputService() => _inputActions = new InputActions();
        public void Dispose() => _inputActions.Player.Disable();
        public void Initialize() => _inputActions.Player.Enable();
    }
}