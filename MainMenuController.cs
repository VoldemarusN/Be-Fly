using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private Button _music;
    [SerializeField] private Button _sfx;
    [SerializeField] private Button _play;


    [Inject]
    private void Construct(SceneLoader sceneLoader) => _play.onClick.AddListener(() => sceneLoader.LoadScene(SceneType.LevelMenu));

    private void Start()
    {
        _music.onClick.AddListener(() =>
        {
            GlobalSettings.Music = !GlobalSettings.Music;
            _music.GetComponent<CanvasGroup>().alpha = GlobalSettings.Music ? 1 : 0.4f;
        });
        _sfx.onClick.AddListener(() =>
        {
            GlobalSettings.SFX = !GlobalSettings.SFX;
            _sfx.GetComponent<CanvasGroup>().alpha = GlobalSettings.SFX ? 1 : 0.4f;
        });
    }


    private void Update()
    {
        _mainCamera.transform.position += Vector3.right * _cameraSpeed * Time.deltaTime;
    }
}