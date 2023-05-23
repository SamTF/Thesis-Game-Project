using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Container for the Stamina UI icons. Displays the Player's current Stamina in TBOI-style hearts.
/// </summary>
public class StaminaUI : MonoBehaviour
{
    // Icons
    private string iconName = "Stamina";
    [SerializeField]
    private Sprite[] iconSprites = new Sprite[3];
    private Vector2Int iconSize = new Vector2Int(8, 8);

    // Static Objects
    public static HeartType Full;
    public static HeartType Half;
    public static HeartType Empty;
    public static HeartType[] staminaTypes;

    // UI Elements
    private VisualElement root;
    private VisualElement staminaContainer;

    // Script references
    private Stamina playerStamina = null;


    // Subscribing and unsubscribing to events
    private void OnEnable() {
        Stamina.onStaminaChange += DrawHearts;
    }
    private void OnDisable() {
        Stamina.onStaminaChange -= DrawHearts;
    }


    private void Awake() {
        // creating sprites from spritesheet
        iconSprites = ImageLoader.CreateAllSprites(iconName, "UI", iconSprites.Length, iconSize);

        // Initialising HeartType objects
        Full = new HeartType(HeartFill.Full, iconSprites[0]);
        Half = new HeartType(HeartFill.Half, iconSprites[1]);
        Empty = new HeartType(HeartFill.Empty, iconSprites[2]);
        staminaTypes = new HeartType[] { Empty, Half, Full };
    }

    private void Start() {
        // Get elements
        root = GetComponent<UIDocument>().rootVisualElement;
        staminaContainer = root.Q<VisualElement>("StaminaContainer");

        // Get script references
        playerStamina = Player.instance.Stamina;

        /// instantiating icons
        InitHearts();

        DrawHearts();
    }

    /// <summary>
    /// Creates all Hearts and sets them to Full to match the Max Player Health.
    /// </summary>
    private void InitHearts() {
        // Get amount of hearts to draw
        int hearts = playerStamina.MaxOrbs;

        // Clear all current child elements
        staminaContainer.Clear();

        for (int i = 0; i < hearts; i++)
        {
            // Create new heart element
            HeartUI heartIcon = new HeartUI(Full);
            heartIcon.AddToClassList("HeartIcon");      // add CSS class

            // Add element to container
            staminaContainer.Add(heartIcon);
        }
    }

    /// <summary>
    /// Creates all hearts and sets their respective stauses to match the Player's health.
    /// </summary>
    private void DrawHearts() {
        // Clear all current child elements
        staminaContainer.Clear();

        int maxHearts = playerStamina.MaxOrbs;

        for (int i = 0; i < maxHearts; i++) {
            int t = staminaTypes.Length - 1;
            int statusRemainder = Mathf.Clamp(playerStamina.stamina - (i*t), 0, t);
            HeartType status = staminaTypes[statusRemainder];

            // Create new heart element
            HeartUI heartIcon = new HeartUI(status);
            heartIcon.AddToClassList("HeartIcon");      // add CSS class

            // Add element to container
            staminaContainer.Add(heartIcon);
        }
    }
}
