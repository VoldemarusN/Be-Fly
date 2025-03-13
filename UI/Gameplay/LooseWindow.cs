using System.Collections;
using System.Collections.Generic;
using Services;
using TMPro;
using UI.Gameplay;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Zenject;

public class LooseWindow : EndGameWindow
{
    [SerializeField] private Slider _lastProgressSlider;
    [SerializeField] private Slider _currentProgressSlider;
    [SerializeField] private Button _garageButton;
    [SerializeField] private Button _restartButton;


    [Inject]
    public void Construct(SceneLoader loader) => _garageButton.onClick.AddListener(() => loader.LoadScene(SceneType.Garage));


    public void SetLastProgressNormalized(float value) => _lastProgressSlider.value = value;
    public void SetCurrentProgressNormalized(float value) => _currentProgressSlider.value = value;
}