using UnityEngine;
using UnityEngine.Events;
using YG;
using YG.Utils.Pay;
using YGTemplate.Utils;

namespace YGTemplate.IAP
{
    public class IAPManager : Singleton<IAPManager>
    {
        [SerializeField] private IAPPurchaseData[] purchasesData;
        [SerializeField] private bool spawnWindowForIAPManager;
        [SerializeField] private IAPWindowUI prefab_IAPWindowUI;

        [SerializeField] private bool spawnWindowIcons;
        [SerializeField] private IAPWindowIconUI prefab_IAPWindowIconPrefab;
        //    [SerializeField] private GameObject prefab_IAPWindowIconPrefab_GO;

        public IAPWindowUI iapWindowUI;

        public UnityEvent<string> OnPurchaseSuccess;
        public UnityEvent<string> OnPurchaseFailed;

        public override void Awake()
        {
            base.Awake();
        }

        public void Start()
        {
            CreatePurchaseArray();
            YG2.onPurchaseSuccess += PurchaseSuccess;
            YG2.onPurchaseFailed += PurchaseFailed;
        }

        public void SpawnWindow(Transform parent)
        {
            if (!spawnWindowForIAPManager) return;

            iapWindowUI = Instantiate(prefab_IAPWindowUI, parent);
            SpawnOrLinkIcons();
        }

        public void SpawnOrLinkIcons()
        {
            if (spawnWindowIcons)
            {
                //prefab_IAPWindowUI.SpawnIcons(prefab_IAPWindowIconPrefab_GO, purchasesData);
                iapWindowUI.SpawnIcons(prefab_IAPWindowIconPrefab, purchasesData);
            }
            else
            {
                iapWindowUI.LinkIconsToData();
            }
        }

        public void CreatePurchaseArray()
        {
            Debug.LogWarning("[" + this.GetType().Name + "] " + "Recreate purchases array" + purchasesData.Length);

            YG2.purchases = new Purchase[purchasesData.Length];

            /*
             if smth goes wrong 
            in the purchaseYG you should comment line with 
            //        private void Start() => UpdateEntries(YG2.PurchaseByID(id));

            because we don't want to update on start, because it can break us
            so...
            you have to update by your own hands somewhere else

             */

            for (int i = 0; i < purchasesData.Length; i++)
            {
                YG2.purchases[i] = new Purchase
                {
                    id = purchasesData[i].purchaseID,
                    title = purchasesData[i].title,
                    description = purchasesData[i].description,
                    price = purchasesData[i].price.ToString()
                };
            }
        }

        public void ShowWindow()
        {
            iapWindowUI.SetActive(true);
        }

        public void PurchaseSuccess(string purchID)
        {
            Debug.Log("[" + this.GetType().Name + "] " + "Successful purchase with id: " + purchID);
            //        OnPurchaseSuccess?.Invoke(purchID);

            OnPurchaseSuccess?.Invoke(GetPurchasesRewardData(purchID));
        }

        public void PurchaseFailed(string purchID)
        {
            Debug.LogWarning("[" + this.GetType().Name + "] " + "There are exists failed purchase with id: " + purchID);
            // if failed, try to check array, maybe we don't have that kind of purchase at all => will generate error

            // 
            OnPurchaseFailed?.Invoke(GetPurchasesRewardData(purchID));
        }

        public void OnDestroy()
        {

        }

        // yeah, it's just mapping purchId from yg into reward data string from game
        // that's all
        public string GetPurchasesRewardData(string purchID)
        {
            for (int i = 0; i < purchasesData.Length; i++)
            {
                if (purchasesData[i].purchaseID.Equals(purchID))
                {
                    return purchasesData[i].purchaseData;
                }
            }

            Debug.LogWarning("[" + this.GetType().Name + "] " + "There are no purchase in data with id: " + purchID);
            return "";
        }
    }
}