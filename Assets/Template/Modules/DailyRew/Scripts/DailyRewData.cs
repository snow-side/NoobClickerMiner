using UnityEngine;

namespace YGTemplate.DailyReward
{
    [CreateAssetMenu(fileName = "DailyRewData", menuName = "Scriptable Objects/DailyRewData")]
    public class DailyRewData : ScriptableObject
    {
        public string dailyRewID;
        public int daysRowCount;
        public string rewardData;

        [Header("For Generation")]
        public Sprite icon;
        public string description;
    }
}