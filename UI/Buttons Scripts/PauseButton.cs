using UnityEngine;

namespace UI.Buttons_Scripts
{
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseObj;
        private bool _pause;

        public void OnCLick(){
            _pauseObj.SetActive(!_pauseObj.activeSelf);
            _pause = !_pause;
            if (_pause){
                Time.timeScale = 0;
            }
            else{
                Time.timeScale = 1;
            }
       
        }
        private void OnDestroy(){
            Time.timeScale = 1;
        }

    }
}
