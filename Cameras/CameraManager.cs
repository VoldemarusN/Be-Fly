using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

namespace Cameras
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _scalePercents;
        [SerializeField] private float _scaleSpeed;


        private CinemachineTransposer _transposer;

        private void Start()
        {
            _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();

        }

        public async void EnterLaunchState()
        {
            float targetOSize = _camera.m_Lens.OrthographicSize * (1 + (_scalePercents / 100));
            float targetOffsetSize = _transposer.m_FollowOffset.x * (1 + (_scalePercents / 100));
            float step = Time.fixedDeltaTime * _scaleSpeed;
            int stepsAmount = (int)((targetOSize - _camera.m_Lens.OrthographicSize) / step) + 1;
            float offsetStep = (targetOffsetSize - _transposer.m_FollowOffset.x) / stepsAmount;


            for (int i = 0; i < stepsAmount; i++)
            {
                _camera.m_Lens.OrthographicSize += step;
                _transposer.m_FollowOffset += Vector3.right * offsetStep;

                await Task.Yield();
            }
            _camera.m_Lens.OrthographicSize = targetOSize;
            _transposer.m_FollowOffset =
                new Vector3(targetOffsetSize, _transposer.m_FollowOffset.y, _transposer.m_FollowOffset.z);
        }
    }



}
