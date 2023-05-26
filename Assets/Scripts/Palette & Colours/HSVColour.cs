using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class to work with HSV colour values instead of the Unity RGB default
/// </summary>
[System.Serializable]
public class HSVColour
{
    int hue = 0;
    int saturation = 0;
    int value = 0;

    /// <summary>
    /// Create a HSV Colour value from a given RGB colour.
    /// </summary>
    /// <param name="colour">The RGB colour</param>
    public HSVColour(Color colour) {
        float h, s, v;
        Color.RGBToHSV (colour, out h, out s, out v);

        this.hue = (int)(h * 360);
        this.saturation = (int)(s * 100);
        this.value = (int)(v * 100);
    }

    /// <summary>
    /// Create a HSV Colour value from Hue, Saturation, and Brightness values
    /// </summary>
    /// <param name="h">Hue (0-360)</param>
    /// <param name="s">Saturation (0-100)</param>
    /// <param name="v">Brightness (0-100)</param>
    public HSVColour(int h, int s, int v) {
        this.hue = Mathf.Clamp(h, 0, 360);
        this.saturation = Mathf.Clamp(s, 0, 100);
        this.value = Mathf.Clamp(v, 0, 100);
    }

    
    /// Getters & Setters
    public Color Colour => Color.HSVToRGB(hue / 360f, saturation / 100f, value / 100f);

    /// <summary>The Hue value of this Colour.</summary>
    public int Hue {
        get { return hue; }
        set { hue = Mathf.Clamp(value, 0, 360); }
    }

    /// <summary>The Saturation value of this Colour.</summary>
    public int Saturation {
        get { return saturation; }
        set { saturation = Mathf.Clamp(value, 0, 100); }
    }

    /// <summary>The Brightness value of this Colour.</summary>
    public int Brightness {
        get { return value; }
        set { value = Mathf.Clamp(value, 0, 100); }
    }

    /// <summary>String representation of this class instance.</summary>
    /// <returns>String</returns>
    public override string ToString() {
        return $"HSV Colour >>> H: {this.hue}, S: {this.saturation}, V: {this.value}";
    }
}
