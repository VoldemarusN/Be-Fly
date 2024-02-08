using System;
using System.Collections.Generic;
using System.Linq;
using Achievements;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Achievements;
using UnityEngine;
using Zenject;

namespace DI
{
    public class AchievementInstaller : MonoInstaller
    {
        [SerializeField] private AchievementPanelView _achievementPanelView;
        [SerializeField] private AchievementConfig[] _configs;
        private Plane.PlaneType _currentPlaneType;
        public override void InstallBindings()
        {
            ResetProgress(); // сброс прогресса для отладки, потом удалить
            GetCurrentPlaneType();
            for (int i = 0; i < _configs.Length; i++)
            {
                if(_configs[i].isDone == false) 
                {
                var Handler = Type.GetType(_configs[i].Handler);
                    if (IsAchieveForThisPlane(_configs[i].planeTypeForAchievement))
                    {
                    Container.Bind<IAchievementHandler>().To(Handler).AsSingle().WithArguments(_configs[i]).NonLazy();
                    Debug.Log("Инициализирована ачивка: " + Handler.Name);
                    }
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
        private bool IsAchieveForThisPlane(Plane.PlaneType planeTypeInAchievement)
        {
            if(planeTypeInAchievement == _currentPlaneType || planeTypeInAchievement == Plane.PlaneType.None)
            {
                return true;
            }
            else
            {
                return false;
            }
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