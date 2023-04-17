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
    [SerializeField]
    private int goblin = 1;
    [SerializeField]
    private Sprite newSpriteTest = null;
    [SerializeField]
    private Sprite heartSpriteTest = null;

    //  Objects to update at runtime
    /// <summary>Player Icon element using Visual Element</summary>
    private VisualElement uiPlayerIcon;
    /// <summary>Player Icon using a UI Image sprite.</summary>
    private MyImage playerSprite;

    private VisualElement heartsContainer;


    private void Start() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        uiPlayerIcon = root.Q<VisualElement>("PlayerIcon");
        playerSprite = root.Q<MyImage>("PlayerSprite");
        heartsContainer = root.Q<VisualElement>("HeartsContainer");

        // from: https://stackoverflow.com/questions/70222544/dynamically-change-visual-elements-background-image-in-unity-ui-builder
        uiPlayerIcon.style.backgroundImage = new StyleBackground(newSpriteTest);
        uiPlayerIcon.style.display = DisplayStyle.None;

        // Using my Image class 
        playerSprite.sprite = GameManager.instance.PlayerSprite;
    }
}
