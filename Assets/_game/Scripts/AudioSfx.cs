using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AudioSfx : MonoBehaviour
{
    public UnityEvent SfxLoaded = new();

    [SerializeField]
    string Name;

    AudioSource AudioSource;

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        StartCoroutine(WaitAndPlay());
    }

    IEnumerator WaitAndPlay()
    {
        yield return new WaitUntil(() => ResLoader.Instance.SfxLoaded);
        AudioSource.clip = ResLoader.Instance.GetClip(Name);
        if (AudioSource.playOnAwake) AudioSource.Play();
        SfxLoaded.Invoke();
    }
}
