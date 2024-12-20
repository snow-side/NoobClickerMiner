using System;
using System.Linq;
using UnityEngine;

public static class MobNames
{
    public static string[] Items { get; private set; }

    static MobNames()
    {
        var data = Resources.Load<TextAsset>("mobNames").text;
        Items = data
        .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Trim())
        .ToArray();
    }
}
