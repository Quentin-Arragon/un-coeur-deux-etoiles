using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Selectable))]
public class DialogChoiceEntryView : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{

    [SerializeField]
    private Image background = null;

    [SerializeField, Range(0f, 1f)]
    private float unfocusedAlpha = 0.5f;

    [SerializeField]
    private GameObject selectedFeedback = null;

    private DialogChoiceEntry _entry;

    public DialogChoiceEntry Entry => _entry;
    public event Action<DialogChoiceEntryView> Submitted;

    public void Display(DialogChoiceEntry entry)
    {
        _entry = entry;
        SetAlpha(unfocusedAlpha);
        SetFeedbackActive(false);
    }

    public void OnSelect(BaseEventData _)
    {
        SetAlpha(1f);
        SetFeedbackActive(true);
    }

    public void OnDeselect(BaseEventData _)
    {
        SetAlpha(unfocusedAlpha);
        SetFeedbackActive(false);
    }

    public void OnSubmit(BaseEventData _) => Submitted?.Invoke(this);

    private void SetFeedbackActive(bool active)
    {
        if (selectedFeedback != null)
            selectedFeedback.SetActive(active);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _entry.color;
        color.a = alpha;
        background.color = color;
    }
}
