using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameSaveData
{
    public bool SfxOn { get; set; } = true;
    public WalletSaveData Wallet { get; set; } = new WalletSaveData() { Crystal = 3 };
    public EntitySaveData[] Mines { get; set; } = Array.Empty<EntitySaveData>();
}

public class WalletSaveData
{
    public float Gold { get; set; }
    public float Crystal { get; set; }
    public float GoldInBag { get; set; }
}

public class EntitySaveData
{
    public EntitySaveData(string name, int level)
    {
        Name = name;
        Level = level;
    }

    public string Name { get; set; }
    public int Level { get; set; }
    public float Time { get; set; }

    public static EntitySaveData GetByName(EntitySaveData[] items, string name)
     => Array.Find(items, x => x.Name == name) ?? new EntitySaveData(name, 0);
}

public static class GameSave
{
    const string SaveName = "pyx-games-noob-miner-clicker";

    public static void Save(GameSaveData data)
    {
        if (data == null)
            return;
        try
        {
            var json = JsonConvert.SerializeObject(data);
#if UNITY_EDITOR
            var path = Application.persistentDataPath + "/saves.json";
            File.WriteAllText(path, json);
#else
        YandexSdk.Instance.Save(SaveName, json);
#endif
        }
        catch (Exception)
        {
            Debug.LogError("Save error");
        }

    }

    public static void Load()
    {
        string data = null;
#if UNITY_EDITOR
        var path = Application.persistentDataPath + "/saves.json";
        if (!File.Exists(path))
            File.WriteAllText(path, string.Empty);
        data = File.ReadAllText(path);
        LoadCall(data);
#else
        YandexSdk.Instance.LoadSave(SaveName);
#endif

    }

    public static GameSaveData LoadCall(string json)
    {
        GameSaveData inst;
        try
        {
            inst = string.IsNullOrEmpty(json) ? new GameSaveData() : JsonConvert.DeserializeObject<GameSaveData>(json);
            inst ??= new GameSaveData();
        }
        catch (Exception)
        {
            Debug.LogError("Error on load save data");
            inst = new GameSaveData();
        }
        GameEvents.GameSavesLoaded.Invoke(inst);
        return inst;
    }
}
