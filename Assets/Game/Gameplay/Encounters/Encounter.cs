using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Encounter : MonoBehaviour
{
    [SerializeField]
    private float introDuration = 2;
    [SerializeField]
    private AudioClip dialog_1 = null;
    [SerializeField]
    private float dialog_1_duration = 3f;

    [SerializeField]
    private GameObject _character = null;




    public IEnumerator StartEncounter()
    {
        GoFullscreen();
        _character.GetComponent<RectTransform>().DOAnchorPos(new Vector2(290f, 0f), introDuration).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(introDuration);
        // play sound
        yield return new WaitForSeconds(dialog_1_duration);
        // display choices
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
