using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Gameplay
{
    public class WinWindow : EndGameWindow
    {
        [SerializeField] private Button _levelButton;


        [Inject]
        public void Construct(SceneLoader loader) =>
            _levelButton.onClick.AddListener(() => loader.LoadScene(SceneType.LevelMenu));
    }
}