using Plane;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Garage
{
    public class GaragePlaneView : MonoBehaviour
    {
        public PlaneType PlaneType;
        public PassiveUpgradeProgressBar ProgressBar => _progressBar;
        
        [SerializeField] private Image _image;
        [SerializeField] private Sprite[] _bodySprites;
        [SerializeField] private PassiveUpgradeProgressBar _progressBar;
        
        public void SetLevel(int level) => 
            _image.sprite= _bodySprites[level];
    }
}