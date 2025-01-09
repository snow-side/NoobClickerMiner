using UnityEngine;
using UnityEngine.UI;
using YGTemplate.Ads;

public class RewardedAdsComponent : MonoBehaviour
{
    [Tooltip("if it's null will try find in current gameObject")]
    [SerializeField] private Button showAdsButton;
    [SerializeField] private string rewardData;

    public void Awake()
    {
        if (showAdsButton == null) showAdsButton = GetComponent<Button>();
        showAdsButton?.onClick.AddListener(() =>
        {
            OnClick();
        });
    }

    public virtual void OnClick() {
        AdsManager.Instance?.ShowRewardAds(rewardData);
    }
}
