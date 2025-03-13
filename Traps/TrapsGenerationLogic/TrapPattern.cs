using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plane;
using Sirenix.OdinInspector;
using Traps.Wind;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Traps.TrapsGenerationLogic
{
    public class TrapPattern : MonoBehaviour
    {
        public event Action<TrapPattern> OnAllTrapsDeactivated;
        public Action<BaseTrap> OnTrapDestroyed;
        public Action<BaseTrap> OnTrapDestroyedWithPlayer;


        public TrapPatternDifficulty Difficulty;
        public string Hash => _hash;
        public BaseTrap[] Traps { get; private set; }
        public BaseTrap[] MovableTraps { get; private set; }

        public float Width { get; private set; }

        public float Height { get; private set; }


        [SerializeField, ReadOnly] private string _hash;
        [SerializeField] private int _chunkVerticalCount;
        [SerializeField] private float _spaceBetweenChunks;
        [SerializeField, HideInInspector] private Vector3 _localCenter;
        private Vector3 Center => transform.TransformPoint(_localCenter);

        private float _threshold;

        private Transform _planeTransform;
        private float _offsetToDestroy;
        private int _disabledTrapsCount;
        private List<Vector3> _trapsLocalPositions;


        public void GenerateHash()
        {
            _hash = Guid.NewGuid().ToString();
        }


        private void OnEnable() => _disabledTrapsCount = 0;

        private void Update() => HandleCreatedTraps();

        public void InitializeTraps()
        {
            Traps = GetComponentsInChildren<BaseTrap>(true);
            MovableTraps = Traps.Where(trap => trap is MovableTrap).ToArray();
            _trapsLocalPositions = new List<Vector3>();
            foreach (var baseTrap in Traps)
            {
                baseTrap.OnDestroyed += HandleTrapDestroying;
                baseTrap.OnTrapDestroyedWithPlayer += TrapDestroyedByPlayer;
                _trapsLocalPositions.Add(baseTrap.transform.localPosition);
            }

            float minX = Traps.Min(t => t.transform.position.x);
            float maxX = Traps.Max(t => t.transform.position.x);
            Width = maxX - minX;

            float minY = Traps.Min(t => t.transform.position.y);
            float maxY = Traps.Max(t => t.transform.position.y);
            Height = maxY - minY;
        }

        private void TrapDestroyedByPlayer(BaseTrap baseTrap)
        {
            OnTrapDestroyedWithPlayer.Invoke(baseTrap);
        }

        public void ResetMovableTraps()
        {
            for (var index = 0; index < MovableTraps.Length; index++)
            {
                var baseTrap = MovableTraps[index];
                baseTrap.transform.localPosition = _trapsLocalPositions[index];
            }
        }

        public void Compose(Transform planeTransform, float offsetToDestroy)
        {
            _offsetToDestroy = offsetToDestroy;
            _planeTransform = planeTransform;
        }

        public void ComposeTraps(Dictionary<Type, ITrapComposer> trapComposers)
        {
            foreach (var trap in Traps)
            {
                if (trap is IComposableTrap)
                {
                    trapComposers[trap.GetType()].ComposeTrap(trap);
                }
            }
        }

        private void HandleCreatedTraps()
        {
            foreach (var baseTrap in Traps)
            {
                if (!baseTrap.gameObject.activeSelf) continue;
                if (_planeTransform.position.x - baseTrap.transform.position.x >= _offsetToDestroy)
                    HandleTrapDestroying(baseTrap);
            }
        }

        private void HandleTrapDestroying(BaseTrap baseTrap)
        {
            baseTrap.gameObject.SetActive(false);
            OnTrapDestroyed?.Invoke(baseTrap);
            _disabledTrapsCount++;
            if (_disabledTrapsCount == Traps.Length) OnAllTrapsDeactivated?.Invoke(this);
        }


        public void MoveToCenter()
        {
            transform.position += transform.position - Center;
        }

        public void Activate()
        {
            ResetMovableTraps();
            foreach (var trap in Traps)
            {
                trap.OnSpawned();
                trap.gameObject.SetActive(true);
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Center, 1f);
        }


        [Button]
        private void Generate()
        {
            GameObject originChunk = transform.GetChild(0).gameObject;
            if (originChunk.name != "Origin") return;
            ClearGeneratedChunks();
            GenerateChunks(originChunk);
        }


        private void GenerateChunks(GameObject originChunk)
        {
            int from = _chunkVerticalCount % 2 == 0 ? -_chunkVerticalCount / 2 : -(_chunkVerticalCount + 1) / 2;
            for (int i = from; i < _chunkVerticalCount / 2; i++)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + i * _spaceBetweenChunks,
                    0f);
                Instantiate(originChunk, position, Quaternion.identity, transform.GetChild(1));
            }
        }

        private void ClearGeneratedChunks()
        {
            var child = transform.GetChild(1);
            for (int i = child.childCount - 1; i >= 0; i--)
                DestroyImmediate(child.GetChild(i).gameObject);
        }


        [Button]
        private void CalculateCenter()
        {
            Vector3 sumVector = new Vector3(0f, 0f, 0f);

            Transform[] children = GetComponentsInChildren<BaseTrap>().Select(x => x.transform).ToArray();
            foreach (Transform child in children)
            {
                sumVector += child.position;
            }

            _localCenter = sumVector / children.Count();
        }

#endif
    }
}