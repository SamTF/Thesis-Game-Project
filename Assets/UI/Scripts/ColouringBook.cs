using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColouringBook : MonoBehaviour
{
    [Header("COLOURING NOTEBOOK!")]

    // UI Documents
    [Header("Main UI Documents")]
    [SerializeField][Tooltip("The UI Document of the Colour Palette")]
    private UIDocument colourPaletteUI = null;
    [SerializeField][Tooltip("The UI Document of the Notebook")]
    private UIDocument notebookUI = null;
    [SerializeField][Tooltip("The UI Document of the Paper")]
    private UIDocument paperUI = null;

    [Header("Element Names")]
    [SerializeField]
    private string canvasID = "Canvas";
    [SerializeField]
    private string saveButtonID = "BtnSave";
    [SerializeField]
    private string spritePreviewID = "Preview";

    // Pixel Editor vars
    private int canvasSize = 16;
    private PixelBlockUI[ , ] pixelGrid = new PixelBlockUI[16, 16];
    private Color selectedColor = Color.magenta;
    private Texture2D drawingTex = null;

    // Visual Elements
    private VisualElement canvas = null;
    private VisualElement spritePreview = null;
    private Button saveButton = null;

    // Components in Children
    [Header("Child Scripts")]
    [SerializeField]
    private ColourPaletteUI colourPalette = null;
    [SerializeField]
    private Texture2D pencilCursor = null;

    /// Singleton thing
    private static ColouringBook _instance = null;
    public static ColouringBook instance
    {
        get {return _instance;}
    }


    private void Awake() {
        // Singleton - there can only be one pixel art editor!
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        
        // getting child pbjects if they weren't added in the inspector
        if (colourPalette == null) colourPalette = GetComponentInChildren<ColourPaletteUI>();

        // Pause the game
        GameManager.instance.GameIsPaused = true;
    }


    private void Start() {
        // Set position
        SetPosition();

        // Initialise Variables
        pixelGrid = new PixelBlockUI[canvasSize, canvasSize];
        drawingTex = CreateTexture(true);

        // Get UI Elements
        canvas = notebookUI.rootVisualElement.Q<VisualElement>(canvasID);
        spritePreview = paperUI.rootVisualElement.Q<VisualElement>(spritePreviewID);
        saveButton = paperUI.rootVisualElement.Q<Button>(saveButtonID);

        // Start Preview
        spritePreview.style.backgroundImage = drawingTex;
        saveButton.clicked += SaveTexture;
        saveButton.clicked += Close;

        // Instantiate all Pixel blocks inside the canvas
        CreatePixelGrid();

        // Mouse cursor
        UnityEngine.UIElements.Cursor cursor = new UnityEngine.UIElements.Cursor();
        cursor.texture = pencilCursor;
        canvas.style.cursor = cursor;
        colourPaletteUI.rootVisualElement.style.cursor = cursor;

        canvas.style.cursor = new StyleCursor(cursor);
        colourPaletteUI.rootVisualElement.style.cursor = new StyleCursor(cursor);
        saveButton.style.cursor = new StyleCursor(cursor);
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
                Vector2Int position = new Vector2Int(x, canvasSize - (y + 1)); // invert Y position
                PixelBlockUI p = new PixelBlockUI(position, Color.clear);

                // Callback Events
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
        pixel.Colour = colourPalette.SelectedColour;
        Debug.Log($"Changed colour of pixel @ {pixel.Position} to {ColorUtility.ToHtmlStringRGBA(colourPalette.SelectedColour)}");
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

    /// <summary>
    /// Sets the world position of the parent game object to the center of the screen
    /// </summary>
    private void SetPosition() {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 screenCenter = Camera.main.ScreenToWorldPoint(screenSize/2);
        transform.position = screenCenter;
    }

    /// <summary>
    /// Closes the Notebook and resumes the game.
    /// </summary>
    private void Close() {
        GetComponent<Animator>().SetTrigger("Out");
        StartCoroutine(CloseAnimation());
    }

    private IEnumerator CloseAnimation() {
        yield return new WaitForSecondsRealtime(1f); // must be realtime because the game is paused
        GameManager.instance.GameIsPaused = false;
        Destroy(gameObject);
    }


    /// <summary>Reset the singleton.</summary>
    private void OnDestroy() {
        if (this == _instance) { _instance = null; }
    }
}
