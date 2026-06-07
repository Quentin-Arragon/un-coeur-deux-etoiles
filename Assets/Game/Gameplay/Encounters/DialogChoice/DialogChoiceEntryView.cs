using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

[RequireComponent(typeof(Selectable))]
public class DialogChoiceEntryView : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{

    [SerializeField]
    private Image background = null;

    [SerializeField, Range(0f, 1f)]
    private float unfocusedAlpha = 0.5f;

    [SerializeField]
    private GameObject selectedFeedback = null;
    [SerializeField]
    private GameObject _sounfIcon = null;

    [SerializeField]
    private float soundIconPulseScale = 1.2f;
    [SerializeField]
    private float soundIconPulseDuration = 0.5f;

    private DialogChoiceEntry _entry;
    private Vector3 _soundIconBaseScale = Vector3.one;
    private Tween _soundIconPulse;

    public DialogChoiceEntry Entry => _entry;
    public event Action<DialogChoiceEntryView> Submitted;

    public void Display(DialogChoiceEntry entry)
    {
        _entry = entry;
        if (_sounfIcon != null)
            _soundIconBaseScale = _sounfIcon.transform.localScale;
        SetAlpha(unfocusedAlpha);
        SetFeedbackActive(false);
    }

    public void OnSelect(BaseEventData _)
    {
        SetAlpha(1f);
        SetFeedbackActive(true);
        PulseSoundIcon();
    }

    public void OnDeselect(BaseEventData _)
    {
        SetAlpha(unfocusedAlpha);
        SetFeedbackActive(false);
        StopSoundIconPulse();
    }

    private void PulseSoundIcon()
    {
        if (_sounfIcon == null)
            return;

        _soundIconPulse?.Kill();
        _sounfIcon.transform.localScale = _soundIconBaseScale;
        _soundIconPulse = _sounfIcon.transform
            .DOScale(_soundIconBaseScale * soundIconPulseScale, soundIconPulseDuration * 0.5f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void StopSoundIconPulse()
    {
        if (_sounfIcon == null)
            return;

        _soundIconPulse?.Kill();
        _soundIconPulse = null;
        _sounfIcon.transform.localScale = _soundIconBaseScale;
    }

    private void OnDisable() => StopSoundIconPulse();

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
