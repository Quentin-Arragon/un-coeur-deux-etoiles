using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;


public class Intro : MonoBehaviour
{
    [SerializeField]
    private float introDuration = 3f;
    [SerializeField]
    private GameObject anyKeyPrompt;

    private bool hasCompletedIntro = false;


    IEnumerator Start()
    {
        anyKeyPrompt.SetActive(false);
        yield return new WaitForSeconds(introDuration);
        anyKeyPrompt.SetActive(true);
        hasCompletedIntro = true;
    }

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}