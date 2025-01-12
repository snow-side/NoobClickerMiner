using System.Collections;
using UnityEngine;
using YG;
using YGTemplate.Localization;

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

        Load();
        /*...*/
        YG2.onGetSDKData+= GameSave.Load;
        TemplateManager.Instance.InitInThisScene(sceneUIManager.containerForTemplate);
 


        switch (YG2.envir.language) {
            case "ar":
                LocalizationManager.Instance.SetLanguage("ar");
                break;
            case "es":
                LocalizationManager.Instance.SetLanguage("es");
                break;
            case "ru":
                LocalizationManager.Instance.SetLanguage("ru");
                break;
            case "de":
                LocalizationManager.Instance.SetLanguage("de");
                break;
            case "en":
                LocalizationManager.Instance.SetLanguage("en");
                break;
            case "tr":
                LocalizationManager.Instance.SetLanguage("tr");
                break;
            default:
                LocalizationManager.Instance.SetLanguage("ru");
                break;

        }
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
        //        GameAudio.Instance.Setup();
        
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
        /*
        if (!focus)
            Save();
            Time.timeScale = 1;

        if (!GameReady)
            return;
        */
    }

    void OnApplicationPause(bool paused)
    {
        /*
        if (paused)
            Save();
        */
    }

    private void OnDestroy()
    {
        YG2.onGetSDKData -= GameSave.Load;
    }

    public void EduShown() {
        YG2.saves.isFirstTime= false;
        YG2.SaveProgress();
    }

    private void Load()
    {
        if (YG2.saves.isFirstTime || YG2.isFirstGameSession) {
            sceneUIManager.ShowEdu();
        }
        GameSave.Load();
    }
}
