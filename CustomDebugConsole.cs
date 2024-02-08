using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class CustomDebugConsole : MonoBehaviour
    {
#if UNITY_EDITOR

        private string[] messages = new string[100];
        private void Start() => Instance = this;
        public static CustomDebugConsole Instance;

        public void ShowString(int index, string value)
        {
            messages[index] = value;
        }


        private void OnGUI()
        {
            Rect rect = new Rect(Screen.width - 200, 0, 200, 200);
            
            string message = "";
            for (int i = 0; i < messages.Length; i++)
            {
                if (messages[i] == null) continue;
                message += messages[i] + "\n";
            }
            GUI.Box(rect, message);
        }

#endif
    }
}