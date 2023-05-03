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
    [SerializeField]
    private string saveButtonID = "BtnSave";
    [SerializeField]
    private string spritePreviewID = "Preview";

    // Pixel Editor vars
    private int canvasSize = 16;
    private PixelBlockUI[ , ] pixelGrid = new PixelBlockUI[16, 16];
    private Color selectedColor = Color.magenta;
    private Texture2D drawingTex = null;

    public delegate void ColourDelegate(Color colour);
    public ColourDelegate SetColour;

    // UI Elements
    private Button pixelBlock = null;
    private VisualElement canvas = null;
    private VisualElement colourPalette = null;
    private VisualElement spritePreview = null;
    private Button saveButton = null;


    private void Start() {
        // Initialise Variables
        pixelGrid = new PixelBlockUI[canvasSize, canvasSize];
        drawingTex = CreateTexture(true);
        selectedColor = Palette.Colours[0];
        
        // Get UI Elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        canvas = root.Q<VisualElement>(canvasID);
        pixelBlock = root.Q<Button>(pixelBlockID);
        colourPalette = root.Q<VisualElement>(paletteContainerID);
        spritePreview = root.Q<VisualElement>(spritePreviewID);
        saveButton = root.Q<Button>(saveButtonID);

        spritePreview.style.backgroundImage = drawingTex;
        saveButton.clicked += SaveTexture;

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
        canvas.Clear();

        // Loop thru the Grid, creating a PixelBlock at every coordinate
        for (int y = 0; y < pixelGrid.GetLength(1); y++) {
            for (int x = 0; x < pixelGrid.GetLength(0); x++) {
                // Create new PixelBlock
                Vector2Int position = new Vector2Int(x, canvasSize - (y + 1));
                PixelBlockUI p = new PixelBlockUI(position, Color.clear);

                // Callback Events
                p.RegisterCallback<ClickEvent>(OnClick); // test function
                p.RegisterCallback<PointerEnterEvent>(OnPointerEnter, TrickleDown.TrickleDown);
                p.RegisterCallback<PointerDownEvent>(OnPointerDown, TrickleDown.TrickleDown);

                // Add to Canvas
                canvas.Add(p);

                // Add to 2D Array
                pixelGrid[position.x, position.y] = p;
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
        // Debug.Log($"Clicked Pixel Block {target.name} @ {target.transform}");
        Debug.Log($"Clicked Pixel Block {target.name} @ {target.Position}");
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
    /// Change the Colour of the clicked Pixel to the active colour
    /// </summary>
    /// <param name="pixel">The UI Pixel element in the canvas</param>
    private void ChangeColour(PixelBlockUI pixel) {
        pixel.Colour = selectedColor;
        Debug.Log($"Changed colour of pixel @ {pixel.Position} to {ColorUtility.ToHtmlStringRGBA(selectedColor)}");
        UpdateTexture(pixel.Position, pixel.Colour);
    }

    /// <summary>
    /// Erase the colour of the clicked Pixel (revert to original colour or transparent?)
    /// </summary>
    /// <param name="pixel">The UI Pixel element in the canvas</param>
    private void ClearColour(PixelBlockUI pixel) {
        pixel.Colour = Color.clear;
        UpdateTexture(pixel.Position, pixel.Colour);
    }


    /// <summary>
    /// Display all Colours available to the Player.
    /// </summary>
    private void DisplayColours() {
        // Remove any existing element
        colourPalette.Clear();

        Color[] colours = Palette.Colours;

        // Create new elements
        for (int i = 0; i < 4; i++)
        {
            Color colour = colours[i];
            string text = GameManager.instance.PlayerStats.Colour2Stat[colour].Name;

            // Instantiate new ColourItem and add it to the UI
            ColourItem colourItem = new ColourItem(colour, text, SetColour);
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


    /// <summary>
    /// Creates a Texture2D from scratch
    /// </summary>
    /// <returns>A Texture 2D object</returns>
    private Texture2D CreateTexture(bool empty = false) {
        // create empty texture of correct length
        Texture2D tex = new Texture2D(pixelGrid.GetLength(0), pixelGrid.GetLength(1), TextureFormat.ARGB32, false);

        // set point filter mode (no filtering)
        tex.filterMode = FilterMode.Point;

        // Set its colours
        for (int y = 0; y < tex.height; y++) {
            for (int x = 0; x < tex.width; x++) {
                Color color = empty ? Color.clear : pixelGrid[x, y].Colour;
                tex.SetPixel(x, y, color);
            }
        }

        // Apply changes to texture
        tex.Apply();
        return tex;
    }


    /// <summary>
    /// Updates the Drawing Texture in realtime!
    /// </summary>
    /// <param name="coordinates">Pixel coordinate to update.</param>
    /// <param name="colour">New color to give that pixel.</param>
    private void UpdateTexture(Vector2Int coordinates, Color colour) {
        Debug.Log($"Updating Sprite Texture >>> {coordinates} -> {colour}");
        drawingTex.SetPixel(coordinates.x, coordinates.y, colour);
        drawingTex.Apply();
    }


    /// <summary>
    /// Saves the drawn texture to disk -> CUSTOM folder
    /// </summary>
    private void SaveTexture() {
        // Don't save the image if it has already been saved - kinda hacky way but it's only for now
        if (saveButton.text == "SAVED!")    return;

        // encode texture to byte array
        byte[] texBytes = drawingTex.EncodeToPNG();

        // Hash the bytes to generate a unique filenames - why not?
        Hash128 hash = new Hash128();
        hash.Append(texBytes);
        
        // Save bytes as a PNG in the CUSTOM/MyDrawings folder (for now)
        System.IO.File.WriteAllBytes($"{ModManager.ModDirectory}/MyDrawings/{hash.ToString()}.png", texBytes);

        // Update button text
        saveButton.text = "SAVED!";
    }

}
