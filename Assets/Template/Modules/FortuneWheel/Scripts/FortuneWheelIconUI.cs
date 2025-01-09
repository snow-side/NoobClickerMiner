using TMPro;
using UnityEngine;

namespace YGTemplate.FortuneWheel
{
    public class FortuneWheelIconUI : IconUI
    {
        [SerializeField] private TextMeshProUGUI text_RestTimer;

        public override void OnIconClicked()
        {
            FortuneWheelManager.Instance.ShowWindow();
        }

        public override void Start()
        {
            base.Start();
            window = FortuneWheelManager.Instance.fortuneWheelWindowUI;
            FortuneWheelManager.Instance.OnHaveSpins.AddListener(SetNotificationMark);
            FortuneWheelManager.Instance.OnHaveSpins.AddListener(HideTimer);

            FortuneWheelManager.Instance.OnRestTimeChanged.AddListener(UpdateTimer);

            HideTimer(FortuneWheelManager.Instance.IsHaveSpins());
            UpdateTimer(FortuneWheelManager.Instance.restTimeBeforeSpin);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            FortuneWheelManager.Instance.OnHaveSpins.RemoveListener(SetNotificationMark);
            FortuneWheelManager.Instance.OnHaveSpins.RemoveListener(HideTimer);

            FortuneWheelManager.Instance.OnRestTimeChanged.RemoveListener(UpdateTimer);
        }


        // ~ ! SetActive(True)
        public void HideTimer(bool value)
        {
            text_RestTimer?.gameObject.SetActive(!value);
        }

        public void UpdateTimer(float restSeconds)
        {
            if (text_RestTimer == null) return;

            if (!text_RestTimer.IsActive()) return;

            int seconds = Mathf.FloorToInt(restSeconds);

            int time_min = seconds / 60;
            int time_sec = seconds % 60;

            string time_Str = string.Format("{0:d2}:{1:d2}", time_min, time_sec);

            text_RestTimer.text = time_Str;
        }
    }
}