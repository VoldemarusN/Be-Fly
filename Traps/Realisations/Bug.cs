using System;
using Traps.TrapsGenerationLogic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Traps
{
    public class Bug : MovableTrap
    {
        [SerializeField] private float _amplitude;
        [SerializeField] private float _minMoveDistance;
        [SerializeField] private float _maxMoveDistance;
        [SerializeField] private float _maxSpread;
        [SerializeField] private float _minSpread;


        private Vector3 _destination;
        private Vector3 _previousPosition;
        private float _normalizedProgress;
        private Vector3 _startPosition;


        public override void OnSpawned()
        {
            _startPosition = transform.position;
            _previousPosition = _startPosition;
            _destination = _previousPosition;
            CalculateDestination();
        }

        protected override void Move()
        {
            ProcessMove();
            if (_normalizedProgress >= 1) CalculateDestination();
        }

        private void CalculateDestination()
        {
            _previousPosition = _destination;
            bool isUp = Random.Range(0, 2) == 0;
            float angle = Random.Range(_minSpread, _maxSpread);
            float distance = Random.Range(_minMoveDistance, _maxMoveDistance);

            if (isUp)
                _destination = _previousPosition + Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.left * distance;
            else
                _destination = _previousPosition + Quaternion.AngleAxis(-angle, Vector3.forward) * Vector3.left * distance;

            if (_destination.y > _startPosition.y + _amplitude / 2)
                _destination = _previousPosition + Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.left * distance;
            else if (_destination.y < _startPosition.y - _amplitude / 2)
                _destination = _previousPosition + Quaternion.AngleAxis(-angle, Vector3.forward) * Vector3.left * distance;

            _normalizedProgress = 0;
        }

        private void ProcessMove()
        {
            _normalizedProgress += _moveSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(_previousPosition, _destination, _normalizedProgress);
        }


#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            DrawAmplitude();
            DrawSpread();
            DrawDistance();
        }

        private void DrawAmplitude()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(transform.position.x - 1000, _startPosition.y + _amplitude / 2), new Vector3(transform.position.x + 1000, _startPosition.y + _amplitude / 2));
            Gizmos.DrawLine(new Vector3(transform.position.x - 1000, _startPosition.y - _amplitude / 2), new Vector3(transform.position.x + 1000, _startPosition.y - _amplitude / 2));
        }

        private void DrawDistance()
        {
            //min Distance
            Gizmos.color = Color.red;
            Vector3 upPointOnMinLine = transform.position + Quaternion.AngleAxis(_minSpread, Vector3.forward) * Vector3.left * _minMoveDistance;
            Vector3 upPointOnMaxLine = transform.position + Quaternion.AngleAxis(_maxSpread, Vector3.forward) * Vector3.left * _minMoveDistance;
            Gizmos.DrawLine(upPointOnMinLine, upPointOnMaxLine);

            Vector3 downPointOnMinLine = transform.position + Quaternion.AngleAxis(-_minSpread, Vector3.forward) * Vector3.left * _minMoveDistance;
            Vector3 downPointOnMaxLine = transform.position + Quaternion.AngleAxis(-_maxSpread, Vector3.forward) * Vector3.left * _minMoveDistance;
            Gizmos.DrawLine(downPointOnMinLine, downPointOnMaxLine);

            //max Distance
            Gizmos.color = Color.green;
            Vector3 upPointOnMinLine2 = transform.position + Quaternion.AngleAxis(_minSpread, -Vector3.forward) * Vector3.left * _maxMoveDistance;
            Vector3 upPointOnMaxLine2 = transform.position + Quaternion.AngleAxis(_maxSpread, -Vector3.forward) * Vector3.left * _maxMoveDistance;
            Gizmos.DrawLine(upPointOnMinLine2, upPointOnMaxLine2);

            Vector3 downPointOnMinLine2 = transform.position + Quaternion.AngleAxis(-_minSpread, -Vector3.forward) * Vector3.left * _maxMoveDistance;
            Vector3 downPointOnMaxLine2 = transform.position + Quaternion.AngleAxis(-_maxSpread, -Vector3.forward) * Vector3.left * _maxMoveDistance;
            Gizmos.DrawLine(downPointOnMinLine2, downPointOnMaxLine2);
        }

        private void DrawSpread()
        {
            //min spread
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(_minSpread, Vector3.forward) * Vector3.left * 2f);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(_minSpread, -Vector3.forward) * Vector3.left * 2f);
            //max spread
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(_maxSpread, Vector3.forward) * Vector3.left * 2f);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(_maxSpread, -Vector3.forward) * Vector3.left * 2f);
        }
#endif
    }
}