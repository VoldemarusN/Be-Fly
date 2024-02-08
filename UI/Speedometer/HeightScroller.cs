using System;
using DanielLochner.Assets.SimpleScrollSnap;
using Plane;
using UnityEngine;
using Zenject;

namespace UI.Speedometer
{
    public class HeightScroller : MonoBehaviour
    {
        public SimpleScrollSnap ScrollSnap1;
        public SimpleScrollSnap ScrollSnap2;
        public SimpleScrollSnap ScrollSnap3;
        public float Factor;
        public float Factor2;
        public float Factor3;
        [SerializeField] private float _startHeight;


        [Inject] private PlaneView _plane;


        private void Update()
        {
            float height = _plane.transform.position.y - _startHeight;

            SetPosition(height, ScrollSnap1, Factor);
            SetPosition(height, ScrollSnap2, Factor2);
            SetPosition(height, ScrollSnap3, Factor3);
        }

        private void SetPosition(float value, SimpleScrollSnap scrollSnap, float factor)
        {
            scrollSnap.Content.anchoredPosition = -Vector2.up * value * factor;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(new Vector3(0, _startHeight, 0), Vector3.right * 1000);
        }
    }
}