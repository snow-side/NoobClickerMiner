using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YGTemplate.InGameTimeReward
{
    public class InGameTimeWindowUI : WindowUI
    {
        [Header("InGameTimeWindowUI")]
        [SerializeField] private Button button_GetAllReward;

        [Header("Have To Set If Generate")]
        [SerializeField] private Transform _transform_ContainerForIcons;
        public Transform transform_ContainerForIcons
        {
            get
            {
                return _transform_ContainerForIcons;
            }
        }

        [Header("Have To Set if not going to generate")]
        [SerializeField] private List<InGameTimeWindowIconUI> listOfRewWindowIcon;

        public override void Awake()
        {
            base.Awake();

            button_GetAllReward?.onClick.AddListener(() =>
            {
                GetAllRewards();
                PlayClickSound();
            });
        }

        public void GetAllRewards()
        {
            for (int i = 0; i < listOfRewWindowIcon.Count; i++)
            {
                listOfRewWindowIcon[i].TryGetReward();
            }
        }


        #region spawn
        public void SpawnIcons(InGameTimeWindowIconUI iconPrefab, InGameTimeRewData[] dataArray)
        {
            listOfRewWindowIcon = new List<InGameTimeWindowIconUI>();

            for (int i = 0; i < dataArray.Length; i++)
            {
                SpawnIcon(iconPrefab, dataArray[i]);
            }
        }

        protected void SpawnIcon(InGameTimeWindowIconUI iconPrefab, InGameTimeRewData data)
        {
            InGameTimeWindowIconUI newIcon = Instantiate(iconPrefab, transform_ContainerForIcons);

            newIcon.SetUpIcon(data);

            listOfRewWindowIcon.Add(newIcon);
        }
        #endregion

        public void LinkIconsToData()
        {
            if (listOfRewWindowIcon == null) return;

            for (int i = 0; i < listOfRewWindowIcon.Count; i++)
            {
                listOfRewWindowIcon[i].LinkToInGameTimeRew();
            }
        }
    }
}