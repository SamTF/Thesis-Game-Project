using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text = null;
    [SerializeField]
    private Transform panel = null;
    [SerializeField]
    private GameObject textPrefab = null;

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
        foreach (Stat s in stats)
        {
            GameObject textObject = Instantiate(textPrefab, parent:panel);
            TextMeshProUGUI textUI = textObject.GetComponent<TextMeshProUGUI>();
            string text = $"{s.Name}: {s.Value}";
            textUI.text = text;
            textUI.color = s.Colour;
        }
    }
}
