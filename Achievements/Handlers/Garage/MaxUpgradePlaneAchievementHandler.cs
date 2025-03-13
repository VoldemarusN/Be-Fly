using DI;
using System;
using System.Collections.Generic;
using UI;
using UI.Garage;
using UnityEngine;

namespace Achievements.Handlers
{
    public class MaxUpgradePlaneAchievementHandler : AchievementHandler
    {
        List<AchievementConfig> ConfigsWithSameHandler;
        private PassiveUpgradeController _passiveUpgradeController;
        private AchievementGarageInstaller _achievementGarageInstaller;
        private Plane.PlaneType _requiredPlaneType;
        public MaxUpgradePlaneAchievementHandler(AchievementConfig achievementConfig, UI.Garage.PassiveUpgradeController passiveUpgradeController)
            : base(achievementConfig)
        {
            _passiveUpgradeController = passiveUpgradeController;
            _passiveUpgradeController.AchievementMaxUpgradePlaneIsDone += MaxLevelUp;
            _requiredPlaneType = achievementConfig.planeTypeForAchievement;


        }
        private void MaxLevelUp(Plane.PlaneType planeType, int currentProgress, int MaxProgress)
        {
            if (currentProgress == MaxProgress && planeType == _requiredPlaneType)
            { 
                OnOnAchievementCompleted(this);
            }        
        }
        protected override void TryComplete()
        {
        }
    }
}