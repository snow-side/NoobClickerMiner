using System;
using UnityEngine;

namespace YGTemplate.FortuneWheel
{
    [Serializable]
    public class FortuneWheelSegmentData
    {
        public float weight;
        public string rewardData;
        public Sprite icon;
        public string description;
        public Color color = Color.white;

        public FortuneWheelSegmentData()
        {
            weight = 1f;
            color = Color.white;
        }
    }
}