using System;
using UnityEngine;

namespace Settings
{
    public class ScreenResolutionScaler : MonoBehaviour
    {
        private void Awake()
        {
            // Screen.SetResolution(1080, 1920, false);
        }

        private void OnRectTransformDimensionsChange()
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
}
