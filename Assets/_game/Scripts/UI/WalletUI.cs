using TMPro;
using UnityEngine;

public class WalletUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Money;

    [SerializeField]
    TextMeshProUGUI Crystal;

    void Start()
    {
        var wallet = GetComponent<Wallet>();
        wallet.OnAddCrystal.AddListener((val) => Crystal.text = val.Short(1));
        wallet.OnAddGold.AddListener((val) => Money.text = val.Short(1));
    }
}
