using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Encounter : MonoBehaviour
{
    [Title("Configs")]
    [SerializeField]
    private float introDuration = 2;
    [SerializeField]
    private AudioClip dialog_1 = null;
    [SerializeField]
    private float dialog_1_duration = 3f;
    [SerializeField]
    private DialogChoice[] _choices = null;

    [Title("References")]
    [SerializeField]
    private DialogChoiceView _dialogChoiceView = null;
    [SerializeField]
    private GameObject _character = null;




    public IEnumerator StartEncounter()
    {
        GoFullscreen();
        _character.GetComponent<RectTransform>().DOAnchorPos(new Vector2(200f, 0f), introDuration).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(introDuration);
        // AudioSourcesManager.Instance.PlayDialog(dialog_1);

        foreach (var choice in _choices)
        {
            DialogChoiceEntry chosen = null;
            yield return _dialogChoiceView.DisplayChoices(choice, entry => chosen = entry);

            yield return new WaitUntil(() => chosen != null);

            Debug.Log($"Choix sélectionné : {chosen.id}");
            // ... fais quelque chose avec chosen.id ici
        }
    }


    private void GoFullscreen()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
