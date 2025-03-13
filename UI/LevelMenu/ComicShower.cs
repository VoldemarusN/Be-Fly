using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelMenu
{
    public class ComicShower : MonoBehaviour
    {
        [SerializeField] private Image _content;


        public void SetFrame(Sprite sprite) => _content.sprite = sprite;

        public void Show()
        {
            _content.enabled = true;
            _content.DOFade(1, 1).From(0);
        }
    }
}