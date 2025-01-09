using System;
using UnityEngine;
using UnityEngine.Events;

public class Wallet : MonoBehaviour
{
    public static Wallet Instance;

    [HideInInspector]
    public UnityEvent<float> OnAddGold = new();

    [HideInInspector]
    public UnityEvent<float> OnAddCrystal = new();

    [SerializeField]
    public Sprite GoldIcon;

    [SerializeField]
    public Sprite CrystalIcon;

    [SerializeField]
    float Gold;

    [SerializeField]
    float Crystal;

    [SerializeField]
    float GoldInBag;

    [SerializeField]
    [Range(1, 50)]
    float GoldBagRate;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);

        GameEvents.GameReady.AddListener(() =>
        {
            Gold = Game.Instance.SaveData.Wallet.Gold;
            Crystal = Game.Instance.SaveData.Wallet.Crystal;
            GoldInBag = Game.Instance.SaveData.Wallet.GoldInBag;
            AddGold(0);
            AddCrystal(0);
        });
    }

    public float GetGoldBagRate() => GoldBagRate;

    public float GetGoldInBag() => GoldInBag;

    public bool EnoughGold(float val) => val <= Gold;

    public bool EnoughCrystal(float val) => val <= Crystal;

    public void AddGold(float val, bool addToBag = true)
    {
        Gold = Mathf.Clamp(Gold + val, 0, float.MaxValue);
        OnAddGold.Invoke(Gold);
        if (addToBag && val > 0) AddGoldToBag(val);
        Game.Instance.SaveData.Wallet.Gold = Gold;
    }

    public void AddCrystal(float val)
    {
        Crystal = Mathf.Clamp(Crystal + val, 0, float.MaxValue);
        OnAddCrystal.Invoke(Crystal);
        Game.Instance.SaveData.Wallet.Crystal = Crystal;
    }

    void AddGoldToBag(float val)
    {
        GoldInBag = Mathf.Clamp(GoldInBag + val * (GoldBagRate / 100), 0, float.MaxValue);
        Game.Instance.SaveData.Wallet.GoldInBag = GoldInBag;
    }
    
    public void TakeGoldFromBag()
    {
        AddGold(GoldInBag, false);
        GoldInBag = 0;
    }
}
