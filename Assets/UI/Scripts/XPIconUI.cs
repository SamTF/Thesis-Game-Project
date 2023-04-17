using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Shows current XP progress by changing the XP icon sprite.
/// </summary>
public class XPIconUI : MonoBehaviour
{
    /// CONSTANTS
    /// <summary>The dimensions of the XP Icon sprite in px.</summary>
    private Vector2Int iconSize = new Vector2Int(16, 24);
    /// <summary>Name of the XP Icon Progression spritesheet file.</summary>
    private string iconName = "XPIcons";
    /// <summary>Name of the Numbers spritesheet file.</summary>
    private string numbersName = "LvlNumbers";
    /// <summary>File extension of the image files.</summary>
    private string fileType = "png";

    // Sprites
    private Sprite[] iconSprites =  new Sprite[5];
    private Sprite[] numberSprites = new Sprite[5];

    // UI Element
    private VisualElement xpIconContainer = null;
    private MyImage lvlNumber = null;


    // Subscribing/Unsubscribing to events
    private void OnEnable() {
        LevelSystem.onXPGained += UpdateIcon;
        LevelSystem.onLevelUp += UpdateLevel;
    }
    private void OnDisable() {
        LevelSystem.onXPGained -= UpdateIcon;
        LevelSystem.onLevelUp -= UpdateLevel;
    }

    private void Start() {
        // Get elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        xpIconContainer = root.Q<VisualElement>("XPIcon");
        lvlNumber = root.Q<MyImage>("XPLevel");

        // Set the XP progress icon to Empty (aka 0)
        xpIconContainer.style.backgroundImage = new StyleBackground(iconSprites[0]);

        // Create the Sprites for the Icon and Numbers from their spritesheets
        CreateAllSprites(iconName, ref iconSprites);
        CreateAllSprites(numbersName, ref numberSprites);

        // Set current icon and level number
        UpdateIcon();
        UpdateLevel();
    }


    /// <summary>
    /// Create all Sprites from a spritesheet file of a given name, and store them in the given array.
    /// </summary>
    /// <param name="textureName">Name of the image file (must be identical in both Resources and CUSTOM folder)</param>
    /// <param name="spriteArray">Sprite array to save the Sprites into</param>
    private void CreateAllSprites(string textureName, ref Sprite[] spriteArray) {
        Texture2D tex = null;

        // Checking if the player created a custom number spritesheet
        if (ModManager.ModExists($"{textureName}.{fileType}")) {
            Debug.Log("[XP ICON]>>> Custom texture found");
            tex = ImageLoader.LoadTextureFromFile(iconName);
        }
        // If not, use the default icon
        else {
            Debug.Log("[XP ICON]>>> Loading default texture...");
            tex = Resources.Load($"UI/{textureName}") as Texture2D;  
        }

        for (int i = 0; i < spriteArray.Length; i++)
        {
            Sprite s = CreateSprite(tex, Pivot.Center, iconSize.x * i, iconSize);
            spriteArray[i] = s;
        }
    }

    /// <summary>
    /// Sets the appropriate XP Icon for the current XP progress
    /// </summary>
    private void UpdateIcon() {
        int progress = LevelSystem.instance.Progress;
        xpIconContainer.style.backgroundImage = new StyleBackground(iconSprites[progress]);
    }

    /// <summary>
    /// Sets the appropriate Level Number for the current XP level.
    /// </summary>
    private void UpdateLevel() {
        int level = LevelSystem.instance.Level;
        int i = Mathf.Clamp(level - 1, 0, numberSprites.Length - 1);
        lvlNumber.sprite = numberSprites[i];
    }


    /// <summary>
    /// Creates a Sprite from a texture with a custom rect/slice.
    /// </summary>
    /// <param name="tex">The spritesheet image.</param>
    /// <param name="pivot">Sprite pivot point.</param>
    /// <param name="start">Where to start the rect. Only set this if creating a sprite from a spritesheet.</param>
    /// <param name="imageSize">Dimensions of the Sprite. Only set this if creating a sprite from a spritesheet.</param>
    /// <returns>A Sprite object.</returns>
    private Sprite CreateSprite(Texture2D tex, Pivot pivot, int start=0, Vector2Int? imageSize=null) {
        // Getting the dimensions for the Sprite Rect. Using Texture dimensions if none were given.
        Vector2Int rectSize = imageSize == null ? new Vector2Int(tex.width, tex.height) : imageSize.Value;

        // Creating the Sprite object.
        Sprite newSprite = Sprite.Create(
            tex,
            new Rect(start, 0, rectSize.x, rectSize.y),
            pivot.value,
            16
        );

        return newSprite;
    }
}
