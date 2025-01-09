using UnityEngine;

namespace YGTemplate.IAP
{
    public class IAPIconUI : IconUI
    {
        public override void OnIconClicked()
        {
            IAPManager.Instance.ShowWindow();
        }

        public override void Start()
        {
            base.Start();
            window = IAPManager.Instance.iapWindowUI;
            SetNotificationMark(false);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}