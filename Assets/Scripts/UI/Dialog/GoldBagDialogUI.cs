using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            YandexSdk.Instance.RewardAd(() =>
            {
                Wallet.Instance.TakeGoldFromBag();
                Hide();
                YandexSdk.Instance.Goal("reward_goldbag", string.Empty);
            });
        });
    }

    public override void Show()
    {
        var gold = Wallet.Instance.GetGoldInBag();
        var rate = Wallet.Instance.GetGoldBagRate();
        grifferText.text = $"<color=white>ХА-ХА-ХА я забираю <color=green>{rate}%</color> твоего добытого золота</color>";
        goldText.text = $"<color=white>У меня <color=#FFD100>{gold.Short()}</color>";
        BtnTake.interactable = gold > 0;
        base.Show();
    }
}
