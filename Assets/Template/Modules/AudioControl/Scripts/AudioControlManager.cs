using System;
using UnityEngine;

namespace YGTemplate.Audio
{
    public class AudioControlManager
    {
        private static float _soundVolume;
        public static float soundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = Mathf.Clamp01(value);
                OnSoundVolumeChanged?.Invoke(_soundVolume);
            }
        }
        public static event Action<float> OnSoundVolumeChanged;

        private static float _musicVolume;
        public static float musicVolume
        {
            get { return _musicVolume; }
            set
            {
                _musicVolume = Mathf.Clamp01(value);
                OnMusicVolumeChanged?.Invoke(_musicVolume);
            }
        }
        public static event Action<float> OnMusicVolumeChanged;

        public static void SetSoundVolumeWithoutNotification(float value)
        {
            _soundVolume = value;
        }

        public static void SetMusicVolumeWithoutNotification(float value)
        {
            _musicVolume = value;
        }
    }
}