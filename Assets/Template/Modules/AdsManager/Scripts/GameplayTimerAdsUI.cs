using UnityEngine;
using TMPro;

namespace YGTemplate.Ads
{
    public class GameplayTimerAdsUI : WindowUI
    {
        [SerializeField] private TextMeshProUGUI text_Dialogue;
        [SerializeField] private TextMeshProUGUI text_Timer;

        public override void Awake()
        {
            base.Awake();
        }

        public void OnEnable()
        {
            AdsManager.Instance.OnRestTimerNewVal += UpdateTimer;
        }

        public void OnDisable()
        {
            AdsManager.Instance.OnRestTimerNewVal -= UpdateTimer;
        }

        public void SetUpTimerAd()
        {
            SetActive(true);
            text_Timer.text = Mathf.FloorToInt(AdsManager.Instance.timeBeforeAd).ToString();
        }

        public void SetUpRewardAdsWindow(string text_Dial)
        {
            SetActive(true);
            text_Dialogue.text = text_Dial;
        }

        public void UpdateTimer(float val)
        {
            text_Timer.text = Mathf.FloorToInt(val).ToString();
        }

        public void OnDestroy()
        {
            AdsManager.Instance.OnRestTimerNewVal -= UpdateTimer;
        }
    }
}