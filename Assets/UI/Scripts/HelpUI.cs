using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Hardcoded PNG menu that shows the Player the controls and what they're supposed to be doing. Click anywhere to close the Menu.
/// </summary>
public class HelpUI : MonoBehaviour
{
    private VisualElement notebook = null;
    private Button closeBtn = null;

    /// Singleton thing
    private static HelpUI _instance = null;
    public static HelpUI instance
    {
        get {return _instance;}
    }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        
        // getting elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        notebook = root.Q<VisualElement>("Notebook");
        closeBtn = root.Q<Button>();
    }

    private void Start() {
        // call backs
        // notebook.AddManipulator(new Clickable(evt => OnClick(evt)));

        if (closeBtn != null) {
            closeBtn.clicked += Close;
            notebook.focusable = true;
            notebook.Focus();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Escape)) {
            Close();
        }
    }

    private void OnClick(EventBase evt) {
        Debug.Log("ON CLICK!");
        Close();
    }

    private void Close() {
        // MainMenu.instance.GetComponent<UIDocument>().enabled = true;
        // PauseMenu.instance.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
