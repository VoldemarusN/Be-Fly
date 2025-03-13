using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.Gameplay
{
    public class LevelProgressView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private RectTransform _sliderAreaRectTransform;
        [SerializeField] private RectTransform _pointsParentRectTransform;
        [SerializeField] private RectTransform _handleRectTransform;


        [SerializeField] private GameObject _IncomePrefub;
        [SerializeField] private GameObject _TrapPrefub;
        [SerializeField] private ParticleSystem _AwardEffect;
        [SerializeField] private Camera _camera;
        private float _startCameraOrtographicSize;
        private float _startPartickleSize;
        private Dictionary<PointType, List<GameObject>> _pointInstanes = new Dictionary<PointType, List<GameObject>>();

        private void Awake()
        {
            _startCameraOrtographicSize = _camera.orthographicSize;
            _startPartickleSize = _AwardEffect.main.startSize.constant;
            _slider = GetComponent<Slider>();
        }

        private Slider _slider;

        public void SetProgress(float normalizedValue, float distance)
        {
            _slider.value = normalizedValue;
            _text.text = distance.ToString(CultureInfo.InvariantCulture) + " m";
        }

        public void SetPoint(float normalizedDistance, PointType type)
        {
            var prefab = type == PointType.Income ? _IncomePrefub : _TrapPrefub;

            //get position of point in sliderAreaRect from normalizedDistance
            Vector2 min = _sliderAreaRectTransform.rect.min;
            Vector2 max = _sliderAreaRectTransform.rect.max;

            Vector3 pointPosition = new Vector3
            {
                x = Mathf.Lerp(min.x, max.x, normalizedDistance),
                y = 0, //* 1.52f
                z = 0
            };
            var obj = Instantiate(prefab, _pointsParentRectTransform);
            obj.transform.localPosition = pointPosition;
            obj.transform.SetSiblingIndex(0);

            if (_pointInstanes.ContainsKey(type) == false) _pointInstanes.Add(type, new List<GameObject>());
            _pointInstanes[type].Add(obj);
        }

        public void ClearPoints(PointType type)
        {
            if (_pointInstanes.TryGetValue(type, out var instane))
                foreach (var obj in instane)
                    Destroy(obj);
        }

        public void ClearFirstPoint(PointType type)
        {
            Destroy(_pointInstanes[type][0]);
            _pointInstanes[type].RemoveAt(0);
        }

        public void ShowAwardEffect()
        {
            if (_AwardEffect != null)
            {
                var AwardEffectMain = _AwardEffect.main;
                float size = _startPartickleSize * ((_camera.orthographicSize / _startCameraOrtographicSize) + 1);
                AwardEffectMain.startSize = new ParticleSystem.MinMaxCurve(size, size);
                _AwardEffect.transform.position = _handleRectTransform.position;
                _AwardEffect.Play();
            }
        }
    }

    public enum PointType
    {
        Income,
        Trap
    }
}