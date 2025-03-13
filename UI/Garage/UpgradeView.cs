using System;
using System.Collections.Generic;
using Plane.PlaneData.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.View
{
    public class UpgradeView : MonoBehaviour
    {
        public event Action<UpgradeView> OnClick;

        public string CharacteristicName;
        [SerializeField] private Transform _bulletsParent;
        [SerializeField] private Sprite _upgradedBulletSprite;
        [SerializeField] private Sprite _defaultBulletSprite;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _costText;

        [SerializeField] private LocalizedString _name;
        [SerializeField] private LocalizedString _completedText;

        private Button _button;
        private List<Image> _bullets;

        private void Awake()
        {
            _bullets = new List<Image>();
            _button = GetComponent<Button>();
            _nameText.text = _name.GetLocalizedString();
        }


        public void SetCost(int cost) => _costText.text = cost.ToString();


        public void SetMaxLevel(int level)
        {
            foreach (var bullet in _bullets) Destroy(bullet.gameObject);


            for (var i = 0; i < level; i++)
            {
                GameObject o = new GameObject("Bullet");
                o.transform.SetParent(_bulletsParent);
                o.transform.localScale = Vector3.one;
                Image image = o.AddComponent<Image>();
                image.sprite = _defaultBulletSprite;
                _bullets.Add(image);
            }
        }

        public void SetLevel(int level)
        {
            for (var i = 0; i < _bullets.Count && i < level; i++) _bullets[i].sprite = _upgradedBulletSprite;
            for (var i = level; i < _bullets.Count; i++) _bullets[i].sprite = _defaultBulletSprite;
        }


        private void OnEnable() => _button.onClick?.AddListener(InvokeOnClickEvent);
        private void OnDisable() => _button.onClick?.RemoveListener(InvokeOnClickEvent);

        private void OnDestroy()
        {
            OnClick = null;
        }

        private void InvokeOnClickEvent() => OnClick?.Invoke(this);

        public void SetInteractivity(bool b)
        {
            _button.interactable = b;
        }

        public void Complete()
        {
            _completedText.GetLocalizedStringAsync().Completed += (x) => _costText.text = x.Result;
        }
    }
}