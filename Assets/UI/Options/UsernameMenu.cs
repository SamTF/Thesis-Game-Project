using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
 using System.Text.RegularExpressions;

public class UsernameMenu : MonoBehaviour
{
    private VisualElement root;
    private TextField textInput;
    private Button btnGo;

    private void Awake() {
        root = GetComponent<UIDocument>().rootVisualElement;
        textInput = root.Q<TextField>();
        btnGo = root.Q<Button>();

        btnGo.clicked += OnClick;
    }

    private void Start() {
        CustomCursor.visible = true;
    }

    private void OnClick() {
        // https://stackoverflow.com/questions/3905180/how-to-trim-whitespace-between-characters

        // remove all EXTRA spaces (keeps spaces between words)
        string usernameTrimmed = Regex.Replace(textInput.value.Trim(), " +", " ");
        
        // remove non-alphanumeric characters but keep spaces
        string usernameFinal = Regex.Replace(usernameTrimmed, "[^a-zA-Z0-9 - ]+", "", RegexOptions.Compiled);
        Debug.Log(usernameFinal);
        
        PlayerPrefs.SetString("username", usernameFinal);
        GameManager.instance.ChangeScene(GameManager.Scenes.Game);
    }
}
