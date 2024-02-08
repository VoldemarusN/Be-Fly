using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UI.LaunchForce;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class OilScrollView : MonoBehaviour, IScrollView
    {
        [SerializeField] private Image _oilImage;
        [SerializeField] private float _oilTickValue;

        [SerializeField, Range(0, 1)] private float _oilNormalizedValueDebug;
        
        
        public void SetValue(float valueNormalized)
        {
            _oilImage.fillAmount = Mathf.RoundToInt(valueNormalized / _oilTickValue) * _oilTickValue;
        }
            
        [Button]
        private void SetValueDebug()
        {
            SetValue(_oilNormalizedValueDebug);
        }
        
    }
}