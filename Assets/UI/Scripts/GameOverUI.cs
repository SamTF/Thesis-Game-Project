using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/// <summary>
/// UI Menu that gets displayed when the Player dies.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [Header("GAME OVER")]
    [SerializeField]
    private string retryID = "retry";
    [SerializeField]
    private string quitID = "quit";
    [SerializeField]
    private string spriteID = "Sprite";
    [SerializeField]
    private string timerID = "TimeSurvived";

    // UI Elements
    private VisualElement mainContainer = null;
    private Button retryBtn = null;
    private Button quitBtn = null;
    private MyImage playerSprite = null;
    private Label timer = null;

    /// Singleton thing
    private static GameOverUI _instance = null;
    public static GameOverUI instance
    {
        get {return _instance;}
    }


    private void Awake() {
        // Singleton
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        // elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        mainContainer = root.Q<VisualElement>("MainContainer");
        retryBtn = mainContainer.Q<Button>(retryID);
        quitBtn = mainContainer.Q<Button>(quitID);
        playerSprite = mainContainer.Q<MyImage>(spriteID);
        timer = mainContainer.Q<Label>(timerID);

        // button callbacks
        retryBtn.clicked += Retry;
        quitBtn.clicked += QuitToMenu;
    }

    private void Start() {
        // Set position
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 screenCenter = Camera.main.ScreenToWorldPoint(screenSize/2);
        Vector2 screenBottom = Camera.main.ScreenToWorldPoint(Vector2.zero);

        // Lil animation
        StartCoroutine(FlyAnimation(screenBottom, screenCenter, 2f, SetFocus));

        // set player sprite
        playerSprite.sprite = Player.instance.Sprite;

        // set time survived
        timer.text = $"{GameManager.instance.Timer.currentTime.String} mins";

        // show cursor
        CustomCursor.visible = true;
    }

    /// <summary>
    /// Same as going back to Menu and instantly pressing Play
    /// </summary>
    private void Retry() {
        GameManager.instance.RestartLevel();
    }

    /// <summary>
    /// Resets everything
    /// </summary>
    private void QuitToMenu() {
        Time.timeScale = 1f;
        GameManager.instance.ChangeScene(GameManager.Scenes.MainMenu);
    }

    private void SetFocus() {
        retryBtn.focusable = true;
        retryBtn.Focus();
        // element.RegisterCallback<AttachToPanelEvent>(evt => element.Focus());
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


    /// <summary>
    /// Reset the singleton
    /// </summary>
    private void OnDestroy() {
        if (this == _instance) { _instance = null; }
    }
}
