using System;
using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneOnClick : ValidatedMonoBehaviour
{
    [SerializeField, NaughtyAttributes.Scene]
    private string _sceneName;

    [SerializeField, Self] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick() => SceneManager.LoadScene(_sceneName);
}