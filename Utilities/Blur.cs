using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities
{
    public class Blur : MonoBehaviour
    {
        [SerializeField] private Material _blurMaterial;
        [SerializeField] private float _duration;
        private float BlurSize
        {
            get => _blurMaterial.GetFloat("_BlurSize");
            set => _blurMaterial.SetFloat("_BlurSize", value);
        }

        private float _startBlur;
        private bool _isBlur;

        private void Awake() => _startBlur = BlurSize;

        private void Start() => BlurSize = 0;

        [Button]
        public void ToggleBlur()
        {
            _isBlur = !_isBlur;
            
            if (_isBlur)
                DOVirtual.Float(0, _startBlur, _duration, value => BlurSize = value);
            else
                DOVirtual.Float(_startBlur, 0, _duration, value => BlurSize = value);
        }


        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var temporaryTexture = RenderTexture.GetTemporary(source.width, source.height);
            Graphics.Blit(source, temporaryTexture, _blurMaterial, 0);
            Graphics.Blit(temporaryTexture, destination, _blurMaterial, 1);
            RenderTexture.ReleaseTemporary(temporaryTexture);
        }

        private void OnDisable() => BlurSize = _startBlur;
    }
}