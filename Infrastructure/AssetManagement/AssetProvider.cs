using System.Collections.Generic;
using System.Linq;
using Plane;
using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public class AssetProvider : MonoBehaviour, IAssetProvider
    {
        [SerializeField] private PlaneView[] _planeViews;
        public PlaneView GetPlaneView(PlaneType planeType) =>
            _planeViews.FirstOrDefault(x => x.Type == planeType);
    }
}