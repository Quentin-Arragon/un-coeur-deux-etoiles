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

    [SerializeField, Range(0f, 1f)]
    private float feedbackPulseMinAlpha = 0.4f;
    [SerializeField, Range(0f, 1f)]
    private float feedbackPulseMaxAlpha = 1f;
    [SerializeField]
    private float feedbackPulseDuration = 0.8f;

    [SerializeField]
    private float submitFlickerDuration = 0.15f;

    private DialogChoiceEntry _entry;
    private Vector3 _soundIconBaseScale = Vector3.one;
    private Tween _soundIconPulse;
    private CanvasGroup _feedbackCanvasGroup;
    private Tween _feedbackPulse;
    private Tween _submitFlicker;

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

        if (_entry != null && !string.IsNullOrEmpty(_entry.soundId))
            AudioSourcesManager.Instance.PlayPlayerVoice(_entry.soundId);
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

    private void OnDisable()
    {
        StopSoundIconPulse();
        StopFeedbackPulse();
        _submitFlicker?.Kill();
        _submitFlicker = null;
    }

    public void OnSubmit(BaseEventData _)
    {
        AudioSourcesManager.Instance.StopPlayerVoice();
        FlickerOnSubmit();
        Submitted?.Invoke(this);
    }

    private void FlickerOnSubmit()
    {
        if (background == null)
            return;

        StopFeedbackPulse();

        _submitFlicker?.Kill();
        SetAlpha(1f);
        _submitFlicker = background
            .DOFade(unfocusedAlpha, submitFlickerDuration * 0.5f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .OnComplete(() => SetAlpha(1f));
    }

    private void SetFeedbackActive(bool active)
    {
        if (selectedFeedback == null)
            return;

        selectedFeedback.SetActive(active);

        if (active)
            PulseFeedbackAlpha();
        else
            StopFeedbackPulse();
    }

    private void PulseFeedbackAlpha()
    {
        if (_feedbackCanvasGroup == null)
            _feedbackCanvasGroup = selectedFeedback.GetComponent<CanvasGroup>();
        if (_feedbackCanvasGroup == null)
            _feedbackCanvasGroup = selectedFeedback.AddComponent<CanvasGroup>();

        _feedbackPulse?.Kill();
        _feedbackCanvasGroup.alpha = feedbackPulseMaxAlpha;
        _feedbackPulse = _feedbackCanvasGroup
            .DOFade(feedbackPulseMinAlpha, feedbackPulseDuration * 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void StopFeedbackPulse()
    {
        _feedbackPulse?.Kill();
        _feedbackPulse = null;
        if (_feedbackCanvasGroup != null)
            _feedbackCanvasGroup.alpha = feedbackPulseMaxAlpha;
    }

    private void SetAlpha(float alpha)
    {
        Color color = _entry.color;
        color.a = alpha;
        background.color = color;
    }
}
