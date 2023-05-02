using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

/// <summary>
/// UI Element in World Space that pauses the game, and displays a pause menu with various options.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private string resumeID = "resume";
    [SerializeField]
    private string optionsID = "options";
    [SerializeField]
    private string restartID = "restart";
    [SerializeField]
    private string mainMenuID = "menu";

    // UI Elements
    private VisualElement root = null;
    private VisualElement mainContainer = null;
    private Button resumeBtn = null;
    private Button optionsBtn = null;
    private Button restartBtn = null;
    private Button mainMenuBtn = null;

    // Position
    private Vector2 originalPosition;
    private Vector2 offsetPosition;
    private int offsetY = -12;

    /// Singleton thing
    private static PauseMenu _instance = null;
    public static PauseMenu instance
    {
        get {return _instance;}
    }

    private void Awake() {
        // Singleton - there can only be one pause menu!
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        // Get UI Elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        mainContainer = root.Q<VisualElement>("MainContainer");
        resumeBtn = root.Q<Button>(resumeID);
        optionsBtn = root.Q<Button>(optionsID);
        restartBtn = root.Q<Button>(restartID);
        mainMenuBtn = root.Q<Button>(mainMenuID);

        // Set events
        resumeBtn.clicked += OnResumeGame;
        restartBtn.clicked += RestartLevel;

    }

    private void Start() {
        // Set position
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 screenCenter = Camera.main.ScreenToWorldPoint(screenSize/2);
        transform.position = screenCenter;
        originalPosition = transform.position;

        // Offset position
        offsetPosition = new Vector2( originalPosition.x, originalPosition.y + offsetY);
        transform.position = offsetPosition;

        // Pause
        PauseGame();

        // Lil animation
        StartCoroutine(FlyAnimation(offsetPosition, originalPosition, 0.25f, SetFocus));
    }

    /// <summary>
    /// Sets keyboard focus on the Resume buttons
    /// </summary>
    /// https://forum.unity.com/threads/focus-doesnt-seem-to-work.901130/
    private void SetFocus() {
        resumeBtn.focusable = true;
        resumeBtn.Focus();
        // element.RegisterCallback<AttachToPanelEvent>(evt => element.Focus());
    }

    private void AnimationTest() {
        // UI animations tween
        // https://forum.unity.com/threads/ui-toolkit-runtime-ui-animation.1327413/
    }


    /// <summary>
    /// Code that completely pauses the game
    /// </summary>
    private void PauseGame() {
        // More info on pausing the game correctly:
        // https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Public method called from other scripts. Begins the animation to resume the game.
    /// </summary>
    public void OnResumeGame() {
        StartCoroutine(FlyAnimation(originalPosition, offsetPosition, 0.1f, ResumeGame));
    }

    /// <summary>
    /// Executes the code that actually resumes the game.
    /// </summary>
    private void ResumeGame() {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    /// <summary>
    /// Restart the current level
    /// </summary>
    private void RestartLevel() {
        Time.timeScale = 1f;
        GameManager.instance.RestartLevel();
    }

    /// <summary>
    /// Quits the game. Temporary.
    /// </summary>
    private void Quit() {
        Application.Quit();
    }

    /// <summary>
    /// Simple fly animation to glide the Pause UI a start position to a target position.
    /// </summary>
    /// <param name="start">Start of the animation</param>
    /// <param name="target">End of the animation</param>
    /// <param name="duration">How long to take to travel between the start and target position (in seconds)</param>
    /// <param name="callback">Function to execute after animation is completed</param>
    private IEnumerator FlyAnimation(Vector2 start, Vector2 target, float duration = 0.1f, Action callback = null) {
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


    /// <summary>
    /// Reset the singleton
    /// </summary>
    private void OnDestroy() {
        if (this == _instance) { _instance = null; }
    }
}
