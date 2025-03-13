using Plane;
using Plane.PlaneData;
using Scriptable_Objects;
using Services;
using Traps.TrapsGenerationLogic;
using UI;
using UI.Gameplay;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.DI
{
    public class GameplaySubsystemInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private LooseWindow _gameOverWindow;
        [SerializeField] private WinWindow _winWindow;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelConfig>().FromInstance(_levelConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<Oil>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle().WithArguments(_levelConfig, _gameOverWindow, _winWindow);

            Container.BindInterfacesAndSelfTo<TrapPatternHandler>().AsSingle();
            Container.Bind<CameraPositionService>().AsSingle();
            Container.Bind<AppodealAddShower>().AsSingle();
        }
    }
}