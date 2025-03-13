using Plane;
using SavingSystem;
using Scriptable_Objects;
using Services;
using TMPro;
using UI.Garage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace UI.View
{
    public class GarageController : IInitializable
    {
        private readonly PlaneScroller _planeScroller;
        private readonly UpgradesController _upgradesController;
        private readonly Storage _storage;
        private readonly Button _goButton;

        public GarageController(PlaneScroller planeScroller, UpgradesController upgradesController,
            TextMeshProUGUI tmp, Storage storage,
            Button goButton, SceneLoader loader)
        {
            _planeScroller = planeScroller;
            _upgradesController = upgradesController;
            _storage = storage;
            _goButton = goButton;
            _planeScroller.OnPlaneSwitched += OnPlaneSwitched;
            upgradesController.OnPlaneBought += SetGoButton;
            storage.StorageData.OnMoneyChanged += m => tmp.text = m.ToString();
            tmp.text = storage.StorageData.Money.ToString();
            goButton.onClick.AddListener(() => _ = loader.LoadScene(storage.StorageData.LastLevel));
        }


        private void OnPlaneSwitched(PlaneType planeType)
        {
            _upgradesController.SetUpgradesFromPlaneType(planeType);
            _storage.StorageData.LastSelectedPlaneType = planeType;
            SetGoButton(planeType);
        }

        private void SetGoButton(PlaneType planeType)
        {
            var isUnlocked = _storage.StorageData.PlaneData[planeType].IsUnlocked;
            _goButton.interactable = isUnlocked;
            _goButton.GetComponent<CanvasGroup>().alpha = isUnlocked ? 1 : 0.4f;
        }

        public void Initialize()
        {
            _planeScroller.MoveToPlaneTypeImmediately(_storage.StorageData.LastSelectedPlaneType);
            _upgradesController.SetUpgradesFromPlaneType(_storage.StorageData.LastSelectedPlaneType);
            SetGoButton(_planeScroller.CurrentPlaneType);
        }
    }
}