using System;
using Plane.PlaneData;
using Services.Input;
using UnityEngine;
using Zenject;

namespace Plane
{
    public class Oil
    {
        public float OilNormalized => _oilAmount / _data.Amount;
        private float _oilAmount;
        public event Action OnOilEnded;
        private readonly OilData _data;
        private readonly InputService _inputService;
        private bool _started;

        public Oil(OilData data, InputService inputService)
        {
            _data = data;
            _inputService = inputService;
            _oilAmount = data.Amount;
        }


        public void ProcessOil()
        {
            _oilAmount -= 0.2f * Time.deltaTime;
            if (OilNormalized <= 0)
            {
                _oilAmount = 0;
                _started = false;
                OnOilEnded?.Invoke();
            }
        }
    }
}