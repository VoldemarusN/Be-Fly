using Plane;
using UnityEngine;
namespace Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        PlaneView GetPlaneView(PlaneType planeType);
    }
}
