using System;
using UI;
using UnityEngine;

namespace Achievements.Handlers
{
    public class ReachDistanceAchievementHandler : AchievementHandler
    {
        private readonly LevelController _levelController;
        protected float _reachDistance;
        protected Plane.PlaneView _planeView;


        public ReachDistanceAchievementHandler(AchievementConfig achievementConfig, LevelController levelController,
            Plane.PlaneView planeView)
            : base(achievementConfig)
        {
            _levelController = levelController;
            _planeView = planeView;
            _reachDistance = achievementConfig.ValueToReach;
        }

        protected override void TryComplete()
        {
            if (_levelController.PassedDistance >= _reachDistance)
            {
                OnOnAchievementCompleted(this);
            }
        }
    }
}