using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class Intro : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer introVideo;
    [SerializeField]
    private GameObject anyKeyPrompt;
    [SerializeField]
    [Tooltip("Temps en secondes avant l'affichage du bouton \"any key\".")]
    private float anyKeyPromptDelay = 18f;

    private bool animationFinished;

    IEnumerator Start()
    {
        anyKeyPrompt.SetActive(false);

        introVideo.isLooping = false;
        introVideo.loopPointReached += OnVideoFinished;
        introVideo.Play();

        yield return new WaitForSeconds(anyKeyPromptDelay);
        anyKeyPrompt.SetActive(true);
    }

    void OnVideoFinished(VideoPlayer source)
    {
        FinishIntro();
    }

    void FinishIntro()
    {
        if (animationFinished)
        {
            return;
        }
        animationFinished = true;
        anyKeyPrompt.SetActive(true);
    }

    void SkipToEnd()
    {
        // Saute directement à la dernière frame de la vidéo.
        if (introVideo.frameCount > 0)
        {
            introVideo.frame = (long)introVideo.frameCount - 1;
        }
        introVideo.Pause();
        FinishIntro();
    }

    void Update()
    {
        if (!Keyboard.current.anyKey.wasPressedThisFrame)
        {
            return;
        }

        if (animationFinished)
        {
            SceneManager.LoadScene("Gameplay");
        }
        else
        {
            SkipToEnd();
        }
    }

    void OnDestroy()
    {
        if (introVideo != null)
        {
            introVideo.loopPointReached -= OnVideoFinished;
        }
    }
}
