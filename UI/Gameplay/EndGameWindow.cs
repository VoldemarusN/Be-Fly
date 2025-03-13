using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace UI.Gameplay
{
    public class EndGameWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private AchievementPanel _achievementPanel;


        [SerializeField] private LocalizedString _moneyLocalizedString;
        [SerializeField] private LocalizedString _distanceLocalizedString;
        [SerializeField] private LocalizedString _flagTitleLocalizedString;
        [SerializeField] private LocalizedString _flagDeskLocalizedString;
        [SerializeField] private LocalizedString _metersLocalizedString;

        public void SetDistance(int distance) => _distanceText.text = _distanceLocalizedString.GetLocalizedString()
                                                                      + " " + distance + _metersLocalizedString.GetLocalizedString();

        public void SetMoney(int money) => _moneyText.text = _moneyLocalizedString.GetLocalizedString() + " " + money;

        public void SetAchievement(int amount)
        {
            _achievementPanel.gameObject.SetActive(true);
            _achievementPanel.SetSettings(
                _flagTitleLocalizedString.GetLocalizedString(),
                $"{_flagDeskLocalizedString.GetLocalizedString()} {amount.ToString()}");
        }
    }
}