using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YGTemplate.FortuneWheel
{
    public class FortuneWheelSegment : MonoBehaviour
    {
        public Image image_Segment;
        public TextMeshProUGUI text_Description;

        public AspectRatioFitter dataContainerAspectRatio;
        public float weight;
        public Color color;
        public string rewardData;

        public float centerOffset;

        public void SetUpSegment(FortuneWheelSegmentData data, float totalWeight)
        {

            weight = data.weight;
            color = data.color;
            text_Description.text = data.description;
            rewardData = data.rewardData;

            image_Segment.fillAmount = data.weight / totalWeight;
            Vector3 rot = new Vector3(0, 0, -weight / totalWeight * 180);
            image_Segment.transform.Rotate(rot);
            image_Segment.color = color;

            dataContainerAspectRatio.aspectRatio = 1f / Mathf.Tan(weight / totalWeight);
        }
    }
}
