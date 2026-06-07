using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class GameSound
{
    public string id;

    [ValueDropdown(nameof(Categories))]
    public string category;

    public AudioClip clip;

    private static readonly string[] Categories = { "trumpet", "drumb", "violin" };
}
