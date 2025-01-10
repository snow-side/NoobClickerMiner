using System.Collections;
using UnityEngine;
using YG;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public bool Paused { get; private set; }

    public GameSaveData SaveData { get; private set; }

    public bool GameReady { get; private set; }

    [SerializeField] private SceneUIManager sceneUIManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
        GameEvents.GameExit.AddListener(Save);
    }

    public void Start()
    {
        GameEvents.GameSavesLoaded.AddListener(LoadHandler);
        GameSave.Load();

        /*...*/
        YG2.onGetSDKData+= GameSave.Load;
        TemplateManager.Instance.InitInThisScene(sceneUIManager.containerForTemplate);
    }

    public void SetPause(bool val)
    {
        Paused = val;
        if (Paused)
            Cursor.lockState = CursorLockMode.Confined;
        /*else
            Cursor.lockState = CursorLockMode.Locked;*/
    }

    public void Save()
    {
        GameSave.Save(SaveData);
        YG2.SaveProgress();
    }

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
        
        // YAFIX
        /*
        YandexSdk.Instance.Goal("game_ready", string.Empty);
        */
        
        StartCoroutine(SavePeriodically());
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
            Save();
            Time.timeScale = 1;

        if (!GameReady)
            return;
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
            Save();
    }

    private void OnDestroy()
    {
        YG2.onGetSDKData -= GameSave.Load;
    }
}
