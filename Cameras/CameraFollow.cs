using Cinemachine;
using NaughtyAttributes;
using Plane;
using UnityEngine;
using Zenject;

namespace Cameras
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _sizeMultiplier;
        [SerializeField] private float _smoothness;

        [BoxGroup("References")] [SerializeField]
        private CinemachineVirtualCamera _camera;

        [BoxGroup("References")] [SerializeField]
        private UnityEngine.Camera _cameraBlur;

        private float _startOrthographicSize;
        private CinemachineTransposer _transposer;
        private float _followOffsetXRatio;
        private Transform _planeTransform;
        private IPlaneMovement _planeMovement;

        [Inject]
        public void Construct(Transform planeTransform, IPlaneMovement planeMovement)
        {
            _planeMovement = planeMovement;
            _planeTransform = planeTransform;
        }


        private void Awake()
        {
            _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();

            _startOrthographicSize = _camera.m_Lens.OrthographicSize;
            _followOffsetXRatio = _startOrthographicSize / _transposer.m_FollowOffset.x;
        }

        private void Start() => _camera.Follow = _planeTransform;


     

        private void LateUpdate()
        {
            CalculateOrthographicSize();
            PreserveXOffset();
        }

        private void PreserveXOffset()
        {
            Vector3 offset = _transposer.m_FollowOffset;
            offset.x = _camera.m_Lens.OrthographicSize / _followOffsetXRatio;
            _transposer.m_FollowOffset = offset;
        }

        private void CalculateOrthographicSize()
        {
            float delta = _planeMovement.VelocityMagnitude;
            if (delta < 0) delta = 0;
            var finalSize = _startOrthographicSize + delta * _sizeMultiplier;

            _camera.m_Lens.OrthographicSize =
                Mathf.Lerp(_camera.m_Lens.OrthographicSize, finalSize, Time.deltaTime * _smoothness);
            _cameraBlur.orthographicSize =
                Mathf.Lerp(_camera.m_Lens.OrthographicSize, finalSize, Time.deltaTime * _smoothness);
        }
    }
}