using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityHFSM;

/// <summary>
/// State machine du flux de jeu :
///   Intro (durée fixe) -> WaitForInput (bouton "Press any key") -> Gameplay.
/// À placer sur un GameObject de la scène GameFlow (scène de démarrage).
/// Le manager survit aux changements de scène (DontDestroyOnLoad).
/// </summary>
public class GameFlow : MonoBehaviour
{
    [Header("Noms des scènes (ajoutées dans Build Settings)")]
    [SerializeField] private string introSceneName = "Intro";
    [SerializeField] private string gameplaySceneName = "Gameplay";

    [Header("Intro")]
    [Tooltip("Durée de l'intro avant l'apparition du bouton (secondes).")]
    [SerializeField, Min(0f)] private float introDuration = 3f;

    [Header("UI")]
    [Tooltip("Bouton/texte 'Press any key to start'. Caché au lancement, doit être enfant de ce GameObject pour survivre au chargement de scène.")]
    [SerializeField] private GameObject pressAnyKeyPrompt;

    private StateMachine fsm;
    private bool anyKeyPressed;

    private void Awake()
    {
        // Le manager (et son UI enfant) doit survivre au chargement des autres scènes.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (pressAnyKeyPrompt != null)
            pressAnyKeyPrompt.SetActive(false);

        fsm = new StateMachine();

        // 1) Intro : charge la scène Intro, cache le bouton, attend une durée fixe.
        fsm.AddState("Intro", new State(
            onEnter: state =>
            {
                if (pressAnyKeyPrompt != null) pressAnyKeyPrompt.SetActive(false);
                SceneManager.LoadScene(introSceneName, LoadSceneMode.Single);
            }
        ));

        // 2) WaitForInput : affiche le bouton et attend n'importe quelle touche.
        fsm.AddState("WaitForInput", new State(
            onEnter: state =>
            {
                anyKeyPressed = false;
                if (pressAnyKeyPrompt != null) pressAnyKeyPrompt.SetActive(true);

                // Nouveau Input System : capture la 1re pression de n'importe quel
                // bouton (clavier, souris, manette). Se désabonne automatiquement.
                InputSystem.onAnyButtonPress.CallOnce(control => anyKeyPressed = true);
            }
        ));

        // 3) LoadGameplay : charge la scène Gameplay.
        fsm.AddState("LoadGameplay", new State(
            onEnter: state =>
            {
                if (pressAnyKeyPrompt != null) pressAnyKeyPrompt.SetActive(false);
                SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
            }
        ));

        // --- Transitions ---

        // Intro -> WaitForInput après une durée fixe.
        fsm.AddTransition(new TransitionAfter("Intro", "WaitForInput", introDuration));

        // WaitForInput -> LoadGameplay dès qu'une touche est pressée.
        fsm.AddTransition(new Transition("WaitForInput", "LoadGameplay",
            condition: transition => anyKeyPressed));

        fsm.SetStartState("Intro");
        fsm.Init();
    }

    private void Update()
    {
        fsm.OnLogic();
    }
}
