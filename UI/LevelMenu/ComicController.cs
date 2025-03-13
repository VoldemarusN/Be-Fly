using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Services.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;

public class ComicController : MonoBehaviour
{
    public event Action OnComicEnded;
    public event Action<int> OnFrameSwitched;
    [SerializeField] private float _timeToFade;
    [SerializeField] private float _timeToMove;
    [SerializeField] private float _scrollValue;
    [SerializeField] private Image _soloImage;
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _activityObject;
    [SerializeField] private GameObject _commonComicObject;
    [SerializeField] private GameObject _soloComicObject;
    [SerializeField] private Image _comicFramePrefab;


    private List<Image> _frames;
    private int _currentFrameIndex;
    private InputService _inputService;


    [Inject]
    private void Construct(InputService inputService)
    {
        _inputService = inputService;
        _frames = new List<Image>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        _inputService.Touch.performed += ScreenTouched;
    }

    private void OnDestroy()
    {
        _inputService.Touch.performed -= ScreenTouched;
    }

    private void ScreenTouched(InputAction.CallbackContext _)
    {
        if (_commonComicObject.activeSelf)
            ProcessCommonComic();
        else if (_soloComicObject.activeSelf)
            HideSoloComic();
    }

    private void HideSoloComic() => HideComic();


    private void ProcessCommonComic()
    {
        if (_currentFrameIndex >= _frames.Count)
        {
            HideComic();
            return;
        }

        GoToNextFrame();
    }

    private void HideComic()
    {
        _canvasGroup.DOFade(0, _timeToFade).onComplete += () =>
        {
            OnComicEnded?.Invoke();
            _activityObject.SetActive(false);
            _commonComicObject.SetActive(false);
            _soloComicObject.SetActive(false);
        };
    }

    private void GoToNextFrame()
    {
        if (_currentFrameIndex >= _frames.Count) return;
        _frames[_currentFrameIndex].DOFade(1, _timeToFade);
        if (_currentFrameIndex > 3 && _currentFrameIndex % 2 == 0 && _currentFrameIndex != 0)
            _scroll.DOHorizontalNormalizedPos(_scroll.normalizedPosition.x + _scrollValue, _timeToMove);
        _currentFrameIndex++;
        OnFrameSwitched?.Invoke(_currentFrameIndex);
    }

    public void ShowMultiComic(Sprite[] sprites)
    {
        InitializeFrames(sprites);

        _activityObject.SetActive(true);
        _commonComicObject.SetActive(true);
        _soloComicObject.SetActive(false);
        _canvasGroup.DOFade(1, _timeToFade).onComplete += GoToNextFrame;
    }

    private void InitializeFrames(Sprite[] sprites)
    {
        _frames.Clear();

        foreach (var sprite in sprites)
        {
            var frame = Instantiate(_comicFramePrefab, _scroll.content);
            frame.sprite = sprite;
            frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 0);
            _frames.Add(frame);
        }
    }

    public void ShowFrame(Sprite sprite)
    {
        _soloImage.sprite = sprite;
        _activityObject.SetActive(true);
        _soloComicObject.SetActive(true);
        _commonComicObject.SetActive(false);
        _canvasGroup.DOFade(1, _timeToFade);
    }
}