using System;
using UI;
using UnityEngine;

namespace Achievements.Handlers
{
    public class PerfectLaunchAchievementHandler : AchievementHandler
    {
        private UI.LaunchForce.LaunchScrollController _launchScrollController;


        public PerfectLaunchAchievementHandler(AchievementConfig achievementConfig, UI.LaunchForce.LaunchScrollController launchScrollController)
            : base(achievementConfig)
        {
            _launchScrollController = launchScrollController;
            _launchScrollController.OnLaunched += CheckPerfectLaunch;
        }
        private void CheckPerfectLaunch(float value)
        {
            _launchScrollController.OnLaunched -= CheckPerfectLaunch;

            if (value > 0.4 && value < 0.6)
            {
                OnOnAchievementCompleted(this);

            }
        }
        protected override void TryComplete() // вся логика в CheckPerfectLaunch т.к. запуск производится в одном кадре
        {
        }
    }
}
