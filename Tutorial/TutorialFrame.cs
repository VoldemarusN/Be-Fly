using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Plane;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;

public class TutorialFrame : MonoBehaviour
{
    private const string IsTutorialPartOneShowedKey = "isTutorialPartOneShowed";
    private const string IsTutorialPartTwoShowedKey = "isTutorialPartTwoShowed";

    [SerializeField] private GameObject _tutorialContainer;
    [SerializeField] private GameObject _textContainer;
    [SerializeField] private bool _startByDefault;
    private PlaneView _planeView;
    private Texture2D _tutorialFrameTexture;
    [SerializeField] private RawImage _image;
    [SerializeField] private float _planeSpriteRadius;

    [SerializeField] private bool _isPartOneCompleted;
    [SerializeField] private bool _isPartTwoCompleted;

    [Inject]
    private void Construct(PlaneView planeView)
    {
        _planeView = planeView;
    }

    private IEnumerator Start()
    {
        if (_startByDefault)
        {
            SetTutorialComplication(1, _isPartOneCompleted);
            SetTutorialComplication(2, _isPartTwoCompleted);
        }

        if (PlayerPrefs.HasKey(IsTutorialPartOneShowedKey) == false)
            SetTutorialComplication(1, false);


        if (PlayerPrefs.HasKey(IsTutorialPartTwoShowedKey) == false)
            SetTutorialComplication(2, false);


        yield return null;

        if (IsTutorialCompleted(1) == false)
            StartCoroutine(StartTutorial(1));
        else if (IsTutorialCompleted(2) == false && _planeView.Type != PlaneType.Paper)
            StartCoroutine(StartTutorial(2));
    }

    private static bool IsTutorialCompleted(int part)
    {
        int tutorialResult = PlayerPrefs.GetInt(part == 1 ? IsTutorialPartOneShowedKey : IsTutorialPartTwoShowedKey);
        return tutorialResult == 1;
    }

    private IEnumerator StartTutorial(int part)
    {
        if (part == 2)
        {
            yield return new WaitForSeconds(0.3f);
            SetTutorialTexture();
            SetTutorialText();
            Time.timeScale = 0f;
        }

        ShowTutorialContainer();

        yield return StartCoroutine(DelayAndWaitForClickCoroutine(part));

        if (part == 1 && IsTutorialCompleted(2) == false && _planeView.Type != PlaneType.Paper)
            StartCoroutine(StartTutorial(2));
    }

    private void SetTutorialText()
    {
        Vector3 planePosition = Camera.main.WorldToScreenPoint(_planeView.transform.position);
        if (_planeSpriteRadius == 0) GetPlaneRadius();
        planePosition.Set((planePosition.x + 3.5f * _planeSpriteRadius), (planePosition.y + _planeSpriteRadius / 2), planePosition.z);

        TextMeshProUGUI textParameters = _textContainer.GetComponent<TextMeshProUGUI>();
        textParameters.text = "У всех самолётов, кроме бумажного, есть двигатель. Двигатель работает пока вы нажимаете на экран.";

        RectTransform rectTransformText = _textContainer.GetComponent<RectTransform>();
        rectTransformText.position = planePosition;
    }

    private void SetTutorialTexture()
    {
        Vector2Int textureResolution = new Vector2Int();
        Vector3 planePosition = Camera.main.WorldToScreenPoint(_planeView.transform.position);

        textureResolution.x = Screen.width;
        textureResolution.y = Screen.height;

        _tutorialFrameTexture = new Texture2D(textureResolution.x, textureResolution.y);

        GetPlaneRadius();

        for (int y = 0; y < textureResolution.y; y++)
        {
            for (int x = 0; x < textureResolution.x; x++)
            {
                _tutorialFrameTexture.SetPixel(x, y, Color.black);
                if (IsPointInsideCircle(x, y, (planePosition.x - planePosition.x / 45), planePosition.y))
                    _tutorialFrameTexture.SetPixel(x, y, Color.clear);
            }
        }

        _tutorialFrameTexture.Apply();
        _image.texture = _tutorialFrameTexture;
    }

    private bool IsPointInsideCircle(int x, int y, float x0, float y0)
    {
        var r = _planeSpriteRadius;
        return Mathf.Pow((x - x0), 2) + Mathf.Pow((y - y0), 2) <= r * r;
    }

    private void GetPlaneRadius()
    {
        Vector2 planeSize = new Vector2(_planeView.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit, _planeView.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        _planeSpriteRadius = planeSize.x >= planeSize.y ? planeSize.x : planeSize.y;
        _planeSpriteRadius = (_planeSpriteRadius / 2.5f);
    }

    private IEnumerator DelayAndWaitForClickCoroutine(int part)
    {
        while (Mouse.current.leftButton.isPressed == false)
            yield return null;

        SetTutorialComplication(part, true);
        HideTutorialContainer();
        Time.timeScale = 1;
    }

    private static void SetTutorialComplication(int part, bool isCompleted) =>
        PlayerPrefs.SetInt(part == 1 ? IsTutorialPartOneShowedKey : IsTutorialPartTwoShowedKey, Convert.ToInt32(isCompleted));

    private void HideTutorialContainer() => _tutorialContainer.SetActive(false);

    private void ShowTutorialContainer() => _tutorialContainer.SetActive(true);
}