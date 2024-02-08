using System;
using UnityEngine;
using UnityEngine.UI;
namespace UI.Window
{
    public class WindowBase : MonoBehaviour
    {
        [SerializeField]
        protected Button _closeButton;

        private void Awake()
        {
            OnAwake();
        }
        protected virtual void OnAwake()
        {
            _closeButton.onClick.AddListener(() => Destroy(gameObject));
        }
       



    }
}
