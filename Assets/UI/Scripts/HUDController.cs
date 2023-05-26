using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controller for the new HUD system using Unity UI Builder.
/// </summary>
public class HUDController : MonoBehaviour
{
    [Header("HUD CONTROLLER")]
    private VisualElement root;
    /// <summary>Player Icon element using Visual Element</summary>
    private VisualElement uiPlayerIcon;
    /// <summary>Player Icon using a UI Image sprite.</summary>
    private MyImage playerSprite;
    /// <summary>Text element showing the time survived on current level.</summary>
    private Label timerLabel;

    // Child UI
    private VisualElement statsUI;

    // Visibilty
    private int mode = 0;

    // Singleton Thing
    private static HUDController _instance = null;
    public static HUDController instance
    {
        get {return _instance;}
    }

    // Subscribing to events
    private void OnEnable() {
        Player.onSpriteUpdated += UpdatePlayerSprite;
        LevelSystem.onLevelUp += HideHUD;
    }
    private void OnDisable() {
        Player.onSpriteUpdated -= UpdatePlayerSprite;
        LevelSystem.onLevelUp -= HideHUD;
    }


    private void Awake() {
        // Singleton Thing
        if (instance == null)   { _instance = this; }
        else                    { Destroy(gameObject); }
        
        // getting elements
        root = GetComponent<UIDocument>().rootVisualElement;

        uiPlayerIcon = root.Q<VisualElement>("PlayerIcon");
        playerSprite = root.Q<MyImage>("PlayerSprite");
        timerLabel = root.Q<Label>("timer");

        // Hide placeholder icon
        uiPlayerIcon.style.display = DisplayStyle.None;

        // getting child elements
        statsUI = GetComponentInChildren<UIDocument>().rootVisualElement.Q<VisualElement>("PaperContainer");
    }

    private void Start() {
        // Using my Image class 
        playerSprite.sprite = Player.instance.Sprite;
    }

    private void Update() {
        if (timerLabel != null)
            timerLabel.text = GameManager.instance.Timer.currentTime.String;
        
        // Toggle HUD
        if (Input.GetKeyDown(KeyCode.Tab)) {
            CycleVisibility();
        }
    }

    /// <summary>
    /// Changes the sprite icon by fetching the current Player sprite
    /// </summary>
    public void UpdatePlayerSprite() {
        playerSprite.sprite = Player.instance.Sprite;
    }

    private void HideHUD() {
        root.visible = false;
    }

    /// <summary>
    /// Toggles the visibilty of HUD elements ON/OFF.
    /// </summary>
    private void CycleVisibility() {
        mode++;
        mode = mode % 3;
        
        switch (mode)
        {
            // hide everything
            case 2:
                root.visible = false;
                statsUI.visible = false;
                break;

            // hide stats show hud
            case 1:
                root.visible = true;
                statsUI.visible = false;
                break;

            // show everything
            case 0:
            default:
                root.visible = true;
                statsUI.visible = true;
                break;
        }
    }

    /// <summary>Whether the HUD is currently rendered and visible.</summary>
    public bool visible {
        get { return root.visible; }
        set {
            root.visible = value;
            statsUI.visible = value;
            mode = value ? 0 : 2;
        }
    }
}
