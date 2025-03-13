using System;
using Plane;
using UnityEngine;

namespace Traps.TrapsGenerationLogic
{
    public abstract class MovableTrap : BaseTrap
    {
        [SerializeField] private float _slowForce;
        [SerializeField] protected float _moveSpeed;
        protected abstract void Move();

     
     

        private void Update()
        {
            Move();
        }

        protected override void InteractWithPlayer(PlaneView planeView)
        {
            planeView.Slow(_slowForce);
            OnDestroyed?.Invoke(this);
            OnTrapDestroyedWithPlayer?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}