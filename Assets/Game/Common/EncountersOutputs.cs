using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "EncountersOutputs", menuName = "Scriptable Objects/EncountersOutputs")]
public class EncountersOutput : ScriptableObject
{
    [ShowInInspector]
    public Dictionary<string, List<bool>> records = new Dictionary<string, List<bool>>();

    public void AddRecord(string characterId, bool choiceSuccess)
    {
        if (!records.TryGetValue(characterId, out List<bool> list))
        {
            list = new List<bool>();
            records[characterId] = list;
        }

        list.Add(choiceSuccess);
    }

    public void Clear()
    {
        records.Clear();
    }
}
