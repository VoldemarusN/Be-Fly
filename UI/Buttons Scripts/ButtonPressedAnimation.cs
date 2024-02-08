using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons_Scripts
{
    public class ButtonPressedAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Vector3 _startScale;
        private void Start(){
            _startScale = transform.localScale;
        }
        public void OnPointerDown(PointerEventData eventData){
            transform.localScale = new Vector3(transform.localScale.x * 0.92f, transform.localScale.y * 0.92f, 1);
        }


        public void OnPointerUp(PointerEventData eventData){
            transform.localScale =  _startScale;
        }
    }
}
