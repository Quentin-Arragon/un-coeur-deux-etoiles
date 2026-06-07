using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField]
    private AnimationAsset animationAsset;

    [SerializeField]
    private Image target;

    [SerializeField]
    private bool loop;

    [SerializeField]
    private bool playOnStart;

    private Coroutine playback;

    void Start()
    {
        if (playOnStart)
        {
            Play();
        }
    }

    // Déclenche l'animation depuis la première frame.
    public void Play()
    {
        Play(animationAsset, null);
    }

    // Déclenche l'animation et appelle onComplete à la fin (ignoré si loop).
    public void Play(Action onComplete)
    {
        Play(animationAsset, onComplete);
    }

    // Déclenche un animationAsset spécifique.
    public void Play(AnimationAsset asset, Action onComplete = null)
    {
        Stop();
        animationAsset = asset;
        playback = StartCoroutine(PlayRoutine(asset, onComplete));
    }

    // Stoppe l'animation en cours.
    public void Stop()
    {
        if (playback != null)
        {
            StopCoroutine(playback);
            playback = null;
        }
    }

    private IEnumerator PlayRoutine(AnimationAsset asset, Action onComplete)
    {
        if (asset == null || asset.Frames == null || asset.Frames.Length == 0 || target == null)
        {
            yield break;
        }

        Sprite[] frames = asset.Frames;
        float frameDuration = asset.FrameDuration;

        do
        {
            for (int i = 0; i < frames.Length; i++)
            {
                target.sprite = frames[i];
                yield return frameDuration > 0f
                    ? new WaitForSeconds(frameDuration)
                    : null;
            }
        } while (loop);

        playback = null;
        onComplete?.Invoke();
    }
}
