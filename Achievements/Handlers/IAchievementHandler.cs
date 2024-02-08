using System;

namespace DefaultNamespace.Achievements
{
    public interface IAchievementHandler
    {
        AchievementConfig AchievementConfig { get; }
        public event Action<IAchievementHandler> OnAchievementCompleted;
        void TryToCompleteAchievement();
    }
}