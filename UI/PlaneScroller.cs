using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DanielLochner.Assets.SimpleScrollSnap;
using Plane;
using UI.Garage;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlaneScroller : MonoBehaviour
    {
        public GaragePlaneView[] GaragePlaneViews;
        public PlaneType CurrentPlaneType => (PlaneType)_simpleScrollSnap.CurrentPanel;
        public event Action<PlaneType> OnPlaneSwitched;
        private SimpleScrollSnap _simpleScrollSnap;

        private void Awake()
        {
            _simpleScrollSnap = GetComponent<SimpleScrollSnap>();
            _simpleScrollSnap.onPanelChanged.AddListener(InvokeSwitchEvent);
            _simpleScrollSnap.onPanelChanged.AddListener(TryDisableButtonsOnEdge);
            GaragePlaneViews = _simpleScrollSnap.Content.GetComponentsInChildren<GaragePlaneView>();
        }

        private void Start() => TryDisableButtonsOnEdge();


        private void OnDisable() => _simpleScrollSnap.onPanelChanged.RemoveListener(InvokeSwitchEvent);

        private void InvokeSwitchEvent() => OnPlaneSwitched?.Invoke(CurrentPlaneType);

        private void MoveToPlaneType(PlaneType storageLastSelectedPlaneType)
        {
            _simpleScrollSnap.GoToPanel((int)storageLastSelectedPlaneType);
            OnPlaneSwitched?.Invoke(storageLastSelectedPlaneType);
        }


        public async UniTaskVoid MoveToPlaneTypeImmediately(PlaneType storageDataLastSelectedPlaneType)
        {
            var temp = _simpleScrollSnap.snappingSpeed;
            _simpleScrollSnap.snappingSpeed = 10000000f;
            MoveToPlaneType(storageDataLastSelectedPlaneType);
            await UniTask.NextFrame();
            _simpleScrollSnap.snappingSpeed = temp;
        }

        private void TryDisableButtonsOnEdge()
        {
            _simpleScrollSnap.previousButton.gameObject.SetActive(_simpleScrollSnap.CurrentPanel != 0);
            _simpleScrollSnap.nextButton.gameObject.SetActive(_simpleScrollSnap.CurrentPanel != _simpleScrollSnap.NumberOfPanels - 1);
        }
    }
}