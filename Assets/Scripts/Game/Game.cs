using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public bool Paused { get; private set; }

    public GameSaveData SaveData { get; private set; }

    public bool GameReady { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
        GameEvents.GameExit.AddListener(Save);
    }

    void Start()
    {
        GameEvents.GameSavesLoaded.AddListener(LoadHandler);
        GameSave.Load();
    }

    public void SetPause(bool val)
    {
        Paused = val;
        if (Paused)
            Cursor.lockState = CursorLockMode.Confined;
        /*else
            Cursor.lockState = CursorLockMode.Locked;*/
    }

    public void Save() => GameSave.Save(SaveData);

    IEnumerator SavePeriodically()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(5f);
            Save();
        }
    }

    void OnApplicationQuit()
    {
        Save();
        Debug.Log("ApplicationQuit");
    }

    void LoadHandler(GameSaveData data)
    {
        SaveData = data;
        GameAudio.Instance.Setup();
        GameEvents.GameReady.Invoke();
        GameReady = true;
        Debug.Log("GameReady");
        YandexSdk.Instance.Goal("game_ready", string.Empty);
        StartCoroutine(SavePeriodically());
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
            Save();

        if (focus && !YandexSdk.AdvShowing)
            Time.timeScale = 1;

        if (!GameReady)
            return;

        if (YandexSdk.AdvShowing)
            Cursor.lockState = CursorLockMode.Confined;
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
            Save();

        if (!paused && !YandexSdk.AdvShowing)
            Time.timeScale = 1;
    }
}
