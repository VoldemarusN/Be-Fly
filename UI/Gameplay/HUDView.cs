using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private MaskableGraphic _oilIndicatorImage;
        [SerializeField] private MaskableGraphic _crashIndicatorImage;

        [SerializeField] private OilScrollView _oilScrollView;
        [SerializeField] private ArrowSpeedScrollView _arrowSpeedScrollView;

        private Coroutine _pingRoutine;

        public void SetOil(float normalizedValue) => _oilScrollView.SetValue(normalizedValue);
        public void SetSpeed(float value) => _arrowSpeedScrollView.SetValue(value);

        public void SetOilIndicatorActivity(bool isActive) =>
            SetIndicatorActivity(isActive, _oilIndicatorImage);

        public void SetCrashIndicatorActivity(bool isActive) =>
            SetIndicatorActivity(isActive, _crashIndicatorImage);


        private void SetIndicatorActivity(bool isActive, MaskableGraphic indicatorImage)
        {
            if (isActive && indicatorImage.enabled == false)
            {
                indicatorImage.enabled = true;
                _pingRoutine = StartCoroutine(PingImageCoroutine(indicatorImage));
            }
            else if (isActive == false && indicatorImage.enabled)
            {
                if (_pingRoutine != null) StopCoroutine(_pingRoutine);
                indicatorImage.enabled = false;
            }
        }

        private IEnumerator PingImageCoroutine(MaskableGraphic oilIndicatorImage)
        {
            while (true)
            {
                yield return oilIndicatorImage.DOFade(1, 0.3f).WaitForCompletion();
                yield return oilIndicatorImage.DOFade(0, 0.3f).WaitForCompletion();
            }
        }
    }
}