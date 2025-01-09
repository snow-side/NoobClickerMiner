using System.Collections;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance;

    AudioSource AudioSource;

    [SerializeField]
    int CurTrack;

    [SerializeField]
    AudioSource AudioSourceSfx;

    bool MusicLoaded;

    float TrackPosition;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
        GameEvents.GameReady.AddListener(() => StartCoroutine(WaitAndLoopPlay()));
    }
    void Start() => AudioSource = GetComponent<AudioSource>();

    public void PlaySfx(string name, bool random = false)
    {
        if (AudioSourceSfx.isPlaying)
            return;

        var clip = random ? ResLoader.Instance.GetRandomClip(name) : ResLoader.Instance.GetClip(name);
        AudioSourceSfx.clip = clip;
        AudioSourceSfx.Play();
    }

    public void OnOffSfx()
    {
        Game.Instance.SaveData.SfxOn = !Game.Instance.SaveData.SfxOn;
        AudioListener.volume = Game.Instance.SaveData.SfxOn ? 1 : 0;
        OnOffMusic();
        Game.Instance.Save();
    }

    public void StopAll()
    {
        AudioListener.volume = 0;
        Debug.Log("GameAudio StopAll");
    }

    public void ResumeAll()
    {
        if (Game.Instance.SaveData.SfxOn)
            AudioListener.volume = 1f;
        if (Game.Instance.SaveData.SfxOn && MusicLoaded)
        {
            Debug.Log("ResumeAll Music");
            StartCoroutine(FixMusic());
        }
    }

    public void Setup()
    {
        if (!Game.Instance.SaveData.SfxOn)
            StopAll();
    }

    public void OnOffMusic()
    {
        if (Game.Instance.SaveData.SfxOn)
        {
            if (!AudioSource.isPlaying)
                AudioSource.Play();
            AudioSource.volume = 0.5f;
        }
        else
        {
            TrackPosition = AudioSource.time;
            AudioSource.volume = 0;
        }
    }

    IEnumerator WaitAndLoopPlay()
    {
        yield return new WaitUntil(() => ResLoader.Instance.TracksLoaded);
        AudioSource.clip = ResLoader.Instance.GetTrack($"Track{CurTrack}");
        MusicLoaded = true;
        Debug.Log("Music_loaded");
        if (Game.Instance.SaveData.SfxOn)
        {
            Debug.Log("Music_play");
            StartCoroutine(FixMusic());
        }
    }

    IEnumerator FixMusic()
    {
        yield return new WaitForFixedUpdate();
        AudioSource.UnPause();
        yield return new WaitForFixedUpdate();
        AudioSource.time = TrackPosition;
        AudioSource.volume = 0.5f;
        yield return new WaitForFixedUpdate();
        AudioSource.time = TrackPosition;

        yield return new WaitForSeconds(1f);
        if (!AudioSource.isPlaying)
        {
            AudioSource.time = TrackPosition;
            yield return new WaitForFixedUpdate();
            AudioSource.Play();
            yield return new WaitForFixedUpdate();
            AudioSource.time = TrackPosition;
        }
    }

    void OnApplicationFocus(bool focus) => Silient(!focus);

    void OnApplicationPause(bool paused) => Silient(paused);

    void Silient(bool paused)
    {
        if (!Game.Instance.GameReady)
            return;

        if (paused)
            StopAll();
        // YAFIX
        /*
        else if (!YandexSdk.AdvShowing)
            ResumeAll();
            */
    }
}
