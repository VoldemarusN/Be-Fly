using NaughtyAttributes;
using Plane.PlaneData.Upgrade;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Plane.PlaneData
{
    [CreateAssetMenu(fileName = "New Plane (Rename it!)", menuName = "Configs/Plane", order = 0)]
    public class PlaneConfig : ScriptableObject
    {
        public OilData Oil;
        public PlaneType Type;
        public PlanePhysicConfig PlanePhysicData;
        public int Price;
        public UpgradeData[] Upgrades;
        [TableList] public PassiveUpgradeLevel[] PassiveUpgrades;
        public int RequiredUpgradeCountForMaxLevel;
    }
}