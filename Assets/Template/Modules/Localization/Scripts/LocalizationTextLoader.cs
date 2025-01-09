using UnityEngine;
using TMPro;

namespace YGTemplate.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizationTextLoader : MonoBehaviour
    {
        private TextMeshProUGUI textMesh;

        [SerializeField] private string localizationKey;

        private void Start()
        {
            textMesh = GetComponent<TextMeshProUGUI>();

            UpdateText();
            LocalizationManager.Instance.OnLanguageChange.AddListener(UpdateText);
        }

        public void UpdateText()
        {
            textMesh.text = LocalizationManager.Instance.GetLocalizedText(localizationKey);
        }

        public void UpdateText(string langCode)
        {
            UpdateText();
        }

        public void OnDestroy()
        {
            LocalizationManager.Instance?.OnLanguageChange.RemoveListener(UpdateText);
        }
    }
}