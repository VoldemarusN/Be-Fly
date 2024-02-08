using NaughtyAttributes;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "Level Config", menuName = "Configs/Level", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        public LevelAward[] LevelAwards;    
        public float MoneyAmountForMeter;
        public float LevelDistance;
        [Scene] public string SceneName;
    }

    [System.Serializable]
    public class LevelAward
    {
        public int Amount;
        public float Distance;
    }
}