using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

/// <summary>
/// UI World Space Object that displays the Colour Palette on screen and sets the active selected colour.
/// </summary>
public class ColourPaletteUI : MonoBehaviour
{
    [Header("COLOUR PALETTE UI")]
    [SerializeField]
    private string colourSplashID = "ColourSplash";

    // Vars
    [SerializeField]
    private Color selectedColour;

    public delegate void ColourDelegate(Color colour);
    public ColourDelegate OnClick;

    public static event Action<Color> onColourChange;

    // Elements
    private VisualElement root = null;
    List<VisualElement> elementList = null;
    List<ColourSplash> splashList = new List<ColourSplash>();


    /// <summary>
    /// Helper UI Element representing each Colour in the Palette UI. It's a Visual Element containing a Button and Sprite.
    /// <br>Invisible button that sets given Colour as active on click</br>
    /// <br>Shows the Colour as an ink blob dynamically!</br>
    /// </summary>
    private class ColourSplash {
        // Vars
        private readonly Color colour;
        private bool selected = false;

        // Elements
        private readonly VisualElement rootElement = null;
        private readonly Button button = null;
        private readonly MyImage sprite = null;


        /// <summary>
        /// Helper UI Element representing each Colour in the Palette UI. It's a Visual Element containing a Button and Sprite.
        /// <br>Invisible button that sets given Colour as active on click</br>
        /// <br>Shows the Colour as an ink blob dynamically!</br>
        /// </summary>
        /// <param name="element">UI Element in the UXML that this ColourSplash represents</param>
        /// <param name="colour">The Colour from the Palette that this splash will represent</param>
        /// <param name="onClick">Function to execute on click</param>
        public ColourSplash(VisualElement element, Color colour, ColourDelegate onClick) {
            // set properties
            this.rootElement = element;
            this.colour = colour;
            rootElement.visible = true;

            // fetch elements
            button = rootElement.Q<Button>();
            sprite = rootElement.Q<MyImage>();

            // set up Button
            button.clicked += () => {
                onClick?.Invoke(this.colour);
            };

            // Set up Sprite
            sprite.sprite = sprite.style.backgroundImage.value.sprite;
            sprite.tintColor = Color.green;
            sprite.style.unityBackgroundImageTintColor = this.colour;
        }
    };


    private void Start() {
        // Initialise vars
        SetColour(Palette.Colours[0]);
        OnClick = SetColour;
        
        // Get Elements
        root = GetComponent<UIDocument>().rootVisualElement;
        elementList = root.Query<VisualElement>(colourSplashID).ToList();

        // Hide all elements so only unlocked colours are displayed
        elementList.ForEach(element => { element.visible = false; });

        // Initialise ColourSplash elements for each unlocked colour
        Color[] colours = LevelSystem.instance.UnlockedColours;

        for (int i = 0; i < colours.Length; i++) {
            VisualElement element = elementList[i];
            Color color = colours[i];

            ColourSplash cs = new ColourSplash(element, Palette.Colours[i], OnClick);
            splashList.Add(cs);
        }
    }

    /// <summary>
    /// Change the currently Selected Colour (i.e. the colour that that will be added to the canvas when clicked).
    /// This is a delegate event called from the Child elements.
    /// </summary>
    /// <param name="newColour">New Colour to paint with.</param>
    private void SetColour(Color newColour) {
        selectedColour = newColour;
        onColourChange?.Invoke(newColour);
    }


    // Public Getters
    /// <summary>The currently selected colour to draw with.</summary>
    public Color SelectedColour => selectedColour;
}
