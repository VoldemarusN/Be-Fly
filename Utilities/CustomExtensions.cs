using UnityEngine;

namespace Utilities
{
    public static class VladExtensions
    {
        public static void InitializeObj(this GameObject obj, Vector3 position, Transform holder)
        {
            obj.transform.position = position;
            obj.transform.parent = holder;
        }
    }
}