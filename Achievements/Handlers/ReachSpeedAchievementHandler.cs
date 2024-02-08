using System;
using UI;
using UnityEngine;

namespace Achievements.Handlers
{
    public class ReachSpeedAchievementHandler : AchievementHandler
    {
        private float _reachSpeed;
        private Plane.PlaneView _planeView;

        public ReachSpeedAchievementHandler(AchievementConfig achievementConfig, Plane.PlaneView planeView, Plane.IPlaneMovement planeMovement)
            : base(achievementConfig)
        {
            _planeView = planeView;
            _reachSpeed = achievementConfig.ValueToReach;
        }
        protected override void TryComplete()
        {
            if (_planeView.Speed >= _reachSpeed)
            {
                OnOnAchievementCompleted(this);
            }
        }
    }

}
