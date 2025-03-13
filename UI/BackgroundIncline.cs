using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class BackgroundIncline : MonoBehaviour
    {
        [SerializeField] private float _factor;

        private RawImage _rawImage;
        private void Start()
        {
            _rawImage = GetComponent<RawImage>();
        }
        void Update()
        {
            float inclineX = Input.acceleration.x * _factor;
            float includeXAfterLerp = Mathf.Lerp(_rawImage.uvRect.x, inclineX, 0.1f);
            _rawImage.uvRect = new Rect(new Vector2(includeXAfterLerp, _rawImage.uvRect.y), _rawImage.uvRect.size);
        
        }

    }
}
