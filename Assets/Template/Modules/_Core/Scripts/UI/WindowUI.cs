using UnityEngine;
using UnityEngine.UI;

namespace YGTemplate
{
    public class WindowUI : MonoBehaviour
    {
        [Header("WindowUI")]
        [SerializeField] protected Button button_CloseWindowCanvas;

        public virtual void Awake()
        {
            if (button_CloseWindowCanvas != null)
            {
                button_CloseWindowCanvas.onClick.AddListener(() =>
                {
                    PlayClickSound();
                    SetActive(false);
                });
            }
        }

        public virtual void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public virtual void PlayClickSound()
        {

        }
    }
}