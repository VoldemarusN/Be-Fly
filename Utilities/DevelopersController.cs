using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DevelopersController : MonoBehaviour
{

    [SerializeField] private Button _mainMenu;


    [Inject]
    private void Construct(SceneLoader sceneLoader) => _mainMenu.onClick.AddListener(() => sceneLoader.LoadScene(SceneType.MainMenu));

}