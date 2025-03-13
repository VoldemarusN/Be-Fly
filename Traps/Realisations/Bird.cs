using System;
using Traps.TrapsGenerationLogic;
using UnityEngine;

namespace Traps
{
    public class Bird : MovableTrap
    {
        [SerializeField] private float _amplitude;
        [Header("Up")] [SerializeField] private float _upMoveAngle;

        [SerializeField] private float _upMoveSpeed;
        [SerializeField] private AnimationCurve _upMoveCurve;

        [Header("Down")] [SerializeField] private float _downMoveAngle;
        [SerializeField] private AnimationCurve _downMoveCurve;

        [SerializeField] private float _downMoveSpeed;

        private float _normalizedProgress;
        private bool _isMovingUp = true;
        private Vector3 _destination;
        private Vector3 _previousDestination;
        private Vector3 _startPosition;


        public override void OnSpawned()
        {
            _startPosition = transform.position;
            _destination = _startPosition;
            CalculateDestination();
        }
 
        protected override void Move()
        {
            ProcessMove();
            if (_normalizedProgress >= 1) ChangeDestination();

            /*_destination.x += _newDeltaX;
            _previousPosition += new Vector3(_newDeltaX, 0, 0);
            transform.position += new Vector3(_newDeltaX, 0, 0);*/
        }

        private void ProcessMove()
        {
            if (_isMovingUp) _normalizedProgress += _upMoveCurve.Evaluate(_upMoveSpeed) * Time.fixedDeltaTime;
            else _normalizedProgress += _downMoveCurve.Evaluate(_downMoveSpeed) * Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(_previousDestination, _destination, _normalizedProgress);
        }

        private void ChangeDestination()
        {
            _isMovingUp = !_isMovingUp;
            _normalizedProgress = 0;

            CalculateDestination();
        }

        private void CalculateDestination()
        {
            _previousDestination = _destination;
            if (_isMovingUp)
            {
                Vector2 normalizedVectorUp = Quaternion.AngleAxis(-_upMoveAngle, Vector3.forward) * Vector2.left;
                float hypotenuse = _amplitude / Mathf.Cos(Mathf.Deg2Rad * (90 - _upMoveAngle));
                _destination = (Vector2)_previousDestination + normalizedVectorUp * hypotenuse;
            }
            else
            {
                Vector2 normalizedVectorDown = Quaternion.AngleAxis(_downMoveAngle, Vector3.forward) * Vector2.left;
                float hypotenuseDown = _amplitude / Mathf.Cos(Mathf.Deg2Rad * (90 - _downMoveAngle));
                _destination = (Vector2)_previousDestination + normalizedVectorDown * hypotenuseDown;
            }
        }


        private void OnValidate()
        {
            if (Application.isPlaying == false) _startPosition = transform.position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(transform.position.x - 1000, _startPosition.y + _amplitude), new Vector3(transform.position.x + 1000, _startPosition.y + _amplitude));
            Gizmos.DrawLine(new Vector3(transform.position.x - 1000, _startPosition.y), new Vector3(transform.position.x + 1000, _startPosition.y));

            Gizmos.color = Color.green;

            Vector2 normalizedVectorUp = Quaternion.AngleAxis(-_upMoveAngle, Vector3.forward) * Vector2.left;
            Vector2 normalizedVectorDown = Quaternion.AngleAxis(_downMoveAngle, Vector3.forward) * Vector2.left;


            float hypotenuse = _amplitude / Mathf.Cos(Mathf.Deg2Rad * (90 - _upMoveAngle));
            Gizmos.DrawLine(_startPosition, (Vector2)_startPosition + normalizedVectorUp * hypotenuse);
            float hypotenuseDown = _amplitude / Mathf.Cos(Mathf.Deg2Rad * (90 - _downMoveAngle));
            Gizmos.DrawLine((Vector2)_startPosition + normalizedVectorUp * hypotenuse,
                ((Vector2)_startPosition + normalizedVectorUp * hypotenuse) + normalizedVectorDown * hypotenuseDown);
        }
    }
}