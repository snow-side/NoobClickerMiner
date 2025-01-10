using UnityEngine;
using YGTemplate.Ads;

public class RewardMapper : MonoBehaviour
{

    public void Start()
    {
        TemplateManager.Instance.OnReward.AddListener(GetReward);
    }

    public void GetReward(string rewardData) {
        Debug.Log("[" + this.GetType().Name + "] " + "Get reward data: " + rewardData);

        string[] reward = rewardData.Split('_');

        switch (reward[0])
        {
            case "emeralds":
                if (reward.Length < 2) return;
                int valueCr = 0;
                if (int.TryParse(reward[1], out valueCr))
                {
                    Wallet.Instance.AddCrystal(valueCr);
                }
                else {
                    Debug.LogWarning("[" + this.GetType().Name + "] " + "Received emeraldData with strange value: " + rewardData);
                }
                break;

            case "gold":
                if (reward.Length < 2) return;
                int valueGold = 0;
                if (int.TryParse(reward[1], out valueGold))
                {
                    Wallet.Instance.AddCrystal(valueGold);
                }
                else
                {
                    Debug.LogWarning("[" + this.GetType().Name + "] " + "Received gold rewardData with strange value: " + rewardData);
                }
                break;
        }
    }

    public void OnDestroy()
    {
        TemplateManager.Instance.OnReward.RemoveListener(GetReward);
    }
}
