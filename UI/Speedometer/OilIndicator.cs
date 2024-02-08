using System;
using UnityEngine;
using UnityEngine.UI;
namespace UI.Speedometer
{
    public class OilIndicator : MonoBehaviour
    {
       [SerializeField] private float _maxFilledFloat;
       private Image _image;
       

       private void Start()
       {
           _image = GetComponent<Image>();
       }

       public void SetFillAmount(float value)
       {
           _image.fillAmount = value * _maxFilledFloat;
       }
       
    }
}
