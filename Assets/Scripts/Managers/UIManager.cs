using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Transform panel = null;
    [SerializeField]
    private GameObject textPrefab = null;
    [SerializeField]
    private GameObject pauseMenu = null;
    private PauseMenu pauseMenuInstance = null;

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
    }
    private void OnDisable() {
        GameManager.onPause -= PauseMenuToggle;
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
        if (pauseMenuInstance) {
            pauseMenuInstance.OnResumeGame();
            return;
        }
            
        // Instantiate a Pause Menu
        GameObject pauseMenuObject = Instantiate(pauseMenu);
        pauseMenuInstance = pauseMenuObject.GetComponent<PauseMenu>();
    }
}
