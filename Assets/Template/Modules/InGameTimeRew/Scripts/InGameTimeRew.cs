using System;

namespace YGTemplate.InGameTimeReward
{
    [Serializable]
    public class InGameTimeRew
    {
        public RewardStatus _rewardStatus; // todo back to private

        public string inGameTimeRewID;
        public float rewardTime;
        public string rewardData;

        public event Action<RewardStatus> OnNewStatus;
        public event Action<float> OnNewRestTime;

        public RewardStatus rewardStatus
        {
            get { return _rewardStatus; }
            set
            {
                _rewardStatus = value;
                OnNewStatus?.Invoke(value);
            }
        }

        public InGameTimeRew(InGameTimeRewData data)
        {
            inGameTimeRewID = data.inGameTimeRewID;
            rewardTime = data.rewardTime;
            rewardData = data.rewardData;

            rewardStatus = RewardStatus.Locked;

            InGameTimeRewManager.Instance.OnGameTimeChanged.AddListener(CheckTimer);
        }

        ~InGameTimeRew()
        {
            InGameTimeRewManager.Instance.OnGameTimeChanged.RemoveListener(CheckTimer);
        }

        public void CheckTimer(float crntTime)
        {
            if (rewardStatus != RewardStatus.Locked) return;
            if (rewardTime < 1) return;

            if (crntTime >= rewardTime)
            {
                rewardStatus = RewardStatus.Available;
                InGameTimeRewManager.Instance.AvailableReward(inGameTimeRewID);
            }
            else
            {
                OnNewRestTime?.Invoke(rewardTime - crntTime);
            }
        }

        public void GetReward()
        {
            if (rewardStatus != RewardStatus.Available) return;

            rewardStatus = RewardStatus.Received;
            InGameTimeRewManager.Instance.GetReward(rewardData);
        }

        public bool IsAvailable()
        {
            return rewardStatus == RewardStatus.Available;
        }
    }
}