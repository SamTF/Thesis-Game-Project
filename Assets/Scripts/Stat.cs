using UnityEngine;

/// <summary>
/// Class that holds all the necessary info about each attribute.
/// </summary>
[System.Serializable]
public class Stat {
    [Header("Character Attribute")]
    [SerializeField][Tooltip("Name of the attribute")]
    private string name;
    [SerializeField][Tooltip("Its lightness value in the palette")]
    private ColourValue colourValue;
    private Color colour;
    private int _value = 0;

    /// <summary>
    /// Initialise the attribute with constant values.
    /// </summary>
    /// <param name="name">Name of he attribute for the UI</param>
    /// <param name="colourValue">Role/position in the colour palette</param>
    /// <param name="colour">Actual Color value code</param>
    public Stat(string name, ColourValue colourValue, Color colour) {
        this.name = name;
        this.colourValue = colourValue;
        this.colour = colour;
    }

    // Getter/Setter for the attribute value
    public int Value {
        get { return _value; }
        set { _value = value; }
    }

    // Getters
    public string Name => name;
    public ColourValue ColourValue => colourValue;
    public Color Colour => colour;
}