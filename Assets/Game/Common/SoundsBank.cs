using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundsBank", menuName = "Scriptable Objects/SoundsBank")]
public class SoundsBank : ScriptableObject
{
    [SerializeField]
    private List<GameSound> sounds = new();

    public GameSound GetSound(string id)
    {
        return sounds.Find(s => s.id == id);
    }
}
