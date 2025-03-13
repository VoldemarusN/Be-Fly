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
        private readonly RedBordersOnHit _redBordersOnHit;

        public WindComposer(CameraPositionService cameraPositionService, PlaneView planeView, PlaneController planeController, RedBordersOnHit redBordersOnHit)
        {
            _cameraPositionService = cameraPositionService;
            _planeView = planeView;
            _planeController = planeController;
            _redBordersOnHit = redBordersOnHit;
        }


        public void ComposeTrap(BaseTrap trap) =>
            (trap as WindSpawner)?.Compose(_cameraPositionService, _planeView, _planeController, _redBordersOnHit);
    }
}