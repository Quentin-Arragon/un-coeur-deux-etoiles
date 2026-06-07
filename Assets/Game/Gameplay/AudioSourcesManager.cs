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
        PlayOn(playerVoiceAudioSource, id);
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
