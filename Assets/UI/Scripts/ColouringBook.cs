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
    private string spritePreviewID = "Preview";
    [SerializeField]
    private string saveButtonID = "BtnSave";
    [SerializeField]
    private string resetButtonID = "BtnReset";
    [SerializeField]
    private string clearButtonID = "BtnClear";
    

    // Pixel Editor vars
    private int canvasSize = 16;
    private PixelBlockUI[ , ] pixelGrid = new PixelBlockUI[16, 16];
    private Color selectedColor = Color.magenta;
    private Texture2D drawingTex = null;
    private Texture2D originalTex = null;

    // Visual Elements
    private VisualElement canvas = null;
    private VisualElement spritePreview = null;
    private Button saveButton = null;
    private Button resetButton = null;
    private Button clearButton = null;

    // Components in Children
    [Header("Child Scripts")]
    [SerializeField]
    private ColourPaletteUI colourPalette = null;
    [SerializeField]
    private StatsUI statsUI = null;
    [SerializeField]
    private Texture2D pencilCursor = null;

    /// Singleton thing
    private static ColouringBook _instance = null;
    public static ColouringBook instance
    {
        get {return _instance;}
    }

    // Subscribing to Events
    private void OnEnable() {
        PaperUI.onSaveBtnClicked += SaveTexture;
        PaperUI.onSaveBtnClicked += UpdateSprite;
        PaperUI.onSaveBtnClicked += Close;

        PaperUI.onResetBtnClicked += ResetCanvas;

        PaperUI.onClearBtnClicked += ClearCanvas;
    }
    private void OnDisable() {
        PaperUI.onSaveBtnClicked -= SaveTexture;
        PaperUI.onSaveBtnClicked -= Close;
        PaperUI.onSaveBtnClicked -= UpdateSprite;

        PaperUI.onResetBtnClicked -= ResetCanvas;

        PaperUI.onClearBtnClicked -= ClearCanvas;
    }


    private void Awake() {
        // Singleton - there can only be one pixel art editor!
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        
        // getting child objects if they weren't added in the inspector
        if (colourPalette == null) colourPalette = GetComponentInChildren<ColourPaletteUI>();
        if (statsUI == null) statsUI = GetComponentInChildren<StatsUI>();
    }


    private void Start() {
        // Pause the game
        GameManager.instance.GameIsPaused = true;
        CustomCursor.visible = true;

        // Set position
        SetPosition();

        // Get original texture
        if (LevelSystem.instance.Level > 0)
            originalTex = Player.instance?.Sprite.texture;
        else
            originalTex = CreateTexture(true);

        // Initialise Variables
        pixelGrid = new PixelBlockUI[canvasSize, canvasSize];
        drawingTex = CreateTexture(true, originalTex);
        

        // Get UI Elements
        canvas = notebookUI.rootVisualElement.Q<VisualElement>(canvasID);
        spritePreview = paperUI.rootVisualElement.Q<VisualElement>(spritePreviewID);
        saveButton = paperUI.rootVisualElement.Q<Button>(saveButtonID);

        // Start Preview
        spritePreview.style.backgroundImage = drawingTex;

        // Instantiate all Pixel blocks inside the canvas
        CreatePixelGrid();

        // Mouse cursor
        CustomCursor.SetCursor(CustomCursor.Style.brush);

        // Hide HUD
        HUDController.instance.visible = false;
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
                Color pixelColour = originalTex ?
                    originalTex.GetPixel(x, position.y)
                    : Color.clear;

                PixelBlockUI p = new PixelBlockUI(position, pixelColour);

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
        // Do nothing if the pixel already has the desired colour (to avoid unnecessary operations)
        if (pixel.Colour == colourPalette.SelectedColour)
            return;
        
        statsUI.UpdateStatValue(pixel.Colour, false);   // stats UI - decrement old colour

        pixel.Colour = colourPalette.SelectedColour;    // change pixel colour
        
        UpdateTexture(pixel.Position, pixel.Colour);    // update the preview texture
        statsUI.UpdateStatValue(pixel.Colour, true);    // stats UI - increment new colour

        Debug.Log($"Changed colour of pixel @ {pixel.Position} to {ColorUtility.ToHtmlStringRGBA(colourPalette.SelectedColour)}");
    }

    /// <summary>
    /// Erase the colour of the clicked Pixel (revert to original colour or transparent?)
    /// </summary>
    /// <param name="pixel">The UI Pixel element in the canvas</param>
    private void ClearColour(PixelBlockUI pixel) {
        // Do nothing if the pixel already has the desired colour (to avoid unnecessary operations)
        if (pixel.Colour == Color.clear)
            return;

        statsUI.UpdateStatValue(pixel.Colour, false);   // stats UI - decrement old colour
        pixel.Colour = Color.clear;                     // set the colour to transparent
        UpdateTexture(pixel.Position, pixel.Colour);    // update the preview texture
    }


    /// <summary>
    /// Creates a Texture2D from scratch
    /// </summary>
    /// <returns>A Texture 2D object</returns>
    private Texture2D CreateTexture(bool empty = true, Texture2D copyTexture = null) {
        // create empty texture of correct length
        Texture2D tex = new Texture2D(pixelGrid.GetLength(0), pixelGrid.GetLength(1), TextureFormat.ARGB32, false);

        // set point filter mode (no filtering)
        tex.filterMode = FilterMode.Point;

        // Set its colours
        for (int y = 0; y < tex.height; y++) {
            for (int x = 0; x < tex.width; x++) {
                Color color = empty ? Color.clear : pixelGrid[x, y].Colour;
                if (copyTexture) color = copyTexture.GetPixel(x, y);
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
        
        // Colour[] newColours = ImageAnalyser.Analyse(drawingTex);
        // Player.instance.Stats.GetStatsFromImage(newColours);
        // statsUI.UpdateStats();
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
        string hashStr = DataTracker.HashTexture(drawingTex);
        
        // Improved file name
        string username = PlayerPrefs.GetString("username");
        string datetime = System.DateTime.Now.ToString("dd_MM_HHmm");
        string fileName = $"{username}-{hashStr}-{LevelSystem.instance.Level}-{datetime}";

        // Save bytes as a PNG in the CUSTOM/MyDrawings folder
        try {
            System.IO.File.WriteAllBytes($"{ModManager.DrawingsDirectory}/{fileName}.png", texBytes);
        } catch (System.Exception) {
            return;
        }
        
        // Update button text
        saveButton.text = "SAVED!";
    }

    /// <summary>
    /// Resets the drawing back to the original state.
    /// </summary>
    private void ResetCanvas() {
        for (int y = 0; y < pixelGrid.GetLength(1); y++) {
            for (int x = 0; x < pixelGrid.GetLength(0); x++) {
                Vector2Int position = new Vector2Int(x, canvasSize - (y + 1)); // invert Y position
                PixelBlockUI p = pixelGrid[x, y];

                // update the pixels on the canvas
                p.Colour = originalTex.GetPixel(x, y);

                // update the texture preview
                UpdateTexture(new Vector2Int(x, y), originalTex.GetPixel(x, y));
            }
        }

        // Update stats UI values
        statsUI.RefreshAllStats();
    }

    /// <summary>
    /// Erases every colour in the drawing. Blank canvas!
    /// </summary>
    private void ClearCanvas() {
        for (int y = 0; y < pixelGrid.GetLength(1); y++) {
            for (int x = 0; x < pixelGrid.GetLength(0); x++) {
                Vector2Int position = new Vector2Int(x, canvasSize - (y + 1)); // invert Y position
                PixelBlockUI p = pixelGrid[x, y];

                // update the pixels on the canvas
                p.Colour = Color.clear;

                // update the texture preview
                UpdateTexture(position, Color.clear);
            }
        }

        // Update stats UI values
        statsUI.RefreshAllStats(drawingTex);
    }

    /// <summary>
    /// Updates the Sprite of the GameObject that is currently being edited.
    /// </summary>
    private void UpdateSprite() {
        Player.instance.ResetMovement();
        Player.instance.UpdateSprite(drawingTex);
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
        // play closing animation
        GetComponent<Animator>().SetTrigger("Out");
        StartCoroutine(CloseAnimation());

        // reset the cursor
        CustomCursor.SetCursor(CustomCursor.Style.auto);
        CustomCursor.visible = false;

        // show the HUD
        HUDController.instance.visible = true;
    }

    private IEnumerator CloseAnimation() {
        yield return new WaitForSecondsRealtime(0.5f); // must be realtime because the game is paused
        GameManager.instance.GameIsPaused = false;
        Destroy(gameObject);
    }


    /// <summary>Reset the singleton.</summary>
    private void OnDestroy() {
        if (this == _instance) { _instance = null; }
    }
}
