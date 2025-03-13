using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _achieveName;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] TextMeshProUGUI Progress;
    [SerializeField] Image Icon;
    [SerializeField] Image BackgroundImage;

    public void SetSettings(AchievementConfig achievement, int progress)
    {
        _achieveName.text = achievement.AchievementNameLocalizedString.GetLocalizedString();
        Description.text = achievement.DescriptionLocalizedString.GetLocalizedString();
        if (Progress != null)
            Progress.text = $"{progress} / {achievement.MaxProgress}";
        Icon.sprite = achievement.Icon;

        if (progress < 1)
        {
            Icon.color = Color.gray;
            BackgroundImage.color = Color.gray;
        }
    }

    public void SetSettings(string title, string text)
    {
        _achieveName.text = title;
        Description.text = text;
    }
}