using System;
using System.Collections;
using System.IO;
using System.Linq;
using GameComponents;
using Traps;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Plane
{
    public class PlaneView : MonoBehaviour, ISlowable
    {
        public event Action<float> OnSlowed;
        public event Action OnTouchedGround;
        public float Speed => Rigidbody.velocity.magnitude;
        public bool IsCrashed { get; private set; }
        public AudioClip[] TouchGroundSounds;

        public float ActiveSoundGrowDuration;
        public AudioClip ActiveSound;
        public AudioClip TouchTrapSound;
        public float TrapPitchVariance;
        public Propeller[] Propellers => _propellers;
        public Sprite[] BodySprites => _bodySprites;


        [SerializeField] private float _durationBetweenPropellerFramesInSeconds;
        [SerializeField] private Sprite[] _bodySprites;
        [SerializeField] private Propeller[] _propellers;

        [SerializeField] public PlaneType Type;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _centerOfMassTransform;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private PolygonCollider2D _collider;
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private SpriteRenderer _propellerSpriteRenderer;

        private Sprite[] _currentPropellerSprites;
        private int _currentPropellerFrame;
        private int[] _backFrameOrder = new[] { 0, 3, 1, 2, 4, 5, 6 };
        private bool _proppellerIsStopped = true;
        private Coroutine _stopPropellerCoroutine;
        private Coroutine _startPropellerCoroutine;
        private YieldInstruction _propellerYieldInstruction;

        public Transform CenterOfMassTransform => _centerOfMassTransform;
        public Rigidbody2D Rigidbody => _rigidbody;


        private void Start() =>
            _propellerYieldInstruction = new WaitForSeconds(_durationBetweenPropellerFramesInSeconds);

        public void SetLevel(int level)
        {
            _bodySpriteRenderer.sprite = _bodySprites[level];
            Destroy(_collider);
            _collider = gameObject.AddComponent<PolygonCollider2D>();

            Propeller propeller = _propellers.LastOrDefault(x => x.RequiredLevel <= level);
            if (propeller == null) propeller = _propellers[0];
            _currentPropellerSprites = propeller.GetSprites();
            _propellerSpriteRenderer.sprite = _currentPropellerSprites[0];
        }


        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ground"))
            {
                OnTouchedGround?.Invoke();
                if (_particle != null && !_particle.isPlaying) _particle.Play();
                IsCrashed = true;
            }
        }

        private void Update()
        {
            if (IsCrashed)
                if (_particle != null)
                    _particle.transform.eulerAngles = new Vector3(-90, 0, 0);
        }

        public void StopPlane()
        {
            StopPropeller();
        }

        public void SetTransparent()
        {
            gameObject.layer = 13;
        }


        public void StopPropeller()
        {
            if (_proppellerIsStopped) return;
            if (_startPropellerCoroutine != null) StopCoroutine(_startPropellerCoroutine);
            _proppellerIsStopped = true;
            _stopPropellerCoroutine = StartCoroutine(StopPropellerCoroutine());
            if (_particle != null && !_particle.isPaused) _particle.Pause();
        }


        public void StartPropeller()
        {
            if (_proppellerIsStopped == false) return;
            if (_stopPropellerCoroutine != null) StopCoroutine(_stopPropellerCoroutine);
            _proppellerIsStopped = false;
            _startPropellerCoroutine = StartCoroutine(StartPropellerCoroutine());
            if (_particle != null && !_particle.isPlaying) _particle.Play();
        }


        private IEnumerator StartPropellerCoroutine()
        {
            while (true)
            {
                if (_currentPropellerFrame >= _currentPropellerSprites.Length)
                    _currentPropellerFrame -= 3;
                _propellerSpriteRenderer.sprite = _currentPropellerSprites[_currentPropellerFrame];
                _currentPropellerFrame++;
                yield return _propellerYieldInstruction;
            }
        }

        private IEnumerator StopPropellerCoroutine()
        {
            _currentPropellerFrame = _backFrameOrder.Length - 1;
            while (_currentPropellerFrame > 0)
            {
                _propellerSpriteRenderer.sprite = _currentPropellerSprites[_backFrameOrder[_currentPropellerFrame]];
                _currentPropellerFrame--;
                yield return _propellerYieldInstruction;
            }

            _currentPropellerFrame = 0;
        }


        public void Slow(float force) => OnSlowed?.Invoke(force);


#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(_centerOfMassTransform.position, Vector3.one * 0.05f);
            Gizmos.DrawRay(transform.position, _rigidbody.velocity * _rigidbody.velocity.magnitude * 0.01f);
        }
#endif
    }
}