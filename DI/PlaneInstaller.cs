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
    private PlanePhysicConfig _physicData;
    private OilData _oilData;


    public override void InstallBindings()
    {
        Storage storage = Container.Resolve<Storage>();
        var planeType = storage.StorageData.LastSelectedPlaneType;
        PlaneConfig config = _configs.First(x => x.Type == planeType);
        PlaneData data = storage.GetPlaneData(planeType);

        Container.Bind<PlaneFactory>().AsSingle();
        PlaneView planeView = Container.Resolve<PlaneFactory>()
            .BuildPlaneView(planeType, _spawnPoint.position, _levelConfig.Angle, data.Level);

        //Container.Bind<LevelConfig>().AsSingle();
        Container.Bind<Transform>().FromInstance(planeView.transform).AsSingle();


        Container.Bind<PlaneView>().FromInstance(planeView).AsSingle();
        _oilData = ClonedData(config.Oil);
        _physicData = ClonedData(config.PlanePhysicData);

        ModifyDataFromLevels(config.Upgrades, storage.StorageData.PlaneData[config.Type].CharacteristicLevels);
        _physicData.TrapResistance = storage.StorageData.PlaneData[config.Type].TrapResistance;

        Container.Bind<PlanePhysicConfig>().FromInstance(_physicData).AsSingle();
        Container.Bind<OilData>().FromInstance(_oilData).AsSingle();

        Container.Bind<IPlaneMovement>().To<PlaneMovement>().AsSingle();


        Container.BindInterfacesAndSelfTo<PlaneController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlaneLauncher>().AsSingle().WithArguments(_blur).NonLazy();
    }


    private void ModifyDataFromLevels(UpgradeData[] upgrades, Dictionary<string, int> levels)
    {
        foreach (var upgradeData in upgrades)
        {
            int level = levels[upgradeData.name];
            level = Mathf.Clamp(level, 0, upgradeData.Levels.Length - 1);

            float augmentValue = 0;
            float oilAugmentValue = 0;
            for (int j = 1; j <= level + 1; j++)
            {
                augmentValue += upgradeData.Levels[j - 1].AugmentValue;
                oilAugmentValue += upgradeData.Levels[j - 1].AdditiveOilAmount;
            }

            AddUpgradeValueToConfigs(upgradeData, augmentValue);
            _oilData.Amount += oilAugmentValue;
        }
    }

    private static T ClonedData<T>(T oldData) where T : class, ICloneable
    {
        T clonedData = oldData.Clone() as T;
        return clonedData;
    }

    private void AddUpgradeValueToConfigs(UpgradeData upgradeData, float augmentValue)
    {
        switch (upgradeData.Characteristic)
        {
            case UpgradeCharacteristic.PassiveRotationSpeedDown:
                _physicData.PassiveRotationSpeedDown += augmentValue;
                break;
            case UpgradeCharacteristic.PassiveSpeedRight:
                _physicData.PassiveSpeedRight += augmentValue;
                break;
            case UpgradeCharacteristic.AngularDrag:
                _physicData.AngularDrag += augmentValue;
                break;
            case UpgradeCharacteristic.LinearDrag:
                _physicData.LinearDrag += augmentValue;
                break;
            case UpgradeCharacteristic.FallAngle:
                _physicData.FallAngle += augmentValue;
                break;
            case UpgradeCharacteristic.ActiveRotationSpeedUp:
                _physicData.ActiveRotationSpeedUp += augmentValue;
                break;
            case UpgradeCharacteristic.ActiveSpeedRight:
                _physicData.ActiveSpeedRight += augmentValue;
                break;
            case UpgradeCharacteristic.DirectionDownInfluenceToPassiveRightSpeed:
                _physicData.DirectionDownInfluenceToPassiveRightSpeed += augmentValue;
                break;
            case UpgradeCharacteristic.Oil:
                _oilData.Amount += augmentValue;
                break;
        }
    }
}