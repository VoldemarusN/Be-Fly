using System;
using Plane.PlaneData;
using Player.Input;
using Scriptable_Objects;
using UnityEngine;
using Zenject;

namespace Plane
{
    public class PlaneMovement : IPlaneMovement
    {
        public float VelocityMagnitude => _view.Rigidbody.velocity.magnitude;
        public float MaxLinearSpeed { get; private set; }
        public bool DestinateTorqueToRightDir { get; set; }

        private readonly PlanePhysicConfig _planePhysicData;
        private readonly PlaneView _view;
        private Vector2 _startCenterOfMass;

        public PlaneMovement(PlanePhysicConfig planePhysicData, PlaneView view)
        {
            _planePhysicData = planePhysicData;
            _view = view;
            InitRigidbody();
            MaxLinearSpeed = Mathf.Sqrt(Mathf.Max(_planePhysicData.ActiveSpeedRight, _planePhysicData.PassiveSpeedRight) / _planePhysicData.LinearDrag);
        }

        public void ClampPlaneSpeed()
        {
            if (_view.Rigidbody.velocity.magnitude < _planePhysicData.MinSpeed)
            {
                _view.Rigidbody.velocity = _view.Rigidbody.velocity.normalized * _planePhysicData.MinSpeed;
            }
        }

        public void ActiveMove()
        {
            AddTorque(_planePhysicData.ActiveRotationSpeedUp);
            AddForce(_planePhysicData.ActiveSpeedRight);

            float planeDownDirDot =
                Vector2.Dot(Quaternion.Euler(Vector3.forward * _planePhysicData.FallAngle) * Vector2.right,
                    _view.transform.right);
            float angle = _view.transform.eulerAngles.z % 360;
            bool moveUp = angle > 90 && angle < 360 + _planePhysicData.FallAngle;
            CalculateLinearDrag(planeDownDirDot);
            AddTorqueIfVerticalDir(planeDownDirDot, moveUp);
        }


        public void PassiveMove()
        {
            float planeDownDirDot =
                Vector2.Dot(Quaternion.Euler(Vector3.forward * _planePhysicData.FallAngle) * Vector2.right,
                    _view.transform.right);

            float angle = _view.transform.eulerAngles.z % 360;
            bool moveUp = angle > 90 && angle < 360 + _planePhysicData.FallAngle;

            if (DestinateTorqueToRightDir)
            {
                if (angle < 360 && angle > 180)
                {
                    AddTorque(_planePhysicData.PassiveRotationSpeedDown * (1 - planeDownDirDot));
                }
            }
            else
            {
                if (moveUp) AddTorque(_planePhysicData.PassiveRotationSpeedDown * (1 - planeDownDirDot));
                else AddTorque(-_planePhysicData.PassiveRotationSpeedDown * (1 - planeDownDirDot));
                AddTorqueIfVerticalDir(planeDownDirDot, moveUp);
            }

            CalculateLinearDrag(planeDownDirDot);
            ApplyPassiveAccelerationRight();
        }

        private void AddTorqueIfVerticalDir(float planeDownDirDot, bool moveUp)
        {
            if (planeDownDirDot > 0) return;
            float force = Mathf.Lerp(0, _planePhysicData.DownTorquieIfDirectionUp, -planeDownDirDot);
            if (moveUp) AddTorque(force);
            else AddTorque(-force);
        }

        private void CalculateLinearDrag(float planeDownDirDot)
        {
            if (planeDownDirDot > 0) _view.Rigidbody.drag = _planePhysicData.LinearDrag;
            else
                _view.Rigidbody.drag = Mathf.Lerp(_planePhysicData.LinearDrag, _planePhysicData.LinearDragWhenUpDirection, -planeDownDirDot);
        }

        public void EnablePhysic()
        {
            _view.Rigidbody.isKinematic = false;
            _view.Rigidbody.gravityScale = 0;
            _view.Rigidbody.centerOfMass = _view.CenterOfMassTransform.position;
        }

        public void DisablePhysic()
        {
            _view.Rigidbody.isKinematic = true;
        }

        public void Ragdoll()
        {
            _view.Rigidbody.gravityScale = 1;
            _view.Rigidbody.centerOfMass = _startCenterOfMass;
        }

        public void ResetAngularSpeed()
        {
            _view.Rigidbody.angularVelocity = 0;
        }

        public void AddForce(float force)
        {
            _view.Rigidbody.AddForce(_view.transform.right * (force * Time.fixedDeltaTime), ForceMode2D.Force);
            AlignViewWithSpeed();
        }

        public void AddForceFromTrap(float force) =>
            AddForce(force * (1 - _planePhysicData.TrapResistance));

        private void AlignViewWithSpeed()
        {
            _view.Rigidbody.velocity = _view.transform.right * _view.Rigidbody.velocity.magnitude;
        }

        private void InitRigidbody()
        {
            Rigidbody2D rigidbody = _view.Rigidbody;
            _startCenterOfMass = rigidbody.centerOfMass;
            rigidbody.centerOfMass = _view.CenterOfMassTransform.localPosition;
            rigidbody.drag = _planePhysicData.LinearDrag;
            rigidbody.angularDrag = _planePhysicData.AngularDrag;
        }


        private void ApplyPassiveAccelerationRight()
        {
            var right = _view.transform.right;
            float dotDown = Mathf.Clamp01(Vector2.Dot(right.normalized, Vector2.down));
            float force = Mathf.Lerp(_planePhysicData.PassiveSpeedRight, dotDown * _planePhysicData.PassiveSpeedRight,
                _planePhysicData.DirectionDownInfluenceToPassiveRightSpeed);
            AddForce(force);
        }

        private void AddTorque(float force) =>
            _view.Rigidbody.AddTorque(force * Time.fixedDeltaTime, ForceMode2D.Force);
    }
}