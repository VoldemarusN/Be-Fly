using System;
using SavingSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zenject;
using UnityEngine.UI;

public class AchievementInitializator : MonoBehaviour
{
    [SerializeField] AchievementConfig[] _configs;
    [SerializeField] AchievementPanel _panelPrefab;
    [SerializeField] Transform _achievementPanelParent;
    private Storage _storage;

    [Inject]
    public void Construct(Storage storage) => _storage = storage;

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += Initialize;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= Initialize;
    }


    public void Initialize(Locale _ = null)
    {
        for (int i = 0; i < _achievementPanelParent.childCount; i++)
        {
            Destroy(_achievementPanelParent.GetChild(i).gameObject);
        }

        foreach (var config in _configs.OrderByDescending(conf => _storage.GetAchievementProgress(conf) >= 1))
        {
            AchievementPanel newPanel = Instantiate(_panelPrefab, _achievementPanelParent);
            newPanel.SetSettings(config, _storage.GetAchievementProgress(config));
        }
    }
}