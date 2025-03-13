using System;
using UI;
using UnityEngine;

namespace Achievements.Handlers
{
    public class WinWithoutTrapsAchievementHandler : AchievementHandler
    {
        private Plane.PlaneView _planeView;
        private bool _isTrapHited;
        private LevelController _levelController;

        public WinWithoutTrapsAchievementHandler(AchievementConfig achievementConfig, Plane.PlaneView planeView, LevelController levelController)
            : base(achievementConfig)
        {
            _planeView = planeView;
            _planeView.OnSlowed += SetFlagToFail;
            _levelController = levelController;
            _levelController.OnWin += CompleteOnWin;
        }
        private void SetFlagToFail(float deleteThis)
        {
            _isTrapHited = true;
        }
        private void CompleteOnWin()
        {
            if(_isTrapHited == false) 
            {
                OnOnAchievementCompleted(this);
            }
        }
        protected override void TryComplete()
        {
           
        }
    }

}