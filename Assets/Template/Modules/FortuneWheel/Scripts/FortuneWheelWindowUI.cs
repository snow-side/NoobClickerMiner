using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGTemplate.Utils;

namespace YGTemplate.FortuneWheel
{
    public class FortuneWheelWindowUI : WindowUI
    {
        [SerializeField] private FortuneWheelSegment prefab_FortuneWheelSegmentUI;
        [SerializeField] private Transform wheel;

        private List<FortuneWheelSegment> listOfSegments;

        [SerializeField] private Button button_SpinWheel;
        [SerializeField] private Button button_StopSpinning;

        [SerializeField] private TextMeshProUGUI text_SpinsCount;
        [SerializeField] private TextMeshProUGUI text_RestTime;

        [SerializeField] private Canvas canvas_RewInfo;
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardText;

        private float totalWeight;

        private List<float> curvePts;
        private bool isSpinning;
        private float timeInSpinning;

        private AnimationCurve crntAnimCurve;
        private float startPoint;
        private float endPoint;

        public override void Awake()
        {
            base.Awake();
            button_SpinWheel.onClick.AddListener(() =>
            {
                PlayClickSound();
                ClickedSpinWheel();
            });
            button_StopSpinning.onClick.AddListener(() =>
            {
                PlayClickSound();
                StopSpinning();
            });
            canvas_RewInfo.gameObject.SetActive(false);
            button_StopSpinning.gameObject.SetActive(false);
        }

        public void Start()
        {
            FortuneWheelManager.Instance.OnSpinsCountNewVal.AddListener(UpdateSpinsCount);
            FortuneWheelManager.Instance.OnRestTimeChanged.AddListener(UpdateRestTimeBeforeSpin);

            UpdateSpinsCount(FortuneWheelManager.Instance.spinsCount);
            UpdateRestTimeBeforeSpin(FortuneWheelManager.Instance.restTimeBeforeSpin);
        }

        public void OnEnable()
        {
            SpawnSegments();

            UpdateSpinsCount(FortuneWheelManager.Instance.spinsCount);
            UpdateRestTimeBeforeSpin(FortuneWheelManager.Instance.restTimeBeforeSpin);
        }

        public void Update()
        {
            UpdateWheelRotation();
        }


        public void UpdateWheelRotation()
        {
            if (!isSpinning) return;

            timeInSpinning += Time.deltaTime;

            float rTimeSpinning = timeInSpinning / FortuneWheelManager.Instance.spinningTime;

            rTimeSpinning = Mathf.Clamp01(rTimeSpinning);

            float crntRotZ = 0;

            if (crntAnimCurve == null)
            {
                crntRotZ = Util.Bezier(rTimeSpinning, curvePts);
            }
            else
            {
                crntRotZ = startPoint + crntAnimCurve.Evaluate(rTimeSpinning) * (endPoint- startPoint);
            }

            wheel.transform.eulerAngles = new Vector3(0, 0, crntRotZ);

            if (rTimeSpinning == 1)
            {
                StopSpinning();
            }
        }

        public void StopSpinning()
        {
            FortuneWheelManager.Instance.GetReward();

            if (!isSpinning) return;

            wheel.transform.eulerAngles = new Vector3(0, 0, endPoint);
            isSpinning = false;
            timeInSpinning = 0;

            button_StopSpinning.gameObject.SetActive(false);
        }

        private void SpawnSegments()
        {

            List<FortuneWheelSegmentData> listData = FortuneWheelManager.Instance.listOfFortuneWheelSegmentData;

            ClearWheel();
            listOfSegments = new List<FortuneWheelSegment>();

            totalWeight = 0;
            for (int i = 0; i < listData.Count; i++)
            {
                totalWeight += listData[i].weight;
            }

            float weightOffset = 0;
            for (int i = 0; i < listData.Count; i++)
            {
                SpawnSegment(listData[i], totalWeight, weightOffset);

                if (i == 0)
                {
                    weightOffset += listData[i].weight / 2;
                }
                else
                {
                    weightOffset += listData[i].weight;
                }
            }
        }

        private void SpawnSegment(FortuneWheelSegmentData data, float totalWeight, float weightOffset)
        {
            FortuneWheelSegment newSegment = Instantiate(prefab_FortuneWheelSegmentUI, wheel);

            newSegment.SetUpSegment(data, totalWeight);

            float weightOffsetEnd = weightOffset;
            if (weightOffset != 0)
            {
                weightOffsetEnd += data.weight / 2;
            }

            Vector3 rot = new Vector3(0, 0, weightOffsetEnd / totalWeight * 360);
            newSegment.transform.Rotate(rot);
            newSegment.centerOffset = weightOffsetEnd / totalWeight * 360;



            listOfSegments.Add(newSegment);
        }

        public void StartSpinning(int indexRes, int spinsCount=1, AnimationCurve curve =null)
        {
            endPoint = CalculateEndVal(indexRes,spinsCount);
            startPoint = wheel.transform.eulerAngles.z;

            if (curve == null)
            {
                float midPoint = 0.75f * (endPoint - startPoint);
                curvePts = new List<float> { startPoint, midPoint, endPoint };
            }
            else {
                crntAnimCurve = curve;
            }

            timeInSpinning = 0;
            isSpinning = true;

            button_StopSpinning.gameObject.SetActive(true);

            Debug.Log("[" + this.GetType().Name + "] " + "Create list for z rotation; Start Poinå: " + startPoint + "; End point = " + endPoint + "; Spins count= " + spinsCount);

        }

        public float CalculateEndVal(int indexRes, int spinsCount=1) {
            float s = (-1) * (listOfSegments[indexRes].centerOffset + spinsCount * 360 + UnityEngine.Random.Range(listOfSegments[indexRes].weight / totalWeight * (-180), listOfSegments[indexRes].weight / totalWeight * 180));
            return s;
        }

        public void ClickedSpinWheel()
        {
            if (isSpinning) return;

            FortuneWheelManager.Instance.ClickedSpinWheel();
        }



        public void UpdateSpinsCount(int count)
        {
            text_SpinsCount.text = count.ToString();
        }

        public void UpdateRestTimeBeforeSpin(float value)
        {
            text_RestTime.text = Mathf.FloorToInt(value).ToString();
        }

        public void SetUpRewardData(FortuneWheelSegmentData rewData)
        {
            canvas_RewInfo.gameObject.SetActive(true);

            rewardImage.sprite = rewData.icon;
            rewardText.text = rewData.description;
        }

        public void OnDesctroy()
        {
            FortuneWheelManager.Instance.OnSpinsCountNewVal.RemoveListener(UpdateSpinsCount);
            FortuneWheelManager.Instance.OnRestTimeChanged.RemoveListener(UpdateRestTimeBeforeSpin);
        }

        public void ClearWheel()
        {
            if (listOfSegments == null) return;

            while (listOfSegments.Count > 0)
            {
                FortuneWheelSegment segm = listOfSegments[0];
                Destroy(segm.gameObject);
                listOfSegments.RemoveAt(0);
            }
        }
    }
}