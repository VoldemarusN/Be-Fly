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
        
        protected override void Move()
        {
            transform.position += new Vector3(_moveSpeed,
                Mathf.Sin(Time.time * _frequency) * _amplitude) * Time.deltaTime;
        }


#if UNITY_EDITOR



        [Header("Debug")] [SerializeField] private int _resolution;

        private void OnDrawGizmosSelected()
        {
            /*Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(transform.position.x - 1000, _startPosition.y + _amplitude),
                new Vector3(transform.position.x + 1000, _startPosition.y + _amplitude));
            Gizmos.DrawLine(new Vector3(transform.position.x - 1000, transform.position.y),
                new Vector3(transform.position.x + 1000, transform.position.y));
            DrawSin();*/
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