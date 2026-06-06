using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogChoiceEntryView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label = null;
    [SerializeField]
    private Image background = null;

    public void Display(DialogChoiceEntry entry)
    {
        label.text = entry.label;
        background.color = entry.color;
    }
}
