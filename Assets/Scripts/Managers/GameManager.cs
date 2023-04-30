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

    // Status
    private bool gameIsPaused = false;

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

        ModManager.ListMods();
        Palette.LoadPalette();
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

        // it's probably best to just make the player a singleton tbh...
        if (!player) {
            player = FindObjectOfType<Player>();
        }
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
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelStart;
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
}
