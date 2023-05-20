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

        // Hide placeholder icon
        uiPlayerIcon.style.display = DisplayStyle.None;

        // Using my Image class 
        playerSprite.sprite = Player.instance.Sprite;
    }

    /// <summary>
    /// Changes the sprite icon by fetching the current Player sprite
    /// </summary>
    private void UpdatePlayerSprite() {
        playerSprite.sprite = Player.instance.Sprite;
    }
}
