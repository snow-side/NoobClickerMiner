using System;

namespace YGTemplate.DailyReward
{
    [Serializable]
    public class DailyRew
    {
        public RewardStatus _rewardStatus; // todo back to private

        public string dailyRewID;
        public int daysRowCount;
        public string rewardData;

        public event Action<RewardStatus> OnNewStatus;

        public RewardStatus rewardStatus
        {
            get { return _rewardStatus; }
            set
            {
                _rewardStatus = value;
                OnNewStatus?.Invoke(value);
            }
        }

        public DailyRew(DailyRewData data)
        {
            dailyRewID = data.dailyRewID;
            daysRowCount = data.daysRowCount;
            rewardData = data.rewardData;
            rewardStatus = RewardStatus.Locked;

            DailyRewManager.Instance.OnNewDaysRowValue.AddListener(CheckData);
        }

        ~DailyRew()
        {
            DailyRewManager.Instance.OnNewDaysRowValue.RemoveListener(CheckData);
        }

        public void CheckData(int crntDaysRow)
        {
            if (rewardStatus != RewardStatus.Locked) return;

            if (crntDaysRow >= daysRowCount)
            {
                rewardStatus = RewardStatus.Available;
                DailyRewManager.Instance.AvailableReward(dailyRewID);
            }
        }

        public void GetReward()
        {
            if (rewardStatus != RewardStatus.Available) return;

            rewardStatus = RewardStatus.Received;
            DailyRewManager.Instance.GetReward(rewardData);
        }

        public bool IsAvailable()
        {
            return rewardStatus == RewardStatus.Available;
        }
    }
}