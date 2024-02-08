using Achievements;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Achievements;
using System;
using UnityEngine;
using Zenject;

namespace DI
{
    public class AchievementGarageInstaller : MonoInstaller
    {
            [SerializeField] private AchievementPanelView _achievementPanelView;
            [SerializeField] private AchievementConfig[] _configs;
            public override void InstallBindings()
            {
                ResetProgress(); // сброс прогресса для отладки, потом удалить
                for (int i = 0; i < _configs.Length; i++)
                {
                    if (_configs[i].isDone == false)
                    {
                        var Handler = Type.GetType(_configs[i].Handler);
                       
                            Container.Bind<IAchievementHandler>().To(Handler).AsSingle().WithArguments(_configs[i]).NonLazy();
                            Debug.Log("Инициализирована ачивка: " + Handler.Name);
                        
                    }
                }

                Container.BindInstance(_achievementPanelView);
                Container
                    .BindInterfacesTo<AchievementManager>().AsSingle()
                    .WithArguments(Container.ResolveAll<IAchievementHandler>(), _achievementPanelView);

            }
            private void ResetProgress() // я заибався ручками прогресс снимать
            {
                for (int i = 0; i < _configs.Length; i++)
                {
                    _configs[i].isDone = false;
                    _configs[i].CurrentProgress = 0;
                }
            }
        }
}