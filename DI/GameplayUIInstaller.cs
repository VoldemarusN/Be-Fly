using Scriptable_Objects;
using TMPro;
using UI;
using UI.Gameplay;
using UI.LaunchForce;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace DI
{
    public class GameplayUIInstaller : MonoInstaller
    {
        [SerializeField] private LaunchConfig _launchConfig;
        [SerializeField] private LevelProgressView _levelProgressView;
        [SerializeField] private LaunchScrollView _launchScrollerView;
        [SerializeField] private HUDView _hudView;
        [SerializeField] private RedBordersOnHit _redBordersOnHit;


        public override void InstallBindings()
        {
            Container.Bind<HUDView>().FromInstance(_hudView).AsSingle();
            Container.BindInstance(_levelProgressView);
            Container.Bind<IScrollView>().To<LaunchScrollView>().FromInstance(_launchScrollerView).AsSingle();
            Container.Bind<LaunchConfig>().FromInstance(_launchConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<HUDController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LaunchScrollController>().AsSingle();
            Container.Bind<RedBordersOnHit>().FromInstance(_redBordersOnHit).AsSingle();
        }
    }
}