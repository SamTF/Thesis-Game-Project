using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Helper UI Class for displaying items in the Colour Palette. It's a Visual Element containing a Button and Label.
/// <br>Button: Colour picker that sets active colour on click.</br>
/// <br>Label: Name of the stat the colour corresponds to.</br>
/// </summary>
public class ColourItem : VisualElement
{
    // Constants
    private const string baseClass = "ColourItem";
    private const string btnClass = "ColourPicker";
    private const string txtClass = "StatText";

    // Vars
    private Color colour;
    private bool selected = false;


    /// <summary>
    /// Create a new ColourItem: a visual element containing a Button and Label.
    /// Button: Colour picker.
    /// Label: Name of the stat the colour corresponds to.
    /// </summary>
    /// <param name="colour">Colour of the button => will change selected colour to this colour on click.</param>
    /// <param name="text">Text to display on the label.</param>
    /// <param name="onClick">Delegate for when the button is clicked.</param>
    public ColourItem(Color colour, string text, PixelEditor.ColourDelegate onClick) {
        // Set properties
        this.AddToClassList(baseClass);
        this.colour = colour;

        // Add Button
        Button btn = new Button();
        btn.AddToClassList(btnClass);
        btn.style.backgroundColor = colour;

        // Invoke delegate on click
        btn.clicked += () => {
            onClick?.Invoke(this.colour);
        };

        this.Add(btn);

        // Add Label
        Label label = new Label(text);
        label.AddToClassList(txtClass);
        this.Add(label);
    }


    // Getters & Setters
    public Color Colour => colour;
    
    public bool Selected {
        get { return selected; }
    }
}
