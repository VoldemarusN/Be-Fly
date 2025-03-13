using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveUpgradeProgressBar : MonoBehaviour
{
    [SerializeField] private Image _fill;
    [SerializeField] private Image _shield1;
    [SerializeField] private Image _shield2;
    [SerializeField] private Image _shield3;
    [SerializeField] private Sprite _upgradedShield1Sprite;
    [SerializeField] private Sprite _upgradedShield2Sprite;
    [SerializeField] private Sprite _upgradedShield3Sprite;
    [SerializeField] private Sprite _baseShield1Sprite;
    [SerializeField] private Sprite _baseShield2Sprite;
    [SerializeField] private Sprite _baseShield3Sprite;

    private float LowerStartFill = 0.15f;
    private float LowerEndFill = 0.42f;
    private float UpperStartFill = 0.62f;
    private float UpperEndFill = 0.9f;
    public void SetProgressNormalized(float progress)
    {
        SetFillNormalized(progress);
        switch (progress)
        {
            case >= 0.99f:
                _shield3.sprite = _upgradedShield3Sprite;
                _shield2.sprite = _baseShield2Sprite;
                _shield1.sprite = _baseShield1Sprite;
                break;
            case >= 0.5f:
                _shield3.sprite = _baseShield3Sprite;
                _shield2.sprite = _upgradedShield2Sprite;
                _shield1.sprite = _baseShield1Sprite;
                break;
            case > 0f:
                _shield3.sprite = _baseShield3Sprite;
                _shield2.sprite = _baseShield2Sprite;
                _shield1.sprite = _upgradedShield1Sprite;
                break;
        }
    }
    private void SetFillNormalized(float progress)
    {
        float CorrectFillValue = 0;
        switch (progress)
        {
            case < 0.5f:
                CorrectFillValue = progress * 2;
                _fill.fillAmount = Mathf.Lerp(LowerStartFill, LowerEndFill, CorrectFillValue);
                break;
            case > 0.5f:
                CorrectFillValue = (progress - 0.5f) * 2;
                _fill.fillAmount = Mathf.Lerp(UpperStartFill, UpperEndFill, CorrectFillValue);
                break;
        }
        
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}