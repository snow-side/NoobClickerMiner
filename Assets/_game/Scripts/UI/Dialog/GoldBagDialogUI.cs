using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGTemplate.Ads;

public class GoldBagDialogUI : DialogBaseUI
{
    [SerializeField] TextMeshProUGUI grifferText;
    [SerializeField] TextMeshProUGUI goldText;

    [SerializeField]
    Button BtnTake;

    void Start()
    {
        BtnTake.onClick.AddListener(() =>
        {
            AdsManager.Instance.ShowRewardAds(() =>
            {

                Wallet.Instance.TakeGoldFromBag();
                Hide();
                Debug.LogWarning("[" + this.GetType().Name + "] " + "Init Reward Ads with func Wallet.Instance.TakeGoldFromBag");
            });
        });
    }

    public override void Show()
    {
        var gold = Wallet.Instance.GetGoldInBag();
        var rate = Wallet.Instance.GetGoldBagRate();
        grifferText.text = $"<color=white>ХА-ХА-ХА я забираю <color=green>{rate}%</color> твоих добытых алмазов</color>";
        goldText.text = $"<color=white>У меня <color=#00FFFF>{gold.Short()}</color>";
        BtnTake.interactable = gold > 0;
        base.Show();
    }
}
