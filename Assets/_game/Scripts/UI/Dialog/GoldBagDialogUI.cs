using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGTemplate.Ads;
using YGTemplate.Localization;

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
        LocalizationManager.Instance.OnLanguageChange.AddListener(UpdateUI);
    }

    public override void Show()
    {
        UpdateUI();
        var gold = Wallet.Instance.GetGoldInBag();
        BtnTake.interactable = gold > 0;
        base.Show();
    }

    public void UpdateUI(string lang) {
        UpdateUI();
    }

    public void UpdateUI() {
        var gold = Wallet.Instance.GetGoldInBag();
        var rate = Wallet.Instance.GetGoldBagRate();
        grifferText.text = string.Format(LocalizationManager.Instance.GetLocalizedText("griffer_Text"), rate);
        //        grifferText.text = $"<color=white>ХА-ХА-ХА я забираю <color=green>{rate}%</color> твоих добытых алмазов</color>";
        goldText.text = string.Format(LocalizationManager.Instance.GetLocalizedText("griffer_Gold"), gold.Short());
        //goldText.text = $"<color=white>У меня <color=#00FFFF>{gold.Short()}</color>";
    }

    public void OnDestroy()
    {
        LocalizationManager.Instance.OnLanguageChange.RemoveListener(UpdateUI);
    }
}
