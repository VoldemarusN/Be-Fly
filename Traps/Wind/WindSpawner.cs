using System;
using System.Collections;
using System.Collections.Generic;
using Plane;
using Services;
using Traps;
using Traps.TrapsGenerationLogic;
using Traps.Wind;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class WindSpawner : BaseTrap, IComposableTrap
{
    [SerializeField] private float _warningLifetime;
    [SerializeField] private float _lifeDistance;
    [SerializeField] private float _timeToActivateCollider;
    [SerializeField] private float _force;


    [Header("Links")] [SerializeField] private WindType _type;
    [SerializeField] private Wind _badWindPrefab;
    [SerializeField] private Wind _goodWindPrefab;
    [SerializeField] private GameObject _warningGoodPrefab;
    [SerializeField] private GameObject _warningBadPrefab;

    private CameraPositionService _cameraPositionService;
    private GameObject _warningInstance;
    private Wind _windInstance;
    private PlaneView _planeView;
    private float _startPlaneXCoordinateWhenWindWasSpawned;
    private PlaneController _planeController;
    private bool _isInitialized;

    public void Compose(CameraPositionService cameraPositionService, PlaneView planeView, PlaneController planeController)
    {
        _planeView = planeView;
        _planeController = planeController;
        _cameraPositionService = cameraPositionService;
    }


    protected override void InteractWithPlayer(PlaneView planeView)
    {
    }

    private IEnumerator Initialize()
    {
        SpawnWarning();
        yield return new WaitForSeconds(_warningLifetime);
        SpawnWind(_warningInstance.transform.position);
        DestroyWarning();
        yield return CheckDistanceCoroutineForDestroy();
        DestroyWind();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isInitialized == false)
        {
            if (CheckPlanePosition())
            {
                StartCoroutine(Initialize());
                _isInitialized = true;
            }
        }
        else
        {
            UpdatePosition();
            if (_windInstance)
                if (_windInstance.PlaneIsOnTrigger)
                    _planeController.Slow(_force);
        }
    }

    private bool CheckPlanePosition() => _planeView.transform.position.x >= transform.position.x;


    private void UpdatePosition()
    {
        float positionX;
        if (_type == WindType.Bad) positionX = _cameraPositionService.Camera.transform.position.x + _cameraPositionService.GetCameraHalfWidth() - 2;
        else positionX = _cameraPositionService.Camera.transform.position.x - _cameraPositionService.GetCameraHalfWidth() + 2;

        transform.position = new Vector3(positionX, transform.position.y, 0);
    }

    private void SpawnWarning()
    {
        _warningInstance = Instantiate(_type == WindType.Bad ? _warningBadPrefab : _warningGoodPrefab, transform);
    }

    private void DestroyWarning() => Destroy(_warningInstance);

    private void SpawnWind(Vector3 position)
    {
        var windPrefab = _type == WindType.Bad ? _badWindPrefab : _goodWindPrefab;
        _windInstance = Instantiate(windPrefab, position, Quaternion.identity, transform);
        _windInstance.ActivateColliderAfter(_timeToActivateCollider);
    }

    private void DestroyWind() => Destroy(_windInstance.gameObject);

    private IEnumerator CheckDistanceCoroutineForDestroy()
    {
        _startPlaneXCoordinateWhenWindWasSpawned = _planeView.transform.position.x;
        yield return new WaitUntil(() => _planeView.transform.position.x - _startPlaneXCoordinateWhenWindWasSpawned
                                         >= _lifeDistance);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            new Vector3(transform.position.x, -1000, transform.position.z),
            new Vector3(transform.position.x, 1000, transform.position.z));

        switch (_type)
        {
            case WindType.Bad:
                Gizmos.color = Color.red;
                Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
                break;
            case WindType.Good:
                Gizmos.color = Color.green;
                Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
                break;
        }
    }
#endif
}