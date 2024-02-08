using UnityEngine;
using UnityEngine.Serialization;

namespace Background
{
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _xPercent;

        [SerializeField, Range(0, 1)] private float _yPercent;

        private Transform _cameraTransform;
        private Vector3 _startCameraPosition;
        private Material _layerMaterial;
        private Vector2 _startPosition;

        void Start()
        {
            _cameraTransform = Camera.main.transform;
            _startCameraPosition = _cameraTransform.position;
            _startCameraPosition.y -= Camera.main.orthographicSize;

            _layerMaterial = GetComponent<SpriteRenderer>().material;
            _startPosition = transform.position;
        }

        private void LateUpdate()
        {
            var camPos = _cameraTransform.position;
            camPos.y -= Camera.main.orthographicSize;
            var delta = camPos - _startCameraPosition;

            _layerMaterial.mainTextureOffset = new Vector2(_startPosition.x + delta.x * (1 - _xPercent) * 0.1f, 0);
            transform.position = new Vector3(camPos.x, _startPosition.y + delta.y * _yPercent, 0);
        }
    }
}