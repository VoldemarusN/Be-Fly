using Plane;
using UnityEngine;
using Zenject;

namespace UI.Gameplay
{
    public class HUDController : ITickable
    {
        private readonly HUDView _hudView;
        private readonly IPlaneMovement _planeMovement;
        private readonly Oil _oil;

        public HUDController(HUDView hudView, Oil oil, IPlaneMovement movement, PlaneView planeView)
        {
            _hudView = hudView;
            _planeMovement = movement;
            _oil = oil;

            planeView.OnTouchedGround += ActivateHeightIndicator;
            _hudView.SetOilIndicatorActivity(false);
            _hudView.SetCrashIndicatorActivity(false);
            
        }

        public void Tick()
        {
            if (Time.timeScale == 0) return;

            _hudView.SetOil(_oil.OilNormalized);
            _hudView.SetOilIndicatorActivity(_oil.OilNormalized < 0.2f);


            _hudView.SetSpeed(_planeMovement.VelocityMagnitude);
        }

        private void ActivateHeightIndicator() => _hudView.SetCrashIndicatorActivity(true);

        private void PingOilIndicator() => _hudView.SetOilIndicatorActivity(true);
    }
}