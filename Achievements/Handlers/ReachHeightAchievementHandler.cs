using System;
using UI;
using UnityEngine;

namespace Achievements.Handlers
{
    public class ReachHeightAchievementHandler : AchievementHandler
    {
        protected float _reachHeight;
        private Transform _planeController;

        public ReachHeightAchievementHandler(AchievementConfig achievementConfig, LevelController levelController, Plane.PlaneController planeController)
            : base(achievementConfig)
        {
            _planeController = planeController.View.transform;
            _reachHeight = achievementConfig.ValueToReach;
        }
        protected override void TryComplete()
        {
            if (_planeController.position.y >= _reachHeight)
            {
                OnOnAchievementCompleted(this);
            }
        }
    }

}

