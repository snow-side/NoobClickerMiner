using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YGTemplate.Ads
{
    public class RewardedAdsUI : WindowUI
    {
        [SerializeField] private TextMeshProUGUI text_Dialogue;
        [SerializeField] private Button button_Positive;
        [SerializeField] private Button button_Negative;

        public event Action OnTryShowRewardAd;
        public event Action OnAbortShowRewardAd;

        public override void Awake()
        {
            base.Awake();
            button_Positive?.onClick.AddListener(() =>
            {
                OnTryShowRewardAd?.Invoke();
            });
            button_Negative?.onClick.AddListener(() =>
            {
                OnAbortShowRewardAd?.Invoke();
                SetActive(false);
            });
        }

        public void SetUpRewardAdsWindow(string text_Dial)
        {
            SetActive(true);
            text_Dialogue.text = text_Dial;
        }
    }
}