using System;
using NaughtyAttributes;
using UI.LaunchForce;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class ArrowSpeedScrollView : MonoBehaviour, IScrollView
    {
        [SerializeField] private Transform _arrowTransform;

        [SerializeField] private Vector2 _finalArrowPosition;
        [SerializeField] private float _finalArrowZ;
        [SerializeField] private float _arrowMultiplier = 0.015f;

        private Vector3 _arrowStartLocalPosition;
        private float _arrowStartZRotation;

        private void Start()
        {
            _arrowStartLocalPosition = _arrowTransform.localPosition;
            _arrowStartZRotation = _arrowTransform.eulerAngles.z;
        }

        public void SetValue(float valueNormalized)
        {
            CalculateArrowPositionAndRotation(valueNormalized * _arrowMultiplier);
        }

        private void CalculateArrowPositionAndRotation(float normalizedValue)
        {
            _arrowTransform.localPosition = Vector3.Lerp(_arrowStartLocalPosition, _finalArrowPosition, normalizedValue);
            _arrowTransform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(_arrowStartZRotation, _finalArrowZ, normalizedValue));
        }
    }
}