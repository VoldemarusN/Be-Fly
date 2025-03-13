using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons_Scripts
{
    public class SoundButton: MonoBehaviour
    {
        private bool _isMuted;
        [SerializeField] private Sprite _offSprite;
        [SerializeField] private Sprite _onSprite;

        private Image _image;
        private void Start(){
            _image = GetComponent<Image>();
        }


        public void OnCLick(){
            _isMuted = !_isMuted;
            AudioListener.pause = _isMuted;
            if (_isMuted){
                _image.sprite = _offSprite;
            }
            else{
                _image.sprite = _onSprite;
            }
        }
    


    }
}
