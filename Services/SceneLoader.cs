using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Services
{
    public class SceneLoader : IService
    {
        public void LoadScene(SceneType scene)
        {
            string name;

            switch (scene)
            {
                case SceneType.MainMenu:
                    name = "MainMenu";
                    break;
                case SceneType.Level1:
                    name = "Level 1";
                    break;
                case SceneType.Level2:
                    name = "Level 2";
                    break;
                case SceneType.Level3:
                    name = "Level 3";
                    break;
                case SceneType.LevelMenu:
                    name = "Level Menu";
                    break;
                case SceneType.Garage:
                    name = "Garage";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
            }


            SceneManager.LoadScene(name);
        }

        public void LoadLevelScene(int i)
        {
            switch (i)
            {
                case 0:
                    LoadScene(SceneType.Level1);
                    break;
                case 1:
                    LoadScene(SceneType.Level2);
                    break;
                case 2:
                    LoadScene(SceneType.Level3);
                    break;
            }
        }
    }

    public enum SceneType
    {
        MainMenu,
        LevelMenu,
        Level1,
        Level2,
        Level3,
        Garage
    }
}