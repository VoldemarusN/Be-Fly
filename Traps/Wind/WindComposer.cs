using Infrastructure.AssetManagement;
using Plane;
using Services;
using UnityEngine;
using Zenject;

namespace Traps.Wind
{
    public class WindComposer : ITrapComposer
    {
        private readonly CameraPositionService _cameraPositionService;
        private readonly PlaneView _planeView;
        private readonly PlaneController _planeController;

        public WindComposer(CameraPositionService cameraPositionService, PlaneView planeView, PlaneController planeController)
        {
            _cameraPositionService = cameraPositionService;
            _planeView = planeView;
            _planeController = planeController;
        }


        public void ComposeTrap(BaseTrap trap) =>
            (trap as WindSpawner)?.Compose(_cameraPositionService, _planeView, _planeController);
    }
}