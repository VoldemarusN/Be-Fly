using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private Button _music;
    [SerializeField] private Button _sfx;
    [SerializeField] private Button _play;
    [SerializeField] private Button _developers;
    [SerializeField] private Button _exit;
    [SerializeField] private Button _changeLanguageButton;


    [Inject]
    private void Construct(SceneLoader sceneLoader)
    {
        _play.onClick.AddListener(() => sceneLoader.LoadScene(SceneType.LevelMenu));
        _developers.onClick.AddListener(() => sceneLoader.LoadScene(SceneType.Developers));
        _exit.onClick.AddListener(() => Application.Quit());
    }

    private async void Start()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            GlobalSettings.Music = PlayerPrefs.GetInt("Music") == 1;
            _music.GetComponent<CanvasGroup>().alpha = GlobalSettings.Music ? 1 : 0.4f;
        }

        if (PlayerPrefs.HasKey("SFX"))
        {
            GlobalSettings.SFX = PlayerPrefs.GetInt("SFX") == 1;
            _sfx.GetComponent<CanvasGroup>().alpha = GlobalSettings.SFX ? 1 : 0.4f;
        }

        if (PlayerPrefs.HasKey("Language"))
        {
            await LocalizationSettings.InitializationOperation;
            GlobalSettings.Language = PlayerPrefs.GetString("Language");
            SetLocale(GlobalSettings.Language);
        }


        _changeLanguageButton.onClick.AddListener(() =>
        {
            var locale = SwitchLanguage();
            PlayerPrefs.SetString("Language", locale.LocaleName);
            GlobalSettings.Language = locale.LocaleName;
        });

        _music.onClick.AddListener(() =>
        {
            GlobalSettings.Music = !GlobalSettings.Music;
            _music.GetComponent<CanvasGroup>().alpha = GlobalSettings.Music ? 1 : 0.4f;
            PlayerPrefs.SetInt("Music", GlobalSettings.Music ? 1 : 0);
        });
        _sfx.onClick.AddListener(() =>
        {
            GlobalSettings.SFX = !GlobalSettings.SFX;
            _sfx.GetComponent<CanvasGroup>().alpha = GlobalSettings.SFX ? 1 : 0.4f;
            PlayerPrefs.SetInt("SFX", GlobalSettings.SFX ? 1 : 0);
        });
    }

    private Locale SwitchLanguage()
    {
        SetLocale(LocalizationSettings.AvailableLocales.Locales[0] == LocalizationSettings.SelectedLocale
            ? LocalizationSettings.AvailableLocales.Locales[1].LocaleName
            : LocalizationSettings.AvailableLocales.Locales[0].LocaleName);
        return LocalizationSettings.SelectedLocale;
    }

    private async void SetLocale(string selectedLocaleName)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales
            .First(locale => locale.LocaleName == selectedLocaleName);
    }


    private void Update()
    {
        _mainCamera.transform.position += Vector3.right * _cameraSpeed * Time.deltaTime;
    }
}