using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private Encounter[] encounter = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return encounter[0].StartEncounter();
        yield return new WaitForSeconds(0.5f);
        yield return encounter[1].StartEncounter();
        SceneManager.LoadScene("Scores");
    }

}
