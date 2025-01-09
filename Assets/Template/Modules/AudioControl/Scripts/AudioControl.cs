using UnityEngine;

namespace YGTemplate.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioControl : MonoBehaviour
    {
        [SerializeField] private AudioType type;
        [SerializeField] private AudioSource audioSource;
        private float baseInitVolume;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            baseInitVolume = audioSource.volume;

            switch (type)
            {
                case AudioType.Music:
                    AudioControlManager.OnMusicVolumeChanged += SetVolume;
                    SetVolume(AudioControlManager.musicVolume);
                    break;
                case AudioType.Sound:
                    AudioControlManager.OnSoundVolumeChanged += SetVolume;
                    SetVolume(AudioControlManager.soundVolume);
                    break;
            }
        }

        private void OnDestroy()
        {
            switch (type)
            {
                case AudioType.Music:
                    AudioControlManager.OnMusicVolumeChanged -= SetVolume;
                    break;
                case AudioType.Sound:
                    AudioControlManager.OnSoundVolumeChanged -= SetVolume;
                    break;

            }
        }

        private void SetVolume(float volume)
        {
            audioSource.volume = baseInitVolume * volume;
        }

        public void PlayOneShot(AudioClip audioclip)
        {
            audioSource.PlayOneShot(audioclip);
        }

        public void PlayLoop(AudioClip audioClip)
        {
            audioSource.loop = true;
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public void Play()
        {
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }
    }
}