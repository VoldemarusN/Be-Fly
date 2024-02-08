using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Traps.TrapsGenerationLogic
{
    public class TrapGenerationSettings : MonoBehaviour
    {
        public List<Zone> Zones;
        public float PlaneOffsetToDestroy;
        public float MinYPosition;
        public TrapPattern[] Patterns;
        
        [Header("StaticTraps")]
        public StaticTrapPref[] StaticTraps;
        public float StaticTrapHightPosition;

        private void OnDrawGizmosSelected()
        {
            DrawStaticTrapDistances();
            DrawZones();
            DrawMinYPosition();
        }

        private void DrawStaticTrapDistances()
        {
            Gizmos.color = Color.red;
            foreach (var trap in StaticTraps)
            {
                Gizmos.DrawLine(new Vector3(trap.Distance,-1000,0), new Vector3(trap.Distance, 1000, 0));
            }
        }

        private void DrawMinYPosition()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-1000, MinYPosition, 0), new Vector3(1000, MinYPosition, 0));
        }


        private void DrawZones()
        {
            foreach (var zone in Zones)
            {
                switch (zone.Difficulty)
                {
                    case TrapPatternDifficulty.Easy:
                        Gizmos.color = Color.green;
                        break;
                    case TrapPatternDifficulty.Medium:
                        Gizmos.color = Color.yellow;
                        break;
                    case TrapPatternDifficulty.Hard:
                        Gizmos.color = Color.red;
                        break;
                }

                Vector3 centerOfCube = new Vector3(zone.XPosition, 0, 0);
                Vector3 sizeOfCube = new Vector3(3, 300, 0);
                Gizmos.DrawCube(centerOfCube, sizeOfCube);
            }
        }

        [Serializable]
        public class Zone
        {
            public TrapPatternDifficulty Difficulty;
            public float XPosition;
        }
    }

    public enum TrapPatternDifficulty
    {
        Easy,
        Medium,
        Hard,
        Booster
    }
}