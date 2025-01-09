using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using YG.Utils.Pay;

namespace YGTemplate.IAP
{
    [RequireComponent(typeof(PurchaseYG))]
    public class IAPWindowIconUI : MonoBehaviour
    {
        [SerializeField] public string purchaseID { get; private set; }
        [SerializeField] private Image image_Icon;
        [SerializeField] private Button button_BuyPurchase;

        [SerializeField] private PurchaseYG purchaseYG;

        public void Awake()
        {
            button_BuyPurchase?.onClick.AddListener(() =>
            {
                purchaseYG.BuyPurchase();
            });
        }


        public void SetUpIcon(IAPPurchaseData data)
        {
            if (purchaseYG == null) purchaseYG = GetComponent<PurchaseYG>();
            purchaseID = data.purchaseID;

            /*
            first of all get data from array in YG
            after that, update what we want to update
            */

            purchaseYG.id = purchaseID;
            purchaseYG.UpdateEntries(YG2.PurchaseByID(purchaseID));


            image_Icon.sprite = data.icon;
        }

        public void LinkToPurchaseData()
        {
            if (purchaseYG == null) purchaseYG = GetComponent<PurchaseYG>();
            purchaseYG.id = purchaseID;
        }
    }
}