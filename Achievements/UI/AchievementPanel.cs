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
    public void SetSettings(AchievementConfig achievement)
    {
        _achieveName.text = achievement.AchieveName;
        Description.text = achievement.Description;
        if(Progress !=null)
        Progress.text = $"{achievement.CurrentProgress} / {achievement.MaxProgress}";
        Icon.sprite = achievement.Icon;
    }
}
