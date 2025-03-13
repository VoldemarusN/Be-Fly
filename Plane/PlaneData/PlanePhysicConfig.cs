using System;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Plane.PlaneData
{
    [Serializable]
    public class PlanePhysicConfig : ICloneable
    {
        public float ActiveRotationSpeedUp;
        public float PassiveRotationSpeedDown;
        public float DownTorquieIfDirectionUp;
        
        public float ActiveSpeedRight;
        public float PassiveSpeedRight;
        public float MinSpeed;
        public float TrapResistance;

        [HorizontalLine(color: EColor.Blue)] public float LinearDrag;
        public float LinearDragWhenUpDirection;
        public float AngularDrag;


        [Sirenix.OdinInspector.BoxGroup("Угол падения")] [Range(0, -90)]
        public float FallAngle;

        [Range(0, 1)] public float DirectionDownInfluenceToPassiveRightSpeed;


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}