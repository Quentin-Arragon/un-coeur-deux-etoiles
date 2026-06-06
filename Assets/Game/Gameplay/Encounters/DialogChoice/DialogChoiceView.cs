using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DialogChoiceView : MonoBehaviour
{
    [SerializeField]
    private DialogChoiceEntryView choiceEntryViewPrefab = null;
    [SerializeField]
    private Transform choicesContainer = null;

    private Action<DialogChoiceEntry> _onChoiceSubmitted;

    public void DisplayChoices(DialogChoice dialogChoice, Action<DialogChoiceEntry> onChoiceSubmitted)
    {
        _onChoiceSubmitted = onChoiceSubmitted;

        foreach (Transform child in choicesContainer)
            Destroy(child.gameObject);

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
        _onChoiceSubmitted?.Invoke(view.Entry);
    }
}
