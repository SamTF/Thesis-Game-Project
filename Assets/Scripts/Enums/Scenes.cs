using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scenes
{

    private int _buildIndex;
    private string _sceneName;

    public Scenes(int id, string name) {
        _buildIndex = id;
        _sceneName = name;
    }

    public int buildIndex => _buildIndex;
    public string name => _sceneName;

    // Creating Static variables
    public static Scenes MainMenu = new Scenes(0, "Main Menu");
    public static Scenes Game = new Scenes(1, "Game");

}
