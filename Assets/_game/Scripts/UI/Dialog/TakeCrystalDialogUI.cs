using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGTemplate.Ads;

public class TakeCrystalDialogUI : DialogBaseUI
{
    [SerializeField]
    TextMeshProUGUI Text;

    [SerializeField]
    Button BtnTake;

    [SerializeField] private int crystalMult = 3;

    int Crystal;

    void Start()
    {
        BtnTake.onClick.AddListener(() =>
        {
            AdsManager.Instance.ShowRewardAds(() =>
            {
                Wallet.Instance.AddCrystal(Crystal);
                Hide();
                Debug.LogWarning("[" + this.GetType().Name + "] " + "Init Reward Ads with func Wallet.Instance.AddCrystal(Crystal);");
            });
        });
    }

    public override void Show()
    {
        Crystal = crystalMult * Mathf.Max(Mathf.FloorToInt(MineManager.Instance.GetMineBoostPrice(MineManager.Instance.GetLastOpenedMine())),1);
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
