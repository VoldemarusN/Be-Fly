using System;
using System.Collections;
using System.Collections.Generic;
using Plane;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class GroundGenerator : MonoBehaviour
{
    [SerializeField] private Transform _startChunkPoint;

    [SerializeField] private GameObject _groundPrefab;


    [SerializeField] private float _offsetToCreateNewChunk = 10f;
    [SerializeField] private float _offsetToDestroyNewChunk = 10f;


    private IObjectPool<GameObject> _groundPool;
    private readonly List<GameObject> _createdChunks = new List<GameObject>();

    private Transform _planeTransform;
    private float _nextCoordinate;
    private float _chunkWidth;
    private Vector2 _startChunkPosition;
    private Vector2 _lastLeftChunkPosition; // îòïðàâíàÿ òî÷êà ñîçäàíèÿ ÷àíêîâ ïðè ïîëåòå íàçàä

    [Inject]
    public void Construct(Transform transform) => _planeTransform = transform;

    private void Awake()
    {
        _groundPool = new ObjectPool<GameObject>(CreateGroundItem, OnGroundGotten,defaultCapacity: 20);

        _chunkWidth = _groundPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x * _groundPrefab.transform.localScale.x;
        _startChunkPosition = _startChunkPoint.position;
        _lastLeftChunkPosition = _startChunkPosition;
        _nextCoordinate = _startChunkPosition.x;
    }

    private void OnGroundGotten(GameObject obj) => obj.gameObject.SetActive(true);

    private GameObject CreateGroundItem()
    {
        var groundInstance = Instantiate(_groundPrefab, transform);
        groundInstance.gameObject.SetActive(false);
        return groundInstance;
    }

    private void Update()
    {
        if (CanCreateNewChunk())
        {
            _nextCoordinate += _chunkWidth;
            CreateNewChunk(GetNewPosition(_nextCoordinate));
        }

        HandleCreatedChunks();

        if (IsPlanePositionEqualsLastLeftChunk())
        {
            _lastLeftChunkPosition.x = _createdChunks[0].gameObject.transform.position.x;
            CreateNewBackChunk(GetNewPosition(_lastLeftChunkPosition.x - _chunkWidth));
        }
    }


    private void HandleCreatedChunks()
    {
        for (var i = 0; i < _createdChunks.Count; i++)
        {
            var chunk = _createdChunks[i];
            if (_planeTransform.position.x - chunk.transform.position.x >= _offsetToDestroyNewChunk) 
                DestroyChunk(chunk);
        }
    }

    private bool IsPlanePositionEqualsLastLeftChunk() =>
        _planeTransform.position.x < _createdChunks[0].gameObject.transform.position.x;

    private bool CanCreateNewChunk() =>
        _planeTransform.position.x + _offsetToCreateNewChunk >= _nextCoordinate;

    private Vector2 GetNewPosition(float NextCoordinate)
    {
        var position = _startChunkPosition;
        position.x = NextCoordinate;
        return position;
    }

    private void CreateNewChunk(Vector2 position)
    {
        var groundInstance = _groundPool.Get();
        groundInstance.transform.position = position;
        _createdChunks.Add(groundInstance);
    }

    private void CreateNewBackChunk(Vector2 position) // ñîçäàåò ÷àíê è äîáàâëÿåò åãî â ÍÀ×ÀËÎ ëèñòà
    {
        var groundInstance = _groundPool.Get();
        groundInstance.transform.position = position;
        _createdChunks.Insert(0, groundInstance);
    }

    private void DestroyChunk(GameObject chunk)
    {
        _groundPool.Release(chunk);
        _createdChunks.Remove(chunk);
    }
}