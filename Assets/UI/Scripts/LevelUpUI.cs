using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class LevelUpUI : MonoBehaviour
{
    [Header("LEVEL UP MENU UI")]
    [SerializeField]
    private string mainContainerID = "MainContainer";
    [SerializeField]
    private string colourItemsContainerID ="ColourItemsContainer";
    [SerializeField]
    private string newLevelID = "NewLevel";

    // Elements
    private VisualElement mainContainer = null;
    private VisualElement colourItemsContainer = null;
    private Label newLevel = null;

    // Events
    /// <summary>Triggered when the Player chooses a new Colour to unlock. Events passes along with colour they chose.</summary>
    public static event Action<Color> onNewColourChosen;

    /// Singleton thing
    private static LevelUpUI _instance = null;
    public static LevelUpUI instance {
        get {return _instance;}
    }


    private void Awake() {
        // Singleton - there can only be one pixel art editor!
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        // Pause the game
        GameManager.instance.GameIsPaused = true;
    }

    // Subscribing to Events
    private void OnEnable() {
        NewColourItem.onClick += OnClick;
    }
    private void OnDisable() {
        NewColourItem.onClick -= OnClick;
    }


    private void Start() {
        // getting elements
        mainContainer = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>(mainContainerID);
        colourItemsContainer = mainContainer.Q<VisualElement>(colourItemsContainerID);
        newLevel = mainContainer.Q<Label>(newLevelID);

        // setting values
        newLevel.text = "OO YESS";
        colourItemsContainer.Clear();

        // Instantiating the Colour Items
        for (int i = 0; i < 3; i++) {
            Stat stat = Player.instance.Stats.StatsArray[i];

            NewColourItem item = new NewColourItem(
                stat,
                stat.Colour,
                stat.Name,
                $"this is the\ndescription yes",
                i
            );

            colourItemsContainer.Add(item.element);
        }

        colourItemsContainer.Query<VisualElement>("ColourItem").Last().Focus();
    }

    /// <summary>
    /// When a Colour Item card thingy has been clicked
    /// </summary>
    /// <param name="colour">The Colour corresponding to that item.</param>
    private void OnClick(Color colour) {
        // LevelSystem.instance.UnlockColour(colour);
        onNewColourChosen?.Invoke(colour);
        Close();
    }

    /// <summary>
    /// Closes this Menu and resumes the game
    /// </summary>
    private void Close() {
        GameManager.instance.GameIsPaused = false;
        Destroy(gameObject);
    }

    /// <summary>Reset the singleton</summary>
    private void OnDestroy() {
        if (this == _instance) { _instance = null; }
    }
}