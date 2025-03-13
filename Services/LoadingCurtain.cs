using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UI.LaunchForce;
using UnityEngine;
using UnityEngine.UI;

namespace Services
{
    internal class LoadingCurtain : MonoBehaviour, IHideable
    {
        [SerializeField] private CanvasGroup _loadingCurtainGroup;
        [SerializeField] private Image _backGFX;

        [SerializeField] private Transform _spinner;
        [SerializeField] private TextMeshProUGUI _tmp;

        [SerializeField] private float _spinnerFrequency = 1f;
        [SerializeField] private float _amplitude = 1f;
        [SerializeField] private float _rotationAmplitude;

        private Vector3 _spinnerInitialPosition;
        private string _textInitialValue;

        private void Start()
        {
            _spinnerInitialPosition = _spinner.position;
            _textInitialValue = _tmp.text;
        }

        UniTask IHideable.Show()
        {
            _backGFX.material.color = new Color(1, 1, 1, 0);
            _backGFX.material.DOBlendableColor(new Color(1, 1, 1, 1), 1f);
            StartCoroutine(TextTypingCoroutine());
            _loadingCurtainGroup.gameObject.SetActive(true);
            return _loadingCurtainGroup.DOFade(1, 1f).AsyncWaitForCompletion().AsUniTask();
        }

        public UniTask Hide()
        {
            _backGFX.material.DOBlendableColor(new Color(1, 1, 1, 0), 1f);
            StopAllCoroutines();
            var tweenerCore = _loadingCurtainGroup.DOFade(0, 1f);
            tweenerCore.onComplete += () => _loadingCurtainGroup.gameObject.SetActive(false);
            return tweenerCore.AsyncWaitForCompletion().AsUniTask();
        }


        private void Update()
        {
            if (_loadingCurtainGroup.gameObject.activeSelf)
            {
                RotateSpinner();
            }
        }

        private void RotateSpinner()
        {
            Vector3 newPosition = new Vector3(
                _spinnerInitialPosition.x,
                _spinnerInitialPosition.y + Mathf.Sin(Time.time * _spinnerFrequency) * _amplitude,
                _spinnerInitialPosition.z);
            _spinner.position = newPosition;

            float tiltAngle =
                Mathf.Sin(Time.time * _spinnerFrequency) *
                _rotationAmplitude; // Амплитуда поворота (например, 30 градусов)

// Применяем вращение к объекту (по оси X)
            _spinner.rotation =
                Quaternion.Euler(_spinner.rotation.eulerAngles.x, _spinner.rotation.eulerAngles.y, tiltAngle);
        }


        private IEnumerator TextTypingCoroutine()
        {
            while (true)
            {
                _tmp.text = _textInitialValue;
                yield return new WaitForSeconds(0.3f);
                for (int i = 0; i < 3; i++)
                {
                    _tmp.text += '.';
                    yield return new WaitForSeconds(0.3f);
                }
            }
        }
    }
}