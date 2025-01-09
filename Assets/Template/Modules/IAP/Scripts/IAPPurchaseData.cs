using UnityEngine;

namespace YGTemplate.IAP
{
    [CreateAssetMenu(fileName = "IAPPurchaseData", menuName = "Scriptable Objects/IAPPurchaseData")]
    public class IAPPurchaseData : ScriptableObject
    {
        public string purchaseID;
        public int price;
        public Sprite icon;
        public string title;
        public string description;
        public string purchaseData;
    }
}