using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Achievements;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Achievement", menuName = "ScriptableObjects/Achievement", order = 51)]
public class AchievementConfig : ScriptableObject
{
    public string AchieveName;
    public string Description;
    public bool isDone;
    public int CurrentProgress;            
    public int MaxProgress;
    public Sprite Icon;
    [ValueDropdown(nameof(GetValues))] public string Handler;
    [Scene] public string Scene;
    public Plane.PlaneType planeTypeForAchievement = Plane.PlaneType.None;
    public float ValueToReach;

    private string[] GetValues()
    {
        //get all types derived from IAchievementHandler
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IAchievementHandler).IsAssignableFrom(p) && p != typeof(IAchievementHandler))
            .Select(t => t.FullName).ToArray();
    }
}