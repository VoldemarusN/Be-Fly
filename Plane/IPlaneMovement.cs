using UnityEngine;

namespace Plane
{
    public interface IPlaneMovement
    {
        float VelocityMagnitude { get; }
        float MaxLinearSpeed { get; }
        void ActiveMove();
        void PassiveMove();
        void EnablePhysic();
        void DisablePhysic();
        void ResetAngularSpeed();
        void AddForce(float force);
        void Ragdoll();
        void ClampPlaneSpeed();
        void AddForceFromTrap(float force);
    }
}