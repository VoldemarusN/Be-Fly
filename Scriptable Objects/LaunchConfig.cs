using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Configs/Create LaunchConfig", fileName = "LaunchConfig")]
    public class LaunchConfig : ScriptableObject
    {
        [CurveRange(0, 0, 1, 1, EColor.Blue)] public AnimationCurve CurveOfSpeed;
        public float HandleSpeed;
        [FormerlySerializedAs("WindBoostEffectRange")] public Range BoostEffectRange;
        public float Force;
        public float Angle;
        
        public float BoostDuration;
        public float BoostForce;
    }

    [Serializable]
    public struct Range
    {
        public float Min;
        public float Max;

        public Range(float max, float min)
        {
            Max = max;
            Min = min;
        }
    }
}