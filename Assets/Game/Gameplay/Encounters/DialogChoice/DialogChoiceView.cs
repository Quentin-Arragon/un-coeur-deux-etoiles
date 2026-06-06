using UnityEngine;
using System;

public class DialogChoiceView : MonoBehaviour
{
    [SerializeField]
    private DialogChoiceEntryView choiceEntryViewPrefab = null;
    [SerializeField]
    private Transform choicesContainer = null;


    public void DisplayChoices(DialogChoiceEntry[] choices)
    {
        foreach (var choice in choices)
        {
            var choiceEntryView = Instantiate(choiceEntryViewPrefab, choicesContainer);
            choiceEntryView.Display(choice);
        }
    }
}
