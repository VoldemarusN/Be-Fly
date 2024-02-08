using System;
using DefaultNamespace.Achievements;

namespace Achievements.Handlers
{
    public abstract class AchievementHandler : IAchievementHandler
    {
        public AchievementConfig AchievementConfig { get; }
        public event Action<IAchievementHandler> OnAchievementCompleted;
        protected AchievementHandler(AchievementConfig config) => AchievementConfig = config;
        protected void OnOnAchievementCompleted(IAchievementHandler handler) => 
            OnAchievementCompleted?.Invoke(handler); //инвокатор для наследников

        public void TryToCompleteAchievement()
        {
            if (AchievementConfig.isDone) return;
            TryComplete();
        }
        protected abstract void TryComplete();
    }
}