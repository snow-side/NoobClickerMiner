
using System;
using UnityEngine;

public class MineStats
{
    public const int MAX = 30;

    public const int MAX_PROGRESS = 6;

    public float BoostTime { get; private set; }

    public int ProgressVal { get; private set; }

    public int Level;

    public ResourceData Data { get; private set; }

    public MineStats(ResourceData data, int lvl, float time)
    {
        Data = data;
        Level = lvl;
        BoostTime = time;
    }

    public float UnlockCost => Data.Level * 120 * Mathf.Pow(1.85f, Data.Level);

    public float UpgradeCost => Data.Level * 50 * Mathf.Pow(1.2f, Level);

    public float BoostCost => Data.Level;

    public bool IsOpened => Level > 0;

    public bool IsMax => Level == MAX;

    public void Unlock() => Level = 1;

    public void Upgrade() => Level = Mathf.Clamp(Level + 1, Level, MAX);

    public float MinePerSec => Data.Level * Mathf.Pow(1.09f, Level) * MAX_PROGRESS;

    public float MineClick => Data.Level * 1.75f * Mathf.Pow(1.12f, Level);

    public float MineDelay => BoostTime == 0 ? 1f : 0.33F;

    public float MineSpeed => BoostTime == 0 ? 1f : 3F;

    public void Boost() => BoostTime = 60;

    public void Progress() => ProgressVal = Mathf.Clamp(ProgressVal + 1, 0, MAX_PROGRESS);

    public void ProgressReset() => ProgressVal = 0;

    public void Tick()
    {
        if (!IsOpened)
            return;

        if (BoostTime > 0)
            BoostTime = Mathf.Clamp(BoostTime - 1, 0, BoostTime);
    }
}
