using System;
using System.Runtime.InteropServices;
using UnityEngine;


public class YandexSdk : MonoBehaviour
{
    public static YandexSdk Instance { get; private set; }

    [DllImport("__Internal")]
    private static extern void InitSDK(string photoSize, string scopes);

    [DllImport("__Internal")]
    static extern void SaveLocalStorage(string key, string json);

    [DllImport("__Internal")]
    static extern void LoadLocalStorage(string key);

    [DllImport("__Internal")]
    private static extern void FullAdShow();

    [DllImport("__Internal")]
    private static extern void RewardedShow(string id);

    [DllImport("__Internal")]
    private static extern void MetricaGoal(string name, string value);

    public static bool AdvShowing { get; private set; }

    static Action RewardAction;

    static string RewardId;

    static DateTime? RewardDate;

    public void ShowAd()
    {
        Debug.Log("YandexSdk ShowAd");

#if !UNITY_EDITOR && UNITY_WEBGL
        FullAdShow();
#endif
    }

    public void OpenFullAd()
    {
        Debug.Log("YandexSdk OpenFullAd");
        AdvShowing = true;
        GameAudio.Instance.StopAll();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void CloseFullAd(string ok)
    {
        Debug.Log("YandexSdk CloseFullAd");
        if (AdvShowing)
        {
            GameAudio.Instance.ResumeAll();
            Time.timeScale = 1;
        }
        AdvShowing = false;
        if (Game.Instance.Paused)
            Cursor.lockState = CursorLockMode.Confined;
        /*else
            Cursor.lockState = CursorLockMode.Locked;*/
    }

    public void ErrorFullAd() => Debug.Log("YandexSdk ErrorFullAd");

    public void RewardAd(Action action)
    {
        var now = DateTime.Now;
        if (RewardDate != null && (now - RewardDate.Value).TotalSeconds < 1.5)
        {
            Debug.Log($"YandexSdk RewardAd tomany reqs");
            return;
        }

        RewardDate = now;
        RewardId = $"reward_{UnityEngine.Random.Range(0, int.MaxValue)}";
        RewardAction = action;
#if !UNITY_EDITOR && UNITY_WEBGL
        RewardedShow(RewardId);
#else
        Invoke(nameof(DbgInvoke), 3f);
#endif
    }

    void DbgInvoke() => RewardVideo(RewardId);

    public void OpenVideo(string id)
    {
        Debug.Log($"YandexSdk OpenReward {id}");
        AdvShowing = true;
        GameAudio.Instance.StopAll();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void CloseVideo()
    {
        Debug.Log("YandexSdk CloseReward");
        if (AdvShowing)
        {
            AdvShowing = false;
            GameAudio.Instance.ResumeAll();
            Time.timeScale = 1;
            if (Game.Instance.Paused)
                Cursor.lockState = CursorLockMode.Confined;
            /*else
                Cursor.lockState = CursorLockMode.Locked;*/
        }
        RewardAction = null;
        RewardId = null;
        RewardDate = null;
    }

    public void RewardVideo(string id)
    {
        Debug.Log($"YandexSdk RewardOK {id}");
        Time.timeScale = 1;
        if (id == RewardId)
        {
            RewardAction?.Invoke();
            RewardAction = null;
            RewardId = null;
            RewardDate = null;
        }
        else
            Debug.Log("YandexSdk wrong id");
    }

    public void GameVisible()
    {
        Debug.Log("YandexSdk GameVisible");
        if (AdvShowing)
            return;
        GameAudio.Instance.ResumeAll();
        Time.timeScale = 1;
    }

    public void WindowBlur()
    {
        Debug.Log("YandexSdk WindowBlur");
        GameAudio.Instance.StopAll();
        Time.timeScale = 0;
    }

    public void WindowFocus()
    {
        Debug.Log("YandexSdk WindowFocus");
        if (AdvShowing)
            return;
        GameAudio.Instance.ResumeAll();
        Time.timeScale = 1;
    }

    public void GameHidden()
    {
        Debug.Log("YandexSdk GameHidden");
        GameAudio.Instance.StopAll();
        Time.timeScale = 0;
    }

    public void LoadLocalStorageCall(string data)
    {
        Debug.Log($"YandexSdk LoadLocalStorageCall");
        GameSave.LoadCall(data);
    }

    public void LoadSave(string name)
    {
        Debug.Log("YandexSdk LoadSave");
        LoadLocalStorage(name);
    }

    public void Goal(string name, string value)
    {
        Debug.Log($"YandexSdk Goal {name} {value}");
#if !UNITY_EDITOR && UNITY_WEBGL
        MetricaGoal(name, value);
#endif
    }

    public void Save(string name, string data)
    {
        Debug.Log($"YandexSdk Save");
        SaveLocalStorage(name, data);
    }

    public void GameExitCall()
    {
        Debug.Log("YandexSdk GameExitCall");
        GameEvents.GameExit.Invoke();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        GameEvents.GameReady.AddListener(() =>
        {
            Debug.Log("YandexSdk InitSDK");
#if !UNITY_EDITOR && UNITY_WEBGL
        InitSDK("", "");
        ShowAd();
#endif
        });
    }

}
