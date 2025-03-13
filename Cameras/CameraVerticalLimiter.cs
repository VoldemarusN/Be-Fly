using Cinemachine;
using UnityEngine;

namespace Cameras
{
    public class CameraVerticalLimiter : CinemachineExtension
    {
        [SerializeField] private float _minYPosition;
        

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage != CinemachineCore.Stage.Finalize) return;
            var camPosY = vcam.transform.position.y - state.Lens.OrthographicSize;
            if (camPosY < _minYPosition)
            {
                state.PositionCorrection += Vector3.up * (_minYPosition - camPosY);
            }

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector2(-1000, _minYPosition), new Vector2(1000, _minYPosition));
            Gizmos.color = Color.white;
        }


    }
}
