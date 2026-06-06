using UnityEngine;
using UnityEngine.UI;

public class Encounter : MonoBehaviour
{
    [SerializeField]
    private float introDuration = 2;
    [SerializeField]
    private AudioClip dialog_1 = null;




    public void StartEncounter()
    {
        GoFullscreen();
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
