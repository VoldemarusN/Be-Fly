using TMPro;
using UnityEngine;
using Zenject;

public class LevelMenuInstaller : MonoInstaller
{
    [SerializeField] private TextMeshProUGUI _versionText;


    public override void InstallBindings()
    {
        _versionText.text = Application.version;
    }
}