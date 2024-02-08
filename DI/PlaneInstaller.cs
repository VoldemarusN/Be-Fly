using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.AssetManagement;
using Plane;
using Plane.PlaneData;
using Plane.PlaneData.Upgrade;
using SavingSystem;
using Scriptable_Objects;
using UI.LaunchForce;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using Zenject;

public class PlaneInstaller : MonoInstaller
{
    [SerializeField] private PlaneConfig[] _configs;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private LaunchConfig _levelConfig;
    [SerializeField] private Blur _blur;
    


    public override void InstallBindings()
    {
        Storage storage = Container.Resolve<Storage>();
        var planeType = storage.StorageData.LastSelectedPlaneType;
        PlaneConfig config = _configs.First(x => x.Type == planeType);
        PlaneData data = storage.GetPlaneData(planeType);

        Container.Bind<PlaneFactory>().AsSingle();
        PlaneView planeView = Container.Resolve<PlaneFactory>()
            .BuildPlaneView(planeType, _spawnPoint.position, _levelConfig.Angle, data.Level);

        Container.Bind<LevelConfig>().AsSingle();
        Container.Bind<Transform>().FromInstance(planeView.transform).AsSingle();


        Container.Bind<PlaneView>().FromInstance(planeView).AsSingle();
        OilData oilData = ModifyData(config.Oil, config.Upgrades, storage.StorageData.PlaneData[config.Type].CharacteristicLevels, ModifyOilData);
        PlanePhysicConfig physicData = ModifyData(config.PlanePhysicData, config.Upgrades, storage.StorageData.PlaneData[config.Type].CharacteristicLevels, ModifyPhysicData);
        physicData.TrapResistance = storage.StorageData.PlaneData[config.Type].TrapResistance;

        Container.Bind<PlanePhysicConfig>().FromInstance(physicData).AsSingle();
        Container.Bind<OilData>().FromInstance(oilData).AsSingle();

        Container.Bind<IPlaneMovement>().To<PlaneMovement>().AsSingle();


        Container.BindInterfacesAndSelfTo<PlaneController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlaneLauncher>().AsSingle().WithArguments(_blur).NonLazy();
    }


    private T ModifyData<T>(T oldData, UpgradeData[] upgrades,
        Dictionary<string, int> levels, Action<UpgradeCharacteristic, float, T> upgradeCallback) where T : class, ICloneable
    {
        T clonedData = oldData.Clone() as T;
        for (var i = 0; i < upgrades.Length; i++)
        {
            int level = levels[upgrades[i].name];
            level = Mathf.Clamp(level, 0, upgrades[i].Levels.Length - 1);
            var upgrade = upgrades[i];

            float augmentValue = 0;
            for (int j = 1; j <= level + 1; j++)
                augmentValue += upgrade.Levels[j - 1].AugmentValue;

            upgradeCallback?.Invoke(upgrade.Characteristic, augmentValue, clonedData);
        }

        return clonedData;
    }

    private void ModifyOilData(UpgradeCharacteristic upgradeCharacteristic, float augmentValue, OilData data)
    {
        if (upgradeCharacteristic == UpgradeCharacteristic.Oil) data.Amount += augmentValue;
    }

    private void ModifyPhysicData(UpgradeCharacteristic upgradeCharacteristic, float augmentValue, PlanePhysicConfig planePhysicData)
    {
        switch (upgradeCharacteristic)
        {
            case UpgradeCharacteristic.PassiveRotationSpeedDown:
                planePhysicData.PassiveRotationSpeedDown += augmentValue;
                break;
            case UpgradeCharacteristic.PassiveSpeedRight:
                planePhysicData.PassiveSpeedRight += augmentValue;
                break;
            case UpgradeCharacteristic.AngularDrag:
                planePhysicData.AngularDrag += augmentValue;
                break;
            case UpgradeCharacteristic.LinearDrag:
                planePhysicData.LinearDrag += augmentValue;
                break;
            case UpgradeCharacteristic.FallAngle:
                planePhysicData.FallAngle += augmentValue;
                break;
            case UpgradeCharacteristic.ActiveRotationSpeedUp:
                planePhysicData.ActiveRotationSpeedUp += augmentValue;
                break;
            case UpgradeCharacteristic.ActiveSpeedRight:
                planePhysicData.ActiveSpeedRight += augmentValue;
                break;
            case UpgradeCharacteristic.DirectionDownInfluenceToPassiveRightSpeed:
                planePhysicData.DirectionDownInfluenceToPassiveRightSpeed += augmentValue;
                break;
        }
    }
}