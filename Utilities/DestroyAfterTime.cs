using UnityEngine;

namespace Traps.Wind
{
    internal class DestroyAfterTime : MonoBehaviour
    {

        public void StartTimer(float timer)
        {
            Invoke(nameof(DestroySelf), timer);
        }
        
        private void DestroySelf() => Destroy(gameObject);
    }
}