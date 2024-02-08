using System;
using Achievements.Handlers;
using Plane;
using UnityEngine;

namespace DefaultNamespace.Achievements
{
    public class BarrelAchievementHandler : AchievementHandler
    {
        private readonly PlaneView _planeView;

        public BarrelAchievementHandler(AchievementConfig achievementConfig, PlaneView planeView) : base(achievementConfig)
        {
            _planeView = planeView;
        }
        protected override void TryComplete()
        {
            if (_planeView.IsCrashed == false && _planeView.transform.right is { x: < -0.7f, y: < -0.7f }) 
                OnOnAchievementCompleted(this);
        }
    }
}