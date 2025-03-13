using System;
using UI;
using UnityEngine;

namespace Achievements.Handlers
{

    public class BackwardMoveAchievementHandler : AchievementHandler
    {
        private readonly LevelController _levelController;
        protected float _reachDistance;
        protected Plane.PlaneView _planeView;
        private float _currentReachedDistance;
        private float? StartedCoordinate;
        public BackwardMoveAchievementHandler(AchievementConfig achievementConfig, LevelController levelController, Plane.PlaneView planeView)
             : base(achievementConfig)
        {
            _levelController = levelController;
            _planeView = planeView;
            _reachDistance = achievementConfig.ValueToReach;
        }
        private void CountingMetres()
        {

            if (StartedCoordinate == null)
            {
                StartedCoordinate = _planeView.transform.position.x;
            }
            else
            {
                float Distance = _planeView.transform.position.x - (float)StartedCoordinate;
                if(Math.Abs(Distance) >= _reachDistance)
                {
                    OnOnAchievementCompleted(this);

                }
            }
        }
        protected override void TryComplete()
        {
            if (_planeView.IsCrashed == false && _planeView.transform.right is { x: < 0})
            {
                CountingMetres();
            }
            else
            {
                StartedCoordinate = null;
            }
        }
    }
}
