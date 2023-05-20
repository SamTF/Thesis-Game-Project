using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Compact version of StatsItemUI that only needs height of 8px and doesn't use text.
/// Displays two labels: stat name and stat value
/// </summary>
public class StatsItemCompactUI : VisualElement
{
    // Constants
    private const string baseClass = "stat-item";
    private string[] nameClasses = {"text-small", "name"};
    private string[] valueClasses = {"text-small", "value"};

    // Vars
    private Stat stat;
    private readonly Color colour;
    private string textName;
    private string textValue;
    private int value;

    // Elements
    private Label labelName = null;
    private Label labelValue = null;
    

    /// <summary>
    /// Create a new Compact Stats Item: UI element that displays a Stat's Name and Value using only text.
    /// </summary>
    /// <param name="stat">The stat to display. All values are automatically fetched from it.</param>
    public StatsItemCompactUI(Stat stat) {
        // setting vars
        this.stat = stat;
        this.colour = stat.Colour;
        this.value = stat.Value;
        this.textName = stat.Name;
        this.textValue = stat.Value.ToString();

        // root element properties
        this.AddToClassList(baseClass);

        // creating elements
        // add stat name label
        labelName = new Label(textName);
        foreach (string c in nameClasses) {
            labelName.AddToClassList(c);
        }

        labelValue = new Label(textValue);
        foreach (string c in valueClasses) {
            labelValue.AddToClassList(c);
        }

        labelName.style.color = colour;
        labelValue.style.color = colour;

        this.Add(labelName);
        this.Add(labelValue);
        
    }

    /// <summary>
    /// Refreshes the displayed value by fetching the Stat's current value
    /// </summary>
    public void UpdateValue() {
        labelValue.text = stat.Value.ToString();
    }
}
