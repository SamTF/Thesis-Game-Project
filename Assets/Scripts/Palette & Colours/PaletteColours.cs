using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class that holds all colours in the palette
/// </summary>
public class PaletteColours {
    public Color light;
    public Color midLight;
    public Color midDark;
    public Color dark;

    private Color[] colourList = new Color[4];
    private Dictionary<Color, ColourValue> colour2PaletteValue = new Dictionary<Color, ColourValue>();

    /// <summary>
    /// Initialise all Palette Colours and store them in this object
    /// </summary>
    /// <param name="colours">The Colours in the palette</param>
    public PaletteColours(Color[] colours) {
        if (colours.Length < 4) {
            Debug.LogError("Given Colour Palette does not have enough colours");
            return;
        }

        colourList = colours;
        light = colours[0];
        midLight = colours[1];
        midDark = colours[2];
        dark = colours[3];

        colour2PaletteValue.Add(light, ColourValue.Light);
        colour2PaletteValue.Add(midLight, ColourValue.MidLight);
        colour2PaletteValue.Add(midDark, ColourValue.MidDark);
        colour2PaletteValue.Add(dark, ColourValue.Dark);
    }

    /// <summary>
    /// Gets a colour's respective lightness value in the palette
    /// </summary>
    /// <param name="colour"></param>
    /// <returns></returns>
    public ColourValue Colour2PaletteValue(Color colour) {
        return colour2PaletteValue[colour];
    }
}