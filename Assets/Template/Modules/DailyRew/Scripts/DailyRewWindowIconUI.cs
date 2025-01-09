using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YGTemplate.DailyReward
{

    public class DailyRewWindowIconUI : MonoBehaviour
    {
        [SerializeField] private string dailyRewID;

        [SerializeField] private Button rewardButton;
        [SerializeField] private Transform lockedCanvasTransform;
        [SerializeField] private Transform receivedCanvasTransform;

        [SerializeField] private Image image_Icon;
        [SerializeField] private TextMeshProUGUI text_daysRowCount;
        [SerializeField] private TextMeshProUGUI text_description;

        private DailyRew dailyRew;

        public RewardStatus rewardStatus
        {
            get { return dailyRew.rewardStatus; }
            set
            {
                switch (value)
                {
                    case RewardStatus.Locked:
                        lockedCanvasTransform?.gameObject.SetActive(true);
                        receivedCanvasTransform?.gameObject.SetActive(false);
                        break;

                    case RewardStatus.Available:
                        lockedCanvasTransform?.gameObject.SetActive(false);
                        receivedCanvasTransform?.gameObject.SetActive(false);
                        break;

                    case RewardStatus.Received:
                        lockedCanvasTransform?.gameObject.SetActive(false);
                        receivedCanvasTransform?.gameObject.SetActive(true);
                        break;
                }
            }
        }

        public void OnEnable()
        {
            rewardStatus = rewardStatus; // force update status, because if its disabled its not going to change enable/disable other things
        }

        protected virtual void GetReward()
        {
            Debug.Log("[" + this.GetType().Name + "] " + "Reward receiving init with uid " + dailyRewID);

            dailyRew?.GetReward();
        }

        public virtual void TryGetReward()
        {
            if (dailyRew.rewardStatus == RewardStatus.Available)
                GetReward();
        }

        private void SetStatus(RewardStatus status)
        {
            rewardStatus = status;

            Debug.Log("[" + this.GetType().Name + "] " + "Change Icon " + dailyRewID + " status to : " + status.ToString());
        }



        public virtual void Awake()
        {
            rewardButton.onClick.AddListener(() =>
            {
                TryGetReward();
            });
            rewardStatus = RewardStatus.Locked;
        }

        public void SetUpIcon(DailyRewData data)
        {
            dailyRewID = data.dailyRewID;

            text_daysRowCount.text = data.daysRowCount.ToString();

            image_Icon.sprite = data.icon;
            text_description.text = data.description;

            LinkToDailyRew();
        }

        public void LinkToDailyRew()
        {
            dailyRew = DailyRewManager.Instance.GetDailyRew(dailyRewID);
            dailyRew.OnNewStatus += SetStatus;
        }

        public void OnDestroy()
        {
            dailyRew.OnNewStatus -= SetStatus;
        }
    }
}