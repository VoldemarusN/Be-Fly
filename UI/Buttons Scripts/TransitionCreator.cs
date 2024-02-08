using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Buttons_Scripts
{
   public class TransitionCreator : MonoBehaviour
   {
      public UnityEvent Action;
      public float SecondCount;

      public void CreateTransition(){
         StopAllCoroutines();
         StartCoroutine(StartEvent());
      }
   

      IEnumerator StartEvent(){
         yield return new WaitForSecondsRealtime(SecondCount);
         Action.Invoke();
      }


   }
}
