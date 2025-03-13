using UnityEngine;

namespace UI.Buttons_Scripts
{
    public class MenuButtonFunctions : MonoBehaviour
    {
        [SerializeField] private Animator _settingButtonAnimator;
        private bool _settingsIsActivated;


        public void OnClickSettingButton(){
            if (_settingButtonAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_settingButtonAnimator.IsInTransition(0)){
                if (_settingsIsActivated){
                    _settingsIsActivated = false;
                    _settingButtonAnimator.Play("Close_Settings");
                    return;
                }
                _settingsIsActivated = true;
                _settingButtonAnimator.Play("Open_Settings");
            }
        }

    
    
    
    
    }
}
