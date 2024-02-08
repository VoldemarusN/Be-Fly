using UnityEngine;

namespace Utilities
{
    public class PlaneFollower : MonoBehaviour
    {
        [SerializeField] protected bool _isVerticalFollow;
        [SerializeField] protected bool _isHorizontalFollow;
        [SerializeField] protected bool _isUseOffset;
        private Vector2 _offset = new Vector2();
        private Vector3 _resultVector;

        public Transform Target;

  

    

        protected virtual void Start()
        {
            _resultVector = transform.position;
            if (_isUseOffset) _offset = transform.position - Target.transform.position;
        }
        protected virtual void Update()
        {
            if (_isVerticalFollow)
            {
                _resultVector.y = Target.position.y;
                _resultVector.y += _offset.y;
            }
            if (_isHorizontalFollow)
            {
                _resultVector.x = Target.position.x;
                _resultVector.x += _offset.x;
            }

            transform.position = _resultVector;
        }
    }
}
