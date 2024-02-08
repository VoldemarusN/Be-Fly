using Sirenix.OdinInspector;
using UI.View;
using UnityEngine;

namespace Plane.PlaneData.Upgrade
{
    [CreateAssetMenu(fileName = "New UpgradeData", menuName = "Configs/Upgrade")]
    public class UpgradeData : ScriptableObject
    {
        public UpgradeView View;
        public UpgradeCharacteristic Characteristic;
        [TableList] public UpgradeLevel[] Levels;
    }
}