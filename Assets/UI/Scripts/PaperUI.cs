using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PaperUI : MonoBehaviour
{
    [Header("PAPER SIDEBAR UI")]
    [SerializeField]
    private string mainContainerID = "MainContainer";
    [SerializeField]
    private string spritePreviewID = "Preview";
    [SerializeField]
    private string saveButtonID = "BtnSave";
    [SerializeField]
    private string resetButtonID = "BtnReset";
    [SerializeField]
    private string clearButtonID = "BtnClear";
    [SerializeField]
    private string textInfoID = "Text";

    // Elements
    private VisualElement mainContainer = null;
    private VisualElement spritePreview = null;
    private Button saveButton = null;
    private Button resetButton = null;
    private Button clearButton = null;
    private Label textInfo = null;

    // Events
    public static event System.Action onSaveBtnClicked;
    public static event System.Action onResetBtnClicked;
    public static event System.Action onClearBtnClicked;

    private void Start() {
        // Getting elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        spritePreview   = root.Q<VisualElement>(spritePreviewID);
        saveButton      = root.Q<Button>(saveButtonID);
        resetButton     = root.Q<Button>(resetButtonID);
        clearButton     = root.Q<Button>(clearButtonID);
        textInfo        = root.Q<Label>(textInfoID);

        /// Assigning button callbacks

        // Save
        if (saveButton != null) 
            saveButton.clicked += () => onSaveBtnClicked?.Invoke();
        else
            Debug.LogError("[PAPER UI]>>> No Save Button found!");
        
        // Reset
        if (resetButton != null) 
            resetButton.clicked += () => onResetBtnClicked?.Invoke();
        else
            Debug.LogError("[PAPER UI]>>> No Reset Button found!");
        
        // Clear
        if (clearButton != null) 
            clearButton.clicked += () => onClearBtnClicked?.Invoke();
        else
            Debug.LogError("[PAPER UI]>>> No Clear Button found!");


        /// Setting Info Text
        if (textInfo != null) {
            // show intro text if level 0
            if (LevelSystem.instance.Level < 1) {
                textInfo.text = $"Draw your \ncharacter!\n:D";
            }
            // show newest colour if levelled up at least once
            else {
                Stat newStat = LevelSystem.instance.UnlockedStats[ LevelSystem.instance.UnlockedStats.Length - 1 ];
                textInfo.text = $"New stat:\n{newStat.Name.ToUpper()}!";
                textInfo.style.color = newStat.Colour;
            }
            
        }        
    }
}
