using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Plane;
using Plane.PlaneData;
using UnityEngine;
using Zenject;

namespace DI
{
    public class GlobalProviderInstaller : MonoInstaller
    {
        [SerializeField] private PlaneConfig[] _configs;

        public override void InstallBindings()
        {
            Dictionary<PlaneType, PlaneConfig> planeConfigs = new Dictionary<PlaneType, PlaneConfig>();
            foreach (var config in _configs) planeConfigs[config.Type] = config;
            Container.Bind<Dictionary<PlaneType, PlaneConfig>>().FromInstance(planeConfigs).AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().FromComponentOn(gameObject).AsSingle();
        }
    }
}