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

    //  Objects to update at runtime
    /// <summary>Player Icon element using Visual Element</summary>
    private VisualElement uiPlayerIcon;
    /// <summary>Player Icon using a UI Image sprite.</summary>
    private MyImage playerSprite;
    /// <summary>Text element showing the time survived on current level.</summary>
    private Label timerLabel;

    // Subscribing to events
    private void OnEnable() {
        Player.onSpriteUpdated += UpdatePlayerSprite;
    }
    private void OnDisable() {
        Player.onSpriteUpdated -= UpdatePlayerSprite;
    }


    private void Start() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        uiPlayerIcon = root.Q<VisualElement>("PlayerIcon");
        playerSprite = root.Q<MyImage>("PlayerSprite");
        timerLabel = root.Q<Label>("timer");

        // Hide placeholder icon
        uiPlayerIcon.style.display = DisplayStyle.None;

        // Using my Image class 
        playerSprite.sprite = Player.instance.Sprite;
    }

    private void Update() {
        timerLabel.text = GameManager.instance.Timer.currentTime.String;
    }

    /// <summary>
    /// Changes the sprite icon by fetching the current Player sprite
    /// </summary>
    private void UpdatePlayerSprite() {
        playerSprite.sprite = Player.instance.Sprite;
    }
}
