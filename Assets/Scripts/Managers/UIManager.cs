using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI MANAGER")]
    [Header("UI Element Menus")]
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

    // Events
    private void OnEnable() {
        GameManager.onPause += PauseMenuToggle;
        LevelSystem.onLevelUp += LevelUp;
        LevelUpUI.onNewColourChosen += ColouringBookToggle;
        Health.onPlayerDeath += GameOver;
    }
    private void OnDisable() {
        GameManager.onPause -= PauseMenuToggle;
        LevelSystem.onLevelUp -= LevelUp;
        LevelUpUI.onNewColourChosen -= ColouringBookToggle;
        Health.onPlayerDeath -= GameOver;
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
        if (ColouringBook.instance || LevelUpUI.instance || HelpUI.instance || GameOverUI.instance || MainMenu.instance)
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
        StartCoroutine(DelayedDisplay(colouringBook, 0.1f));
    }


    private void LevelUp() {
        StartCoroutine(LevelUpCelebration());
    }

    private IEnumerator LevelUpCelebration(float duration = 3f) {
        // Instantiate Level Up text
        GameObject levelUpVFX = Resources.Load("FX/Animated/LevelUpVFX") as GameObject;
        GameObject levelUpObject = Instantiate(levelUpVFX, Player.instance.transform, false);
        levelUpObject.transform.localPosition = new Vector2(0, 2);

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

        // despawn level up text
        Destroy(levelUpObject);

        // enable the level up menu book
        ShowLevelUpMenu();
    }

    /// <summary>
    /// Display the Help Menu on screen.
    /// </summary>
    public void HelpMenuToggle () {
        if (HelpUI.instance == null) {
            Instantiate(helpMenu);
        }
    }

    private void GameOver() {
        if (GameOverUI.instance == null) {
            StartCoroutine(DelayedDisplay(gameOverMenu, 2f));
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
