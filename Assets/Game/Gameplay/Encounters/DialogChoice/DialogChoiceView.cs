using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class DialogChoiceView : MonoBehaviour
{
    [SerializeField]
    private DialogChoiceEntryView choiceEntryViewPrefab = null;
    [SerializeField]
    private Transform choicesContainer = null;
    [SerializeField]
    private GameObject characterDialogContainer = null;

    private Action<DialogChoiceEntry> _onChoiceSubmitted;

    private void Awake()
    {
        characterDialogContainer.SetActive(false);
    }

    public IEnumerator DisplayChoices(DialogChoice dialogChoice, Action<DialogChoiceEntry> onChoiceSubmitted)
    {
        _onChoiceSubmitted = onChoiceSubmitted;

        characterDialogContainer.SetActive(false);
        foreach (Transform child in choicesContainer)
            Destroy(child.gameObject);


        characterDialogContainer.SetActive(true);
        yield return new WaitForSeconds(dialogChoice.characterDialogDuration); // Attendre un peu pour que les objets soient détruits avant d'en créer de nouveaux

        DialogChoiceEntryView first = null;
        foreach (var choice in dialogChoice.playerEntries)
        {
            var choiceEntryView = Instantiate(choiceEntryViewPrefab, choicesContainer);
            choiceEntryView.Display(choice);
            choiceEntryView.Submitted += OnEntrySubmitted;
            if (first == null) first = choiceEntryView;
        }

        // Attendre une frame pour que la destruction des anciens enfants
        // et le layout des nouveaux soient appliqués avant de sélectionner.
        yield return null;

        if (first != null)
            EventSystem.current.SetSelectedGameObject(first.gameObject);
    }

    private void OnEntrySubmitted(DialogChoiceEntryView view)
    {
        _onChoiceSubmitted?.Invoke(view.Entry);
    }
}
