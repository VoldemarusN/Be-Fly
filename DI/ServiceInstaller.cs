using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using Plane;
using SavingSystem;
using Services;
using Services.Input;
using UnityEngine;
using Zenject;

public class ServiceInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<Storage>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
    }
}