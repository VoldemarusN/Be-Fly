using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Plane;
using Services.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;

public class TutorialFrame : MonoBehaviour, IDisposable
{
    private const string IsTutorialShowed = "IsTutorialShowed";

    [SerializeField] private GameObject[] _tutorialContainers;
    [SerializeField] private GameObject _textContainer;
    [SerializeField] private bool _startByDefault;
    [SerializeField] private RawImage _tutorialFirstStepImage;
    [SerializeField] private RawImage _tutorialSecondStepImage;
    [SerializeField] private float _planeSpriteRadius;


    private PlaneLauncher _launcher;
    private Texture2D _tutorialFrameTexture;
    private InputService _inputService;
    private int _currentTutorialContainerIndex;

    [Inject]
    private void Construct(PlaneLauncher launcher, InputService inputService)
    {
        _launcher = launcher;
        _inputService = inputService;
    }

    private void Start()
    {
        if (_startByDefault)
            SetTutorialComplication(false);

        if (IsTutorialCompleted())
            _launcher.Initialize();
        else
        {
            _inputService.Touch.performed += ScreenTouched;
            ShowNextTutorialFrame();
        }
    }

    private void ShowNextTutorialFrame()
    {
        if (_currentTutorialContainerIndex >= _tutorialContainers.Length)
        {
            _launcher.Initialize();
            Dispose();
            return;
        }

        SetTutorialContainerVisible(_tutorialContainers[_currentTutorialContainerIndex], true);
        if (_currentTutorialContainerIndex > 0)
            SetTutorialContainerVisible(_tutorialContainers[_currentTutorialContainerIndex - 1], false);

        _currentTutorialContainerIndex++;
    }

    private void ScreenTouched(InputAction.CallbackContext _)
    {
        ShowNextTutorialFrame();
    }

    private static bool IsTutorialCompleted()
    {
        int tutorialResult = PlayerPrefs.GetInt(IsTutorialShowed);
        return tutorialResult == 1;
    }

    private static void SetTutorialComplication(bool isCompleted) =>
        PlayerPrefs.SetInt(IsTutorialShowed, Convert.ToInt32(isCompleted));

    private void SetTutorialContainerVisible(GameObject tutorial, bool isVisible) =>
        tutorial.SetActive(isVisible);

    public void Dispose()
    {
        foreach (var tutorialContainer in _tutorialContainers) 
            SetTutorialContainerVisible(tutorialContainer, false);
        SetTutorialComplication(true);
        _inputService.Touch.performed -= ScreenTouched;
    }
}