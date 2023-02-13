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
}
