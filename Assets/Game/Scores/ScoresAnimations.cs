using System.Collections.Generic;
using UnityEngine;

public class ScoresAnimations : MonoBehaviour
{
    [SerializeField]
    private EncountersOutput encountersOutput;
    [SerializeField]
    private string character1Id;
    [SerializeField]
    private string character2Id;
    [SerializeField]
    private AnimationPlayer animationPlayer_winner;
    [SerializeField]
    private AnimationAsset character1_winner;
    [SerializeField]
    private AnimationAsset character2_winner;
    [SerializeField]
    private AnimationAsset character1_looser;
    [SerializeField]
    private AnimationAsset character2_looser;
    [SerializeField]
    private string character1_song;
    [SerializeField]
    private string character2_song;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        int character1Positives = CountPositives(character1Id);
        int character2Positives = CountPositives(character2Id);

        if (character1Positives >= character2Positives)
        {
            // Character 1 a reçu le plus de réponses positives
            animationPlayer_winner.Play(character1_winner);
            // animationPlayer_looser.Play(character2_looser);
            AudioSourcesManager.Instance.PlayCharacterVoice(character1_song);
        }
        else
        {
            // Character 2 a reçu le plus de réponses positives
            animationPlayer_winner.Play(character2_winner);
            // animationPlayer_looser.Play(character1_looser);
            AudioSourcesManager.Instance.PlayCharacterVoice(character2_song);
        }
    }

    private int CountPositives(string characterId)
    {
        if (string.IsNullOrEmpty(characterId)
            || !encountersOutput.records.TryGetValue(characterId, out List<bool> answers))
        {
            return 0;
        }

        int count = 0;
        foreach (bool answer in answers)
        {
            if (answer)
            {
                count++;
            }
        }

        return count;
    }

}
