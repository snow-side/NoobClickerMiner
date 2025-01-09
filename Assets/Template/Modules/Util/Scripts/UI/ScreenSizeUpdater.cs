using System;
using UnityEngine;

namespace YGTemplate.Utils.UI
{
    public class ScreenSizeUpdater : Soliton<ScreenSizeUpdater>
    {
        public static Vector2Int screenSize { get; private set; }
        public static event Action<Vector2Int> OnScreenSizeChanged;

        // Update is called once per frame
        void LateUpdate()
        {
            UpdateScreenSize();
        }

        private void UpdateScreenSize()
        {
            Vector2Int newScreenSize = new Vector2Int(Screen.width, Screen.height);
            if (screenSize != newScreenSize)
            {
                OnScreenSizeChanged?.Invoke(newScreenSize);
                screenSize = newScreenSize;
            }
        }
    }
}