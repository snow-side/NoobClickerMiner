using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineManager : MonoBehaviour
{
    public static MineManager Instance { get; private set; }

    [SerializeField]
    GameObject Prefab;

    [SerializeField]
    Transform Container;

    ResourceData[] _Resources;

    List<MineController> Items;

    bool Inited = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        _Resources = Resources.LoadAll<ResourceData>("Resources").OrderBy(x => x.Level).ToArray();
        GameEvents.GameReady.AddListener(() =>
        {
            Init();
            StartCoroutine(SavePreiodicaly());
        });
    }

    void Start() => Wallet.Instance.OnAddGold.AddListener(_Update);

    void Init()
    {
        Items = new List<MineController>(_Resources.Length);
        var pos = Vector3.zero;
        for (int i = 0; i < _Resources.Length; i++)
        {
            var item = _Resources[i];
            var inst = Instantiate(Prefab, Container);
            var mine = inst.GetComponent<MineController>();
            var entity = EntitySaveData.GetByName(Game.Instance.SaveData.Mines, item.name);
            var lvl = item.Level == 1 && entity.Level == 0 ? 1 : entity.Level;
            mine.Init(new MineStats(item, lvl, entity.Time), pos);
            mine.OnUpgraded.AddListener(x => UpgradeHandler());
            Items.Add(mine);
            pos.y -= 4;
        }
        Inited = true;
        //_Update(0);
    }

    void _Update(float _)
    {
        if (!Inited)
            return;
        for (int i = 0; i < Items.Count; i++)
            Items[i].DirtyUpdate();
        GameEvents.EntityUpdated.Invoke(EntityUpdateType.Mines, Items.Any(x => x.UpgradeAvailable));
    }

    void UpgradeHandler()
    {
        for (int i = 0; i < Items.Count; i++)
            Items[i].DirtyUpdate();
        GameEvents.EntityUpdated.Invoke(EntityUpdateType.Mines, Items.Any(x => x.UpgradeAvailable));
        Serialize();
        Game.Instance.Save();
    }

    private void Serialize()
    {
        Game.Instance.SaveData.Mines = Items
        .Select(x => new EntitySaveData(x.Stats.Data.name, x.Stats.Level) { Time = x.Stats.BoostTime })
        .ToArray();
    }

    IEnumerator SavePreiodicaly()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(3);
            Serialize();
        }
    }
}
