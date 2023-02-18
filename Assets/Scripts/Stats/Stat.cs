using UnityEngine;

/// <summary>
/// Class that holds all the necessary info about each attribute.
/// </summary>
[System.Serializable]
public class Stat {
    [Header("Character Attribute")]
    [SerializeField][Tooltip("Attribute type: for use in scripts")]
    private Attribute attribute;
    [SerializeField][Tooltip("Name of the attribute: for UI only")]
    private string name;
    [SerializeField][Tooltip("Its lightness value in the palette")]
    private ColourValue colourValue;
    private Color colour;
    private int _value = 0;

    /// <summary>
    /// Initialise the attribute with constant values.
    /// </summary>
    /// <param name="attribute">Attribute type for use in scripts</param>
    /// <param name="name">Name of the attribute for the UI</param>
    /// <param name="colourValue">Role/position in the colour palette</param>
    /// <param name="colour">Actual Color value code</param>
    public Stat(Attribute attribute, string name, ColourValue colourValue, Color colour) {
        this.attribute = attribute;
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
    public Attribute Attribute => attribute;
    public string Name => name;
    public ColourValue ColourValue => colourValue;
    public Color Colour => colour;
}