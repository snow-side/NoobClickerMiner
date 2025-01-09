using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YGTemplate.Utils;

namespace YGTemplate.FortuneWheel
{
    public class FortuneWheelManager : Singleton<FortuneWheelManager>
    {
        [SerializeField] private bool spawnWindow;
        [SerializeField] private FortuneWheelWindowUI prefab_FortuneWheelWindowUI;


        [SerializeField] public FortuneWheelWindowUI fortuneWheelWindowUI;

        [Range(1, 1000)]
        [SerializeField] private float timeBetweenSpinsAdd = 180f;
        [Range(1, 100)]
        [SerializeField] public float spinningTime;

        [SerializeField] private Vector2Int _minMaxSpinCounts;
        public Vector2Int minMaxSpinCounts
        {
            get { return _minMaxSpinCounts; }    
        }

        [Tooltip("x and y between 0 and 1")]
        [SerializeField] private AnimationCurve _spinWheelCurve;
        public AnimationCurve spinWheelCurve {
            get { return _spinWheelCurve; }
        }


        private int _spinsCount;

        public int spinsCount
        {
            get { return _spinsCount; }
            private set
            {
                _spinsCount = value;
                OnSpinsCountNewVal?.Invoke(_spinsCount);
                OnHaveSpins?.Invoke(IsHaveSpins());
            }
        }

        public UnityEvent<string> OnReward;
        public UnityEvent<int> OnSpinsCountNewVal;
        public UnityEvent<float> OnRestTimeChanged;
        public UnityEvent<bool> OnHaveSpins;

        private float crntInGameTime;
        private int crntIndexRew;

        public float restTimeBeforeSpin
        {
            get
            {
                return Mathf.Max(0, timeBetweenSpinsAdd - crntInGameTime);
            }
        }

        private float deltaTimeMultiplier = 1.0f;

        public List<FortuneWheelSegmentData> listOfFortuneWheelSegmentData;



        public override void Awake()
        {
            base.Awake();
            spinsCount = 0;
            crntIndexRew = -1;
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void LateUpdate()
        {
            UpdateSpinTimer();

        }

        public void GetReward()
        {
            if (crntIndexRew == -1)
            {
                Debug.LogWarning("[" + this.GetType().Name + "] " + "Try get reward when there are no reward, because crntIndexRew = " + crntIndexRew);
                return;
            }

            if (crntIndexRew >= listOfFortuneWheelSegmentData.Count)
            {
                Debug.LogWarning("[" + this.GetType().Name + "] " + "Try get reward with index more than data. crntIndexRew= " + crntIndexRew + "; listOfFortuneWheelSegmentData.Count: " + listOfFortuneWheelSegmentData.Count);
                return;
            }

            Debug.Log("[" + this.GetType().Name + "] " + "Invoke reward from list with index" + crntIndexRew);
            OnReward?.Invoke(listOfFortuneWheelSegmentData[crntIndexRew].rewardData);

            fortuneWheelWindowUI.SetUpRewardData(listOfFortuneWheelSegmentData[crntIndexRew]);
        }

        public void UpdateSpinTimer()
        {
            crntInGameTime += Time.deltaTime * deltaTimeMultiplier;

            if (crntInGameTime >= timeBetweenSpinsAdd)
            {
                crntInGameTime = 0;
                spinsCount += 1;
                OnHaveSpins?.Invoke(true);
            }
            else
            {
                OnRestTimeChanged?.Invoke(restTimeBeforeSpin);
            }
        }

        public void ClickedSpinWheel()
        {

            Debug.Log("[" + this.GetType().Name + "] " + "Try Spin Wheel");
            if (spinsCount <= 0)
            {

                return;
            }

            spinsCount -= 1;
            float spinResValue = GetSpinWeightValue();
            crntIndexRew = GetSegmentSpinIndexValue(spinResValue);


            Debug.Log("[" + this.GetType().Name + "] " + "Get random value for spin " + spinResValue + "; rewardIndex = " + crntIndexRew + "; TotalWeight = " + GetTotalWeight().ToString());
            fortuneWheelWindowUI.StartSpinning(crntIndexRew, GetSpinsCount(minMaxSpinCounts), spinWheelCurve);
        }

        public bool IsHaveSpins()
        {
            return spinsCount > 0;
        }

        private float GetSpinWeightValue()
        {
            float rand = UnityEngine.Random.Range(0, GetTotalWeight());

            return rand;
        }

        private float GetTotalWeight()
        {
            float totalWeight = 0;
            for (int i = 0; i < listOfFortuneWheelSegmentData.Count; i++)
            {
                totalWeight += listOfFortuneWheelSegmentData[i].weight;
            }

            return totalWeight;
        }

        private int GetSegmentSpinIndexValue(float weightValue)
        {
            float weightOffset = 0;
            for (int i = 0; i < listOfFortuneWheelSegmentData.Count; i++)
            {
                if (weightValue <= listOfFortuneWheelSegmentData[i].weight + weightOffset)
                    return i;
                weightOffset += listOfFortuneWheelSegmentData[i].weight;
            }

            return listOfFortuneWheelSegmentData.Count - 1;
        }

        public int GetSpinsCount(Vector2Int minMaxSpinsCount)
        {
            int cnt = UnityEngine.Random.Range(minMaxSpinsCount.x, minMaxSpinsCount.y);

            return cnt;
        }

        #region spawn
        public void SpawnWindow(Transform parent)
        {
            if (!spawnWindow) return;
            fortuneWheelWindowUI = Instantiate(prefab_FortuneWheelWindowUI, parent);
        }

        #endregion

        #region dev_mode_related
        public void SetDeltaTimeMultiplier(float value)
        {
            if (!TemplateManager.DEV_MODE) return;

            deltaTimeMultiplier = value;
        }
        #endregion

        public void ShowWindow()
        {
            fortuneWheelWindowUI.SetActive(true);
        }
    }
}