using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/// <summary>
/// The Main Menu UI of the Game!! Is automatically displayed at the start of the game.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("MAIN MENU")]
    [Header("UI IDs")]
    [SerializeField]
    private string playID = "play";
    [SerializeField]
    private string helpID = "help";
    [SerializeField]
    private string optionsID = "options";
    [SerializeField]
    private string customiseID = "customise";
    [SerializeField]
    private string quitID = "quit";

    [Header("Prefabs")]
    [SerializeField]
    private GameObject usernameMenu = null;

    // UI Elements
    private VisualElement mainContainer = null;
    private Button playBtn = null;
    private Button helpBtn = null;
    private Button optionsBtn = null;
    private Button customiseBtn = null;
    private Button quitBtn = null;

    /// Singleton thing
    private static MainMenu _instance = null;
    public static MainMenu instance
    {
        get {return _instance;}
    }

    private void Awake() {
        // Singleton - there can only be one main menu!
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        // Get UI Elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        mainContainer = root.Q<VisualElement>("MainContainer");
        playBtn = root.Q<Button>(playID);
        helpBtn = root.Q<Button>(helpID);
        optionsBtn = root.Q<Button>(optionsID);
        customiseBtn = root.Q<Button>(customiseID);
        quitBtn = root.Q<Button>(quitID);

        // Button callbacks
        playBtn.clicked += Play;
        helpBtn.clicked += Help;
        quitBtn.clicked += Quit;

    }

    private void Start() {
        Time.timeScale = 1f; // (just in case)

        // focus on play button
        playBtn.focusable = true;
        playBtn.Focus();

        // show cursor
        CustomCursor.visible = true;
    }
    

    private void Play() {
        // play game if username has already been set
        if (PlayerPrefs.HasKey("username")) {
            CustomCursor.visible = false;

            StartCoroutine( FlyAnimation (
                transform.position, 
                (Vector2)transform.position + new Vector2(0, 12), 
                0.25f, 
                CloseMenu
            ));
        }

        // show the username meny if players have not set one yet
        else {
            StartCoroutine( FlyAnimation (
                transform.position, 
                (Vector2)transform.position + new Vector2(0, 12), 
                0.25f, 
                ShowUsernameMenu
            ));
        }
        
    }

    private void Help() {
        UIManager.instance.HelpMenuToggle();
        // GetComponent<UIDocument>().enabled = false;
    }

    private void Options() {
        Debug.Log("Options btn clicked!");
    }

    private void Customise() {
        Debug.Log("Customise btn clicked!");
    }

    private void CloseMenu() {
        SceneManager.LoadScene("Game");
        
        Destroy(gameObject);
    }

    private void Quit() {
        Application.Quit();
    }

    private void ShowUsernameMenu() {
        Instantiate(usernameMenu);
        Destroy(gameObject);
    }

    /// <summary>
    /// Simple fly animation to glide the Pause UI a start position to a target position.
    /// </summary>
    /// <param name="start">Start of the animation</param>
    /// <param name="target">End of the animation</param>
    /// <param name="duration">How long to take to travel between the start and target position (in seconds)</param>
    /// <param name="callback">Function to execute after animation is completed</param>
    private IEnumerator FlyAnimation(Vector2 start, Vector2 target, float duration = 0.1f, System.Action callback = null) {
        float time = 0f;

        // Fly animation
        while (Vector2.Distance(transform.position, target) > 0.01f) {
            transform.position = Vector2.Lerp(start, target, time / duration);
            time += Time.unscaledDeltaTime;

            yield return null;
        }

        // Callback to execute after animation is over
        callback?.Invoke();
    }
}
