using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using ToolBox.Pools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MineController : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<MineController> OnUpgraded = new();

    public MineStats Stats { get; private set; }

    [SerializeField] TextMeshProUGUI HeaderText;
    [SerializeField] TextMeshProUGUI StatsText;
    [SerializeField] TextMeshProUGUI BoostTimerText;
    [SerializeField] TextMeshProUGUI BoostCostText;
    [SerializeField] TextMeshProUGUI UnlockText;
    [SerializeField] TextMeshProUGUI UpgradeText;
    [SerializeField] GameObject UnlockWindow;
    [SerializeField] GameObject InfoWindow;
    [SerializeField] Image ProgressImage;
    [SerializeField] GameObject Miner;
    [SerializeField] GameObject Blocks;
    [SerializeField] GameObject FloatText;
    [SerializeField] Transform FloatTextSpawm;
    [SerializeField] MineBlock MineBlock;

    // Для генерации стены
    [SerializeField] List<Material> blockMaterials; // 10 материалов
    [SerializeField] GameObject wallBlocks;  // Блоки стены

    // Вероятности появления блоков (сумма 100)
    private readonly float[] materialChances = { 60f, 10f, 5f, 5f, 5f, 4f, 3f, 3f, 3f, 2f };

    Animator MinerAnimator;

    public bool UpgradeAvailable => !Stats.IsMax && Wallet.Instance.EnoughGold(Stats.UpgradeCost);
    bool BoostAvailable => Stats.IsOpened && Stats.BoostTime == 0 && Wallet.Instance.EnoughCrystal(Stats.BoostCost);
    bool UnlockAvailable => !Stats.IsOpened && Wallet.Instance.EnoughGold(Stats.UnlockCost);

    void Awake()
    {
        MinerAnimator = Miner.GetComponent<Animator>();
    }

    void Start()
    {
        MineBlock.OnClick.AddListener((point) => ClickHandler(point));
        StartCoroutine(Mine());
        StartCoroutine(Tick());

        GenerateWall(); // Генерация стены при запуске
    }

    public void Init(MineStats stats, Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        Stats = stats;

        foreach (var block in GetAllChildren(Blocks.transform))
        {
            block.GetComponent<MeshRenderer>().material = Stats.Data.Material;
        }
        
        UnlockText.text = $"<color=white>Открыть шахту</color>{Environment.NewLine}<color=#FFD700>{Stats.UnlockCost.Short()} зол.</color>";
        BoostCostText.text = $"{Stats.BoostCost}";
        _Update();
    }
    
    private List<Transform> GetAllChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        // Перебираем всех прямых дочерних элементов
        foreach (Transform child in parent)
        {
            children.Add(child);

            // Рекурсивно добавляем дочерние элементы текущего дочернего
            children.AddRange(GetAllChildren(child));
        }

        return children;
    }

    public void TryUnlock()
    {
        if (UnlockAvailable)
        {
            Wallet.Instance.AddGold(-Stats.UnlockCost);
            Stats.Unlock();
            _Update();
            OnUpgraded.Invoke(this);
            GameAudio.Instance.PlaySfx("unlock");
        }
    }

    public void TryUpgrade()
    {
        if (UpgradeAvailable)
        {
            Wallet.Instance.AddGold(-Stats.UpgradeCost);
            Stats.Upgrade();
            _Update();
            OnUpgraded.Invoke(this);
            GameAudio.Instance.PlaySfx("anvil_use");
        }
    }

    public void TryBoost()
    {
        if (BoostAvailable)
        {
            Wallet.Instance.AddCrystal(-Stats.BoostCost);
            Stats.Boost();
            _Update();
            OnUpgraded.Invoke(this);
            GameAudio.Instance.PlaySfx("eat");
        }
    }

    void _Update()
    {
        if (Stats.IsOpened)
        {
            UnlockWindow.SetActive(false);
            MinerAnimator.enabled = true;
            Miner.SetActive(true);
            InfoWindow.SetActive(true);

            foreach (var block in GetAllChildren(Blocks.transform))
            {
                block.gameObject.SetActive(true);
            }
            
            ProgressImage.gameObject.SetActive(true);
        }
        else
        {
            MinerAnimator.enabled = false;
            Miner.SetActive(false);
            UnlockWindow.SetActive(true);
            InfoWindow.SetActive(false);

            foreach (var block in GetAllChildren(Blocks.transform))
            {
                block.gameObject.SetActive(false);
            }
            
            ProgressImage.gameObject.SetActive(false);
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        UpgradeText.text = $"<color=white>Улучшить</color> <color=#FFD700>{Stats.UpgradeCost.Short(1)}</color>";
        HeaderText.text = $"<color=white>{Stats.Data.Name}</color> <color=orange>[{Stats.Level}] ур.</color>";
        StatsText.text = $"<color=white>Доход за клик:</color><color=#FFD700> {Stats.MineClick.Short(1)} зол.</color>{Environment.NewLine}<color=white>Доход от нубика:</color> <color=#FFD700>{Stats.MinePerSec.Short(1)} зол.</color>";
    }

    void UpdateBoostUI()
    {
        if (Stats.BoostTime > 0)
            BoostTimerText.text = $"<color=white>Ускорение:</color> <color=green>{Stats.BoostTime} сек.</color>";
        else
            BoostTimerText.text = null;
    }

    IEnumerator Mine()
    {
        while (true)
        {
            MinerAnimator.speed = Stats.MineSpeed;
            if (Stats.IsOpened)
                DoProgress();
            yield return new WaitForSeconds(Stats.MineDelay);
        }
    }

    void DoProgress()
    {
        Stats.Progress();
        ProgressImage.fillAmount = Stats.ProgressVal / (float)MineStats.MAX_PROGRESS;
        if (Stats.ProgressVal == MineStats.MAX_PROGRESS)
        {
            var val = Stats.MinePerSec;
            Wallet.Instance.AddGold(val);
            InstFloatText(val, FloatTextSpawm.position);
            Stats.ProgressReset();
        }
    }

    IEnumerator Tick()
    {
        while (true)
        {
            Stats.Tick();
            UpdateBoostUI();
            yield return new WaitForSecondsRealtime(1);
        }
    }

    void ClickHandler(Vector3 point)
    {
        if (!Stats.IsOpened)
            return;

        var val = Stats.MineClick;
        Wallet.Instance.AddGold(val);
        InstFloatText(val, point);
        GameAudio.Instance.PlaySfx("hit", true);
    }

    void InstFloatText(float val, Vector3 pos) => FloatText
    .Reuse<FloatingTextFx>()
    .Spawn(pos, val.Short(1), Color.yellow, 0.25f, sprite: Wallet.Instance.GoldIcon, 3f);

    public void DirtyUpdate() => _Update();

    // Генерация стены с учетом вероятностей
    private void GenerateWall()
    {
        if (wallBlocks == null || blockMaterials == null)
        {
            Debug.LogWarning("Wall blocks or materials are not set up!");
            return;
        }

        foreach (var block in GetAllChildren(wallBlocks.transform))
        {
            int materialIndex = GetRandomMaterialIndex();
            if (block.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                renderer.material = blockMaterials[materialIndex];
            }
            else
            {
                Debug.LogWarning($"Block {block.name} does not have a MeshRenderer!");
            }
        }
    }

    // Получение случайного индекса на основе вероятностей
    private int GetRandomMaterialIndex()
    {
        float total = 0;
        foreach (float chance in materialChances)
        {
            total += chance;
        }

        float randomPoint = UnityEngine.Random.Range(0, total);

        for (int i = 0; i < materialChances.Length; i++)
        {
            if (randomPoint < materialChances[i])
                return i;
            randomPoint -= materialChances[i];
        }

        return materialChances.Length - 1; // На случай, если что-то пойдет не так
    }
}
