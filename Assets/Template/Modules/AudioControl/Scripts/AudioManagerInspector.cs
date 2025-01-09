using UnityEngine;
using UnityEngine.Events;

namespace YGTemplate.Audio
{
    public class AudioManagerInspector : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] private float initMusicVolume = 1f;
        [Range(0, 1)]
        [SerializeField] private float initSoundVolume = 1f;

        [Space(15)]

        [SerializeField] public UnityEvent<float> OnMusicVolumeChange;
        [SerializeField] public UnityEvent<float> OnSoundVolumeChange;

        public void Start()
        {
            Init();
            Subscribe();
        }

        public void Init()
        {
            AudioControlManager.musicVolume = initMusicVolume;
            AudioControlManager.soundVolume = initSoundVolume;
        }

        public void Subscribe()
        {
            AudioControlManager.OnMusicVolumeChanged += MusicVolumeChanged;
            AudioControlManager.OnSoundVolumeChanged += SoundVolumeChanged;
        }

        public void MusicVolumeChanged(float value)
        {
            OnMusicVolumeChange?.Invoke(value);
        }

        public void SoundVolumeChanged(float value)
        {
            OnSoundVolumeChange?.Invoke(value);
        }

        public void OnDestroy()
        {
            AudioControlManager.OnMusicVolumeChanged -= MusicVolumeChanged;
            AudioControlManager.OnSoundVolumeChanged -= SoundVolumeChanged;
        }
    }
}