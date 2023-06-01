using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI MANAGER")]
    [Header("UI Element Menus")]
    [SerializeField]
    private GameObject HUD = null;
    [SerializeField]
    private GameObject mainMenu = null;
    [SerializeField]
    private GameObject pauseMenu = null;
    [SerializeField]
    private GameObject colouringBook = null;
    [SerializeField]
    private GameObject levelUpMenu = null;
    [SerializeField]
    private GameObject gameOverMenu = null;
    [SerializeField]
    private GameObject helpMenu = null;
    [SerializeField]
    private GameObject optionsMenu = null;

    // Singleton Thing
    private static UIManager _instance = null;
    public static UIManager instance
    {
        get {return _instance;}
    }

    private void Awake()
    {
        // Singleton Thing
        if (instance == null)   { _instance = this; }
        else                    { Destroy(gameObject); }
    }

    private void Start() {
        // Only Instantiate the Colouring Book in the Game scene
        if (GameManager.instance.CurrentScene != GameManager.Scenes.Game)
            return;

        bool startWithDefaultCharacter = false;

        // check if the player wants to start with the default character
        if (PlayerPrefs.HasKey("startWithCowboy")) {
            if (PlayerPrefs.GetInt("startWithCowboy") == 1) {
                startWithDefaultCharacter = true;   
            }
        }

        // start the Game with the colouring book if default character is not chosen
        if (!startWithDefaultCharacter) {
            Instantiate(colouringBook);
        }
            
    }

    // Events
    private void OnEnable() {
        GameManager.onPause += PauseMenuToggle;
        LevelSystem.onLevelUp += LevelUp;
        LevelUpUI.onNewColourChosen += ColouringBookToggle;
        Health.onPlayerDeath += GameOver;
        EnemySpawner.onAllEnemiesDefeated += YouWin;
    }
    private void OnDisable() {
        GameManager.onPause -= PauseMenuToggle;
        LevelSystem.onLevelUp -= LevelUp;
        LevelUpUI.onNewColourChosen -= ColouringBookToggle;
        Health.onPlayerDeath -= GameOver;
        EnemySpawner.onAllEnemiesDefeated -= YouWin;
    }

    /// <summary>
    /// Checks whether any UI Menu is currently open
    /// </summary>
    /// <returns></returns>
    private bool IsAnyMenuOpen() {
        return PauseMenu.instance || ColouringBook.instance || LevelUpUI.instance || HelpUI.instance
                || GameOverUI.instance || MainMenu.instance || OptionsMenu.instance;
    }


    /// <summary>
    /// Show the Pause Menu, or resume the Game if the pause menu is already visible.
    /// </summary>
    private void PauseMenuToggle() {
        // If a pause menu instance already exists, resume the game
        if (PauseMenu.instance) {
            PauseMenu.instance.OnResumeGame();
            return;
        }

        // If another menu that pauses the game is running, do nothing
        if (IsAnyMenuOpen())
            return;
            
        // Instantiate a Pause Menu
        GameObject pauseMenuObject = Instantiate(pauseMenu);
    }

    private void ShowLevelUpMenu() {
        // do nothing if there already is a Level Up Menu open
        if (LevelUpUI.instance) {
            return;
        }

        Instantiate(levelUpMenu);
        CustomCursor.visible = true;    // show cursor
    }

    /// <summary>
    /// Instantiate the Colouring Book (if it's now already present)
    /// </summary>
    private void ColouringBookToggle(Color newColour) {
        Debug.Log("COLOURING BOOK TOGGLE");
        // do nothing if there already is a pixel art editor running
        // if (ColouringBook.instance || PauseMenu.instance || LevelUpUI.instance) {
        //     return;
        // }

        // Instantiate(colouringBook);
        StartCoroutine(DelayedDisplay(colouringBook, 0.01f));
    }


    private void LevelUp() {
        GameObject levelUpVFX = Resources.Load("FX/Animated/LevelUpVFX") as GameObject;
        StartCoroutine(Celebration(levelUpVFX, 2f, ShowLevelUpMenu));
    }

    private void YouWin() {
        GameObject youWinVFX = Resources.Load("FX/Animated/YouWin!") as GameObject;
        StartCoroutine(Celebration(youWinVFX, 10f));
    }

    private IEnumerator Celebration(GameObject textFX, float duration = 2f, System.Action callback = null) {
        // make player invulernable
        Player.instance.Status.IsInvulnerable = true;

        // Instantiate Text VFX
        GameObject textObject = Instantiate(textFX, Player.instance.transform, false);
        textObject.transform.localPosition = new Vector2(0, 2);

        // Spawn Fireworks every x milliseconds over the duration period
        GameObject fireworksVFX = Resources.Load("FX/Animated/Fireworks") as GameObject;
        Timer timer = new Timer();
        timer.Start();

        while (timer.currentSeconds <= duration)
        {
            Vector3 rand = new Vector3(
                Random.Range(-6f, 6f),
                Random.Range(-4f, 4f),
                0
            );
            Vector3 position = Player.instance.transform.position + rand;

            Instantiate(fireworksVFX, position, Quaternion.identity);

            yield return new WaitForSeconds( Random.Range(0.01f, 0.1f) );
        }

        // despawn text
        Destroy(textObject);

        // Callback to execute after animation is over
        callback?.Invoke();
    }

    /// <summary>
    /// Display the Help Menu on screen.
    /// </summary>
    public void HelpMenuToggle () {
        if (HelpUI.instance == null) {
            Instantiate(helpMenu);
        }
    }

    /// <summary>
    /// Display the Game Over screen after a few seconds delay.
    /// </summary>
    private void GameOver() {
        if (GameOverUI.instance == null) {
            StartCoroutine(DelayedDisplay(gameOverMenu, 2f));
        }
    }

    /// <summary>
    /// Display the Options Menu on screen.
    /// </summary>
    public void OptionsMenuToggle() {
        if (OptionsMenu.instance == null) {
            Instantiate(optionsMenu);
        }
    }

    /// <summary>
    /// Display a UI Menu after X seconds.
    /// </summary>
    /// <param name="menu">Prefab of menu to display.</param>
    /// <param name="delay">Seconds to wait before displaying.</param>
    private IEnumerator DelayedDisplay(GameObject menu, float delay)  {
        yield return new WaitForSeconds(delay);

        // do nothing if there already is a UI menu running
        if (ColouringBook.instance || PauseMenu.instance || LevelUpUI.instance) {
            yield return null;
        }

        Instantiate(menu);
    }
}
