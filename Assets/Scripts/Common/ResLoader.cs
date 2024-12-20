using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ResLoader : MonoBehaviour
{
    public static ResLoader Instance { get; private set; }

    public bool SfxLoaded => DictSxf.Count == Items.Count;

    public bool TracksLoaded => DictTrack.Count == Tracks.Count && DictTrack.All(x => x.Value != null);

    public int NumTracks => DictTrack.Count;

    [SerializeField]
    List<string> Items;

    [SerializeField]
    List<string> Tracks;

    readonly Dictionary<string, AudioClip> DictSxf = new();

    readonly Dictionary<string, AudioClip> DictTrack = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
        Load();
    }

    void Load()
    {
        var path = Application.streamingAssetsPath;
        foreach (var item in Items)
            StartCoroutine(LoadAudioClip(item, $"{path}/Audio/{item}.mp3", DictSxf));

        foreach (var item in Tracks)
            StartCoroutine(LoadAudioClip(item, $"{path}/Audio/Tracks/{item}.mp3", DictTrack, 1f));
    }

    public AudioClip GetClip(string key)
    {
        if (DictSxf.TryGetValue(key, out var clip))
            return clip;
        return null;
    }

    public AudioClip GetRandomClip(string key)
    {
        var items = DictSxf.Where(x => x.Key.StartsWith(key));
        var num = items.Count();
        if (num == 0)
            return null;
        var ind = Random.Range(0, num);
        return items.ElementAt(ind).Value;
    }

    public AudioClip GetTrack(string key)
    {
        if (DictTrack.TryGetValue(key, out var clip))
            return clip;
        return null;
    }

    IEnumerator LoadAudioClip(string key, string url, Dictionary<string, AudioClip> dict, float wait = 0f)
    {
        yield return new WaitForSeconds(wait);
        using var req = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        yield return req.SendWebRequest();

        switch (req.result)
        {
            case UnityWebRequest.Result.Success:
                dict.Add(key, DownloadHandlerAudioClip.GetContent(req));
                break;
            default:
                dict.Add(key, null);
                break;
        }
    }
}