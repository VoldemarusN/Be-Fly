using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeactivateButtonInEdges : MonoBehaviour
    {
        [SerializeField] private Button _nextPanelButton;
        [SerializeField] private Button _previousPanelButton;

        private SimpleScrollSnap _scrollSnap;

        void Start()
        {
            _scrollSnap = GetComponent<SimpleScrollSnap>();
            _scrollSnap.onPanelChanged.AddListener(CheckAndDeactivateButtonsOnEdges);
        }

        private void CheckAndDeactivateButtonsOnEdges()
        {
            if (_scrollSnap.CurrentPanel >= _scrollSnap.Panels.Length - 1) _nextPanelButton.interactable = false;
            else
            {
                _nextPanelButton.interactable = true;
            }

            if (_scrollSnap.CurrentPanel == 0) _previousPanelButton.interactable = false;
            else
            {
                _previousPanelButton.interactable = true;
            }
        }
    }
}