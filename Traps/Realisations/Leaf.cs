using System;
using Traps.TrapsGenerationLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Traps
{
    public class Leaf : MovableTrap
    {
        [SerializeField] private float _amplitude;
        [SerializeField] private float _frequency;


        private Vector2 _startPosition;


        public override void OnSpawned()
        {
            _startPosition = transform.position;
        }

        protected override void Move()
        {
            transform.position = new Vector2(transform.position.x - _moveSpeed * Time.deltaTime,
                Mathf.Sin(Time.time * _frequency) * _amplitude + _startPosition.y);
        }


#if UNITY_EDITOR

        private void OnValidate()
        {
            if (Application.isPlaying == false) _startPosition = transform.position;
        }

        [Header("Debug")] [SerializeField] private int _resolution;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            //Draw amplitude 
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(transform.position.x - 1000, _startPosition.y + _amplitude),
                new Vector3(transform.position.x + 1000, _startPosition.y + _amplitude));
            Gizmos.DrawLine(new Vector3(transform.position.x - 1000, transform.position.y),
                new Vector3(transform.position.x + 1000, transform.position.y));
            DrawSin();
        }

        private void DrawSin()
        {
            Gizmos.color = Color.green;


            /*Vector3 previousPosition = _startPosition;
            int i = 0;
            while (i < _resolution)
            {
                Vector3 pos = previousPosition - new Vector3(i * 0.5F, Mathf.Sin(i * _frequency) * _amplitude, 0);
                Gizmos.DrawLine(previousPosition,pos);
                previousPosition = pos;
                i++;
            }*/
        }


#endif
    }
}