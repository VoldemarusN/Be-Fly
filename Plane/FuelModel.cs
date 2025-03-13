using System;
using Services.Input;
using UI.Speedometer;

namespace Plane
{
    [Serializable]
    public class FuelModel : IUpdatable
    {
        public float NormalizedFuel => Fuel / _startFuel;
        public float Fuel { get; private set; }

        private float _fuelCost;
        private float _startFuel;

        private readonly InputService _inputService;

        public FuelModel(float fuelCost, float startFuel, InputService inputService)
        {
            _inputService = inputService;
            _fuelCost = fuelCost;
            _startFuel = startFuel;
            Fuel = _startFuel;
        }

        public void Tick()
        {
            if (Fuel <= 0)
            {
                Fuel = 0;
                return;
            }

            if (_inputService.HoldOnScreen.IsPressed()) Fuel -= _fuelCost;
        }
    }
}