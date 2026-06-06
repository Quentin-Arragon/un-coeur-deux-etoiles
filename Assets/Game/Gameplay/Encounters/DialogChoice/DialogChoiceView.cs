using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DialogChoiceView : MonoBehaviour
{
    [SerializeField]
    private DialogChoiceEntryView choiceEntryViewPrefab = null;
    [SerializeField]
    private Transform choicesContainer = null;

    public event Action<DialogChoiceEntry> ChoiceSubmitted;

    public void DisplayChoices(DialogChoice dialogChoice)
    {
        DialogChoiceEntryView first = null;
        foreach (var choice in dialogChoice.playerEntries)
        {
            var choiceEntryView = Instantiate(choiceEntryViewPrefab, choicesContainer);
            choiceEntryView.Display(choice);
            choiceEntryView.Submitted += OnEntrySubmitted;
            if (first == null) first = choiceEntryView;
        }

        if (first != null)
            EventSystem.current.SetSelectedGameObject(first.gameObject);
    }

    private void OnEntrySubmitted(DialogChoiceEntryView view)
    {
        ChoiceSubmitted?.Invoke(view.Entry);
    }
}
