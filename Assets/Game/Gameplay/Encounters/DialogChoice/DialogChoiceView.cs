using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class DialogChoiceView : MonoBehaviour
{
    [SerializeField]
    private DialogChoiceEntryView choiceEntryViewPrefab = null;
    [SerializeField]
    private Transform choicesContainer = null;
    [SerializeField]
    private GameObject characterDialogContainer = null;
    [SerializeField]
    private float scalePulseAmplitude = 0.05f;
    [SerializeField]
    private float scalePulseFrequency = 2f;

    private Action<bool> _onChoiceSubmitted;
    private DialogChoiceEntry _validEntry;
    private GameObject _lockedSelection;

    private void Awake()
    {
        characterDialogContainer.SetActive(false);
    }

    public IEnumerator DisplayChoices(DialogChoice dialogChoice, Action<bool> onChoiceSubmitted)
    {
        _onChoiceSubmitted = onChoiceSubmitted;
        _validEntry = dialogChoice.validPlayerEntryIndex >= 0
            && dialogChoice.validPlayerEntryIndex < dialogChoice.playerEntries.Length
            ? dialogChoice.playerEntries[dialogChoice.validPlayerEntryIndex]
            : null;

        characterDialogContainer.SetActive(false);
        foreach (Transform child in choicesContainer)
            Destroy(child.gameObject);


        characterDialogContainer.SetActive(true);

        if (dialogChoice.character != null && !string.IsNullOrEmpty(dialogChoice.character.soundId))
            AudioSourcesManager.Instance.PlayCharacterVoice(dialogChoice.character.soundId);

        // Faire fluctuer le scale du premier enfant du container pendant l'attente.
        Transform dialogTransform = characterDialogContainer.transform.childCount > 0
            ? characterDialogContainer.transform.GetChild(0)
            : null;
        Vector3 baseScale = dialogTransform != null ? dialogTransform.localScale : Vector3.one;
        Tween pulseTween = dialogTransform != null
            ? dialogTransform
                .DOScale(baseScale * (1f + scalePulseAmplitude), 1f / (scalePulseFrequency * 2f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
            : null;



        yield return new WaitForSeconds(dialogChoice.characterDialogDuration); // Attendre un peu pour que les objets soient détruits avant d'en créer de nouveaux

        if (pulseTween != null)
        {
            pulseTween.Kill();
            dialogTransform.localScale = baseScale;
        }

        DialogChoiceEntryView first = null;
        var selectables = new List<Selectable>();
        foreach (var choice in dialogChoice.playerEntries)
        {
            var choiceEntryView = Instantiate(choiceEntryViewPrefab, choicesContainer);
            choiceEntryView.Display(choice);
            choiceEntryView.Submitted += OnEntrySubmitted;
            selectables.Add(choiceEntryView.GetComponent<Selectable>());
            if (first == null) first = choiceEntryView;
        }

        // Navigation explicite et bouclée : depuis le dernier choix on revient
        // au premier (et inversement), dans les deux axes.
        for (int i = 0; i < selectables.Count; i++)
        {
            Selectable previous = selectables[(i - 1 + selectables.Count) % selectables.Count];
            Selectable next = selectables[(i + 1) % selectables.Count];
            var nav = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = previous,
                selectOnLeft = previous,
                selectOnDown = next,
                selectOnRight = next,
            };
            selectables[i].navigation = nav;
        }

        // Attendre une frame pour que la destruction des anciens enfants
        // et le layout des nouveaux soient appliqués avant de sélectionner.
        yield return null;

        if (first != null)
        {
            _lockedSelection = first.gameObject;
            EventSystem.current.SetSelectedGameObject(first.gameObject);
        }
    }

    private void Update()
    {
        var eventSystem = EventSystem.current;
        if (eventSystem == null || _lockedSelection == null)
            return;

        // Empêcher de sortir de la navigation : si la sélection courante est
        // perdue (clic souris dans le vide) ou pointe ailleurs, on la restaure
        // sur le dernier choix sélectionné.
        GameObject current = eventSystem.currentSelectedGameObject;
        if (current != null && current.transform.IsChildOf(choicesContainer))
        {
            _lockedSelection = current;
        }
        else
        {
            eventSystem.SetSelectedGameObject(_lockedSelection);
        }
    }

    private void OnEntrySubmitted(DialogChoiceEntryView view)
    {
        bool isCorrect = view.Entry == _validEntry;
        _onChoiceSubmitted?.Invoke(isCorrect);
    }
}
