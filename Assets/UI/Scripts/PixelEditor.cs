using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// USEFUL LINKS
// https://docs.unity3d.com/Manual/UIE-Click-Events.html
// https://forum.unity.com/threads/ui-builder-how-to-make-buttons-interactable.786701/
// https://forum.unity.com/threads/custom-data-attributes-on-visualelements-ok-and-buttons-not-ok.1396459/

public class PixelEditor : MonoBehaviour
{
    [Header("PIXEL EDITOR CONTROLLER")]
    [SerializeField]
    private string pixelBlockID = "Pixel";
    [SerializeField]
    private string canvasID = "Canvas";
    [SerializeField]
    private string paletteContainerID = "PaletteContainer";

    // Pixel Editor vars
    private Color[ , ] pixelGrid = new Color[16, 16];
    private Color selectedColor = Color.magenta;
    public delegate void ColourDelegate(Color colour);
    public ColourDelegate SetColour;


    // UI Elements
    private Button pixelBlock = null;
    private VisualElement canvas = null;
    private VisualElement colourPalette = null;


    private void Start() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        canvas = root.Q<VisualElement>(canvasID);
        pixelBlock = root.Q<Button>(pixelBlockID);
        colourPalette = root.Q<VisualElement>(paletteContainerID);

        // pixelBlock.RegisterCallback<ClickEvent>(ChangeColour);

        // root.Q<Button>(className: "Pixel").RegisterCallback<ClickEvent>(ChangeColour);

        // root.Query<Button>().ForEach((button) =>
        // {
        //     button.RegisterCallback<ClickEvent>(OnClick);

        // });

        // Instantiate all Pixel blocks inside the canvas
        CreatePixelGrid();

        // Create SetColour delegate
        SetColour = SetSelectedColour;

        // Display available colours
        DisplayColours();
    }


    /// <summary>
    /// Create the Pixel Grid in the canvas by adding Pixel elements to the Canvas and setting their callback events.
    /// </summary>
    private void CreatePixelGrid() {
        for (int x = 0; x < pixelGrid.GetLength(0); x++) {
            for (int y = 0; y < pixelGrid.GetLength(1); y++) {
                Vector2Int position = new Vector2Int(x, y);
                PixelBlockUI p = new PixelBlockUI(position, Color.clear);

                // Callback Events
                p.RegisterCallback<ClickEvent>(OnClick); // test function
                p.RegisterCallback<PointerEnterEvent>(OnPointerEnter, TrickleDown.TrickleDown);
                p.RegisterCallback<PointerDownEvent>(OnPointerDown, TrickleDown.TrickleDown);


                canvas.Add(p);
            }
        }
    }


    /// <summary>
    /// test function, no longer used
    /// </summary>
    private void OnClick(ClickEvent evt) {
        // Fetch the target button of the click event
        PixelBlockUI target = evt.target as PixelBlockUI;

        // Change its colour
        // target.Colour = Random.ColorHSV(hueMin:0f, hueMax:1f, saturationMin: 1f, saturationMax: 1f, valueMin:1f, valueMax:1f);
        
        // Debug prints
        Debug.Log($"Clicked Pixel Block {target.name} @ {target.transform}");
    }


    /// <summary>
    /// Triggers when clicking on a Pixel.
    /// </summary>
    private void OnPointerDown(PointerDownEvent evt) {
        PixelBlockUI target = evt.target as PixelBlockUI;
        // Debug.Log($"Clicked on [{target.name}] @ {target.worldBound}");

        // Left Click
        if (Input.GetMouseButton(0)) {
            ChangeColour(target);
        }
        // Right Click
        else if (Input.GetMouseButton(1)) {
            ClearColour(target);
        }
    }

    /// <summary>
    /// Triggers when holding mouse over Pixel.
    /// </summary>
    private void OnPointerEnter(PointerEnterEvent evt) {
        PixelBlockUI target = evt.target as PixelBlockUI;
        // Debug.Log($"Hovering mouse over [{target.name}] @ {target.worldBound}");

        // Left Click
        if (Input.GetMouseButton(0)) {
            ChangeColour(target);
        }
        // Right Click
        else if (Input.GetMouseButton(1)) {
            ClearColour(target);
        }

    }


    /// <summary>
    /// Change the Colour of the clicked Pixel to the active colour (currently random)
    /// </summary>
    /// <param name="pixel">The UI Pixel element in the canvas</param>
    private void ChangeColour(PixelBlockUI pixel) {
        pixel.Colour = selectedColor;
        Debug.Log($"Changed colour of pixel @ {pixel.Position} to {ColorUtility.ToHtmlStringRGBA(selectedColor)}");
    }

    /// <summary>
    /// Erase the colour of the clicked Pixel (revert to original colour or transparent?)
    /// </summary>
    /// <param name="pixel">The UI Pixel element in the canvas</param>
    private void ClearColour(PixelBlockUI pixel) {
        pixel.Colour = Color.clear;
    }


    /// <summary>
    /// Display all Colours available to the Player.
    /// </summary>
    private void DisplayColours() {
        // Remove any existing element
        colourPalette.Clear();

        Colour[] colours = Palette.ColourObjects;

        // Create new elements
        for (int i = 0; i < 4; i++)
        {
            Colour colour = colours[i];

            // Instantiate new ColourItem and add it to the UI
            ColourItem colourItem = new ColourItem(colour.colour, colour.hexColour, SetColour);
            colourPalette.Add(colourItem);
        }
    }

    /// <summary>
    /// Change the currently Selected Colour (i.e. the colour that that will be added to the canvas when clicked).
    /// This is a delegate event called from the Child elements.
    /// </summary>
    /// <param name="newColour">New Colour to paint with.</param>
    private void SetSelectedColour(Color newColour) {
        selectedColor = newColour;
    }

}
