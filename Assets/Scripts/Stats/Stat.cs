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
    private Color colour;
    private Sprite icon;

    /// <summary>Amount of pixels of this stat's colour on the character sprite. The Value of the attribute.</summary>
    private int _value = 0;
    /// <summary>The base value of the attribute even when no pixels are present in the character (to ensure the game is still playable)</summary>
    private int _baseValue = 1;

    /// <summary>
    /// Initialise the attribute with constant values.
    /// </summary>
    /// <param name="attribute">Attribute type for use in scripts</param>
    /// <param name="name">Name of the attribute for the UI</param>
    /// <param name="colour">Actual Color value code</param>
    /// <param name="icon">Icon to represent this Stat in the UI</param>
    public Stat(Attribute attribute, string name, Color colour, Sprite icon, int baseValue=1) {
        this.attribute = attribute;
        this.name = name;
        this.colour = colour;
        this.icon = icon;
        this._baseValue = baseValue;
    }

    // Getters & Setters

    /// <summary>Value of this stat - the number of pixels it has present in the character sprite.</summary>
    public int Value {
        get { return _value; }
        set { _value = value; }
    }
    /// <summary>The base value of the attribute even when no pixels are present in the character (to ensure the game is still playable)</summary>
    public int baseValue => _baseValue;
    
    public Attribute Attribute => attribute;
    /// <summary>Name of this Stat for displaying in the UI</summary>
    public string Name => name;
    /// <summary>Colour that this stat corresponds to.</summary>
    public Color Colour => colour;
    /// <summary>UI Icon representation of this Stat.</summary>
    public Sprite Icon => icon;
}