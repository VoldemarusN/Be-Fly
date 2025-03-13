using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Infrastructure.AssetManagement;
using Plane;
using Plane.PlaneData;
using Plane.PlaneData.Upgrade;
using QFSW.QC;
using SavingSystem;
using Scriptable_Objects;
using Services;
using TMPro;
using UI;
using UI.Garage;
using UI.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace DI
{
    public class GarageInstaller : MonoInstaller
    {
        public Storage Storage { get; private set; }

        [SerializeField] private ParticleSystem _upgradeParticles;

        [SerializeField] private PlaneScroller _planeScroller;
        [SerializeField] private UpgradeData[] _uniqUpgrades;

        [SerializeField] private UpgradeViewManager _viewManager;
        [SerializeField] private TextMeshProUGUI _tmp;
        [SerializeField] private Button _goButton;
        [SerializeField] private Button _exitButton;


        public override void InstallBindings()
        {
            var loader = Container.Resolve<SceneLoader>();
            Storage = Container.Resolve<Storage>();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<GarageController>().AsSingle().WithArguments(_tmp, _goButton).NonLazy();
            Container.Bind<PlaneScroller>().FromInstance(_planeScroller).AsSingle();

            Container.BindInterfacesAndSelfTo<UpgradesController>().AsSingle()
                .WithArguments(_uniqUpgrades, _viewManager);
            Container.BindInterfacesAndSelfTo<PassiveUpgradeController>().AsSingle().WithArguments(_upgradeParticles)
                .NonLazy();
            _exitButton.onClick.AddListener(() => loader.LoadScene(SceneType.LevelMenu));
        }


        [Command(aliasOverride: "delete-save")]
        public void DeleteSave()
        {
            PlayerPrefs.SetInt("isTutorialPartOneShowed", 0);
            PlayerPrefs.SetInt("isTutorialPartTwoShowed", 0);
            PlayerPrefs.DeleteKey("IsTutorialShowed");
            
            Storage.InitializeDefaultStorageData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}