using Achievements;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Achievements;
using System;
using SavingSystem;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using Achievements.Handlers;

namespace DI
{
    public class AchievementGarageInstaller : MonoInstaller
    {
        [SerializeField] private AchievementPanelView _achievementPanelView;
        [SerializeField] public AchievementConfig[] _configs;
        public override void InstallBindings()
        {
            var storage = Container.Resolve<Storage>();
            foreach (var config in _configs)
            {
                if (storage.AchievementIsDone(config)) continue;
                
                var handler = Type.GetType(config.Handler);
                
                    //Container.Bind<IAchievementHandler>().To(handler).AsSingle().WithArguments(config).NonLazy();
                    Container.Bind<IAchievementHandler>().To(handler).AsTransient().WithArguments(config).NonLazy();
                    Debug.Log("Инициализирована ачивка: " + handler.Name);        
            }

            Container.BindInstance(_achievementPanelView);
            Container
                .BindInterfacesTo<AchievementManager>().AsSingle()
                .WithArguments(Container.ResolveAll<IAchievementHandler>(), _achievementPanelView);
        }
    }
}