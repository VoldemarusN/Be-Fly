using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementMenu : MonoBehaviour
{
    [SerializeField] AchievementConfig[] Achievements; // ���� ����� ����� ����, ����� ��������� �������� ����� �������
    [SerializeField] AchievementPanel[] panels;
    private void Start()
    {
        for (int i = 0; i < Achievements.Length; i++)
        {
            panels[i].SetSettings(Achievements[i]);
        }
    }
}
