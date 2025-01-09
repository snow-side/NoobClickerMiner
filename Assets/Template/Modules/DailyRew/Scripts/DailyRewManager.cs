using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YG;
using YGTemplate.Utils;

namespace YGTemplate.DailyReward
{
    public class DailyRewManager : Singleton<DailyRewManager>
    {
        [SerializeField] private DailyRewData[] listOfDailyRewData;
        [SerializeField] private bool spawnWindowForDailyRew;
        [SerializeField] private DailyRewWindowUI prefab_DailyRewWindowUI;
        [SerializeField] private bool spawnWindowIconForDailyRew;
        [SerializeField] private DailyRewWindowIconUI prefab_DailyRewWindowIconUI;

        public DailyRewWindowUI dailyRewWindowUI;

        public DateTime lastRowDateTime { get; private set; }
        public int daysRowCount { get; private set; }

        [Space(15)]
        public UnityEvent<bool> OnAvailableReward;
        public UnityEvent<string> OnReward;
        public UnityEvent OnCheckDate;
        public UnityEvent<int> OnNewDaysRowValue;

        private int additionalHours;

#if UNITY_EDITOR
        [SerializeField] private List<DailyRew> listOfDailyRew;
#endif

        private Dictionary<string, DailyRew> dictOfDailyRew;

        [SerializeField] private float checkDateCooldown;
        private float crntCheckDateTime;

        public event Action OnHaveNewData;

        public override void Awake()
        {
            base.Awake();
            additionalHours = 0;
            SpawnDailyRew();
        }

        public void Start()
        {

            /*
             when to save
             */

            OnNewDaysRowValue.AddListener(SetData);
            OnReward.AddListener(SetData);
        }

        public void LateUpdate()
        {
            UpdateCheckDate();
        }

        public void InitUI()
        {
            if (!spawnWindowForDailyRew)
                SpawnOrLinkIcons();
        }

        #region save_load_part
        // load
        public void GetData()
        {
            lastRowDateTime = YG2.saves.lastRowDateTime;
            daysRowCount = YG2.saves.daysRowCount;
            for (int i = 0; i < YG2.saves.dailyRewStatusData.Count; i++)
            {
                TrySetStatusInDailyRew(
                    YG2.saves.dailyRewStatusData[i].dailyRewID,
                    YG2.saves.dailyRewStatusData[i]
                    );
            }
        }

        // Save
        public void SetData(int i)
        {
            SetData();
        }
        public void SetData(string i)
        {
            SetData();
        }

        public void SetData()
        {
            YG2.saves.lastRowDateTime = lastRowDateTime;
            YG2.saves.daysRowCount = daysRowCount;

            foreach (KeyValuePair<string, DailyRew> kvp in dictOfDailyRew)
            {
                YG2.saves.SetDailyRewSave(
                    kvp.Value.dailyRewID,
                    kvp.Value.rewardStatus
                    );
            }

            OnHaveNewData?.Invoke();
        }

        public void TrySetStatusInDailyRew(string dailyRewID, DailyRewSave data)
        {
            if (dictOfDailyRew == null) return;
            if (!dictOfDailyRew.ContainsKey(dailyRewID)) return;

            dictOfDailyRew[dailyRewID].rewardStatus = data.status;
        }

        #endregion

        public void CheckDate()
        {
            int daysDiff = DaysRowDiff();

            OnCheckDate?.Invoke();

            if (IsHaveNewData(daysDiff))
            {
                UpdateDaysRowCount(daysDiff);
            }
        }

        public bool IsHaveNewData(int daysDiff)
        {

            if (daysDiff == 0)
                return false;
            else if (daysDiff == 1)
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        public void UpdateDaysRowCount(int daysDiff)
        {

            if (daysDiff == 1)
            {
                daysRowCount++;
                lastRowDateTime = GetCurrentDate();
                OnNewDaysRowValue?.Invoke(daysRowCount);

                Debug.Log("[" + this.GetType().Name + "] " + "Updated Days Row Count:  " + daysRowCount);

            }
            else if (daysDiff > 1)
            {
                daysRowCount = 0;
                lastRowDateTime = GetCurrentDate();
                OnNewDaysRowValue?.Invoke(daysRowCount);

                Debug.Log("[" + this.GetType().Name + "] " + "Updated Days Row Count:  " + daysRowCount);
            }
        }

        public int DaysRowDiff()
        {
            DateTime serverDateTime = GetCurrentDate();

            Debug.Log("[" + this.GetType().Name + "] " + "Server Date Time " + serverDateTime.ToString());

            int daysDiff = serverDateTime.DayOfYear - lastRowDateTime.DayOfYear;

            if (serverDateTime.DayOfYear == 1 && lastRowDateTime.DayOfYear == (new DateTime(lastRowDateTime.Year, 12, 31)).DayOfYear)
            {
                daysDiff = 1;
            }

            Debug.Log("[" + this.GetType().Name + "] " + "Days Diff  " + daysDiff);

            return daysDiff;
        }

        public DateTime GetCurrentDate()
        {
            DateTime serverDateTime = DateTimeOffset.FromUnixTimeMilliseconds(YG2.ServerTime()).DateTime;

            serverDateTime = serverDateTime.AddHours(additionalHours);

            Debug.Log("Current DateTime " + serverDateTime);

            return serverDateTime;
        }


        public void UpdateCheckDate()
        {
            crntCheckDateTime += Time.deltaTime;

            if (crntCheckDateTime >= checkDateCooldown)
            {
                crntCheckDateTime = 0;
                CheckDate();
            }
        }

        #region Rewards

        public void GetReward(string data)
        {
            OnReward?.Invoke(data);

            OnAvailableReward?.Invoke(IsHaveAvailableRewards());
        }

        public void AvailableReward(string dailyRewID)
        {
            OnAvailableReward?.Invoke(true);

            Debug.Log("[" + this.GetType().Name + "] " + "DailyRew become available with id" + dailyRewID);
        }

        public bool IsHaveAvailableRewards()
        {
            if (dictOfDailyRew == null) return false;

            foreach (KeyValuePair<string, DailyRew> kvp in dictOfDailyRew)
            {
                if (kvp.Value.IsAvailable())
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region SPAWN

        public void SpawnDailyRew()
        {
#if UNITY_EDITOR
            listOfDailyRew = new List<DailyRew>(); //todo remove
#endif


            dictOfDailyRew = new Dictionary<string, DailyRew>();

            for (int i = 0; i < listOfDailyRewData.Length; i++)
            {
                DailyRew newDailyRew = new DailyRew(listOfDailyRewData[i]);

                dictOfDailyRew.Add(newDailyRew.dailyRewID, newDailyRew);

#if UNITY_EDITOR
                listOfDailyRew.Add(newDailyRew); //todo remove
#endif
            }


        }

        public void SpawnWindow(Transform parent)
        {
            if (!spawnWindowForDailyRew) return;

            dailyRewWindowUI = Instantiate(prefab_DailyRewWindowUI, parent);
            SpawnOrLinkIcons();
        }

        public void SpawnOrLinkIcons()
        {
            if (spawnWindowIconForDailyRew)
            {
                dailyRewWindowUI.SpawnIcons(prefab_DailyRewWindowIconUI, listOfDailyRewData);
            }
            else
            {
                dailyRewWindowUI.LinkIconsToData();
            }
        }

        #endregion

        #region dev_mode_related
        public void AddHoursToServer(int count)
        {
            if (!TemplateManager.DEV_MODE) return;
            additionalHours += count;
        }
        #endregion

        #region dict_manipulation
        public DailyRew GetDailyRew(string dailyRewID)
        {
            if (dictOfDailyRew == null)
            {
                Debug.Log("[" + this.GetType().Name + "] " + "Try get access when there are no dictionary  " + dailyRewID);
                return null;
            }

            if (!dictOfDailyRew.ContainsKey(dailyRewID))
            {

                Debug.Log("[" + this.GetType().Name + "] " + "Try get DailyRew from dict with nonExists id  " + dailyRewID);
                return null;
            }

            return dictOfDailyRew[dailyRewID];
        }
        #endregion

        public void ShowWindow()
        {
            dailyRewWindowUI.SetActive(true);
        }

        public void OnDestroy()
        {
            OnNewDaysRowValue.RemoveListener(SetData);
            OnReward.RemoveListener(SetData);
        }
    }
}