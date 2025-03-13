using System;
using System.Collections.Generic;
using Plane;
using Services;
using Traps.TrapsGenerationLogic;
using Traps.Wind;
using UnityEngine;
using Zenject;

namespace DI
{
    public class TrapInstaller : MonoInstaller
    {
        [SerializeField] TrapGenerationSettings _settings;

        public override void InstallBindings()
        {
            Dictionary<Type, ITrapComposer> trapComposers = new Dictionary<Type, ITrapComposer>();
            trapComposers.Add
            (
                typeof(WindSpawner),
                new WindComposer(
                    Container.Resolve<CameraPositionService>(),
                    Container.Resolve<PlaneView>(),
                    Container.Resolve<PlaneController>(),
                    Container.Resolve<RedBordersOnHit>())
            );

            Container.Bind<TrapPatternProvider>().AsSingle().WithArguments(_settings.Patterns, trapComposers);
            Container.Bind<TrapGenerationSettings>().FromInstance(_settings).AsSingle();
            Container.BindInterfacesTo<StaticTrapHandler>().AsSingle();
        }
    }
}