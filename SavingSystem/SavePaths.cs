using UnityEngine;

namespace SavingSystem
{
    internal static class SavePaths
    {
        public const string LastSelectedPlaneType = "LastSelectedPlaneType";
        public const string Configs = "Configs/Plane";
        public const string Main = "SavedData.json";

        public static string PersistantPath => Application.persistentDataPath;
    }
}