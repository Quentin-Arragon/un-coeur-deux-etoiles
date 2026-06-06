using UnityEngine;
using System;

public class DialogChoiceView : MonoBehaviour
{
    [SerializeField]
    private DialogChoiceEntryView choiceEntryViewPrefab = null;
    [SerializeField]
    private Transform choicesContainer = null;


    public void DisplayChoices(DialogChoice dialogChoice)
    {
        foreach (var choice in dialogChoice.playerEntries)
        {
            var choiceEntryView = Instantiate(choiceEntryViewPrefab, choicesContainer);
            choiceEntryView.Display(choice);
        }
    }
}
