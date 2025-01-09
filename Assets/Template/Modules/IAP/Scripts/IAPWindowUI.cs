using System.Collections.Generic;
using UnityEngine;

namespace YGTemplate.IAP
{
    public class IAPWindowUI : WindowUI
    {
        [Header("IAP Window UI")]

        [SerializeField] private Transform _transform_ContainerForIcons;
        public Transform transform_ContainerForIcons
        {
            get
            {
                return _transform_ContainerForIcons;
            }
        }

        [Header("Have To Set if not going to generate")]
        [SerializeField] private List<IAPWindowIconUI> listOfPurchases;

        #region spawn

        public void SpawnIcons(IAPWindowIconUI iconPrefab, IAPPurchaseData[] dataArray)
        {
            listOfPurchases = new List<IAPWindowIconUI>();

            for (int i = 0; i < dataArray.Length; i++)
            {
                SpawnIcon(iconPrefab, dataArray[i]);
            }

        }

        protected void SpawnIcon(IAPWindowIconUI iconPrefab, IAPPurchaseData data)
        {
            IAPWindowIconUI newIcon = Instantiate(iconPrefab, transform_ContainerForIcons); //i don't get it like at all

            newIcon.SetUpIcon(data);

            listOfPurchases.Add(newIcon);

        }

        public void LinkIconsToData()
        {
            if (listOfPurchases == null) return;

            for (int i = 0; i < listOfPurchases.Count; i++)
            {
                listOfPurchases[i].LinkToPurchaseData();
            }
        }
        #endregion
    }
}