using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AchievementPanelView : MonoBehaviour
{
    private Vector3 _achievementHiddenPosition;
    private Vector3 _achievementVisiblePosition;
    [SerializeField] private float _movingTime;
    [SerializeField] private float _showingTime;
    [SerializeField] private AchievementPanel _panel;
    public void Start()
    {
        _achievementVisiblePosition = transform.position;
        transform.localPosition += new Vector3(0, 160f, 0);
        //transform.position += new Vector3(0, 160f, 0);
        _achievementHiddenPosition = transform.position;
    }
    public void SetupPanelAndShow(AchievementConfig config, int progress)
    {
        _panel.SetSettings(config, progress);
        StartCoroutine(ShowAndHideAchievement());
    }

    private IEnumerator ShowAndHideAchievement()
    {
        gameObject.GetComponent<Image>().enabled = true;
        transform.DOMove(_achievementVisiblePosition, _movingTime);
        yield return new WaitForSeconds(_showingTime);

        transform.DOMove(_achievementHiddenPosition, _movingTime);
        yield return new WaitForSeconds(_movingTime);
        gameObject.GetComponent<Image>().enabled = false;
    }


}
