using System;
using System.Collections.Generic;
using Plane;

namespace SavingSystem
{
    public class StorageData
    {
        public readonly LevelData[] Levels;
        public string LastLevel = "Level 1";
        public readonly Dictionary<PlaneType, PlaneData> PlaneData;
        public Dictionary<string, int> AchievementProgresses;
        public bool CutSceneWasPlayed;


        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                OnMoneyChanged?.Invoke(_money);
            }
        }

        public event Action<int> OnMoneyChanged;

        public PlaneType LastSelectedPlaneType { get; set; } = PlaneType.Cardboard;

        private int _money;
        public bool EndCutSceneWasPlayed;


        public StorageData(Dictionary<PlaneType, PlaneData> planeData, LevelData[] levels)
        {
            PlaneData = planeData;
            Levels = levels;
            AchievementProgresses = new Dictionary<string, int>();
        }
    }
}