using System;
using UI;
using UnityEngine;

namespace Achievements.Handlers
{
    public class CompleteLevelAchievementHandler : AchievementHandler
    {
        protected float _reachHeight;
        private LevelController _levelController;

        public CompleteLevelAchievementHandler(AchievementConfig achievementConfig, LevelController levelController)
            : base(achievementConfig)
        {
            _reachHeight = achievementConfig.ValueToReach;
            _levelController = levelController;
            _levelController.OnWin += CompleteAchievement;
        }
        private void CompleteAchievement()
        {
            OnOnAchievementCompleted(this);
        }
        protected override void TryComplete()
        {            
        }
    }

}
