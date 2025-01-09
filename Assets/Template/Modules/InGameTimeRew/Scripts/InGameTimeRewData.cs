using System;
using UnityEngine;

namespace YGTemplate.InGameTimeReward
{
    [CreateAssetMenu(fileName = "InGameTimeRewData", menuName = "Scriptable Objects/InGameTimeRewData")]
    public class InGameTimeRewData : ScriptableObject
    {
        public string inGameTimeRewID;
        public float rewardTime;
        public string rewardData;

        [Header("For Generation")]
        public Sprite icon;
        public string description;
    }
}