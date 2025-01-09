using System;
using UnityEngine;
using UnityEngine.Events;
using YGTemplate.Utils;
using YG;
using YGTemplate.InGameTimeReward;

namespace YGTemplate.Ads
{
    public class AdsManager : Singleton<AdsManager>
    {
        [SerializeField] private bool spawnRewardedWindow;
        [SerializeField] private RewardedAdsUI prefab_RewardedAdsUI;
        [SerializeField] private bool spawnGameplayTimerAdWindow;
        [SerializeField] private GameplayTimerAdsUI prefab_gameplayTimerAdsUI;

        [Tooltip("Have to be set if it's not going to spawn")]
        [SerializeField] private RewardedAdsUI rewardedAds;
        [SerializeField] private GameplayTimerAdsUI gameplayTimerAdsUI;

        [Range(1, 10)]
        [Tooltip("Only for Interstitial Ad")]
        [SerializeField] private float _timeBeforeAd;

        [Tooltip("Only for Interstitial Ad")]
        public float timeBeforeAd
        {
            get { return _timeBeforeAd; }
        }

        private bool isHavingTimerAd = false;
        private float crntTimeBeforeAd = 0;

        public float restTime
        {
            get { return Mathf.Max(0, timeBeforeAd - crntTimeBeforeAd); }
        }

        public event Action<float> OnRestTimerNewVal;

        public UnityEvent<string> OnRewardData;

        public Action OnRewardAdShown;
        public Action OnIntAdShown;

        

        public override void Awake()
        {
            base.Awake();
            isHavingTimerAd = false;
            crntTimeBeforeAd = 0;
        }

        public void LateUpdate()
        {
            UpdateTimer();
        }

        public void UpdateTimer()
        {
            if (!isHavingTimerAd) return;

            crntTimeBeforeAd += Time.deltaTime;
            OnRestTimerNewVal?.Invoke(restTime);
            if (restTime <= 0)
            {
                crntTimeBeforeAd = 0;
                isHavingTimerAd = false;
                ShowTimerAd();
            }
        }

        #region RewardAds

        public void TryShowRewardedAds(string text, string rewardData) {
            TryShowRewardedAds(text, () =>
            {
                Debug.Log("[" + this.GetType().Name + "] " + "Sent Reward with rewardData" + rewardData);
                OnRewardData.Invoke(rewardData);
            });
        }

        public void TryShowRewardedAds(string text, Action OnReward = null)
        {
            Debug.Log("[" + this.GetType().Name + "] " + "Init Reward Ads Dial Window");

            rewardedAds.SetUpRewardAdsWindow(text);
            this.OnRewardAdShown = OnReward;

            rewardedAds.OnTryShowRewardAd += ShowRewardAds;
            rewardedAds.OnAbortShowRewardAd += AbortShowRewardAd;
        }

        public void ShowRewardAds(string rewardData)
        {
            ShowRewardAds(() =>
            {
                Debug.Log("[" + this.GetType().Name + "] " + "Sent Reward with rewardData: " + rewardData);
                OnRewardData.Invoke(rewardData);
            });
        }

        public void ShowRewardAds(Action OnReward) {
            this.OnRewardAdShown = OnReward;

            ShowRewardAds();
        }

        private void ShowRewardAds()
        {
            Debug.Log("[" + this.GetType().Name + "] " + "Clicked Show Reward Ads in Reward Dial Window");

            rewardedAds.SetActive(false);
            YG2.RewardedAdvShow("0", () => AfterRewardAdShown());
        }

        private void AfterRewardAdShown()
        {

            Debug.Log("[" + this.GetType().Name + "] " + "Successful watched Reward Ads");
            OnRewardAdShown?.Invoke();
        }

        private void AbortShowRewardAd()
        {
            OnRewardAdShown = null;

            rewardedAds.OnTryShowRewardAd -= ShowRewardAds;
            rewardedAds.OnAbortShowRewardAd -= AbortShowRewardAd;
        }

        public void OnDestroy()
        {
            if (rewardedAds != null)
            {
                rewardedAds.OnTryShowRewardAd -= ShowRewardAds;
                rewardedAds.OnAbortShowRewardAd -= AbortShowRewardAd;
            }
        }
        #endregion

        #region InterstitialAd
        public void TryShowTimerAds(Action OnReward = null)
        {
            if (!YG2.isTimerAdvCompleted)
            {
                Debug.Log("[" + this.GetType().Name + "] " + "Tried show Timer Ads when there are cooldown between ads");
                return;
            }

            Debug.Log("[" + this.GetType().Name + "] " + "Init Timer Ads");
            gameplayTimerAdsUI.SetUpTimerAd();

            this.OnIntAdShown = OnReward;
            crntTimeBeforeAd = 0;
            isHavingTimerAd = true;
        }

        public void ShowTimerAd()
        {
            Debug.Log("[" + this.GetType().Name + "] " + "Try Show timer Ad (Interstitial)");

            YG2.onCloseInterAdv += AfterIntAdShown;
            YG2.InterstitialAdvShow();
            gameplayTimerAdsUI.SetActive(false);
        }

        private void AfterIntAdShown()
        {
            Debug.Log("[" + this.GetType().Name + "] " + "Successful watched Int Ads");
            OnIntAdShown?.Invoke();

            YG2.onCloseInterAdv -= AfterIntAdShown;
        }

        #endregion

        #region spawn_lake

        public void SpawnWindow(Transform parentContainer)
        {
            if (spawnRewardedWindow)
            {
                rewardedAds = Instantiate(prefab_RewardedAdsUI, parentContainer);
                rewardedAds.SetActive(false);
            }

            if (spawnGameplayTimerAdWindow)
            {
                gameplayTimerAdsUI = Instantiate(prefab_gameplayTimerAdsUI, parentContainer);
                gameplayTimerAdsUI.SetActive(false);
            }
        }
        #endregion
    }
}