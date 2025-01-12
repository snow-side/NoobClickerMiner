using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YGTemplate.Settings
{
    [Serializable]
    public struct LanguageData
    {
        public string code;
        public Sprite flagSprite;
        public string displayedName;
    }

    public class LanguageSelectButtonUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text_LangName;
        [SerializeField] private Image imageFlag;

        [SerializeField] private Transform selectedTransform;
        [SerializeField] private Transform nonSelectedTransform;

        public string langCode { get; private set; }

        public event Action<string> OnClickLangButton;

        public void SetUp(LanguageData data)
        {
            if (imageFlag != null) imageFlag.sprite = data.flagSprite;
            if (text_LangName != null) text_LangName.text = data.displayedName;
            langCode = data.code;
        }

        public void SetActiveLang(bool value)
        {
            selectedTransform?.gameObject.SetActive(value);
            nonSelectedTransform?.gameObject.SetActive(!value);
        }

        public void Awake()
        {
            button.onClick.AddListener(() =>
            {
                OnClickLangButton?.Invoke(langCode);
                PlayClickSound();
            });
        }

        public virtual void PlayClickSound()
        {
            GameAudio.Instance.PlaySfx("click");
        }
    }
}