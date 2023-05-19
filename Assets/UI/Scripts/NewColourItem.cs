using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

/// <summary>
/// Colour Item option that appears in the Level Up menu. Clicking this unlocks this colur/stat and adds it to the Player's palette.
/// Contains a Name label, Icon image, and Description label
/// </summary>
public class NewColourItem : VisualElement
{
    // Element IDs
    private const string titleID = "StatName";
    private const string descriptionID = "Description";
    private const string iconID = "Icon";

    // Elements
    private Label title = null;
    private Label description = null;
    private MyImage icon = null;

    // Main element template
    private VisualTreeAsset asset = null;
    public VisualElement element = null;

    // Properties
    private Stat stat;
    private Color colour;

    // Events
    public static event Action<Color> onClick;

    public NewColourItem(Stat stat, Color colour, string name, string descriptionText, int tabIndex = 0) {
        // Instantiate Visual element
        asset = Resources.Load<VisualTreeAsset>("UIElements/ColourItem");
        element = asset.Instantiate();

        // fetching elements
        title = element.Q<Label>(titleID);
        description = element.Q<Label>(descriptionID);
        icon = element.Q<MyImage>(iconID);

        // setting properties
        this.stat = stat;
        this.colour = colour;
        element.focusable = true;
        element.tabIndex = tabIndex;
        this.focusable = true;
        this.tabIndex = tabIndex;
        

        // setting values
        title.text = name;
        title.style.color = colour;
        description.text = descriptionText;
        icon.style.unityBackgroundImageTintColor = colour;

        // click callback
        element.AddManipulator(new Clickable(evt => OnClick(evt)));
    }

    private void OnClick(EventBase evt) {
        Debug.Log("ON CLICK!");
        onClick?.Invoke(this.colour);
    }
}
