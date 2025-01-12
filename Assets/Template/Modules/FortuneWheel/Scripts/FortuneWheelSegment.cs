using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YGTemplate.FortuneWheel
{
    public class FortuneWheelSegment : MonoBehaviour
    {
        [SerializeField] private Image image_Segment;

        [SerializeField] private Image image_Icon;
        [SerializeField] private TextMeshProUGUI text_Description;

        [SerializeField] private AspectRatioFitter dataContainerAspectRatio;
        [SerializeField] private float aspRationMult = 1f;

        [Header("Set Dynamically")]
        [SerializeField] private float _weight;
        public float weight {
            get { return _weight; }
            }
        [SerializeField] private Color _color;
        public Color color {
            get { return _color; }
        }
        [SerializeField] private string _rewardData;
        public string rewardData
        {
            get { return _rewardData; }
        }
        public float centerOffset;

        public void SetUpSegment(FortuneWheelSegmentData data, float totalWeight)
        {

            _weight = data.weight;
            _color = data.color;
            text_Description.text = data.description;
            _rewardData = data.rewardData;
            image_Icon.sprite = data.icon;

            image_Segment.fillAmount = data.weight / totalWeight;
            Vector3 rot = new Vector3(0, 0, -weight / totalWeight * 180);
            image_Segment.transform.Rotate(rot);
            image_Segment.color = _color;
            

            dataContainerAspectRatio.aspectRatio = aspRationMult / Mathf.Tan(weight / totalWeight);
        }
    }
}
