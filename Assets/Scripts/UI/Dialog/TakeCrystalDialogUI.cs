using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TakeCrystalDialogUI : DialogBaseUI
{
    [SerializeField]
    TextMeshProUGUI Text;

    [SerializeField]
    Button BtnTake;

    int Crystal;

    void Start()
    {
        BtnTake.onClick.AddListener(() =>
        {
            YandexSdk.Instance.RewardAd(() =>
            {
                Wallet.Instance.AddCrystal(Crystal);
                Hide();
                YandexSdk.Instance.Goal("reward_crystal", Crystal.ToString());
            });
        });
    }

    public override void Show()
    {
        Crystal = Random.Range(5, 15);
        Text.text = FormatText(Crystal);
        base.Show();
    }

    string FormatText(float crystal)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"<color=green>{crystal}</color>");
        return builder.ToString();
    }
}
