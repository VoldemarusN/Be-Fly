using System;
using System.Collections.Generic;
using System.Linq;
using Plane.PlaneData.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public class UpgradeViewManager : MonoBehaviour
    {
        public event Action OnPlaneBought;

        [SerializeField] private Transform _upgradeHolder;
        [SerializeField] private Transform _uniqUpgradeHolder;
        [SerializeField] private Button _buyPlaneButton;
        [SerializeField] private GameObject _planeLockedImage;
        [SerializeField] private TextMeshProUGUI _planeCost;


        private readonly List<UpgradeView> _instantiatedUpgradeViews = new List<UpgradeView>();


        private void Awake()
        {
            _buyPlaneButton.onClick.AddListener(() => OnPlaneBought?.Invoke());
        }

        public void SetPlaneCost(int cost)
        {
            _planeLockedImage.SetActive(false);
            _buyPlaneButton.gameObject.SetActive(true);
            _planeCost.text = cost.ToString();
            ClearUpgrades();
        }
        public void ShowBlockImage()
        {
            _planeLockedImage.SetActive(true);
            ClearUpgrades();
        }

        public void SetUniqUpgrades(UpgradeView[] upgradeViews, Action<UpgradeView> onUpgradeClicked)
        {
            foreach (var upgradeView in upgradeViews)
            {
                var instantiatedUpgradeView = Instantiate(upgradeView, _uniqUpgradeHolder);
                instantiatedUpgradeView.OnClick += onUpgradeClicked;
            }
        }

        public void SetUpgrades(UpgradeView[] upgradeViews, Action<UpgradeView> onUpgradeClicked)
        {
            _buyPlaneButton.gameObject.SetActive(false);
            _planeLockedImage.SetActive(false);


            ClearUpgrades(onUpgradeClicked);

            foreach (var view in upgradeViews)
            {
                var upgradeView = Instantiate(view, _upgradeHolder);
                _instantiatedUpgradeViews.Add(upgradeView);
                upgradeView.OnClick += onUpgradeClicked;
            }
        }

        private void ClearUpgrades(Action<UpgradeView> onUpgradeClicked = null)
        {
            foreach (var instantiatedUpgradeView in _instantiatedUpgradeViews)
            {
                if (onUpgradeClicked != null) instantiatedUpgradeView.OnClick -= onUpgradeClicked;
                Destroy(instantiatedUpgradeView.gameObject);
            }

            _instantiatedUpgradeViews.Clear();
        }


        public void SetLevelToUpgrade(string upgradeName, int level, int index)
        {
            foreach (var view in _instantiatedUpgradeViews)
            {
                if (view.CharacteristicName == upgradeName) view.SetLevel(level);
            }
        }

        public void SetCostToUpgradeType(string dataCharacteristic, UpgradeLevel dataLevel)
        {
            foreach (var view in _instantiatedUpgradeViews)
            {
                if (view.CharacteristicName == dataCharacteristic) view.SetCost(dataLevel.Price);
            }
        }

        public void SetInteractivity(string dataCharacteristic, bool b)
        {
            foreach (var view in _instantiatedUpgradeViews)

            {
                if (view.CharacteristicName == dataCharacteristic) view.SetInteractivity(b);
            }
        }

        public void TryCompleteViews(string dataCharacteristic)
        {
            foreach (var view in _instantiatedUpgradeViews)
            {
                if (view.CharacteristicName == dataCharacteristic) view.Complete();
            }
        }

        public void SetMaxLevel(string dataCharacteristic, int level)
        {
            foreach (var view in _instantiatedUpgradeViews)

            {
                if (view.CharacteristicName == dataCharacteristic) view.SetMaxLevel(level);
            }
        }
    }
}