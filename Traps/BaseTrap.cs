using System;
using Plane;
using UnityEngine;

public abstract class BaseTrap : MonoBehaviour
{
    public Action<BaseTrap> OnDestroyed;
    private float _planeThreshold;

    public virtual void OnSpawned()
    {
    }

    protected virtual void OnDespawned()
    {
        
    }


    private void OnEnable()
    {
        OnDestroyed = null;
    }

    private void OnDisable()
    {
        OnDestroyed = null;
        OnDespawned();
    }

    protected abstract void InteractWithPlayer(PlaneView planeView);

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out PlaneView planeView))
        {
            InteractWithPlayer(planeView);
        }
    }
}