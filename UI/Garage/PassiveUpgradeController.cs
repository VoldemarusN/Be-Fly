using System;
using System.Collections.Generic;
using System.Linq;
using Plane;
using Plane.PlaneData;
using Plane.PlaneData.Upgrade;
using SavingSystem;
using UI.View;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Garage
{
    public class PassiveUpgradeController : IInitializable
    {
        private readonly Dictionary<PlaneType, PlaneConfig> _planes;
        private readonly PlaneScroller _planeScroller;
        private readonly Storage _storage;
        private UpgradesController _upgradesController;
        private readonly ParticleSystem _upgradeParticles;
        private readonly AudioManager _audioManager;
        public Action<PlaneType, int, int> AchievementMaxUpgradePlaneIsDone;

        public PassiveUpgradeController(Dictionary<PlaneType, PlaneConfig> planes,
            PlaneScroller planeScroller,
            Storage storage, UpgradesController upgradesController,
            ParticleSystem particleSystem,
            AudioManager audioManager)
        {
            _planes = planes;
            _planeScroller = planeScroller;
            _storage = storage;
            _upgradesController = upgradesController;
            _upgradeParticles = particleSystem;
            _audioManager = audioManager;

            _planeScroller.OnPlaneSwitched += OnPlaneSwitched;
            _upgradesController.OnUpgraded += OnPlaneSwitched;
            _upgradesController.OnPlaneBought += OnPlaneBought;
        }

        private void OnPlaneBought(PlaneType type)
        {
            var planeView = GetPlaneView(type);
            planeView.ProgressBar.Show();
        }

        private GaragePlaneView GetPlaneView(PlaneType type) =>
            _planeScroller.GaragePlaneViews.First(view => view.PlaneView.Type == type);

        private void InitializePlaneSprites()
        {
            foreach (var garagePlaneView in _planeScroller.GaragePlaneViews)
            {
                var type = garagePlaneView.PlaneView.Type;
                var planeData = _storage.GetPlaneData(type);
                garagePlaneView.SetLevel(planeData.Level);
                
                if (planeData.IsUnlocked == false) garagePlaneView.ProgressBar.Hide();
                else garagePlaneView.ProgressBar.SetProgressNormalized(planeData.LevelNormalized);
            }
        }

        public void Initialize() => InitializePlaneSprites();

        private void OnPlaneSwitched(PlaneType planeType)
        {
            int upgradeCount = GetPlaneUpgradeCount(planeType);
            float progress = (float)upgradeCount / _planes[planeType].RequiredUpgradeCountForMaxLevel;
            AchievementMaxUpgradePlaneIsDone?.Invoke(planeType, upgradeCount, _planes[planeType].RequiredUpgradeCountForMaxLevel);
            int resistanceLevel = 0;
            switch (progress)
            {
                case >= 0.99f:
                    resistanceLevel = 2;
                    break;
                case >= 0.5f:
                    resistanceLevel = 1;
                    break;
            }

            var planeData = _storage.GetPlaneData(planeType);
            var planeView = GetPlaneView(planeType);
            if (planeData.Level != resistanceLevel ) PlayUpgradeEffects();

            planeData.TrapResistance = _planes[planeType].PassiveUpgrades[resistanceLevel].TrapResistance;
            planeData.Level = resistanceLevel;
            planeData.LevelNormalized = progress;
            planeView.SetLevel(resistanceLevel);
            planeView.ProgressBar.SetProgressNormalized(progress);
        }


        private void PlayUpgradeEffects()
        {
            _audioManager.PlayOneShotEffect(_audioManager.PassiveUpgradeSound);
            _upgradeParticles.Play();
        }

        private int GetPlaneUpgradeCount(PlaneType planeType)
        {
            PlaneConfig planeConfig = _planes[planeType];

            var planeData = _storage.StorageData.PlaneData[planeType];
            if (planeData.IsUnlocked == false) return 0;

            var planeCharacteristicSavedLevels = planeData.CharacteristicLevels;
            var planeUpgrades = planeConfig.Upgrades;

            int upgradeCount = 0;
            foreach (var upgrade in planeUpgrades)
            {
                int level = planeCharacteristicSavedLevels[upgrade.name];
                if (level == 0) continue;
                upgradeCount += level;
            }

            return upgradeCount;
        }
    }
}