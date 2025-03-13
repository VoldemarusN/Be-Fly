using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfig", menuName = "Configs/GlobalConfig", order = 0)]
internal class GlobalConfig : ScriptableObject
{
    public Vector2 TextOffsetOnButtons => _textOffsetOnButtons;

    [Foldout("Text Offset")]
    [SerializeField] private Vector2 _textOffsetOnButtons;
}