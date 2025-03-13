using System;
using DefaultNamespace.Achievements;
using UnityEngine;

namespace Achievements.Handlers
{
    public abstract class AchievementHandler : IAchievementHandler
    {
        public AchievementConfig AchievementConfig { get; set; }
        public bool IsCompleted { get; protected set; }
        public event Action<IAchievementHandler> OnAchievementCompleted;
        protected AchievementHandler(AchievementConfig config) => AchievementConfig = config;

        protected void OnOnAchievementCompleted(IAchievementHandler handler)
        {
            if (IsCompleted) return;

            Debug.Log("OnAchievementCompleted: " + AchievementConfig.AchieveName + "(" +
                      handler.AchievementConfig.Description + ")");
            OnAchievementCompleted?.Invoke(handler);
            IsCompleted = true;
        }

        public void TryToCompleteAchievement() => TryComplete();
        protected abstract void TryComplete();
    }
}