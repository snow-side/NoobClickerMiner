using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YGTemplate.Audio;
using YGTemplate.Localization;

namespace YGTemplate.Settings
{
    public class SettingsUI : WindowUI
    {
        [Header("SettingsUI")]
        [Header("AudioSettings")]
        [SerializeField] private Button button_MusicTurnOnOff;
        [SerializeField] private Button button_SoundTurnOnOff;

        [SerializeField] private Sprite image_SwitchOn;
        [SerializeField] private Sprite image_SwitchOff;

        [SerializeField] private bool isMusicOn;
        [SerializeField] private bool isSoundOn;

        [Header("LanguageSettings")]
        [SerializeField] private bool spawnLanguageButtons;
        [SerializeField] private LanguageSelectButtonUI prefab_LanguageSelectButtonUI;
        [SerializeField] private Transform langButtonsContainer;
        [SerializeField] private LanguageData[] languageDataForButtons;

        private List<LanguageSelectButtonUI> listOfLanguageSelectButtons;

        public override void Awake()
        {
            base.Awake();

            button_MusicTurnOnOff.onClick.AddListener(() =>
            {
                SetMusic(!isMusicOn);
            });

            button_SoundTurnOnOff.onClick.AddListener(() =>
            {
                SetSound(!isSoundOn);
            });

            listOfLanguageSelectButtons = new List<LanguageSelectButtonUI>();

            SpawnLanguageButtons();

        }

        public void OnEnable()
        {
            isMusicOn = AudioControlManager.musicVolume > 0;
            isSoundOn = AudioControlManager.soundVolume > 0;
            SetMusicIcon(isMusicOn);
            SetSoundIcon(isSoundOn);
        }

        private void SetSound(bool value)
        {
            isSoundOn = value;
            AudioControlManager.soundVolume = value ? 1 : 0;
            SetSoundIcon(value);
        }

        private void SetSoundIcon(bool value)
        {
            button_SoundTurnOnOff.image.sprite = value ? image_SwitchOn : image_SwitchOff;
        }

        private void SetMusic(bool value)
        {
            isMusicOn = value;
            AudioControlManager.musicVolume = value ? 1 : 0;
            SetMusicIcon(value);
        }

        private void SetMusicIcon(bool value)
        {
            button_MusicTurnOnOff.image.sprite = value ? image_SwitchOn : image_SwitchOff;
        }

        private void SpawnLanguageButtons()
        {
            if (!spawnLanguageButtons) return;

            for (int i = 0; i < languageDataForButtons.Length; i++)
            {
                SpawnLanguageButton(languageDataForButtons[i]);
            }
            HighlightLanguage(LocalizationManager.Instance.currentLanguage);
        }

        private void SpawnLanguageButton(LanguageData data)
        {
            if (prefab_LanguageSelectButtonUI == null) return;

            LanguageSelectButtonUI langButton = Instantiate(prefab_LanguageSelectButtonUI, langButtonsContainer);
            langButton.SetUp(data);

            langButton.OnClickLangButton += SelectLanguage;


            listOfLanguageSelectButtons.Add(langButton);
        }

        private void SelectLanguage(string langCode)
        {
            LocalizationManager.Instance.SetLanguage(langCode);
            HighlightLanguage(langCode);
        }

        private void HighlightLanguage(string langCode)
        {
            for (int i = 0; i < listOfLanguageSelectButtons.Count; i++)
            {
                if (langCode == listOfLanguageSelectButtons[i].langCode)
                {
                    listOfLanguageSelectButtons[i].SetActiveLang(true);
                }
                else
                {
                    listOfLanguageSelectButtons[i].SetActiveLang(false);
                }
            }
        }
    }
}
