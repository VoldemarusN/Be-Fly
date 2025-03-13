using System.Collections.Generic;
using Plane.PlaneData.Upgrade;

namespace SavingSystem
{
    public class PlaneData
    {
        public float TrapResistance { get; set; } = 0.0f;
        public bool IsUnlocked { get; set; } = false;
        public Dictionary<string, int> CharacteristicLevels { get; set; }
        public int Level { get; set; }
        public float LevelNormalized { get; set; }

        public PlaneData()
        {
            CharacteristicLevels = new Dictionary<string, int>();
        }
    }
}