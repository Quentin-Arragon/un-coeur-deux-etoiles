using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
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
