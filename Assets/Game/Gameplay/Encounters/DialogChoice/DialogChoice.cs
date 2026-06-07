using System;
using UnityEngine;

[Serializable]
public class DialogChoice
{
    public DialogChoiceEntry[] playerEntries;
    public int validPlayerEntryIndex;
    public DialogChoiceEntry character;
    public float characterDialogDuration = 2;
}
