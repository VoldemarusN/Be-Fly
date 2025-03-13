using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using Plane;
using SavingSystem;
using SDK.Steam;
using Services;
using Services.Input;
using UnityEngine;
using Zenject;

public class ServiceInstaller : MonoInstaller
{
    [SerializeField] private GameObject _console;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<Storage>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        Container.Bind<FrameCounter>().FromComponentOn(this.gameObject).AsSingle().NonLazy();
#if STEAM_INTEGRATION
        Container.BindInterfacesAndSelfTo<SteamIntegration>().AsSingle();
#endif
#if VKPLAY_INTEGRATION
#endif
#if UNITY_EDITOR
        DontDestroyOnLoad(Instantiate(_console));
#endif
    }
}