using System.Linq;
using Plane;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Garage
{
    public class GaragePlaneView : MonoBehaviour
    {
        public PassiveUpgradeProgressBar ProgressBar => _progressBar;
        public PlaneView PlaneView => _planeView;

        [SerializeField] private Image _bodyImage;
        [SerializeField] private Image[] _propellerImages;
        [SerializeField] private PlaneView _planeView;
        [SerializeField] private PassiveUpgradeProgressBar _progressBar;


        public void SetLevel(int level)
        {
            _bodyImage.sprite = _planeView.BodySprites[level];
            foreach (var propellerImage in _propellerImages)
                propellerImage.enabled = false;
            int propellerIndex = Mathf.Clamp(level, 0, _propellerImages.Length - 1);
            _propellerImages[propellerIndex].sprite = _planeView.Propellers.LastOrDefault(x => x.RequiredLevel <= level)?.GetSprites()[0];
            _propellerImages[propellerIndex].enabled = true;
        }
    }
}