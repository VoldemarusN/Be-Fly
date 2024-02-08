using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Gameplay
{
    public class WinWindow : MonoBehaviour
    {
        [SerializeField] private Button _levelButton;
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _moneyText;


        public void SetDistance(int distance) => _distanceText.text = "Distance: " + distance + "m";
        public void SetMoney(int money) => _moneyText.text = "Total points: " + money;


        [Inject]
        public void Construct(SceneLoader loader) => _levelButton.onClick.AddListener(() => loader.LoadScene(SceneType.Garage));
    }
}