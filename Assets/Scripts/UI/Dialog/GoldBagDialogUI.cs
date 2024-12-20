using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldBagDialogUI : DialogBaseUI
{
    [SerializeField]
    TextMeshProUGUI Text;

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
        Text.text = FormatText(gold, rate);
        BtnTake.interactable = gold > 0;
        base.Show();
    }

    string FormatText(float gold, float rate)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"<color=white>Нубик забирает себе <color=green>{rate}%</color> добытого золота</color>");
        builder.AppendLine($"<color=green>Сейчас в сумке:</color> <color=#FFD100>{gold.Short()} золота</color>");
        return builder.ToString();
    }
}
