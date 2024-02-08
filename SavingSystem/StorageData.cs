using System;
using System.Collections.Generic;
using Plane;

namespace SavingSystem
{
    public class StorageData
    {
        public readonly LevelData[] Levels;
        public int LastLevel;
        public readonly Dictionary<PlaneType, PlaneData> PlaneData;
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

        public PlaneType LastSelectedPlaneType { get; set; }

        private int _money;


        public StorageData(Dictionary<PlaneType, PlaneData> planeData, LevelData[] levels)
        {
            PlaneData = planeData;
            Levels = levels;
        }
    }
}