using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Selectable))]
public class DialogChoiceEntryView : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{

    [SerializeField]
    private Image background = null;

    private DialogChoiceEntry _entry;

    public DialogChoiceEntry Entry => _entry;
    public event Action<DialogChoiceEntryView> Submitted;

    public void Display(DialogChoiceEntry entry)
    {
        _entry = entry;
        background.color = entry.color;
        transform.localScale = Vector3.one;
    }

    public void OnSelect(BaseEventData _) => transform.localScale = Vector3.one * 1.1f;
    public void OnDeselect(BaseEventData _) => transform.localScale = Vector3.one;
    public void OnSubmit(BaseEventData _) => Submitted?.Invoke(this);
}
