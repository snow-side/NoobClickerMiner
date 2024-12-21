using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationTextLoader : MonoBehaviour
{
    [SerializeField] private string localizationKey;

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        var textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = LocalizationManager.Instance.GetLocalizedText(localizationKey);
    }
}