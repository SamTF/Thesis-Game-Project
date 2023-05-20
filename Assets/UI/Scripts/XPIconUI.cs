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
    private Sprite[] numberSprites = new Sprite[6];

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
        iconSprites     = ImageLoader.CreateAllSprites(iconName, "UI", iconSprites.Length, iconSize);
        numberSprites   = ImageLoader.CreateAllSprites(numbersName, "UI", numberSprites.Length, iconSize);

        // Set current icon and level number
        UpdateIcon();
        UpdateLevel();
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
        int i = Mathf.Clamp(level, 0, numberSprites.Length - 1);
        lvlNumber.sprite = numberSprites[i];
    }
}
