using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Container for the Heart UI icons. Displays the Player's current health in TBOI-style hearts. Unity UI Builder Edition!
/// </summary>
public class HeartsUI : MonoBehaviour
{
    // Constants
    [Header("Hearts UI")]
    [SerializeField]
    private string iconName = "Hearts";
    private const int iconSize = 8;
    private string fileType = "png";

    // Textures & Sprites
    private Texture2D heartTex = null;
    private Sprite heartFull = null;
    private Sprite heartHalf = null;
    private Sprite heartEmpty = null;

    // Static Objects
    public static HeartType Full;
    public static HeartType Half;
    public static HeartType Empty;
    public static HeartType[] heartTypes;

    // UI Elements
    private VisualElement root;
    private VisualElement heartsContainer;

    // Script references
    private Health playerHealth = null;


    // Load the Heart Spritesheet and create all Sprite icons
    private void Awake() {
        // Setting the name of the file required to mod the image
        string name = iconName != "" ? iconName : this.name;
        iconName = $"{name}.{fileType}";
        Debug.Log(iconName);

        // Checking if the player created a custom heart icon
        if (ModManager.ModExists(iconName)) {
            Debug.Log("[HEART]>>> Custom heart found");
            heartTex = ImageLoader.LoadTextureFromFile(iconName);
        }
        // If not, use the default icon
        else {
            Debug.Log("[HEART]>>> Loading default Heart...");
            heartTex = Resources.Load("UI/Hearts") as Texture2D;  
        }

        // Create the 3 sprites from the single texture and save them in memory
        CreateAllSprites(Pivot.Center);

        // Initialising HeartType objects
        Full = new HeartType(HeartFill.Full, heartFull);
        Half = new HeartType(HeartFill.Half, heartHalf);
        Empty = new HeartType(HeartFill.Empty, heartEmpty);
        heartTypes = new HeartType[] { Empty, Half, Full };
    }

    void Start()
    {
        // Get elements
        root = GetComponent<UIDocument>().rootVisualElement;
        heartsContainer = root.Q<VisualElement>("HeartsContainer");

        // Get script references
        playerHealth = GameManager.instance.Player.Health;

        /// !! Instantiating hearts !!
        InitHearts();
    }


    ////// EVENTS ////////////
    // Subscribing and unsubscribing to events
    private void OnEnable() {
        Health.onPlayerDamaged += DrawHearts;
    }
    private void OnDisable() {
        Health.onPlayerDamaged -= DrawHearts;
    }

    
    /// <summary>
    /// Creates all Hearts and sets them to Full to match the Max Player Health.
    /// </summary>
    private void InitHearts() {
        // Get amount of hearts to draw
        int hearts = playerHealth.MaxHearts;

        // Clear all current child elements
        heartsContainer.Clear();

        for (int i = 0; i < hearts; i++)
        {
            // Create new heart element
            HeartUI heartIcon = new HeartUI(Full);
            heartIcon.AddToClassList("HeartIcon");      // add CSS class

            // Add element to container
            heartsContainer.Add(heartIcon);
        }
    }

    /// <summary>
    /// Creates all hearts and sets their respective stauses to match the Player's health.
    /// </summary>
    private void DrawHearts() {
        // Clear all current child elements
        heartsContainer.Clear();

        int maxHearts = playerHealth.MaxHearts;

        for (int i = 0; i < maxHearts; i++) {
            int t = heartTypes.Length - 1;
            int statusRemainder = Mathf.Clamp(playerHealth.HP - (i*t), 0, t);
            HeartType status = heartTypes[statusRemainder];

            // Create new heart element
            HeartUI heartIcon = new HeartUI(status);
            heartIcon.AddToClassList("HeartIcon");      // add CSS class

            // Add element to container
            heartsContainer.Add(heartIcon);
        }
    }


    ////// HELPER FUNCTIONS ////////////

    /// <summary>
    /// Create all 3 Heart sprites from the texture image.
    /// </summary>
    /// <param name="pivot">Pivot for the heart sprites.</param>
    private void CreateAllSprites(Pivot pivot) {
        heartFull = CreateSprite(heartTex, pivot, 0 * iconSize);
        heartHalf = CreateSprite(heartTex, pivot, 1 * iconSize);
        heartEmpty = CreateSprite(heartTex, pivot, 2 * iconSize);
    }

    /// <summary>
    /// Creates a Sprite from a texture with a custom rect/slice.
    /// </summary>
    /// <param name="tex">The spritesheet image.</param>
    /// <param name="pivot">Sprite pivot point.</param>
    /// <param name="start">Where to start the rect.</param>
    /// <returns>A Sprite object.</returns>
    private Sprite CreateSprite(Texture2D tex, Pivot pivot, int start=0) {
        Sprite newSprite = Sprite.Create(
            tex,
            new Rect(start, 0, iconSize, iconSize),
            pivot.value,
            16
        );

        return newSprite;
    }
}
