using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Traps.TrapsGenerationLogic
{
    public class TrapGenerationSettings : MonoBehaviour
    {
        public float StartGenerationXCoordinate;
        public float DelayBetweenPatterns;
        [Header("Chances")] [Range(0, 100)] public float HardTrapChance;

        [PropertyRange(0, nameof(GetEasyChance))]
        public float EasyTrapChance;

        [PropertyRange(0, nameof(GetBoostChance))]
        public float BoostChance;

        [HorizontalLine(color: EColor.Blue)] public float PlaneOffsetToDestroy;
        public float MinYPosition;
        public TrapPattern[] Patterns;

        [Header("StaticTraps")] public StaticTrapPref[] StaticTraps;
        public float StaticTrapHightPosition;


        private float GetEasyChance() => 100 - HardTrapChance;
        private float GetBoostChance() => 100 - HardTrapChance - EasyTrapChance;

        private void OnDrawGizmosSelected()
        {
            DrawStaticTrapDistances();
            DrawMinYPosition();
            DrawStartXCoordinate();
        }

        private void DrawStartXCoordinate()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(StartGenerationXCoordinate, -1000, 0), new Vector3(StartGenerationXCoordinate, 1000, 0));
        }

        private void DrawStaticTrapDistances()
        {
            Gizmos.color = Color.red;
            foreach (var trap in StaticTraps)
            {
                Gizmos.DrawLine(new Vector3(trap.Distance, -1000, 0), new Vector3(trap.Distance, 1000, 0));
            }
        }

        private void DrawMinYPosition()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-1000, MinYPosition, 0), new Vector3(1000, MinYPosition, 0));
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