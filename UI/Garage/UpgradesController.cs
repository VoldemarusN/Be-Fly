using System;
using System.Collections.Generic;
using System.Linq;
using DanielLochner.Assets.SimpleScrollSnap;
using Plane;
using Plane.PlaneData;
using Plane.PlaneData.Upgrade;
using SavingSystem;
using UnityEngine;
using Scriptable_Objects;
using TMPro;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.View
{
    public class UpgradesController
    {
        public event Action<PlaneType> OnPlaneBought;
        public event Action<PlaneType> OnUpgraded;

        private readonly Dictionary<PlaneType, PlaneConfig> _planes;
        private readonly UpgradeData[] _uniqueUpgrades;
        private readonly Storage _storage;
        private readonly PlaneScroller _planeScroller;
        private readonly UpgradeViewManager _upgradeViewManager;
        private readonly AudioManager _audioManager;
        private PlaneType _currentPlaneType;


        public UpgradesController(Dictionary<PlaneType, PlaneConfig> planes, UpgradeData[] uniqueUpgrades,
            Storage storage, PlaneScroller planeScroller, UpgradeViewManager upgradeViewManager, AudioManager audioManager)
        {
            _planes = planes;
            _uniqueUpgrades = uniqueUpgrades;
            _storage = storage;
            _planeScroller = planeScroller;
            _upgradeViewManager = upgradeViewManager;
            _audioManager = audioManager;

            _upgradeViewManager.OnPlaneBought += TryToBuyPlane;
        }

        private void TryToBuyPlane()
        {
            if (_storage.StorageData.Money >= _planes[_currentPlaneType].Price)
            {
                _storage.StorageData.Money -= _planes[_currentPlaneType].Price;
                _storage.StorageData.LastSelectedPlaneType = _currentPlaneType;
                _storage.StorageData.PlaneData[_currentPlaneType].IsUnlocked = true;
                SetUpgradesFromPlaneType(_currentPlaneType);
                OnPlaneBought?.Invoke(_currentPlaneType);
            }
        }


        public void SetUpgradesFromPlaneType(PlaneType planeType)
        {
            _currentPlaneType = planeType;

            if (_storage.StorageData.PlaneData[_currentPlaneType].IsUnlocked == false)
            {
                if (CheckPlaneIsAvailableToBuy(planeType))
                {
                    _upgradeViewManager.SetPlaneCost(_planes[_currentPlaneType].Price);
                }
                else
                {
                    _upgradeViewManager.ShowBlockImage();
                }

                return;
            }

            UpgradeView[] views = _planes[_currentPlaneType].Upgrades.Select(x => x.View).ToArray();
            _upgradeViewManager.SetUpgrades(views, OnUpgradeClicked);
            SetLevels();
            UpdateCost();
            UpdateInteractivityOfButtons();
        }

        private bool CheckPlaneIsAvailableToBuy(PlaneType planeType)
        {
            int planeIndex = (int)planeType;
            int numberToCompare = int.Parse(_storage.StorageData.LastLevel[^1].ToString()) - 1;
            if (planeIndex <= numberToCompare)
            {
                Debug.Log("Этот самолет доступен");
                return true;
            }
            else
            {
                Debug.Log("Этот самолет пока заблокирован");
                return false;
            }
        }

        private void SetLevels()
        {
            int index = 0;
            foreach (var data in _planes[_currentPlaneType].Upgrades)
            {
                var upgradeData = _planes[_currentPlaneType].Upgrades.First(x => x.Characteristic == data.Characteristic);
                _upgradeViewManager.SetMaxLevel(data.name, upgradeData.Levels.Length);


                Dictionary<string, int> levels = _storage.StorageData.PlaneData[_planeScroller.CurrentPlaneType].CharacteristicLevels;
                levels.TryAdd(data.name, 0);
                int level = levels[data.name];
                _upgradeViewManager.SetLevelToUpgrade(data.name, level, index);
                index++;
            }
        }

        private void UpdateCost()
        {
            foreach (var data in _planes[_currentPlaneType].Upgrades)
            {
                int level = _storage.StorageData.PlaneData[_planeScroller.CurrentPlaneType].CharacteristicLevels[data.name];
                if (level < data.Levels.Length)
                {
                    UpgradeLevel levelData = data.Levels[level];
                    _upgradeViewManager.SetCostToUpgradeType(data.name, levelData);
                }
            }
        }

        private void OnUpgradeClicked(UpgradeView upgradeView)
        {
            UpgradeData upgradeData = SelectUpgradeData(upgradeView);
            PlaneData currentPlaneData = _storage.GetPlaneData(_planeScroller.CurrentPlaneType);
            int upgradeLevel = currentPlaneData.CharacteristicLevels[upgradeData.name];

            if (_storage.StorageData.Money >= upgradeData.Levels[upgradeLevel].Price)
            {
                currentPlaneData.CharacteristicLevels[upgradeData.name]++;
                _storage.StorageData.Money -= upgradeData.Levels[upgradeLevel].Price;
                _audioManager.PlayOneShotEffect(_audioManager.ActiveUpgradeSounds);
                upgradeView.SetLevel(currentPlaneData.CharacteristicLevels[upgradeData.name]);
                UpdateCost();
                UpdateInteractivityOfButtons();
                OnUpgraded?.Invoke(_currentPlaneType);
            }
        }

        private void UpdateInteractivityOfButtons()
        {
            foreach (var data in _planes[_currentPlaneType].Upgrades)
            {
                int upgradeLevel = _storage.StorageData.PlaneData[_currentPlaneType].CharacteristicLevels[data.name];
                if (upgradeLevel >= data.Levels.Length)
                {
                    _upgradeViewManager.SetInteractivity(data.name, false);
                    _upgradeViewManager.TryCompleteViews(data.name);
                    continue;
                }

                _upgradeViewManager.SetInteractivity(data.name, _storage.StorageData.Money >= data.Levels[upgradeLevel].Price);
            }
        }


        private UpgradeData SelectUpgradeData(UpgradeView upgradeView)
        {
            UpgradeData upgradeData = _planes[_currentPlaneType].Upgrades.FirstOrDefault(u => u.name == upgradeView.CharacteristicName);
            return upgradeData ? upgradeData : _uniqueUpgrades.First(u => u.name == upgradeView.CharacteristicName);
        }
    }
}