using UnityEngine;
using System;

public class DialogChoice : MonoBehaviour
{
    [SerializeField]
    private DialogChoiceEntryView choiceEntryViewPrefab = null;


    public void DisplayChoices(DialogChoiceEntry[] choices)
    {
        foreach (var choice in choices)
        {
            var choiceEntryView = Instantiate(choiceEntryViewPrefab, transform);
            choiceEntryView.Display(choice);
        }
    }
}
