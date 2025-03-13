using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        public WindType windType;
        public RedBordersOnHit _redBordersOnHit;


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
            if (col.TryGetComponent(out PlaneView planeView))
            {
                PlaneIsOnTrigger = true;
                if (windType == WindType.Bad) _redBordersOnHit.ShowBordersInWind();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlaneView planeView))
            {
                PlaneIsOnTrigger = false;
                if (windType == WindType.Bad) _redBordersOnHit.HideBordersInWind();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlaneView _))
            {
                if (windType == WindType.Bad)
                    _redBordersOnHit.ShowBordersInWind();
            }
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

        private void OnDestroy()
        {
            if (PlaneIsOnTrigger) 
                _redBordersOnHit.HideBordersInWind();
            StopAllCoroutines();
        }

        public async UniTaskVoid Unfade() => await LerpParticlesMaterials(from: 0, to: 1, 2f);
        public async UniTask Fade() => await LerpParticlesMaterials(from: 1, to: 0, 2f);

        private async UniTask LerpParticlesMaterials(float from, float to, float duration)
        {
            var particleSystemRenderers = _particles.GetComponentsInChildren<ParticleSystemRenderer>();
            var mats = particleSystemRenderers
                .Select(x => x.trailMaterial)
                .Concat(particleSystemRenderers.Select(x => x.material))
                .ToList();

            //remove parent's head mat
            mats.Remove(particleSystemRenderers[0].material);
            
            foreach (var material in mats)
                if (material != null)
                    material.DOFade(to, duration).From(from);

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}