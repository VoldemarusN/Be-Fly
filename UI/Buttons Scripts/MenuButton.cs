using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Buttons_Scripts
{
    public class MenuButton : MonoBehaviour
    {

        public void OnCLick(){
            SceneManager.LoadScene(0);
        }
    
    }
}
