using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YGTemplate.InGameTimeReward
{
    public class InGameTimeWindowIconUI : MonoBehaviour
    {
        [SerializeField] private string inGameTimeRewID;

        [SerializeField] private Button rewardButton;
        [SerializeField] private Transform lockedCanvasTransform;
        [SerializeField] private Transform receivedCanvasTransform;

        [SerializeField] private Image image_Icon;
        [SerializeField] private TextMeshProUGUI text_Timer;
        [SerializeField] private TextMeshProUGUI text_Description;

        private InGameTimeRew inGameTimeRew;

        public RewardStatus rewardStatus
        {
            get { return inGameTimeRew.rewardStatus; }
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
            rewardStatus = rewardStatus;
        }

        protected virtual void GetReward()
        {
            Debug.Log("[" + this.GetType().Name + "] " + "Reward receiving init with uid " + inGameTimeRewID);

            inGameTimeRew?.GetReward();
        }

        public virtual void TryGetReward()
        {
            if (rewardStatus == RewardStatus.Available)
                GetReward();
        }

        private void SetStatus(RewardStatus status)
        {
            rewardStatus = status;

            Debug.Log("[" + this.GetType().Name + "] " + "Change Icon " + inGameTimeRewID + " status to : " + status.ToString());
        }

        public virtual void Awake()
        {
            rewardButton.onClick.AddListener(() =>
            {
                TryGetReward();
            });
            rewardStatus = RewardStatus.Locked;
        }

        public void SetUpIcon(InGameTimeRewData data)
        {
            inGameTimeRewID = data.inGameTimeRewID;

            image_Icon.sprite = data.icon;
            text_Description.text = data.description;

            UpdateTimer(data.rewardTime);

            LinkToInGameTimeRew();
        }

        public void LinkToInGameTimeRew()
        {
            inGameTimeRew = InGameTimeRewManager.Instance.GetInGameTimeRew(inGameTimeRewID);
            inGameTimeRew.OnNewStatus += SetStatus;
            inGameTimeRew.OnNewRestTime += UpdateTimer;
        }

        public void OnDestroy()
        {
            inGameTimeRew.OnNewStatus -= SetStatus;
            inGameTimeRew.OnNewRestTime -= UpdateTimer;
        }

        public void UpdateTimer(float restTime)
        {
            int seconds = Mathf.FloorToInt(restTime);


            int time_hr = seconds / 3600;
            int time_min = (seconds - time_hr * 3600) / 60;
            int time_sec = seconds % 60;

            string time_str = string.Format("{0:d2}:{1:d2}:{2:d2}", time_hr, time_min, time_sec);

            text_Timer.text = time_str;
        }
    }
}