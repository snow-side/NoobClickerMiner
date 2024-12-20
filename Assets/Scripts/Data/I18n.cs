using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class I18n
{
    static readonly Dictionary<string, string> Items = new();

    static I18n()
    {
        var data = Resources.Load<TextAsset>("localization").text;
        var items = data
        .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        .Skip(1)
        .ToArray();

        foreach (var item in items)
        {
            var splts = item.Split(";", StringSplitOptions.RemoveEmptyEntries);
            Items.Add(splts[0], splts[1].Replace("|", Environment.NewLine));
        }
    }

    public static string Get(string key) => Items[key];
}
