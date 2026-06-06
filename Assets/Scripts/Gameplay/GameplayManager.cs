using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private Encounter[] encounter = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        encounter[0].StartEncounter();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
