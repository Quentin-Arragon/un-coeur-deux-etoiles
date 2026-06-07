using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScoresManager : MonoBehaviour
{
    [SerializeField]
    private float introDuration = 3f;
    [SerializeField]
    private GameObject anyKeyPrompt;


    IEnumerator Start()
    {
        anyKeyPrompt.SetActive(false);
        yield return new WaitForSeconds(introDuration);
        anyKeyPrompt.SetActive(true);
    }

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Intro");
        }
    }
}
