using System;
using UnityEngine;
using UnityEngine.Events;
using YG;
using YGTemplate.Utils;
using YGTemplate.Ads;
using YGTemplate.Audio;
using YGTemplate.DailyReward;
using YGTemplate.InGameTimeReward;
using YGTemplate.FortuneWheel;
using YGTemplate.Localization;
using YGTemplate.Review;
using YGTemplate.IAP;
using YGTemplate.Settings;

public class TemplateManager : Soliton<TemplateManager>
{
    public bool devModeOnAwake;
    public static bool DEV_MODE = false;
    private float speedMult = 100.0f;

    public bool dataIsLoaded;

    [Tooltip("Transform, which will be contains all generated windows. Have to be being defined, before Instantiate anything")]
    [SerializeField] private Transform parentContainer;
    
    [SerializeField] private DailyRewManager dailyRewManager; // 1
    [SerializeField] private InGameTimeRewManager inGameTimeRewManager; // 2
    [SerializeField] private FortuneWheelManager fortuneWheelManager; // 3 
    [SerializeField] private AudioManagerInspector audioManager; // 4
    [SerializeField] private LocalizationManager localizationManager; // 6
    [SerializeField] private ReviewManager reviewManager; // 7
    [SerializeField] private AdsManager adsManager; // 8 
    [SerializeField] private IAPManager iapManager; // 9 

    
    // 5
    [SerializeField] private bool spawnSettingsUI;
    [SerializeField] private SettingsUI prefab_SettingsUI;

    [SerializeField] private SettingsUI settingsUI;

    [SerializeField] private bool spawnIcons;
    [SerializeField] private GameObject prefabWithIcons;

    [SerializeField] private bool saveWhenHaveNewData;
    public event Action OnHaveNewData;

    [Tooltip("That's a funnel from every source of rewards defined by rewardData")]
    public UnityEvent<string> OnReward;

    public override void Awake()
    {
        base.Awake();
        /*
        so...
        since we have data in managers (in some of them), we can't just drop them...
        */

        DEV_MODE = devModeOnAwake;
    }

    public void Start()
    {
#if PLATFORM_WEBGL
        GetLoadData();
        YG2.onGetSDKData += GetLoadData;
#endif 
        dailyRewManager.OnHaveNewData += HaveNewData;


        dailyRewManager.OnReward.AddListener(GetReward);
        inGameTimeRewManager.OnReward.AddListener(GetReward);
        fortuneWheelManager.OnReward.AddListener(GetReward);
        adsManager.OnRewardData.AddListener(GetReward);

        if (DEV_MODE) {
            inGameTimeRewManager.SetDeltaTimeMultiplier(speedMult);
            fortuneWheelManager.SetDeltaTimeMultiplier(speedMult);
        }

    }

    public void GetLoadData() {
        dailyRewManager.GetData();
    }

    public void SaveData() {
        YG2.SaveProgress();
    }

    public void HaveNewData() {
        OnHaveNewData?.Invoke();
        if (saveWhenHaveNewData)
            SaveData();
    }
    /*
     i see a pattern here
     */
    public void SpawnWindows() {
        dailyRewManager.SpawnWindow(parentContainer);
        inGameTimeRewManager.SpawnWindow(parentContainer);
        reviewManager.SpawnWindow(parentContainer);
        fortuneWheelManager.SpawnWindow(parentContainer);
        iapManager.SpawnWindow(parentContainer);
        adsManager.SpawnWindow(parentContainer);

        SpawnSettingsUI(parentContainer);
    }

    public void SpawnSettingsUI(Transform parentContainer) {
        if (!spawnSettingsUI) return;
        settingsUI = Instantiate(prefab_SettingsUI, parentContainer);
    }

    public void SpawnIcons(Transform parentContainer) {
        if (!spawnIcons) return;
        Instantiate(prefabWithIcons, parentContainer);
    }

    public void ShowSettings() {
        settingsUI?.SetActive(true);
    }

    public void InitInThisScene(Transform containerForTemplate) {
        parentContainer = containerForTemplate;
        SpawnIcons(parentContainer);
        SpawnWindows();
    }

    public void GetReward(string rewardData) {
        OnReward?.Invoke(rewardData);
    }
}
