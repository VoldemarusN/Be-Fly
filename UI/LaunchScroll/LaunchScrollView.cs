using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LaunchForce
{
    public class LaunchScrollView : MonoBehaviour, IScrollView, IHideable
    {
        private Slider _slider;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void SetValue(float value) => _slider.value = value;

        public async UniTask Show() =>
            await _canvasGroup.DOFade(1, 1.5f).AsyncWaitForCompletion().AsUniTask();

        public async UniTask Hide() =>
            await _canvasGroup.DOFade(0, 1.5f).AsyncWaitForCompletion().AsUniTask();

        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();
            _canvasGroup = GetComponentInChildren<CanvasGroup>();
        }
    }
}