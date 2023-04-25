using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This class is a "button" on the UI representing a Pixel in the Pixel Art Editor Canvas.
/// </summary>
public class PixelBlockUI : MyButton
{
    // Vars
    private string cssClass = "Pixel";
    private Vector2Int position;
    private Color colour;

    /// <summary>
    /// Create a Pixel Block button to use in the Pixel Art Editor. Clicking this button will change its colour to the currently selected one.
    /// </summary>
    /// <param name="position">(X,Y) Position in the Canvas/Image grid.</param>
    /// <param name="colour">Default starting colour.</param>
    public PixelBlockUI(Vector2Int position, Color colour, string name="Pixel Block") {
        this.position = position;
        this.colour = colour;
        this.name = name;

        this.style.backgroundColor = colour;
        this.AddToClassList(cssClass);
        // this.pickingMode = PickingMode.Position;
        // this.focusable = true;
    }

    // Getters & Setters
    /// <summary>Position in the Canvas/Image grid.</summary>
    public Vector2Int Position => position;

    /// <summary>
    /// The Color of this pixel.
    /// </summary>
    /// <value>Unity Color value (for now)</value>
    public Color Colour {
        get { return colour; }
        set {
            colour = value;
            this.style.backgroundColor = value;
        }
    }
}
