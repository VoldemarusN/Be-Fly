using System;
using System.Collections.Generic;
using System.Linq;
using Achievements;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Achievements;
using SavingSystem;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

namespace DI
{
    public class AchievementInstaller : MonoInstaller
    {
        [SerializeField] private AchievementPanelView _achievementPanelView;
        [SerializeField] private AchievementConfig[] _configs;
        private Plane.PlaneType _currentPlaneType;

        public override void InstallBindings()
        {
            var storage = Container.Resolve<Storage>();
            GetCurrentPlaneType();
            foreach (var config in _configs)
            {
                if (storage.AchievementIsDone(config)) continue;

                var handler = Type.GetType(config.Handler);
                if (IsAchieveForThisPlane(config.planeTypeForAchievement) && CheckCorrectScene(config))
                {
                    Container.Bind<IAchievementHandler>().To(handler).AsSingle().WithArguments(config).NonLazy();
                    Debug.Log("Инициализирована ачивка: " + handler.Name);
                }
            }

            Container.BindInstance(_achievementPanelView);
            Container
                .BindInterfacesTo<AchievementManager>().AsSingle()
                .WithArguments(Container.ResolveAll<IAchievementHandler>(), _achievementPanelView);
        }

        private void GetCurrentPlaneType()
        {
            Plane.PlaneView plane = Container.Resolve<Plane.PlaneView>();
            _currentPlaneType = plane.Type;
        }
        private bool CheckCorrectScene(AchievementConfig config)
        {
            if (config.Scene == "MainMenu")
            {
                return true;
            }
            if (config.Scene == SceneManager.GetActiveScene().name)
            {
                return true;
            }
            return false;
        }
        private bool IsAchieveForThisPlane(Plane.PlaneType planeTypeInAchievement)
        {
            if (planeTypeInAchievement == _currentPlaneType || planeTypeInAchievement == Plane.PlaneType.None)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}