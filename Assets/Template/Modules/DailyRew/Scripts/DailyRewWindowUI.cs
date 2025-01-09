using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace YGTemplate.DailyReward
{
    public class DailyRewWindowUI : WindowUI
    {
        [Header("DailyRewWindowUI")]
        [SerializeField] private Button button_GetAllRewards;

        [Header("Have To Set If Generate")]
        [SerializeField] private Transform _transform_ContainerForIcons;
        public Transform transform_ContainerForIcons
        {
            get
            {
                return _transform_ContainerForIcons;
            }
        }

        [Header("Have To Set If Not Generate")]
        [SerializeField] private List<DailyRewWindowIconUI> listOfDailyRew;

        public override void Awake()
        {
            base.Awake();

            button_GetAllRewards?.onClick.AddListener(() =>
            {
                GetAllAvailableRewards();
            });
        }


        public void GetAllAvailableRewards()
        {
            for (int i = 0; i < listOfDailyRew.Count; i++)
            {
                listOfDailyRew[i].TryGetReward();
            }
        }

        #region SPAWN

        public void SpawnIcons(DailyRewWindowIconUI iconPrefab, DailyRewData[] dataArray)
        {
            listOfDailyRew = new List<DailyRewWindowIconUI>();

            for (int i = 0; i < dataArray.Length; i++)
            {
                SpawnIcon(iconPrefab, dataArray[i]);
            }
        }

        protected void SpawnIcon(DailyRewWindowIconUI iconPrefab, DailyRewData data)
        {
            DailyRewWindowIconUI newIcon = Instantiate(iconPrefab, transform_ContainerForIcons);

            newIcon.SetUpIcon(data);

            listOfDailyRew.Add(newIcon);
        }

        #endregion

        public void LinkIconsToData()
        {
            if (listOfDailyRew == null) return;

            for (int i = 0; i < listOfDailyRew.Count; i++)
            {
                listOfDailyRew[i].LinkToDailyRew();
            }
        }
    }
}