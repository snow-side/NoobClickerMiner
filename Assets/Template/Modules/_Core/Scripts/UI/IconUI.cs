using UnityEngine;
using UnityEngine.UI;

namespace YGTemplate
{
    [RequireComponent(typeof(Button))]
    public class IconUI : MonoBehaviour
    {
        private Button button;

        [Header("IconUI")]
        [SerializeField] protected Transform notificationMark;
        [SerializeField] protected WindowUI window;

        public virtual void Awake()
        {
            SetNotificationMark(false);
            button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    OnIconClicked();
                });
            }
        }

        public virtual void Start()
        {
        }

        public virtual void OnIconClicked()
        {
            window?.SetActive(true);
        }

        public void SetNotificationMark(bool value)
        {
            if (notificationMark != null)
            {
                notificationMark.gameObject.SetActive(value);
            }
        }


        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public virtual void OnDestroy()
        {

        }
    }
}