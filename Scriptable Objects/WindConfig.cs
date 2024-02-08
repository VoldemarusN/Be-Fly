using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Create WindConfig", fileName = "WindConfig", order = 0)]
    public class WindConfig : ScriptableObject
    {
        public float MaxSpeedReachTime;
        public float FallAngle;
        public float WindForce;
    }
}