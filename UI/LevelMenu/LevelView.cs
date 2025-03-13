using System;
using DG.Tweening;
using KBCore.Refs;
using NaughtyAttributes;
using SavingSystem;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.LevelMenu
{
    public class LevelView : ValidatedMonoBehaviour
    {
        public LevelConfig Config => _levelConfig;
        public event Action OnClick;

        [SerializeField, Child] private Button _button;
        [SerializeField] private Image _blockedImage;
        [SerializeField] private Image _slider;
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private Transform _rewardContainer;
        [SerializeField] private int _rewardIndex;


        [SerializeField, ReadOnly, ShowAssetPreview]
        private Sprite _preview;

        private bool _isInteractable;
        private int _clickCounter;
        private float _timer;


        private void Start()
        {
            _button.onClick.AddListener(() =>
            {
                _clickCounter++;
                if (!_isInteractable)
                {
                    if (gameObject.GetComponent<Rigidbody2D>()) return;
                    transform.DOShakePosition(0.5f, 5f);
                    return;
                }
                OnClick?.Invoke();
            });
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > 2.3f)
            {
                _timer = 0;
                _clickCounter = 0;
            }
            if (_clickCounter > 8)
            {
                if (gameObject.GetComponent<Rigidbody2D>()) return;
                var component = gameObject.AddComponent<Rigidbody2D>();
                component.gravityScale = 12f;
                var direction = Random.onUnitSphere;
                direction.x = Mathf.Abs(direction.x);
                direction.y = Mathf.Abs(direction.y);
                
                component.AddForce(direction * 300f, ForceMode2D.Impulse);
            }
            
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            _preview = _rewardContainer.GetChild(_rewardIndex).GetComponent<Image>().sprite;
        }


        public float SetProgress(LevelData storageDataLevel)
        {
            float normalizedDistance = storageDataLevel.PassedDistance / Config.LevelDistance;
            _slider.fillAmount = normalizedDistance;
            return normalizedDistance;
        }

        public void ShowReward() => _rewardContainer.GetChild(_rewardIndex).gameObject.SetActive(true);

        private void ShowBlockedImage()
        {
            _blockedImage.gameObject.SetActive(true);
        }

        public void SetInteractivity(bool interactable)
        {
            _isInteractable = interactable;
            if (!interactable) ShowBlockedImage();
        }
    }
}