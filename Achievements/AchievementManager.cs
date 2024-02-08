using System;
using System.Collections.Generic;
using DefaultNamespace.Achievements;
using UnityEngine;
using Zenject;

namespace Achievements
{
    public class AchievementManager : ITickable
    {
        private readonly List<IAchievementHandler> _handlers;
        private readonly AchievementPanelView _achievementPanelView;
        private readonly AudioManager _audioManager;

        public AchievementManager(List<IAchievementHandler> handlers, AchievementPanelView achievementPanelView, AudioManager audioManager)
        {
            _handlers = handlers;
            for (int i = 0; i < handlers.Count; i++)
            {
                _handlers[i].OnAchievementCompleted += TryCompleteAchievement;
            }

            _achievementPanelView = achievementPanelView;
            _audioManager = audioManager;
        }

        public void Tick()
        {
            foreach (var handler in _handlers) handler.TryToCompleteAchievement();
        }

        private void TryCompleteAchievement(IAchievementHandler handler)
        {
            AchievementConfig config = handler.AchievementConfig;
            config.CurrentProgress++;
            if (config.CurrentProgress >= config.MaxProgress)
            {
                config.isDone = true;
                _achievementPanelView.SetupPanelAndShow(config);
                handler.OnAchievementCompleted -= TryCompleteAchievement;
                _audioManager.PlayOneShotEffect(_audioManager.AchievementSound);
            }
        }
    }
}