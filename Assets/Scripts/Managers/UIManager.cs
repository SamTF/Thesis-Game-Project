using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI MANAGER")]
    [SerializeField]
    private Transform panel = null;
    [SerializeField]
    private GameObject textPrefab = null;

    [SerializeField]
    private GameObject pauseMenu = null;

    [SerializeField]
    private GameObject colouringBook = null;

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

        DontDestroyOnLoad(this.gameObject);
    }

    // Events
    private void OnEnable() {
        GameManager.onPause += PauseMenuToggle;
        // LevelSystem.onLevelUp += ColouringBookToggle;
        LevelSystem.onLevelUp += LevelUp;
    }
    private void OnDisable() {
        GameManager.onPause -= PauseMenuToggle;
        LevelSystem.onLevelUp -= ColouringBookToggle;
    }

    /// <summary>
    /// Show a list of colours in the UI: their HEX value and pixel count.
    /// </summary>
    /// <param name="colours">Array of Colour objects</param>
    public void DisplayColours(Colour[] colours) {
        foreach (Colour c in colours)
        {
            GameObject textObject = Instantiate(textPrefab, parent:panel);
            TextMeshProUGUI textUI = textObject.GetComponent<TextMeshProUGUI>();
            string text = $"#{c.hexColour} - {c.value} pixels";
            textUI.text = text;
            textUI.color = c.colour;
        }
    }

    /// <summary>
    /// Shows a list of Stats in the UI: Their name and value, coloured appropriately
    /// </summary>
    /// <param name="stats">Array of Stat objects to display</param>
    public void DisplayStats(Stat[] stats) {
        // Destroying any Text elements that may already exist
        for (int i = panel.childCount-1; i >= 0; i--) {
            Destroy(panel.GetChild(i).gameObject);
        }

        // Creating a Text element for each stat
        foreach (Stat s in stats)
        {
            GameObject textObject = Instantiate(textPrefab, parent:panel);
            TextMeshProUGUI textUI = textObject.GetComponent<TextMeshProUGUI>();
            string text = $"{s.Name}: {s.Value}";
            textUI.text = text;
            textUI.color = s.Colour;
        }
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

        // If a Colouring Book instance exists, do nothing
        if (ColouringBook.instance) return;
            
        // Instantiate a Pause Menu
        GameObject pauseMenuObject = Instantiate(pauseMenu);
    }

    /// <summary>
    /// Instantiate the Colouring Book (if it's now already present)
    /// </summary>
    private void ColouringBookToggle() {
        // do nothing if there already is a pixel art editor running
        if (ColouringBook.instance) {
            return;
        }

        Instantiate(colouringBook);
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

        // enable the colouring book
        ColouringBookToggle();
    }
}
