using System.Collections;
using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LooseWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _distance;
    [SerializeField] private TextMeshProUGUI _money;
    [SerializeField] private Slider _lastProgressSlider;
    [SerializeField] private Slider _currentProgressSlider;
    [SerializeField] private Button _garageButton;
    [SerializeField] private Button _restartButton;

    public Button GarageButton => _garageButton;
    public Button RestartButton => _restartButton;

    [Inject]
    public void Construct(SceneLoader loader) => _garageButton.onClick.AddListener(() => loader.LoadScene(SceneType.Garage));


    public void SetDistance(int distance) => _distance.text = "Distance: " + distance + "m";
    public void SetMoney(int money) => _money.text = "Total points: " + money;

    public void SetLastProgressNormalized(float value) => _lastProgressSlider.value = value;
    public void SetCurrentProgressNormalized(float value) => _currentProgressSlider.value = value;
}