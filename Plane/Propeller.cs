using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plane
{
    [Serializable]
    internal class Propeller
    {
        public Texture2D Texture;
        [ValueDropdown("_levels")]
        public int RequiredLevel = 0;
        private static int[] _levels = { 0, 1, 2 };
        
    }
}