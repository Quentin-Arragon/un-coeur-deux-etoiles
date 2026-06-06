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
    private AudioSource dialogAudioSource = null;

    public void PlayDialog(AudioClip clip)
    {
        dialogAudioSource.PlayOneShot(clip);
    }
}
