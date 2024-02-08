using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    public class Rotatable : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private AnimationCurve _speedCurve;
        [SerializeField] private bool _randomizeRotationOnStart;


        private void Start()
        {
            if (_randomizeRotationOnStart) transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        private void Update()
        {
            transform.Rotate(Vector3.forward, _speedCurve.Evaluate(_rotationSpeed * Time.deltaTime));
        }
    }
}