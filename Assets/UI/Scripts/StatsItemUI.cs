using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Helper UI Class to display stats in the Notebook UI. It's a visual element containing an Image and a Label.
/// <br>Image: Icon of the stat.</br>
/// <br>Label: Name of the stat.</br>
/// </summary>
public class StatsItemUI : VisualElement
{
    // Constants
    private const string baseClass = "stats-item";
    private const string textClass = "text";

    // Vars
    private readonly Color colour;
    private readonly Sprite icon;
    private string text;
    private int value;


    /// <summary>
    /// Create a new Stats Item: UI element that displays a Stat's Icon and Value
    /// </summary>
    /// <param name="colour">Colour of the Stat.</param>
    /// <param name="value">Number to display on the label.</param>
    /// <param name="icon">Sprite icon to display next to value.</param>
    /// <param name="name">Optional name to give this element.</param>
    public StatsItemUI(Color colour, int value, Sprite icon, string name = "StatItem") {
        // set properties
        this.AddToClassList(baseClass);
        this.colour = colour;
        this.value = value;
        this.text = value.ToString();
        this.name = name;

        // Add Image
        MyImage iconImg = new MyImage();
        iconImg.sprite = icon;
        iconImg.tintColor = colour;
        this.Add(iconImg);

        // add label
        Label label = new Label(text);
        label.AddToClassList(textClass);
        label.style.color = colour;
        this.Add(label);
    }

    /// <summary>The Colour value of this stat</summary>
    public Color Colour => colour;

    /// <summary>The numerical valued displayed on this element's label</summary>
    public int Value {
        get { return value; }
        set {
            this.value = Mathf.Clamp(value, 0, 999);
            this.Q<Label>().text = value.ToString();
        }
    }
}
