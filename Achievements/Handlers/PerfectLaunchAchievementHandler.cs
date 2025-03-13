using System;
using Scriptable_Objects;
using UI;
using UnityEngine;

namespace Achievements.Handlers
{
    public class PerfectLaunchAchievementHandler : AchievementHandler
    {
        private UI.LaunchForce.LaunchScrollController _launchScrollController;
        private readonly LaunchConfig _config;


        public PerfectLaunchAchievementHandler(AchievementConfig achievementConfig,
            UI.LaunchForce.LaunchScrollController launchScrollController,
            LaunchConfig config)
            : base(achievementConfig)
        {
            _launchScrollController = launchScrollController;
            _config = config;
            _launchScrollController.OnLaunched += CheckPerfectLaunch;
        }

        private void CheckPerfectLaunch(float value)
        {
            _launchScrollController.OnLaunched -= CheckPerfectLaunch;

            if (value > _config.BoostEffectRange.Min && value < _config.BoostEffectRange.Max)
                OnOnAchievementCompleted(this);
        }

        protected override void TryComplete() // вся логика в CheckPerfectLaunch т.к. запуск производится в одном кадре
        {
        }
    }
}