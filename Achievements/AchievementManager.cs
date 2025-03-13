using System;
using System.Collections.Generic;
using DefaultNamespace.Achievements;
using SavingSystem;
using UnityEngine;
using Zenject;
#if !PLATFORM_ANDROID
using Steamworks.Data;
#endif

namespace Achievements
{
    public class AchievementManager : ITickable
    {
        private readonly List<IAchievementHandler> _handlers;
        private readonly AchievementPanelView _achievementPanelView;
        private readonly AudioManager _audioManager;
        private readonly Storage _storage;

        public AchievementManager(List<IAchievementHandler> handlers, AchievementPanelView achievementPanelView,
            AudioManager audioManager, Storage storage)
        {
            _handlers = handlers;
            for (int i = 0; i < handlers.Count; i++)
            {
                _handlers[i].OnAchievementCompleted += TryCompleteAchievement;
            }

            _achievementPanelView = achievementPanelView;
            _audioManager = audioManager;
            _storage = storage;
        }

        public void Tick()
        {
            foreach (var handler in _handlers) handler.TryToCompleteAchievement();
        }

        private void TryCompleteAchievement(IAchievementHandler handler)
        {
            AchievementConfig config = handler.AchievementConfig;
            IncrementAchievementProgress(config);
            if (_storage.AchievementIsDone(config))
            {
                _achievementPanelView.SetupPanelAndShow(config, _storage.GetAchievementProgress(config));
                handler.OnAchievementCompleted -= TryCompleteAchievement;
#if STEAM_INTEGRATION

                var ach = new Achievement(config.AchivementId);
                ach.Trigger();
#else
                _audioManager.PlayOneShotEffect(_audioManager.AchievementSound);
#endif
            }
        }

        private void IncrementAchievementProgress(AchievementConfig config) =>
            _storage.StorageData.AchievementProgresses[config.name]++;
    }
}