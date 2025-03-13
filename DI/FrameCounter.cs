using System;
using TMPro;
using UnityEngine;

public class FrameCounter : MonoBehaviour
{
    [SerializeField] private GameObject _canvasPrefab;
    private TextMeshProUGUI _text;

    private void Start()
    {
        Application.targetFrameRate = 300;

#if UNITY_EDITOR
        SpawnUI();
#endif
    }

#if UNITY_EDITOR
    private void Update()
    {
        _text.text = ((int)(1.0f / Time.smoothDeltaTime)).ToString();
    }
#endif


    private void SpawnUI()
    {
        _text = Instantiate(_canvasPrefab, transform).GetComponentInChildren<TextMeshProUGUI>();
    }
}