
namespace YGTemplate.InGameTimeReward
{

    public class InGameTimeIconUI : IconUI
    {
        public override void OnIconClicked()
        {
            InGameTimeRewManager.Instance.ShowWindow();
        }

        public override void Start()
        {
            base.Start();
            window = InGameTimeRewManager.Instance.inGameTimeWindowUI;
            InGameTimeRewManager.Instance.OnAvailableReward.AddListener(SetNotificationMark);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            InGameTimeRewManager.Instance.OnAvailableReward.RemoveListener(SetNotificationMark);
        }

    }
}