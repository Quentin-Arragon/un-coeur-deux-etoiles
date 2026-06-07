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

    private bool animationFinished;

    IEnumerator Start()
    {
        anyKeyPrompt.SetActive(false);
        yield return StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // TODO: remplacer ce délai par l'animation quand elle sera implémentée.
        float elapsed = 0f;
        while (elapsed < introDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        FinishIntro();
    }

    void FinishIntro()
    {
        if (animationFinished)
        {
            return;
        }
        // TODO: forcer l'animation à sa dernière frame quand elle sera implémentée.
        animationFinished = true;
        anyKeyPrompt.SetActive(true);
    }

    void Update()
    {
        if (!Keyboard.current.anyKey.wasPressedThisFrame)
        {
            return;
        }

        if (animationFinished)
        {
            SceneManager.LoadScene("Intro");
        }
        else
        {
            // Skip : on stoppe l'intro en cours et on saute directement à la fin.
            StopAllCoroutines();
            FinishIntro();
        }
    }
}
