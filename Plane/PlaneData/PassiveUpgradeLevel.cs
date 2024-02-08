using System;

namespace Plane.PlaneData
{
    [Serializable]
    public class PassiveUpgradeLevel
    {
        [Sirenix.OdinInspector.ProgressBar(0, 1, b: 1)]
        public float TrapResistance;
    }
}