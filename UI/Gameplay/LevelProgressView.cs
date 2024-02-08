using System;
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
        
        [SerializeField] private GameObject _IncomePrefub;
        [SerializeField] private GameObject _TrapPrefub;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private Slider _slider;

        public void SetProgress(float normalizedValue, float distance)
        {
            _slider.value = normalizedValue;
            _text.text = distance.ToString(CultureInfo.InvariantCulture) + " m";
        }

        public void SetPoint(float normalizedDistance, PointType type, float LevelDistance)
        {
            var prefub = type == PointType.Income ? _IncomePrefub : _TrapPrefub;

            Vector2 sliderPosition = _sliderAreaRectTransform.position;
            Vector2 min = _sliderAreaRectTransform.rect.min;
            Vector2 max = _sliderAreaRectTransform.rect.max;
            float scale = _sliderAreaRectTransform.transform.lossyScale.x;
            
            min.x = sliderPosition.x - Math.Abs(min.x) * scale;
            max.x = sliderPosition.x + Math.Abs(max.x) * scale;
            
            Vector2 pointPosition;
            pointPosition.x = min.x + (((max.x - min.x)/LevelDistance) * normalizedDistance);
            pointPosition.y = sliderPosition.y * 1.52f;
            var obj = Object.Instantiate(prefub, pointPosition, Quaternion.identity, _pointsParentRectTransform);
            obj.transform.SetSiblingIndex(0);
        }
    }

    public enum PointType
    {
        Income,
        Trap
    }
}
