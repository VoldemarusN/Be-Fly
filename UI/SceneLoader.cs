using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UI.LaunchForce;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Services
{
    public class SceneLoader : IService
    {
        private IHideable _curtain;


        public SceneLoader(IHideable curtain)
        {
            _curtain = curtain;
        }


        public async UniTaskVoid LoadScene(SceneType scene)
        {
            await _curtain.Show();
            await SceneManager.LoadSceneAsync(scene.ToSceneName());
            _curtain.Hide();
        }

        public async UniTaskVoid LoadScene(string name)
        {
            await _curtain.Show();
            await SceneManager.LoadSceneAsync(name);
            _curtain.Hide();
        }
    }

    public enum SceneType
    {
        MainMenu,
        LevelMenu,
        Level1,
        Level2,
        Level3,
        Garage,
        Developers
    }
}