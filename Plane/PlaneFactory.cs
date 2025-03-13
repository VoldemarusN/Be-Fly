using System.Collections.Generic;
using DI;
using Infrastructure.AssetManagement;
using Plane.PlaneData;
using SavingSystem;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;

namespace Plane
{
    public class PlaneFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly Dictionary<PlaneType, PlaneConfig> _configs;

        public PlaneFactory(IAssetProvider assetProvider, Dictionary<PlaneType, PlaneConfig> configs)
        {
            _assetProvider = assetProvider;
            _configs = configs;
        }

        public PlaneView BuildPlaneView(PlaneType planeType, Vector2 spawnPosition, float angle, int level)
        {
            PlaneView prefabView = _assetProvider.GetPlaneView(planeType);
            PlaneView planeViewInstance = Object.Instantiate(prefabView, spawnPosition, Quaternion.identity);
            planeViewInstance.SetLevel(level);

            planeViewInstance.transform.right = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
            return planeViewInstance;
        }
    }
}