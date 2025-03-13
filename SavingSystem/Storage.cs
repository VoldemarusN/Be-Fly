using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plane;
using Plane.PlaneData;
using QFSW.QC;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SavingSystem
{
    public class Storage : IDisposable
    {
        public StorageData StorageData => _storageData;
        private StorageData _storageData;


        public Storage()
        {
            JObject data = LoadData();
            if (data != null) _storageData = data.ToObject<StorageData>();
            else InitializeDefaultStorageData();
        }

        public void InitializeDefaultStorageData()
        {
            Dictionary<PlaneType, PlaneData> planeData = new Dictionary<PlaneType, PlaneData>();
            planeData.Add(PlaneType.Cardboard, new PlaneData() { IsUnlocked = true });
            planeData.Add(PlaneType.Plastic, new PlaneData());
            planeData.Add(PlaneType.Wood, new PlaneData());
            LevelData[] levels = new LevelData[]
            {
                new LevelData("Level 1", 0f),
                new LevelData("Level 2", 0),
                new LevelData("Level 3", 0)
            };
            _storageData = new StorageData(planeData, levels);
        }

        public PlaneData GetPlaneData(PlaneType planeType)
        {
            if (StorageData.PlaneData.TryGetValue(planeType, out var data)) return data;
            StorageData.PlaneData[planeType] = new PlaneData();
            return StorageData.PlaneData[planeType];
        }


        private void Save()
        {
/*#if STEAMWORKS
            SteamRemoteStorage.FileWrite("save.json",
                Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(StorageData)));
#else*/
            using StreamWriter sw = new StreamWriter(Path.Combine(SavePaths.PersistantPath, "SavedData.json"));
            if (File.Exists(Path.Combine(SavePaths.PersistantPath, "SavedData.json")) == false)
                File.Create(Path.Combine(SavePaths.PersistantPath, "SavedData.json"));
            sw.Write(JsonConvert.SerializeObject(StorageData));
            sw.Close();
//#endif
        }

        public JObject LoadData()
        {
            string path = Path.Combine(SavePaths.PersistantPath, "SavedData.json");
            if (File.Exists(path) == false) return null;

            using StreamReader sr = new StreamReader(path);
            if (File.Exists(Path.Combine(SavePaths.PersistantPath, "SavedData.json")))
            {
                try
                {
                    return JObject.Parse(sr.ReadToEnd());
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public void Dispose()
        {
            Save();
        }

        public int GetAchievementProgress(AchievementConfig config)
        {
            StorageData.AchievementProgresses.TryAdd(config.name, 0);
            return StorageData.AchievementProgresses[config.name];
        }

        public bool AchievementIsDone(AchievementConfig config) =>
            GetAchievementProgress(config) >= config.MaxProgress;
    }
}