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
        Debug.Log($"Encounter started");

        GoFullscreen();
        _character.GetComponent<RectTransform>().DOAnchorPos(new Vector2(124f, 0f), introDuration).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(introDuration);


        foreach (var choice in _choices)
        {
            bool? isCorrect = null;
            yield return _dialogChoiceView.DisplayChoices(choice, correct => isCorrect = correct);

            yield return new WaitUntil(() => isCorrect != null);

            Debug.Log($"Réponse {(isCorrect.Value ? "correcte" : "incorrecte")}");
            // ... fais quelque chose avec isCorrect.Value ici
        }

        Debug.Log($"Encounter ended");
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
