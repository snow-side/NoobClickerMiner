using System.Collections.Generic;
using System;
using YGTemplate;
using YGTemplate.DailyReward;

/*
 part of part class
which belong to daily rew manager
 */

namespace YG
{
    public partial class SavesYG
    {

        #region DailyRew

        public DateTime lastRowDateTime;
        public int daysRowCount;
        public List<DailyRewSave> dailyRewStatusData = new();

        public void SetDailyRewSave(string dailyRewID, RewardStatus status) {

            bool found = false;
            foreach (DailyRewSave dailyRewSave in dailyRewStatusData)
            {
                if (dailyRewID == dailyRewSave.dailyRewID)
                {
                    dailyRewSave.status = status;
                    found = true;
                }
            }

            if (!found) {
                DailyRewSave newData = new DailyRewSave();
                newData.dailyRewID = dailyRewID;
                newData.status = status;

                dailyRewStatusData.Add(newData);
            }
        }

        #endregion
    }
}
