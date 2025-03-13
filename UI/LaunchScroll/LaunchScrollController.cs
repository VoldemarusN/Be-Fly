using Scriptable_Objects;
using System;
using UnityEngine;
using Zenject;

namespace UI.LaunchForce
{
    public class LaunchScrollController : ITickable
    {
        public float Force => _launchConfig.CurveOfSpeed.Evaluate(Value) * _launchConfig.Force;
        public float Value { get; private set; }

        private bool _isStopped;
        private bool _isUp;
        private readonly LaunchConfig _launchConfig;
        private readonly IScrollView _scrollView;
        public Action<float> OnLaunched;

        public LaunchScrollController(LaunchConfig launchConfig, IScrollView scrollView)
        {
            _launchConfig = launchConfig;
            _scrollView = scrollView;
        }

        public void Tick()
        {
            if (Time.timeScale  == 0) return;
            if (_isStopped) return;
            float additiveValue = _launchConfig.CurveOfSpeed.Evaluate(Value) 
                                  * _launchConfig.HandleSpeed * Time.deltaTime;

            if (_isUp == false) additiveValue *= -1;

            if (Value > 1) _isUp = false;
            if (Value < 0) _isUp = true;

            Value += additiveValue;

            _scrollView.SetValue(Value);
        }

        public void Complete()
        {
            OnLaunched?.Invoke(Value);
            _isStopped = true;
            (_scrollView as IHideable)?.Hide();
        }

        public bool IsScrollInBustZone() =>
            Value >= _launchConfig.BoostEffectRange.Min && Value <= _launchConfig.BoostEffectRange.Max;
    }
}