using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plane
{
    [Serializable]
    public class Propeller
    {
        public Texture2D Texture;
        [ValueDropdown("_levels")] public int RequiredLevel = 0;
        private static int[] _levels = { 0, 1, 2 };


        public Sprite[] GetSprites() =>
            Resources.LoadAll<Sprite>(Path.Combine("Plane/Propellers", Texture.name));
    }
}