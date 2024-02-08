using System;
using System.Collections;
using Infrastructure.AssetManagement;
using Plane;
using UnityEngine;
using UnityEngine.Serialization;

namespace Traps.Wind
{
    public class Wind : MonoBehaviour
    {
        public bool PlaneIsOnTrigger { get; private set; }

        [SerializeField] private Collider2D _collider;
        [SerializeField] private ParticleSystem _particles;


        public void EnableCollider()
        {
            _collider.enabled = true;
        }

        public void DisableCollider()
        {
            _collider.enabled = false;
        }

        protected void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out PlaneView planeView)) PlaneIsOnTrigger = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlaneView planeView)) PlaneIsOnTrigger = false;
        }

        public void ActivateColliderAfter(float timeToActivateCollider)
        {
            StartCoroutine(ActivateColliderCoroutine(timeToActivateCollider));
        }

        private IEnumerator ActivateColliderCoroutine(float timeToActivateCollider)
        {
            DisableCollider();
            yield return new WaitForSeconds(timeToActivateCollider);
            EnableCollider();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _particles.Pause();
            _particles.transform.parent = null;
            _particles.gameObject.AddComponent<DestroyAfterTime>().StartTimer(3);
        }
    }
}