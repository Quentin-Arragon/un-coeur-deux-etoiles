using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogChoiceEntryView : MonoBehaviour
{

    [SerializeField]
    private Image background = null;

    public void Display(DialogChoiceEntry entry)
    {

        background.color = entry.color;
    }
}
