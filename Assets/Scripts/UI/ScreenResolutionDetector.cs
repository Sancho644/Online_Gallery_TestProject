using System;
using UnityEngine;

namespace UI
{
    public class ScreenResolutionDetector : MonoBehaviour
    {
        public Action OnScreenResolutionChange;

        private void OnRectTransformDimensionsChange()
        {
            OnScreenResolutionChange?.Invoke();
        }
    }
}