using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Shows current XP progress by changing the XP icon sprite.
/// </summary>
public class XPIconUI : MonoBehaviour
{
    // Sprites
    [SerializeField]
    private Sprite[] iconSprites =  new Sprite[5];

    // UI Element
    private VisualElement xpIconContainer = null;


    // Subscribing/Unsubscribing to events
    private void OnEnable() {
        LevelSystem.onXPGained += UpdateIcon;
    }
    private void OnDisable() {
        LevelSystem.onLevelUp += UpdateIcon;
    }

    private void Start() {
        // Get elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        xpIconContainer = root.Q<VisualElement>("XPIcon");

        // Test
        xpIconContainer.style.backgroundImage = new StyleBackground(iconSprites[0]);
    }

    private void UpdateIcon() {
        Debug.Log("Gained XP!!");
        int progress = LevelSystem.instance.Progress;
        xpIconContainer.style.backgroundImage = new StyleBackground(iconSprites[progress]);
    }
}
