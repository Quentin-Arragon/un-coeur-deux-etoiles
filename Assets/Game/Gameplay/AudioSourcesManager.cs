using System.Collections;
using UnityEngine;

public class AudioSourcesManager : MonoBehaviour
{
    public static AudioSourcesManager Instance { get; private set; }


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    private SoundsBank soundsBank = null;

    [SerializeField]
    private AudioSource dialogAudioSource = null;

    [SerializeField]
    private AudioSource playerVoiceAudioSource = null;

    [SerializeField]
    private AudioSource effectAudioSource = null;

    public void PlayDialog(string id)
    {
        PlayOn(dialogAudioSource, id);
    }

    public void PlayCharacterVoice(string id)
    {
        PlayOn(dialogAudioSource, id);
    }

    public void PlayPlayerVoice(string id)
    {
        PlayPlayerVoice(id, 2);
    }

    public void PlayPlayerVoice(string id, int times)
    {
        GameSound sound = soundsBank.GetSound(id);
        if (sound == null || sound.clip == null)
        {
            Debug.LogWarning($"Sound with id '{id}' not found in the sounds bank.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(PlayRepeated(playerVoiceAudioSource, sound.clip, times));
    }

    private IEnumerator PlayRepeated(AudioSource source, AudioClip clip, int times)
    {
        source.Stop();
        for (int i = 0; i < times; i++)
        {
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }

    public void PlayEffect(string id)
    {
        PlayOn(effectAudioSource, id);
    }

    private void PlayOn(AudioSource source, string id)
    {
        GameSound sound = soundsBank.GetSound(id);
        if (sound == null || sound.clip == null)
        {
            Debug.LogWarning($"Sound with id '{id}' not found in the sounds bank.");
            return;
        }
        source.PlayOneShot(sound.clip);
    }
}
