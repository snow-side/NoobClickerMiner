
namespace YGTemplate.DailyReward
{
    public class DailyRewIconUI : IconUI
    {

        public override void OnIconClicked()
        {
            DailyRewManager.Instance.ShowWindow();
        }

        public override void Start()
        {
            base.Start();
            window = DailyRewManager.Instance.dailyRewWindowUI;
            DailyRewManager.Instance.OnAvailableReward.AddListener(SetNotificationMark);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            DailyRewManager.Instance?.OnAvailableReward.RemoveListener(SetNotificationMark);
        }

    }
}