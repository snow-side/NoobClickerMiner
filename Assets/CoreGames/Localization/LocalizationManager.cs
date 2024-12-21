using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    public TextAsset localization; // Предположим, что локализация хранится в TextAsset

    private Dictionary<string, Dictionary<string, string>> _localizationData;
    private string _currentLanguage = "en"; // По умолчанию английский

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLocalizationData();
        }
        else
        {
            Destroy(gameObject);
        }

        SetLanguage("ru"); // Устанавливаем русский язык
    }

    // Метод загрузки данных локализации
    private void LoadLocalizationData()
    {
        if (localization == null)
        {
            Debug.LogError("Localization TextAsset is not assigned.");
            return;
        }

        string json = localization.text;
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("Localization JSON is empty or invalid.");
            return;
        }

        try
        {
            // Используем Newtonsoft.Json для десериализации
            var wrapper = JsonConvert.DeserializeObject<LocalizationWrapper>(json);
            
            if (wrapper == null)
            {
                Debug.LogError("Deserialization failed. The JSON is likely malformed.");
                return;
            }

            _localizationData = wrapper.ToDictionary();

            // Печатаем все данные локализации для отладки
            foreach (var entry in _localizationData)
            {
                Debug.Log($"Key: {entry.Key}");
                foreach (var translation in entry.Value)
                {
                    Debug.Log($"  {translation.Key}: {translation.Value}");
                }
            }

            if (_localizationData == null || _localizationData.Count == 0)
            {
                Debug.LogError("Failed to convert JSON to localization data.");
            }
            else
            {
                Debug.Log("Localization data loaded successfully.");
            }
        }
        catch (System.Exception ex)
        {
            // Логируем подробную ошибку
            Debug.LogError("Error loading localization data: " + ex.Message);
            Debug.LogError("Stack trace: " + ex.StackTrace);
        }
    }

    // Метод получения переведенного текста по ключу
    public string GetLocalizedText(string key)
    {
        if (_localizationData == null)
        {
            Debug.LogError("Localization data is not loaded.");
            return key; // Возвращаем ключ, если данные не загружены
        }

        if (_localizationData.TryGetValue(key, out var translations) && translations.TryGetValue(_currentLanguage, out var translation))
        {
            return translation;
        }

        Debug.LogWarning($"Translation for key '{key}' not found.");
        return key; // Возвращаем ключ, если перевод не найден
    }

    // Установка языка
    public void SetLanguage(string language)
    {
        _currentLanguage = language;
    }

    // Вспомогательная структура для десериализации JSON
    [System.Serializable]
    private class LocalizationWrapper
    {
        public LocalizationEntry[] entries;

        // Метод преобразования данных в словарь
        public Dictionary<string, Dictionary<string, string>> ToDictionary()
        {
            var dictionary = new Dictionary<string, Dictionary<string, string>>();
            foreach (var entry in entries)
            {
                dictionary[entry.key] = entry.translations;
            }
            return dictionary;
        }
    }

    [System.Serializable]
    private class LocalizationEntry
    {
        public string key;
        public Dictionary<string, string> translations;
    }
}
