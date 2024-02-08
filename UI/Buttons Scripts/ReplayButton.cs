using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Buttons_Scripts
{
    public class ReplayButton : MonoBehaviour
    {

        public void OnClick(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    
   
    }
}
