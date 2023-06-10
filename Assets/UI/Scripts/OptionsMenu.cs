using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class OptionsMenu : MonoBehaviour
{
    [Header("OPTIONS MENU")]
    [Header("Gameplay")]
    [SerializeField]
    private string usernameInputID = "UsernameInput";
    [SerializeField]
    private string cowboyToggleID = "CowboyToggle";
    [SerializeField]
    private string hardModeToggleID = "CowboyToggle";
    [Header("Video")]
    [SerializeField]
    private string fullscreenToggleID = "FullscreenToggle";
    [SerializeField]
    private string pixelPerfectToggleID = "PixelPerfectToggle";
    [SerializeField]
    private string saveBtnID = "BtnSave";
    [SerializeField]
    private string backBtnID = "BtnBack";

    // Elements
    private VisualElement mainContainer = null;
    private TextField usernameInput = null;
    private Toggle cowboyToggle = null;
    private Toggle hardModeToggle = null;
    private Toggle fullscreenToggle = null;
    private Toggle pixelPerfectToggle = null;
    private Button saveBtn = null;
    private Button backBtn = null;

    // Events
    public static event Action onSettingChanged;

    // Singleton Thing
    private static OptionsMenu _instance = null;
    public static OptionsMenu instance
    {
        get {return _instance;}
    }


    private void Awake() {
        // Singleton Thing
        if (instance == null)   { _instance = this; }
        else                    { Destroy(gameObject); }

        // getting elements
        VisualElement root      = GetComponent<UIDocument>().rootVisualElement;
        mainContainer           = root.Q<VisualElement>("MainContainer");
        usernameInput           = mainContainer.Q<TextField>(usernameInputID);
        cowboyToggle            = mainContainer.Q<Toggle>(cowboyToggleID);
        hardModeToggle          = mainContainer.Q<Toggle>(hardModeToggleID);
        fullscreenToggle        = mainContainer.Q<Toggle>(fullscreenToggleID);
        pixelPerfectToggle      = mainContainer.Q<Toggle>(pixelPerfectToggleID);
        saveBtn                 = mainContainer.Q<Button>(saveBtnID);
        backBtn                 = mainContainer.Q<Button>(backBtnID);
    }

    private void Start() {
        CustomCursor.visible = true;

        // btn callbacks
        if (saveBtn != null)    saveBtn.clicked += Save;
        if (backBtn != null)    backBtn.clicked += Back;

        // setting values
        if (PlayerPrefs.HasKey("username"))
            usernameInput.value = PlayerPrefs.GetString("username");

        if (PlayerPrefs.HasKey("startWithCowboy")) {
            cowboyToggle.value = PlayerPrefs.GetInt("startWithCowboy") == 1;
        }

        if (PlayerPrefs.HasKey("hardMode"))
            hardModeToggle.value = PlayerPrefs.GetInt("hardMode") == 1;

        if (PlayerPrefs.HasKey("pixelPerfect"))
            pixelPerfectToggle.value = PlayerPrefs.GetInt("pixelPerfect") == 1;
        
        fullscreenToggle.value = Screen.fullScreen;
        if (PlayerPrefs.HasKey("fullscreen"))
            PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
    }


    /// <summary>
    /// Close Menu without saving changes.
    /// </summary>
    private void Back() {
        Close();
    }

    /// <summary>
    /// Saves all the options chosen to Player Prefs and trigger event.
    /// </summary>
    private void Save() {
        PlayerPrefs.SetString("username", usernameInput.value);
        PlayerPrefs.SetInt("startWithCowboy", cowboyToggle.value ? 1 : 0);
        PlayerPrefs.SetInt("hardMode", hardModeToggle.value ? 1 : 0);
        PlayerPrefs.SetInt("pixelPerfect", pixelPerfectToggle.value ? 1 : 0);
        PlayerPrefs.SetInt("fullscreen", fullscreenToggle.value ? 1 : 0);
        Screen.fullScreen = fullscreenToggle.value;

        onSettingChanged?.Invoke();

        Close();
    }

    /// <summary>
    /// Close the options Menu
    /// </summary>
    private void Close() {
        Destroy(gameObject);
    }
}
