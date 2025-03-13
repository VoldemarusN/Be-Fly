using UnityEngine;

namespace UI.LevelMenu
{
    [CreateAssetMenu(fileName = "ComicData", menuName = "Data/ComicData", order = 1)]
    internal class ComicData : ScriptableObject
    {
        public Sprite[] Sprites1;
        public Sprite[] Sprites2;
        public Sprite[] Sprites3;
    }
}