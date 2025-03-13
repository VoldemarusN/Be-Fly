using System;
using Services;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public static class Extensions
    {
        public static string ToSceneName(this SceneType sceneType)
        {
            switch (sceneType)
            {
                case SceneType.MainMenu:
                    return "MainMenu";
                case SceneType.Level1:
                    return "Level 1";
                case SceneType.Level2:
                    return "Level 2";
                case SceneType.Level3:
                    return "Level 3";
                case SceneType.LevelMenu:
                    return "Level Menu";
                case SceneType.Garage:
                    return "Garage";
                case SceneType.Developers:
                    return "Developers";
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, "Unknown scene type");
            }
        }
    }
}