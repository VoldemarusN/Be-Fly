using System;
using Achievements.Handlers;
using Plane;
using UnityEngine;

namespace DefaultNamespace.Achievements
{
    public class BarrelAchievementHandler : AchievementHandler
    {
        private readonly PlaneView _planeView;
        private bool _firstStepActivated = false;

        public BarrelAchievementHandler(AchievementConfig achievementConfig, PlaneView planeView) : base(achievementConfig)
        {
            _planeView = planeView;
        }
        protected override void TryComplete()
        {
            CheckBarrel();
            if (_firstStepActivated)
            {
                CheckPlaneEndedBarrel();
            }
        }
        private void CheckBarrel()
        {
            if(_planeView.IsCrashed == false && _planeView.transform.right is { x: < -0.1f, y: < -0.9f })
            {
                _firstStepActivated = true;
            }
        }
        private void CheckPlaneEndedBarrel()
        {
            if (_planeView.IsCrashed == false && _planeView.transform.right is { x: >0.2f, y: > -0.5f })
            {
                OnOnAchievementCompleted(this);
            }
        }
    }
}