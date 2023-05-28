using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("GAME MANAGER")]
    [SerializeField]
    private Player player = null;
    [SerializeField]
    private Vector2Int levelRadius = new Vector2Int(50, 50);

    public enum Scenes {
        MainMenu = 0,
        Game = 1
    }

    // Status
    private bool gameIsPaused = false;

    // Statistics
    private Timer timer = null;

    // Customisation
    private Color grassColour;

    // Events
    public static event Action onPause;
    public static event Action onRestart;
    public static event Action onLevelStart;

    /// Singleton thing
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get {return _instance;}
    }

    private void Awake()
    {
        // Singleton Thing
        if (instance == null)   { _instance = this; }
        else                    { Destroy(gameObject); }

        DontDestroyOnLoad(this.gameObject);

        // initialise static classes
        ModManager.ListMods();
        Palette.LoadPalette();
        DataTracker.WakeUp();

        // make cursor visible
        CustomCursor.visible = true;
    }


    // Game-wide Input
    private void Update() {
        // Pause Button Pressed
        if(Input.GetButtonDown("Pause")) {
            gameIsPaused = !gameIsPaused;
            onPause?.Invoke();
        }
    }

    /// <summary>
    /// Triggers an event when the current scene first loads
    /// </summary>
    private void OnLevelStart(Scene scene, LoadSceneMode mode) {
        onLevelStart?.Invoke();

        // reset time scale just in case
        Time.timeScale = 1f;
        
        // start timer
        timer = new Timer();
        timer.Start();

        // hide cursor by default
        CustomCursor.visible = false;

        // it's probably best to just make the player a singleton tbh...
        if (!player) {
            player = FindObjectOfType<Player>();
        }
    }

    /// <summary>
    /// Triggered when the Player dies. Stops the Timer.
    /// </summary>
    private void OnPlayerDeath() {
        timer.Stop();
    }

    /// <summary>
    /// Change to a new Scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    public void ChangeScene(Scenes scene) {
        int sceneID = (int)scene;
        SceneManager.LoadScene(sceneID);
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void RestartLevel() {
        onRestart?.Invoke();
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }

    // Subscribing to Events
    private void OnEnable() {
        SceneManager.sceneLoaded += OnLevelStart;
        Health.onPlayerDeath += OnPlayerDeath;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelStart;
        Health.onPlayerDeath -= OnPlayerDeath;
    }

    // Getters
    /// <summary>The Player object in the scene.</summary>
    public Player Player => player;
    /// <summary>The current position of the Player in world space. </summary>
    public Vector2 PlayerPosition => player.transform.position;
    /// <summary>The current Sprite image of the Player's body.</summary>
    public Sprite PlayerSprite => player.Sprite;
    /// <summary>The Stats Colours of the Player and their current values.</summary>
    public Stats PlayerStats => player.Stats;
    /// <summary>Timer counting the seconds survived in the current level.</summary>
    public Timer Timer => timer;
    /// <summary>The X and Y radius of the current level.</summary>
    public Vector2 LevelRadius => levelRadius;

    /// <summary>Gets or Sets whether the game is paused. Setting it to true will trigger the OnPause event.</summary>
    public bool GameIsPaused {
        get { return gameIsPaused; }
        set {
            gameIsPaused = value;

            // pausing the game
            if (gameIsPaused) {
                onPause?.Invoke();              // trigger the game paused event
                Time.timeScale = 0f;            // freeze the game
                CustomCursor.visible = true;    // show the cursor
            }
            // unpausing the game
            else {
                Time.timeScale = 1f;
                CustomCursor.visible = false;
            }
        }
    }

    /// <summary>Gets the current scene as an Enum.</summary>
    public Scenes CurrentScene => (Scenes)SceneManager.GetActiveScene().buildIndex;
}
