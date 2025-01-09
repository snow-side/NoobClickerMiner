using UnityEngine;

namespace YGTemplate.Utils.UI
{
    public class CustomAspectRatioUI : MonoBehaviour
    {
        [SerializeField] private float aspectRatio = 0.5625f;

        private RectTransform panel;
        private RectTransform parentRectTrans;
        private Canvas canvas;

        private Rect parentRect;

        public void Awake()
        {
            panel = GetComponent<RectTransform>();
            parentRectTrans = transform.parent.GetComponent<RectTransform>();

            ApplyAreaSize();
        }

        public void Start()
        {
            ScreenSizeUpdater.OnScreenSizeChanged += ApplyAreaSize;
        }

        public void ApplyAreaSize(Vector2Int newScreenSize)
        {
            ApplyAreaSize();
        }

        public void ApplyAreaSize()
        {
            parentRect = parentRectTrans.rect;
            Vector2 anchorMin = parentRect.position;
            Vector2 anchorMax = parentRect.position + parentRect.size;

            float endWidth = parentRect.width;
            float endHeight = parentRect.height;

            //        Debug.Log(parentRect.size);

            float currentAspectRation = endWidth / endHeight;
            if (currentAspectRation > aspectRatio)
            {
                endWidth = aspectRatio * endHeight;
            }
            //        Debug.Log(endWidth + "___" + endHeight + "___");

            /*
                    if (endWidth > 0 && endHeight > 0)
                    {
                        anchorMin.x = anchorMin.x / endWidth;
                        anchorMin.y = anchorMin.y / endHeight;
                        anchorMax.x = anchorMax.x / endWidth;
                        anchorMax.y = anchorMax.y / endHeight;
                    }


                    if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                    {
                        panel.anchorMin = anchorMin;
                        panel.anchorMax = anchorMax;
                    }
            */
            panel.sizeDelta = new Vector2(endWidth, endHeight);

            //        Debug.Log(currentAspectRation+"___" +anchorMin + "___" + anchorMax);
        }

        public void OnDestroy()
        {
            ScreenSizeUpdater
                .OnScreenSizeChanged -= ApplyAreaSize;
        }
    }
}